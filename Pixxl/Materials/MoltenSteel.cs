using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class MoltenSteel : Pixel
    {
        // Constructor
        public MoltenSteel(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 4800;
            Conductivity = 3.7f;
            Density = 7f;
            State = 3;
            Strength = 450;
            Melting = new Transformation(999999, typeof(MoltenSteel));
            Solidifying = new Transformation(3200, typeof(Steel));
        }
    }
}
