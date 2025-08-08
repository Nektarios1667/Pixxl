using Consts = Pixxl.Constants;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Storm : Pixel
    {
        // Constructor
        public Storm(Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .4f;
            Density = .0016f;
            State = 3;
            Strength = 90;
            Melting = new Transformation(Int32.MaxValue, typeof(Storm));
            Solidifying = new Transformation(Int32.MinValue, typeof(Storm));
        }
        public override void Update()
        {
            base.Update();

            // Rain and lightning
            Pixel? below = Neighbors[4];
            if (below != null && below.Type == "Air" && Canvas.Rand.Next(0, 4000) == 0)
            {
                AirPool.Return((Air)below);
                SetPixel(Canvas, Index, new Lightning(below.Location, Canvas));
            }
        }
        public override bool Movements() { return false; }
        public override void FluidSpread()
        {
            if (Canvas.Rand.Next(0, 20) != 0) { return; }

            int side = Canvas.Rand.Next(0, 2) == 0 ? -Consts.Screen.PixelSize : Consts.Screen.PixelSize;
            Xna.Vector2 next = new(Location.X + side, Location.Y);
            Pixel? target = Find(next.ToPoint(), 'l');
            if (target != null && (target.Type == "Air" || target.Type == Type))
            {
                SwapTo(next);
                UpdatePositions();
            }
        }
    }
}
