using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Plastic : Pixel
    {
        // Constructor
        public Plastic(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .15f;
            Density = .96f;
            State = 0;
            Strength = 100;
            Melting = new Transformation(290, typeof(MeltedPlastic));
            Solidifying = new Transformation(-999999, typeof(Plastic));
            Gravity = false;
        }
    }
}
