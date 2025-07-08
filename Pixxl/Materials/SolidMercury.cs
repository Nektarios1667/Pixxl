using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class SolidMercury : Pixel
    {
        // Constructor
        public SolidMercury(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = -175;
            Conductivity = 2.45f;
            Density = 14.2f;
            State = 3;
            Strength = 340;
            Melting = new Transformation(-30, typeof(Mercury));
            Solidifying = new Transformation(Int32.MinValue, typeof(SolidMercury));
        }
    }
}
