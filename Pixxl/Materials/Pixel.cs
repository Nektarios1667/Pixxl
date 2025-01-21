using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System;
using Consts = Pixxl.Constants;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Security.AccessControl;
using System.Linq;
using MonoGame.Extended.Serialization.Json;

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
        public int Index => Flat(Coord(Location));
        public Xna.Vector2 Snapped => Snap(Location);
        public Xna.Vector2 Coords => Coord(Location);
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
        private Xna.Vector2 Previous { get; set; }

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
            Canvas = canvas;
            Temperature = temp ?? Consts.Game.RoomTemp;
            Type = GetType().Name;
            TypeId = Registry.Materials.Id(Type);
            Color = ColorSchemes.GetColor(TypeId);
        }

        // Update and draw
        public virtual void Update()
        {
            // Deletion
            if (Skip) { Skip = false; return; }

            // Reset
            GetNeighbors();

            // For the possible moves including diagonals
            Movements();

            // Fluid spreading
            if (State >= 3) {
                if ((Location == Previous && Canvas.Rand.Next(0, Math.Min((int)Density * 3, 8)) == 0)) { FluidSpread(); }
                else if (Canvas.Rand.Next(0, Math.Clamp((int)Density * 10, 10, 40)) == 0) { FluidSpread(); }
            }

            // Heat transfer
            if (!SkipHeat) { HeatTransfer(); }
            else { SkipHeat = false; }

            // Check changes for melting, evaporating, plasmifying, deplasmifying condensing, hardening
            StateCheck();

            // Final
            Previous = Location;
        }
        public virtual bool Move(int offsetX = 0)
        {
            // Movement
            Xna.Vector2 next = Predict(new(Location.X + offsetX, Location.Y), Consts.Game.Gravity);
            // Checks
            if (CollideCheck(Location, next, 'l'))
            {
                // Move array pixels
                Location = Swap(Location, next, 'l');
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

            // This checks if the pixel to the to the right will move down to the bottom-right
            // This is done since downwards movement is prioritized over diagonal movement
            Pixel? right = Find(new(Location.X + Consts.Game.PixelSize, Location.Y), 'l');
            Xna.Vector2 rightMove = right != null ? new(right.Location.X, right.Location.Y + Consts.Game.PixelSize) : new(0, 0);
            bool priority = (right == null || Coord(rightMove) == Coord(right.Location) || !right.CollideCheck(right.Location, rightMove, 'l'));
            // Moves
            if (Move(-Consts.Screen.PixelSize)) { return true; } // down-left

            if (priority && Move(Consts.Screen.PixelSize)) { return true; } // down-right

            return false;
        }
        public virtual void Draw()
        {
            // Calculate red and green values based on the temperature
            Color color = Color;
            // Default is textures
            int thermax = Consts.Visual.ThermalMax;
            float percent = Temperature / thermax;
            if (Canvas.ColorMode == 1)
            {
                float temp = Math.Clamp(Temperature, 0, thermax);
                float r = percent * 255; float g = 255 - r; float b = Temperature > thermax * 2 ? (Temperature / thermax) * 25 : 0;
                color = new((int)r, (int)g, (int)b);
            }
            else if (Canvas.ColorMode == 2)
            {
                float temp = Math.Clamp(Temperature, 0, thermax);
                int saturation = (int)(percent * 255);
                color = new(saturation, saturation, saturation);
            } else if (Canvas.ColorMode == 3)
            {
                color = Registry.Materials.Colors[TypeId];
            }

            Canvas.Batch.FillRectangle(Rect, color);
        }
        // Methods
        public Xna.Vector2 Predict(Xna.Vector2 vec, float velocity)
        {
            return new(vec.X, vec.Y + Math.Min(velocity * Canvas.Delta * Consts.Game.Speed, Consts.Screen.PixelSize));
        }
        public virtual bool CollideCheck(Xna.Vector2 loc, Xna.Vector2 dest, char mode)
        {
            // Setup
            Xna.Vector2 destCoord = ConvertToCoord(dest, mode);

            // Basic checks before getting pixel, if it exists
            if (destCoord.X < 0 || destCoord.X > Consts.Screen.Grid[0] - 1) { return false; } // Out of bounds width
            if (destCoord.Y < 0 || destCoord.Y > Consts.Screen.Grid[1] - 1) { return false; } // Out of bounds height
            if (!Gravity) { return false; } // Not affected
            if (loc == dest) { return false; } // Not moving

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
            if (target.Density == Density && target.Temperature > Temperature && delta.Y < 0) { return false; } // Moving up at same density but at cooler temperature
            if (target.Density == Density && target.Temperature < Temperature && delta.Y > 0) { return false; } // Moving down at same density but at warmer temperature
            if (target.Density == Density && target.Temperature == Temperature) { return false; } // Same stats

            return true;
        }
        public virtual void StateCheck()
        {
            if (Temperature >= Melting.Temperature) { Transform(Melting); }
            else if (Temperature <= Solidifying.Temperature) { Transform(Solidifying); }
        }
        public virtual void Transform(Transformation transformation)
        {
            object[] args = { Location, Canvas };
            Pixel converted = (Pixel)Activator.CreateInstance(transformation.Material, args);
            converted.Temperature = Temperature;

            Canvas.Pixels[Flat(Coords)] = converted;
        }
        public virtual void FluidSpread()
        {
            int side = Canvas.Rand.Next(0, 2) == 0 ? -Consts.Screen.PixelSize : Consts.Screen.PixelSize;
            Xna.Vector2 next = new(Location.X + side, Location.Y);
            Pixel? target = Find(next, 'l');
            if (target != null && (target.Type == "Air" || target.Type == Type) && CollideCheck(Location, next, 'l'))
            {
                Location = Swap(Location, next, 'l');
            }
        }
        public virtual void HeatTransfer()
        {
            // Heat transfers
            float multiplier = Canvas.Delta * Consts.Game.Speed * Consts.Game.HeatTransfer;
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
            int idx = Index;
            for (int n = 0; n < surrounding.Length; n++)
            {
                // Neighbor
                Pixel? neighbor = Find(Index + surrounding[n]);
                // If the neighbor's X is too far it means that the neighbor carried over to the previous or next line so make it null instead
                Neighbors[n] = neighbor == null || Math.Abs(Coords.X - neighbor.Coords.X) > 1 ? null : neighbor;
                if (neighbor == null) { continue; }
            }
        }
        public Pixel? Find(Xna.Vector2 vec, char mode)
        {
            Xna.Vector2 converted = ConvertToCoord(vec, mode);
            if (IndexCheck(converted, 'c'))
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
        public Xna.Vector2 Swap(Xna.Vector2 first, Xna.Vector2 second, char mode)
        {
            // Info
            Xna.Vector2 firstCoord = ConvertToCoord(first, mode);
            Xna.Vector2 secondCoord = ConvertToCoord(second, mode);
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
            Xna.Vector2 coord = ConvertToCoord(loc, mode);
            if (coord.X < 0 || coord.X >= Consts.Screen.Grid[0]) { return false; }
            if (coord.Y < 0 || coord.Y >= Consts.Screen.Grid[1]) { return false; }
            return true;
        }
        public static Xna.Vector2 ConvertToCoord(Xna.Vector2 loc, char mode)
        {
            // Setup
            Xna.Vector2 converted;

            // Converting to coords based on mode
            if (mode == 'c') { converted = loc; } // Coordinate
            else if (mode == 'l') { converted = Coord(loc); } // Location
            else if (mode == 's') { converted = loc * Consts.Screen.PixelSize; } // Snapped location
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
