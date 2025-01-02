using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using MonoGame.Extended.Input.InputListeners;

namespace Pixxl.Materials
{
    public class Pixel
    {
        // Constants
        public float Density { get; set; }
        public int State { get; set; }
        public int Strength { get; set; }
        public int Melting { get; set; }
        public bool Gravity { get; set; }
        public Color Color { get; set; }
        public int Size = Const.PixelSize;

        // Properties
        public float Temperature { get; set; }
        public float Velocity { get; set; }
        public Xna.Vector2 Location { get; set; }
        public Xna.Vector2 Snapped => Snap(Location);
        public Xna.Vector2 Coords => Coord(Location);
        public RectangleF Rect => new(Snapped.X, Snapped.Y, Const.PixelSize, Const.PixelSize);
        

        // Other
        public Canvas Canvas { get; set; }

        // Constructor
        public Pixel(Xna.Vector2 location, Canvas canvas)
        {
            // Constants
            Density = 1f;
            State = 2; // 0 = Solid, 1 = Rigid Powder, 2 = Powder, 3 = Fluid
            Strength = 100;
            Melting = 200;
            Gravity = true;
            Color = ColorSchemes.Debug();

            // Properties
            Location = location;
            Canvas = canvas;
            Temperature = Const.RoomTemp;
            Velocity = 0f;
        }

        // Update and draw
        public virtual void Update()
        {
            // Left, middle, right
            int[] offsets = [];
            if (State == 1) { offsets = [0]; }
            if (State == 2) { offsets = [0, -Const.PixelSize, Const.PixelSize]; }
            if (State == 3) { offsets = [0]; }
            
            if (Gravity) { Velocity = Const.Gravity; }

            // For the possible moves including diagonals
            bool moved = false;
            for (int c = 0; c < offsets.Length; c++)
            {
                // Movement
                Xna.Vector2 next = Predict(new(Location.X + offsets[c], Location.Y), Velocity);
                // Checks
                if (CollideCheck(Location, next, 'l'))
                {
                    // Move array pixels
                    Location = Swap(Location, next, 'l');
                    moved = true;
                    break;
                }
            }

            // Gas spreading
            if (State == 3 && !moved && Canvas.Rand.Next(0, 4) == 0)
            {
                try
                {
                    int side = Canvas.Rand.Next(0, 2) == 0 ? -Const.PixelSize : Const.PixelSize;
                    Xna.Vector2 next = new(Location.X + side, Location.Y);
                    if (Find(next, 'l').GetType().Name == "Air")
                    {
                        Location = Swap(Location, next, 'l');
                    }
                } catch {} // Out of bounds
            }
        }
        public virtual void Draw()
        {
            Canvas.Batch.FillRectangle(Rect, Color);
        }
        // Methods
        public Xna.Vector2 Predict(Xna.Vector2 vec, float velocity)
        {
            return new(vec.X, vec.Y + (velocity * Canvas.Delta));
        }
        public bool CollideCheck(Xna.Vector2 loc, Xna.Vector2 dest, char mode)
        {
            // Setup
            Xna.Vector2 destCoord = ConvertToCoord(dest, mode);

            // Basic checks before getting pixel, if it exists
            if (destCoord.X < 0 || destCoord.X > Const.Grid[0] - 1) { return false; } // Out of bounds width
            if (destCoord.Y < 0 || destCoord.Y > Const.Grid[1] - 1) { return false; } // Out of bounds height
            if (!Gravity) { return false; } // Not affected
            if (loc == dest) { return false; } // Not moving

            // If in bounds then check the available pixel
            Pixel target = Canvas.Pixels[(int)destCoord.Y][(int)destCoord.X];
            Xna.Vector2 delta = dest - loc;

            // Later checks
            if (target == this) { return true; } // If self dont check density
            if (target.Density >= Density && delta.Y > 0) { return false; } // Moving down and hitting denser
            if (target.Density <= Density && delta.Y < 0) { return false; } // Moving up and hitting denser

            return true;

        }
        public Pixel Find(Xna.Vector2 vec, char mode)
        {
            Xna.Vector2 converted = ConvertToCoord(vec, mode);
            return Canvas.Pixels[(int)converted.Y][(int)converted.X];
        }
        public Xna.Vector2 Swap(Xna.Vector2 first, Xna.Vector2 second, char mode)
        {
            // Info
            Xna.Vector2 firstCoord = ConvertToCoord(first, mode);
            Xna.Vector2 secondCoord = ConvertToCoord(second, mode);
            Pixel firstPixel = Find(firstCoord, 'c');
            Pixel secondPixel = Find(secondCoord, 'c');

            // Swap objects
            Canvas.Pixels[(int)firstCoord.Y][(int)firstCoord.X] = secondPixel; // Move second to first
            Canvas.Pixels[(int)secondCoord.Y][(int)secondCoord.X] = firstPixel; // Move first to second

            secondPixel.Location = first;
            return second;
        }
        // Static methods
        public static Xna.Vector2 ConvertToCoord(Xna.Vector2 loc, char mode)
        {
            // Setup
            Xna.Vector2 converted;

            // Converting to coords based on mode
            if (mode == 'l') { converted = Coord(loc); } // Location
            else if (mode == 's') { converted = loc * Const.PixelSize; } // Snapped location
            else if (mode == 'c') { converted = loc; } // Coordinate
            else { throw new ArgumentException("Mode should be 'l', 's', or 'c'"); }

            return converted;
        }
        public static Xna.Vector2 Coord(Xna.Vector2 vec)
        {
            return Xna.Vector2.Floor(vec / Const.PixelSize);
        }
        public static Xna.Vector2 Snap(Xna.Vector2 vec)
        {
            return Coord(vec) * Const.PixelSize;
        }
    }
}
