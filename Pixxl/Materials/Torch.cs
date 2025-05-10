using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Torch : Pixel
    {
        // Constructor
        public Torch(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .05f;
            Density = 2.8f;
            State = 0;
            Strength = 600;
            Melting = new Transformation(999999, typeof(Torch));
            Solidifying = new Transformation(-999999, typeof(Torch));
            Gravity = false;
        }

        public override void Update()
        {
            base.Update();

            // Water
            Pixel? above = Neighbors[0];
            if (above != null && above.Type == "Air")
            {
                above.Skip = true;
                AirPool.Return((Air)above);
                Canvas.Pixels[above.GetIndex()] = new Fire(above.Location, Canvas);
            }
        }
    }
}
