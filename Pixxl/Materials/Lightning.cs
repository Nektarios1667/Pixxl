using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Lightning : Pixel
    {
        // Constructor
        float life;
        public Lightning(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 9000f;
            Conductivity = 8f;
            Density = 999999f;
            State = 4;
            Strength = 999999;
            Melting = new Transformation(999999, typeof(Lightning));
            Solidifying = new Transformation(-999999, typeof(Lightning));
            life = 0;
        }

        public override void Update()
        {
            // Skip
            if (Skip) { Skip = false; return; }

            // Life
            if (life >= .1 || Canvas.Rand.Next(0, 25) == 0)
            {
                Canvas.Pixels[Index] = Canvas.Rand.Next(0, 4) == 0 ? new Plasma(Location, Canvas) : AirPool.Get(Location, Canvas);
                return;
            }

            // Reset
            UpdatePositions();
            GetNeighbors();

            // Heat transfer
            HeatTransfer();

            // "Moving" down
            if (life == 0)
            {
                Pixel? next = Neighbors[4 + Canvas.Rand.Next(-1, 2)];
                if (next != null)
                {
                    next.Skip = true;
                    if (next.State >= 3)
                    {
                        Canvas.Pixels[next.GetIndex()] = new Lightning(next.Location, Canvas);
                        Canvas.Pixels[next.GetIndex()].Update();
                    }
                    else
                    {
                        Canvas.Pixels[Index] = new Explosive(Location, Canvas);
                        return;
                    }
                }
                else
                {
                    Canvas.Pixels[Index] = new Explosive(Location, Canvas);
                    return;
                }
            }
            life += Canvas.Delta;
        }
    }
}
