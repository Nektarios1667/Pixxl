using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Copper : Pixel
    {
        // Constructor
        public Copper(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 60f;
            Density = 9f;
            State = 0;
            Strength = 200;
            Melting = new Transformation(2500, typeof(MoltenCopper));
            Solidifying = new Transformation(Int32.MinValue, typeof(Copper));
            Gravity = false;
        }
    }
}
