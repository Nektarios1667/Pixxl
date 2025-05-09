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
            Melting = new Transformation(999999, typeof(Insulation));
            Solidifying = new Transformation(-999999, typeof(Insulation));
            Gravity = false;
        }
    }
}
