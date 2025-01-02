using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using MonoGame.Extended.Input.InputListeners;
using System.Runtime.InteropServices;
using Pixxl;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Pixxl.Materials
{
    public class Pixel
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
        public int Size = Const.PixelSize;

        // Properties
        public float Temperature { get; set; }
        public float Velocity { get; set; }
        public Xna.Vector2 Location { get; set; }
        public Xna.Vector2 Snapped => Snap(Location);
        public Xna.Vector2 Coords => Coord(Location);
        public RectangleF Rect => new(Snapped.X, Snapped.Y, Const.PixelSize, Const.PixelSize);

        // Constants
        private readonly Xna.Vector2[] adjacents = [new(0, Const.PixelSize), new(Const.PixelSize, 0), new(0, -Const.PixelSize), new(-Const.PixelSize, 0)];

        // Other
        public Canvas Canvas { get; set; }

        // Constructor
        public Pixel(Xna.Vector2 location, Canvas canvas)
        {
            // Constants
            Conductivity = 1f;
            Density = 1f;
            State = 2; // 0 = Solid, 1 = Rigid Powder, 2 = Powder, 3 = Fluid
            Strength = 100;
            Melting = new Transformation(999999, typeof(Pixel));
            Solidifying = new Transformation(-999999, typeof(Pixel));
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
            if (State == 3 && !moved && Canvas.Rand.Next(0, 4) == 0) { GasSpread(); }

            // Heat transfer
            HeatTransfer();

            // Check changes for melting, evaporating, plasmifying, deplasmifying condensing, hardening
            StateCheck();
        }
        public virtual void Draw()
        {
            // Calculate red and green values based on the temperature
            Color color = Color;
            // Default is textures
            if (Canvas.ColorMode == 1)
            {
                float temp = Math.Clamp(Temperature, 0, 500);
                float r = (temp / 500) * 255; float g = 255 - r; float b = 0;
                color = new((int)r, (int)g, (int)b);
            }
            else if (Canvas.ColorMode == 2)
            {
                float temp = Math.Clamp(Temperature, 0, 500);
                int saturation = (int) ((temp / 500) * 255);
                color = new(saturation, saturation, saturation);
            }

            Canvas.Batch.FillRectangle(Rect, color);
        }
        // Methods
        public Xna.Vector2 Predict(Xna.Vector2 vec, float velocity)
        {
            return new(vec.X, vec.Y + (velocity * Canvas.Delta));
        }
        public virtual bool CollideCheck(Xna.Vector2 loc, Xna.Vector2 dest, char mode)
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
        public virtual void StateCheck()
        {
            if (Temperature >= Melting.Temperature) { Transform(Melting); }
            else if (Temperature <= Solidifying.Temperature) { Transform(Solidifying); }
        }
        public virtual void Transform(Transformation transformation)
        {
            object[] args = { Location, Canvas };
            Pixel converted = (Pixel)Activator.CreateInstance(Melting.Material, args);
            converted.Temperature = Temperature; converted.Velocity = Velocity;

            Canvas.Pixels[(int)Coords.Y][(int)Coords.X] = converted;
        }
        public virtual void GasSpread()
        {
            try
            {
                int side = Canvas.Rand.Next(0, 4) == 0 ? -Const.PixelSize : Const.PixelSize;
                Xna.Vector2 next = new(Location.X + side, Location.Y);
                if (CollideCheck(Location, next, 'l'))
                {
                    Location = Swap(Location, next, 'l');
                }
            }
            catch { } // Out of bounds
        }
        public virtual void HeatTransfer()
        {
            // Setup
            List<Pixel> neighbors = [];
            int i = 0;

            // Getting pixel objects
            foreach (Xna.Vector2 dir in adjacents)
            {
                // Add neighbor pixel
                try { neighbors.Add(Find(Location + adjacents[i], 'l')); }
                catch { } // Out of bounds
                i++;
            }

            // Heat transfers
            foreach (Pixel neighbor in neighbors)
            {
                // Lose hear
                if (Temperature > neighbor.Temperature)
                {
                    // Heat transfer simplified equation
                    float transfer = (Temperature - neighbor.Temperature) / ( 1f / Conductivity + 1f / neighbor.Conductivity) * Canvas.Delta;
                    Temperature -= transfer;
                    neighbor.Temperature += transfer;
                }
            }
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
