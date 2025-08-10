using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Torch : Pixel
    {
        // Constructor
        public Torch(Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .05f;
            Density = 2.8f;
            State = 0;
            Strength = 600;
            Melting = new Transformation(Int32.MaxValue, typeof(Torch));
            Solidifying = new Transformation(Int32.MinValue, typeof(Torch));
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
                SetPixel(Canvas, above.GetIndex(), new Fire(above.Location, Canvas));
            }
        }
    }
}
