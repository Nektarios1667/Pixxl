using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Insulation : Pixel
    {
        // Constructor
        public Insulation(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .001f;
            Density = .8f;
            State = 1;
            Strength = 50;
            Melting = new Transformation(Int32.MaxValue, typeof(Insulation));
            Solidifying = new Transformation(Int32.MinValue, typeof(Insulation));
            Gravity = false;
        }
    }
}
