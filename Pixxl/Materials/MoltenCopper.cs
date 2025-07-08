using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class MoltenCopper : Pixel
    {
        // Constructor
        public MoltenCopper(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 2800;
            Conductivity = 5f;
            Density = 6.1f;
            State = 3;
            Strength = 150;
            Melting = new Transformation(Int32.MaxValue, typeof(MoltenCopper));
            Solidifying = new Transformation(2000, typeof(Copper));
        }
    }
}
