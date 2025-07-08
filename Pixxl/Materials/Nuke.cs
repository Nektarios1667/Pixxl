using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Nuke : Explosive
    {
        // Constructor
        public Nuke(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Explosion = 32000;
            Range = 25;
            Conductivity = 1f;
            Density = 1.2f;
            State = 2;
            Strength = 50;
            Melting = new Transformation(Int32.MaxValue, typeof(Explosive));
            Solidifying = new Transformation(Int32.MinValue, typeof(Explosive));
        }
    }
}
