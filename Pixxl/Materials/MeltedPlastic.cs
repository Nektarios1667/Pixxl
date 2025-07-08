using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class MeltedPlastic : Pixel
    {
        // Constructor
        public MeltedPlastic(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 550;
            Conductivity = .05f;
            Density = .96f;
            State = 3;
            Strength = 100;
            Melting = new Transformation(Int32.MaxValue, typeof(MeltedPlastic));
            Solidifying = new Transformation(250, typeof(Plastic));
        }
    }
}
