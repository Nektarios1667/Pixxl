using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Acid : Pixel
    {
        // Constructor
        public Acid(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .02f;
            Density = 1.2f;
            State = 3;
            Strength = Int32.MaxValue;
            Melting = new Transformation(8200, typeof(Plasma));
            Solidifying = new Transformation(Int32.MinValue, typeof(Acid));
        }
        public override void Update()
        {
            base.Update();

            // Burning
            Pixel? down = Neighbors[4];
            if (down != null && down.Type != "Air" && down.Strength < 500 && Canvas.ChancePerSecond(50f / down.Strength))
            {
                down.Skip = true;
                Canvas.Pixels[down.GetIndex()] = AirPool.Get(down.Location, Canvas);
            }
        }
    }
}
