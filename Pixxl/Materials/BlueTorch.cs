using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class BlueTorch : Pixel
    {
        // Constructor
        public BlueTorch(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .05f;
            Density = 2.8f;
            State = 0;
            Strength = 600;
            Melting = new Transformation(Int32.MaxValue, typeof(Faucet));
            Solidifying = new Transformation(Int32.MinValue, typeof(Faucet));
            Gravity = false;
        }

        public override void Update()
        {
            base.Update();

            // Water
            Pixel? above = Neighbors[0];
            if (above != null && above.Type == "Air")
            {
                AirPool.Return((Air)above);
                SetPixel(Canvas, above.Index, new BlueFire(above.Location, Canvas));
            }
        }
    }
}
