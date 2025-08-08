using System;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Gasoline : Fuel
    {
        // Constructor
        public Gasoline(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifetime = 4;
            Ashes = false;
            Conductivity = .01f;
            Density = 0.75f;
            State = 3;
            Strength = 130;
            Melting = new Transformation(90, typeof(GasolineVapor));
            Solidifying = new Transformation(Int32.MinValue, typeof(Gasoline));
        }
        public override void Update()
        {
            base.Update();

            // Vapor
            if (Temperature > 68 && Canvas.ChancePerSecond(Math.Min(Temperature / 140, 2)))
            {
                Pixel? above = Neighbors[0];
                if (above != null && above.Type == "Air")
                {
                    AirPool.Return((Air)above);
                    SetPixel(Canvas, above.GetIndex(), new GasolineVapor(above.Location, Canvas));
                }
            }
        }
    }
}
