using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Void : Pixel
    {
        // Constructor
        public Void(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 0f;
            Density = 1000f;
            State = 0;
            Strength = Int32.MaxValue;
            Melting = new Transformation(Int32.MaxValue, typeof(Void));
            Solidifying = new Transformation(Int32.MinValue, typeof(Void));
            Gravity = false;
        }

        public override void Update()
        {
            base.Update();

            // Destrory
            foreach (Pixel? neighbor in Neighbors)
            {
                if (neighbor != null && neighbor.Type != "Air" && neighbor.Type != "Void")
                {
                    SetPixel(Canvas, Index, AirPool.Get(neighbor.Location, Canvas));
                }
            }
        }
    }
}
