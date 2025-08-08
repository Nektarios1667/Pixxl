using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Flare : Fuel
    {
        // Constructor
        public Flare(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifetime = 25f;
            Ashes = false;
            Conductivity = .02f;
            Density = .88f;
            State = 2;
            Strength = 70;
            Melting = new Transformation(Int32.MaxValue, typeof(Flare));
            Solidifying = new Transformation(Int32.MinValue, typeof(Flare));
        }

        public override void Update()
        {
            base.Update();

            // Burned out
            if (Burned >= Lifetime)
            {
                SetPixel(Canvas, Index, new FlareSmoke(Location, Canvas));
                return;
            }

            // Burn
            Burned += Canvas.Delta;
            // Ontop
            Pixel? above = Neighbors[0];
            Pixel? aboveLeft = Neighbors[7];
            Pixel? aboveRight = Neighbors[1];
            // Above
            if (above != null && above.Type == "Air")
            {
                AirPool.Return((Air)above);
                SetPixel(Canvas, above.GetIndex(), new FlareSmoke(above.Location, Canvas));
            }
            // Above left
            if (aboveLeft != null && aboveLeft.Type == "Air" && Canvas.ChancePerSecond(5))
            {
                AirPool.Return((Air)aboveLeft);
                SetPixel(Canvas, aboveLeft.GetIndex(), new FlareSmoke(aboveLeft.Location, Canvas));
            }
            // Above left
            if (aboveRight != null && aboveRight.Type == "Air" && Canvas.ChancePerSecond(5))
            {
                AirPool.Return((Air)aboveRight);
                SetPixel(Canvas, aboveRight.GetIndex(), new FlareSmoke(aboveRight.Location, Canvas));
            }
        }
    }
}
