using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Gold : Pixel
    {
        // Constructor
        public Gold(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 318f;
            Density = 19.32f;
            State = 2;
            Strength = 100;
            Melting = new Transformation(1950, typeof(MoltenGold));
            Solidifying = new Transformation(Int32.MinValue, typeof(Gold));
        }
    }
}
