using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Chiller : Pixel
    {
        // Constructor
        public Chiller(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = -200f;
            Conductivity = 100f;
            Density = 10f;
            State = 0;
            Gravity = false;
            Strength = Int32.MaxValue;
            Melting = new Transformation(Int32.MaxValue, typeof(Chiller));
            Solidifying = new Transformation(Int32.MinValue, typeof(Chiller));
        }
        public override void Update()
        {
            base.Update();

            Temperature = -200f;
        }

    }
}
