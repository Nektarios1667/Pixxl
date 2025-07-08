using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Helium : Pixel
    {
        // Constructor
        public Helium(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .142f;
            Density = .00018f;
            State = 3;
            Strength = Int32.MaxValue;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(Int32.MinValue, typeof(Helium));
        }
    }
}
