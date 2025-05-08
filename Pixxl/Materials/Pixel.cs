using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System;
using Consts = Pixxl.Constants;
using System.Linq;

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
        public Xna.Vector2 Location { get; set; }
        public int Index { get; set; }
        public Xna.Vector2 Snapped { get; set; }
        public Xna.Vector2 Coords { get; set; }
        public RectangleF Rect => new(Snapped.X, Snapped.Y, Consts.Screen.PixelSize, Consts.Screen.PixelSize);

        // Constants
        private readonly int[] surrounding = Consts.Game.Diagonals ? [
            -Consts.Screen.Grid[0],      // Up
            -Consts.Screen.Grid[0] + 1,  // Top-right
            1,                           // Right
            Consts.Screen.Grid[0] + 1,   // Bottom-right
            Consts.Screen.Grid[0],       // Down
            Consts.Screen.Grid[0] - 1,   // Bottom-left
            -1,                          // Left
            -Consts.Screen.Grid[0] - 1   // Top-left
        ] : [
            -Consts.Screen.Grid[0],  // Up
            1,                       // Right
            Consts.Screen.Grid[0],   // Down
            -1,                      // Left
        ];

        // Other
        public Canvas Canvas { get; set; }
        public Pixel?[] Neighbors { get; set; }
        public int TypeId { get; }
        public string Type { get; }
        public bool Skip { get; set; }
        public bool SkipHeat { get; set; }
        public Xna.Vector2 Previous { get; set; }

        // Constructor
        public Pixel(Xna.Vector2 location, Canvas canvas, float? temp = null)
        {
            // Constants
            Conductivity = 1f;
            Density = 1f;
            State = 2; // 0 = Solid, 1 = Rigid Powder, 2 = Powder, 3 = Fluid, 4 = Energy
            Strength = 100;
            Melting = new Transformation(999999, typeof(Pixel));
            Solidifying = new Transformation(-999999, typeof(Pixel));
            Gravity = true;

            // Properties
            Previous = location;
            Neighbors = new Pixel?[surrounding.Length];
            Location = location;
            Snapped = Snap(Location);
            Coords = ConvertToCoord(Snapped, 's');
            Index = Flat(Coords);
            Canvas = canvas;
            Temperature = temp ?? Consts.Game.RoomTemp;
            Type = GetType().Name;
            TypeId = Registry.Materials.Id(Type);
            Color = ColorSchemes.GetColor(TypeId);
        }

        // Update and draw
        public virtual void Update()
        {
            // Skip
            if (Skip) { Skip = false; return; }

            // Reset
            UpdatePositions('s', 'c', 'i');
            GetNeighbors();

            // Heat transfer
            HeatTransfer();

            // For the possible moves including diagonals
            Movements();

            // Fluid spreading
            if (State >= 3) {
                if ((Location == Previous && Canvas.Rand.Next(0, Math.Min((int)Density * 3, 8)) == 0)) { FluidSpread(); }
                else if (Canvas.Rand.Next(0, Math.Clamp((int)Density * 20, 10, 40)) == 0) { FluidSpread(); }
            }
            UpdatePositions('s', 'c', 'i');

            // Check changes for melting, evaporating, plasmifying, deplasmifying, condensing, solidifying
            StateCheck();

            // Final
            Previous = Location;
        }
        public virtual bool Move(int offsetX = 0)
        {
            // Movement
            Xna.Vector2 next = Predict(new(Location.X + offsetX, Location.Y), Consts.Game.Gravity);
            // Checks
            if (CollideCheck(Location, next))
            {
                // Move array pixels
                Location = Swap(Location, next);
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
            Pixel? right = Find(new(Location.X + Consts.Game.PixelSize, Location.Y), 'l');
            Xna.Vector2 rightMove = right != null ? new(right.Location.X, right.Location.Y + Consts.Game.PixelSize) : new(0, 0);
            bool priority = (right == null || !right.Gravity || Coord(rightMove) == Coord(right.Location) || !right.CollideCheck(right.Location, rightMove));
            if (priority && Move(Consts.Screen.PixelSize)) { return true; } // down-right

            return false;
        }
        public virtual void Draw()
        {
            UpdatePositions('s', 'c', 'i');
            // Calculate red and green values based on the temperature
            Color color = Color;
            // Default is textures
            int thermax = Consts.Visual.ThermalMax;
            float percent = Temperature / thermax;
            if (Canvas.ViewMode == 1)
            {
                float temp = Math.Clamp(Temperature, 0, thermax);
                float r = percent * 255; float g = 255 - r; float b = Temperature > thermax * 2 ? (Temperature / thermax) * 25 : 0;
                color = new((int)r, (int)g, (int)b);
            }
            else if (Canvas.ViewMode == 2)
            {
                float temp = Math.Clamp(Temperature, 0, thermax);
                int saturation = (int)(percent * 255);
                color = new(saturation, saturation, saturation);
            } else if (Canvas.ViewMode == 3)
            {
                color = Registry.Materials.Colors[TypeId];
            }

            Canvas.Batch.FillRectangle(Rect, color);
        }
        // Methods
        public void UpdatePositions(params char[] positions)
        {
            if (positions.Contains('s')) { Snapped = Snap(Location); }
            if (positions.Contains('c')) { Coords = Coord(Location); }
            if (positions.Contains('i')) { Index = Flat(Coord(Location)); }
        }
        public Xna.Vector2 Predict(Xna.Vector2 vec, float velocity)
        {
            return new(vec.X, vec.Y + Math.Min(velocity * Canvas.Delta * Consts.Game.Speed, Consts.Screen.PixelSize));
        }
        public virtual bool CollideCheck(Xna.Vector2 loc, Xna.Vector2 dest)
        {
            // Basic checks
            if (loc == dest) { return false; } // Not moving
            if (!Gravity) { return false; } // Not affected

            // Setup
            Xna.Vector2 destCoord = Coord(dest);

            // If in bounds then check the available pixel
            if (!IndexCheck(dest, 'l')) { return false; }
            Pixel target = Canvas.Pixels[Flat(destCoord)];
            Xna.Vector2 delta = dest - loc;

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
                UpdatePositions('i');
                converted.Temperature = Temperature;
                Canvas.Pixels[Index] = converted;
                Skip = true;
                return;
            }
        }
        public virtual void FluidSpread()
        {
            int side = Canvas.Rand.Next(0, 2) == 0 ? -Consts.Screen.PixelSize : Consts.Screen.PixelSize;
            Xna.Vector2 next = new(Location.X + side, Location.Y);
            Pixel? target = Find(next, 'l');
            if (target != null && (target.Type == "Air" || target.Type == Type) && CollideCheck(Location, next))
            {
                Location = Swap(Location, next);
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
                // Neighbor
                Pixel? neighbor = Find(Index + surrounding[n]);
                // If the neighbor's X is too far it means that the neighbor carried over to the previous or next line so make it null instead
                Neighbors[n] = neighbor == null || Math.Abs(Coords.X - neighbor.GetCoords().X) > 1 ? null : neighbor;
            }
        }
        public int GetIndex() { return Flat(Coord(Location)); }
        public Xna.Vector2 GetCoords() {  return Coord(Location); }
        public Xna.Vector2 GetSnapped() { return Snap(Location); }
        public Pixel? Find(Xna.Vector2 vec, char mode)
        {
            Xna.Vector2 converted = ConvertToCoord(vec, mode);
            if (CoordCheck(converted))
            {
                return Canvas.Pixels[Flat(converted)];
            } else
            {
                return null;
            }
        }
        public Pixel? Find(int idx)
        {
            if (idx >= 0 && idx < Canvas.Pixels.Length) { return Canvas.Pixels[idx]; }
            else { return null; }
        }
        public Xna.Vector2 Swap(Xna.Vector2 first, Xna.Vector2 second)
        {
            // Info
            Xna.Vector2 firstCoord = Coord(first);
            Xna.Vector2 secondCoord = Coord(second);
            Pixel? firstPixel = Find(firstCoord, 'c');
            Pixel? secondPixel = Find(secondCoord, 'c');

            // If null values return current position (don't move)
            if (firstPixel == null || secondPixel == null) { return first; }

            // Swap objects
            Canvas.Pixels[Flat(firstCoord)] = secondPixel; // Move second to first
            Canvas.Pixels[Flat(secondCoord)] = firstPixel; // Move first to second

            secondPixel.Location = first;
            return second;
        }
        // Static methods
        public static bool IndexCheck(Xna.Vector2 loc, char mode)
        {
            if (mode == 'c') { return CoordCheck(loc); }
            Xna.Vector2 coord = ConvertToCoord(loc, mode);
            int[] grid = Consts.Screen.Grid;
            if (coord.X < 0 || coord.X >= grid[0]) { return false; }
            if (coord.Y < 0 || coord.Y >= grid[1]) { return false; }
            return true;
        }
        public static bool CoordCheck(Xna.Vector2 coord)
        {
            int[] grid = Consts.Screen.Grid;
            if (coord.X < 0 || coord.X >= grid[0]) { return false; }
            if (coord.Y < 0 || coord.Y >= grid[1]) { return false; }
            return true;
        }
        public static Xna.Vector2 ConvertToCoord(Xna.Vector2 loc, char mode)
        {
            // Setup
            Xna.Vector2 converted;

            // Converting to coords based on mode
            if (mode == 'c') { converted = loc; } // Coordinate
            else if (mode == 'l') { converted = Coord(loc); } // Location
            else if (mode == 's') { converted = loc / Consts.Screen.PixelSize; } // Snapped location
            else { throw new ArgumentException("Mode should be 'l', 's', or 'c'"); }

            return converted;
        }
        public static Xna.Vector2 Coord(Xna.Vector2 vec)
        {
            return Xna.Vector2.Floor(vec / Consts.Screen.PixelSize);
        }
        public static Xna.Vector2 Snap(Xna.Vector2 vec)
        {
            return Coord(vec) * Consts.Screen.PixelSize;
        }
        public static int Flat(int x, int y) { return Consts.Screen.Grid[0] * y + x; }
        public static int Flat(Xna.Vector2 loc) { return (int)(Consts.Screen.Grid[0] * loc.Y + loc.X); }
        public static int Flat(float x, float y) { return Consts.Screen.Grid[0] * (int)y + (int)x;}
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
