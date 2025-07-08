using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Glowshard : Pixel
    {
        // Constructor
        private float Cycle { get; set; }
        private bool Rising { get; set; }
        public Glowshard(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 200f;
            Density = 36f;
            State = 2;
            Strength = 12000;
            Melting = new Transformation(Int32.MaxValue, typeof(Glowshard));
            Solidifying = new Transformation(Int32.MinValue, typeof(Glowshard));
            Cycle = Canvas.Rand.Next(0, 256);
            Rising = true;
        }
        public override void Update()
        {
            base.Update();

            // Bouncing color
            if (Rising)
            {
                if (Cycle < 255) { Cycle += Canvas.Delta * 60; }
                else { Rising = false; }
            }
            else
            {
                if (Cycle > 0) { Cycle -= Canvas.Delta * 60; }
                else { Rising = true; }
            }
        }
        public override void Draw()
        {
            // Calculate red and green values based on the temperature
            Color color = new(Color.R - (int)Cycle, Color.G, Color.B);
            // Default is textures
            int thermax = Constants.Visual.ThermalMax;
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
            }
            else if (Canvas.ViewMode == 3)
            {
                color = Registry.Materials.Colors[TypeId];
            }

            Canvas.Batch.Draw(Window.OnePixel, Rect, color);
        }
    }
}
