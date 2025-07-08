using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Air : Pixel
    {
        // Constructor
        public Air(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .025f;
            Density = .0012f;
            State = 3;
            Strength = Int32.MaxValue;
            Melting = new Transformation(9500, typeof(Plasma));
            Solidifying = new Transformation(Int32.MinValue, typeof(Air));
        }
    }
}
