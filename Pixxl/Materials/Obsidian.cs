using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Obsidian : Pixel
    {
        // Constructor
        public Obsidian(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .05f;
            Density = 2.6f;
            State = 0;
            Strength = 900;
            Melting = new Transformation(2200, typeof(Lava));
            Solidifying = new Transformation(Int32.MinValue, typeof(Concrete));
            Gravity = false;
        }
    }
}
