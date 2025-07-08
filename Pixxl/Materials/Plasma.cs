using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Plasma : Pixel
    {
        // Constructor
        public Plasma(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 11000f;
            Conductivity = .3f;
            Density = .00018f;
            State = 4;
            Strength = Int32.MaxValue;
            Melting = new Transformation(Int32.MaxValue, typeof(Plasma));
            Solidifying = new Transformation(9500, typeof(Air));
        }

        public override void Update()
        {
            base.Update();

            // Naturally cool down
            Temperature -= Canvas.Delta * 125;
        }
    }
}
