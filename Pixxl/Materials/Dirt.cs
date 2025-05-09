using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Dirt : Pixel
    {
        // Constructor
        public Dirt(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .1f;
            Density = 1.3f;
            State = 2;
            Strength = 50;
            Melting = new Transformation(3200, typeof(Lava));
            Solidifying = new Transformation(-999999, typeof(Dirt));
        }
    }
}
