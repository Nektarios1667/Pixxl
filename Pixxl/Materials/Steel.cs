using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Steel : Pixel
    {
        // Constructor
        public Steel(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 15f;
            Density = 7.9f;
            State = 0;
            Strength = 700;
            Melting = new Transformation(3800, typeof(MoltenSteel));
            Solidifying = new Transformation(-999999, typeof(Steel));
            Gravity = false;
        }
    }
}
