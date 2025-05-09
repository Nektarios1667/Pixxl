using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Smoke : Pixel
    {
        // Constructor
        public Smoke(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 800f;
            Conductivity = .03f;
            Density = .0003f;
            State = 3;
            Strength = 100;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(400, typeof(Air));
        }
    }
}
