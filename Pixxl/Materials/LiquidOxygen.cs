using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class LiquidOxygen : Pixel
    {
        // Constructor
        public LiquidOxygen(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = -200;
            Conductivity = .152f;
            Density = 1.14f;
            State = 3;
            Strength = 60;
            Melting = new Transformation(-183, typeof(Oxygen));
            Solidifying = new Transformation(-218, typeof(SolidOxygen));
        }
    }
}
