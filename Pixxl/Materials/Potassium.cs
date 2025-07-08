using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Potassium : Explosive
    {
        // Constructor
        public Potassium(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Explosion = 120;
            Range = 3;
            Conductivity = 10f;
            Density = .862f;
            State = 2;
            Strength = 125;
            Melting = new Transformation(Int32.MaxValue, typeof(Potassium));
            Solidifying = new Transformation(Int32.MinValue, typeof(Potassium));
        }

        public override bool ExplodeCheck()
        {
            // Touching water
            foreach (Pixel? neighbor in Neighbors)
            {
                if (neighbor != null && neighbor.Type == "Water")
                {
                    neighbor.Skip = true;
                    Canvas.Pixels[neighbor.GetIndex()] = new Fire(neighbor.Location, Canvas);
                    return true;
                }
            }
            return false;
        }
    }
}
