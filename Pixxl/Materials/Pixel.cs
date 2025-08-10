using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Consts = Pixxl.Constants;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public abstract class Pixel
    {
        // Constants
        public float Density { get; set; }
        public float Conductivity { get; set; }
        public int State { get; set; }
        public int Strength { get; set; }
        public Transformation Melting { get; set; }
        public Transformation Solidifying { get; set; }
        public bool Gravity { get; set; }
        public Color Color { get; set; }
        public int Size = Consts.Screen.PixelSize;

        // Properties
        public float Temperature { get; set; }
        public Vector2 Location { get; set; }
        public int Index { get; set; }
        public Point Snapped { get; set; }
        public Point Coords { get; set; }
        public Rectangle Rect => new((int)Snapped.X, (int)Snapped.Y, Consts.Screen.PixelSize, Consts.Screen.PixelSize);
        public bool Dead { get; set; } = false;

        // Constants
        private readonly int[] surrounding = [
            -Consts.Screen.Grid[0],      // Up
            -Consts.Screen.Grid[0] + 1,  // Top-right
            1,                           // Right
            Consts.Screen.Grid[0] + 1,   // Bottom-right
            Consts.Screen.Grid[0],       // Down
            Consts.Screen.Grid[0] - 1,   // Bottom-left
            -1,                          // Left
            -Consts.Screen.Grid[0] - 1   // Top-left
        ];

        // Other
        public Canvas Canvas { get; set; }
        public Pixel?[] Neighbors { get; set; }
        public int TypeId { get; }
        public string Type { get; }
        public bool SkipHeat { get; set; }
        public Vector2 Previous { get; set; }
        // Constructor
        public Pixel(Vector2 location, Canvas canvas)
        {
            // Constants
            Conductivity = 1f;
            Density = 1f;
            State = 2; // 0 = Solid, 1 = Rigid Powder, 2 = Powder, 3 = Fluid, 4 = Energy
            Strength = 100;
            Melting = new Transformation(Int32.MaxValue, typeof(Pixel));
            Solidifying = new Transformation(Int32.MinValue, typeof(Pixel));
            Gravity = true;

            // Properties
            Previous = location;
            Neighbors = new Pixel?[surrounding.Length];
            Location = location;
            Snapped = Snap(Location);
            Coords = ConvertToCoord(Snapped, 's');
            Index = Flat(Coords);
            Canvas = canvas;
            Temperature = Consts.Game.RoomTemp;
            Type = GetType().Name;
            TypeId = Registry.Materials.Id(Type);
            Color = ColorSchemes.GetColor(TypeId);
        }

        // Update and draw
        public virtual void Update()
        {
            // Reset
            GetNeighbors();

            // Heat transfer
            HeatTransfer();

            // For the possible moves including diagonals
            Movements();

            // Fluid spreading
            if (State >= 3)
            {
                if (Location == Previous && Canvas.ChancePerSecond(10f / Math.Clamp(Density, 1, 10))) { FluidSpread(); }
                else if (Canvas.Rand.Next(0, Math.Clamp((int)Density * 20, 10, 40)) == 0) { FluidSpread(); }
            }
            UpdatePositions();

            // Check changes for melting, evaporating, plasmifying, deplasmifying, condensing, solidifying
            StateCheck();

            // Final
            Previous = Location;
        }
        public void Reset(Vector2 location)
        {
            Previous = location;
            Array.Clear(Neighbors);
            Location = location;
            Snapped = Snap(Location);
            Coords = ConvertToCoord(Snapped, 's');
            Index = Flat(Coords);
            Temperature = Consts.Game.RoomTemp;
        }
        public virtual bool Move(int offsetX = 0)
        {
            // Movement
            Vector2 next = Predict(new(Location.X + offsetX, Location.Y), Consts.Game.Gravity);
            // Checks
            if (CollideCheck(Location, next))
            {
                // Move array pixels
                SwapTo(next);
                return true;
            }
            return false;
        }
        public virtual bool Movements()
        {
            // Checks
            if (!Gravity) { return false; }
            if (Move()) { return true; } // Down
            if (State < 2) { return false; } // Not a fluid or energy

            // Moves
            if (Move(-Consts.Screen.PixelSize)) { return true; } // down-left

            // Move right
            // Check if other pixel will move down into down right since adjacent has higher priority
            Pixel? right = Find(new((int)Location.X + Consts.Game.PixelSize, (int)Location.Y), 'l');
            Vector2 rightMove = right != null ? new(right.Location.X, right.Location.Y + Consts.Game.PixelSize) : new(0, 0);
            bool priority = (right == null || !right.Gravity || Coord(rightMove) == Coord(right.Location) || !right.CollideCheck(right.Location, rightMove));
            if (priority && Move(Consts.Screen.PixelSize)) { return true; } // down-right

            return false;
        }
        public virtual void Draw()
        {
            UpdatePositions();
            // Calculate red and green values based on the temperature
            Color color = Color;
            // Default is textures
            int thermax = Consts.Visual.ThermalMax;
            float percent = Temperature / thermax;
            if (Canvas.ViewMode == 1)
            {
                float r = percent * 255; float g = 255 - r; float b = Temperature > thermax * 2 ? (Temperature / thermax) * 25 : 0;
                color = new((int)r, (int)g, (int)b);
            }
            else if (Canvas.ViewMode == 2)
            {
                int saturation = (int)(percent * 255);
                color = new(saturation, saturation, saturation);
            }
            else if (Canvas.ViewMode == 3)
            {
                color = new(255, 255, 0);
            }
            Canvas.Batch.Draw(Window.OnePixel, Rect, color);

        }
        // Methods
        public void UpdatePositions()
        {
            Snapped = Snap(Location.ToPoint());
            Coords = ConvertToCoord(Snapped, 's');
            Index = Flat(Coords);
        }
        public Xna.Vector2 Predict(Xna.Vector2 vec, float velocity)
        {
            return new(vec.X, vec.Y + Math.Min(velocity * Canvas.Delta * Consts.Game.Speed, Consts.Screen.PixelSize));
        }
        public virtual bool CollideCheck(Vector2 loc, Vector2 dest)
        {
            // Basic checks
            if (loc == dest) { return false; } // Not moving
            if (!Gravity) { return false; } // Not affected

            // Setup
            Point destCoord = Coord(dest.ToPoint());

            // If in bounds then check the available pixel
            if (!IndexCheck(dest.ToPoint(), 'l')) { return false; }
            Pixel target = Canvas.Pixels[Flat(destCoord)];
            Vector2 delta = dest - loc;

            // Later checks
            if (target == this) { return true; } // If self dont check density
            if (target.State < 3) { return false; }
            if (!target.Gravity) { return false; } // Pixel doesnt move
            if (target.Density > Density && delta.Y > 0) { return false; } // Moving down and hitting denser
            if (target.Density < Density && delta.Y < 0) { return false; } // Moving up and hitting denser
            if (target.Density == Density)
            {
                if (target.Temperature > Temperature && delta.Y < 0) { return false; } // Moving up at same density but at cooler temperature
                if (target.Temperature < Temperature && delta.Y > 0) { return false; } // Moving down at same density but at warmer temperature
                if (target.Temperature == Temperature) { return false; } // Same stats
            }

            return true;
        }
        public virtual void StateCheck()
        {
            if (!(Melting.Material.ToString() == Type) && Temperature >= Melting.Temperature) { Transform(Melting); }
            else if (!(Solidifying.Material.ToString() == Type) && Temperature <= Solidifying.Temperature) { Transform(Solidifying); }
        }
        public virtual void Transform(Transformation transformation)
        {
            Pixel? converted = (Pixel?)Activator.CreateInstance(transformation.Material, [Location, Canvas]);
            if (converted != null)
            {
                UpdatePositions();
                converted.Temperature = Temperature;
                SetPixel(Canvas, Index, converted);
                
                return;
            }
        }
        public virtual void FluidSpread()
        {
            int side = Canvas.Rand.Next(0, 2) == 0 ? -Consts.Screen.PixelSize : Consts.Screen.PixelSize;
            Vector2 next = new(Location.X + side, Location.Y);
            Pixel? target = Find(next.ToPoint(), 'l');
            if (target != null && CollideCheck(Location, next))
            {
                UpdatePositions();
                SwapTo(next);
            }
        }
        public virtual void HeatTransfer()
        {
            // Skip repeated heat calculations
            if (SkipHeat)
            {
                SkipHeat = false;
                return;
            }

            // Heat transfers
            float multiplier = Canvas.Delta * Consts.Game.HeatTransfer;
            foreach (Pixel? neighbor in Neighbors)
            {
                // Lose heat
                if (neighbor != null && Temperature > neighbor.Temperature)
                {
                    // Heat transfer simplified equation
                    float dTemp = Temperature - neighbor.Temperature;
                    float conductivity = 1f / Conductivity + 1f / neighbor.Conductivity;
                    float transfer = Math.Min((dTemp / conductivity) * multiplier, dTemp / 2);
                    Temperature -= transfer;
                    neighbor.Temperature += transfer;
                    neighbor.SkipHeat = true;
                }
            }
        }
        public virtual void GetNeighbors()
        {
            for (int n = 0; n < surrounding.Length; n++)
            {
                // Check if neighbor already calculated it
                if (Neighbors[n] != null) continue;

                // Neighbor
                Pixel? neighbor = Find(Index + surrounding[n]);
                if (neighbor == null) continue;

                // If the neighbor's X is too far it means that the neighbor carried over to the previous or next line so make it null instead
                Neighbors[n] = Math.Abs(Coords.X - neighbor.GetCoords().X) > 1 ? null : neighbor;
                int reverseN = (n + 4) % 8;
                neighbor.Neighbors[reverseN] = this;
            }
        }
        public int GetIndex() { return Flat(Coord(Location.ToPoint())); }
        public Point GetCoords() { return Coord(Location.ToPoint()); }
        public Point GetSnapped() { return Snap(Location.ToPoint()); }
        public Pixel? Find(Point point, char mode)
        {
            Point converted = ConvertToCoord(point, mode);
            if (CoordCheck(converted))
            {
                return Canvas.Pixels[Flat(converted)];
            }
            else
            {
                return null;
            }
        }
        public Pixel? Find(int idx)
        {
            if (idx >= 0 && idx < Canvas.Pixels.Length) { return Canvas.Pixels[idx]; }
            else { return null; }
        }

        // Static methods
        public static void SetPixel(Canvas canvas, int idx, Pixel pixel)
        {
            canvas.NextPixels[idx] = pixel;
        }
        public static void SetPixel(Canvas canvas, Point coord, Pixel pixel) => SetPixel(canvas, Flat(coord), pixel);
        public static void SetPixel(Canvas canvas, Vector2 pos, Pixel pixel) => SetPixel(canvas, ConvertToCoord(pos.ToPoint(), 'l'), pixel);

        public void SwapTo(Vector2 second)
        {
            // Info
            int secondIndex = Flat(Coord(second.ToPoint()));
            Pixel? secondPixel = Canvas.Pixels[secondIndex];

            // If null values return current position (don't move)
            if (secondPixel == null) return;

            // Swap objects
            SetPixel(Canvas, GetIndex(), secondPixel);
            SetPixel(Canvas, secondIndex, this);

            secondPixel.Location = Location;
            Location = second;
        }
        public static bool IndexCheck(Point pos, char mode)
        {
            if (mode == 'c') { return CoordCheck(pos); }
            Point coord = ConvertToCoord(pos, mode);
            int[] grid = Consts.Screen.Grid;
            if (coord.X < 0 || coord.X >= grid[0]) { return false; }
            if (coord.Y < 0 || coord.Y >= grid[1]) { return false; }
            return true;
        }
        public static bool CoordCheck(Point coord)
        {
            int[] grid = Consts.Screen.Grid;
            if (coord.X < 0 || coord.X >= grid[0]) { return false; }
            if (coord.Y < 0 || coord.Y >= grid[1]) { return false; }
            return true;
        }
        public static Point ConvertToCoord(Point pos, char mode)
        {
            // Setup
            Point converted;

            // Converting to coords based on mode
            if (mode == 'c') { converted = pos; } // Coordinate
            else if (mode == 'l') converted = Coord(pos); // Location
            else if (mode == 's') converted = pos / Consts.Screen.PixelSizePoint; // Snapped location
            else { throw new ArgumentException("Mode should be 'l', 's', or 'c'"); }

            return converted;
        }
        public static Point Coord(Point pos) => pos / Consts.Screen.PixelSizePoint;
        public static Point Coord(Vector2 vec) => new((int)(vec.X / Consts.Screen.PixelSize), (int)(vec.Y / Consts.Screen.PixelSize));
        public static Point Snap(Point vec) => Coord(vec) * Consts.Screen.PixelSizePoint;
        public static Point Snap(Vector2 vec) => new Point((int)(vec.X / Consts.Screen.PixelSize), (int)(vec.Y / Consts.Screen.PixelSize)) * Consts.Screen.PixelSizePoint;
        public static int Flat(int x, int y) => Consts.Screen.Grid[0] * y + x;
        public static int Flat(Point coords) => (Consts.Screen.Grid[0] * coords.Y + coords.X);
        public static int Flat(Vector2 coords) => Consts.Screen.Grid[0] * (int)coords.Y + (int)coords.X;
        public static int Flat(float x, float y) => Consts.Screen.Grid[0] * (int)y + (int)x;

    }
}

namespace Pixxl
{
    public struct Transformation
    {
        public int Temperature { get; set; }
        public Type Material { get; set; }

        public Transformation(int temperature, Type material)
        {
            Temperature = temperature;
            Material = material;
        }
    }
}
