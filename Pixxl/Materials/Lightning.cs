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
            Density = Int32.MaxValue;
            State = 4;
            Strength = Int32.MaxValue;
            Melting = new Transformation(Int32.MaxValue, typeof(Lightning));
            Solidifying = new Transformation(Int32.MinValue, typeof(Lightning));
            life = 0;
        }

        public override void Update()
        {
            // Life
            if (life >= .1 || Canvas.Rand.Next(0, 25) == 0)
            {
                SetPixel(Canvas, Index, Canvas.Rand.Next(0, 4) == 0 ? new Plasma(Location, Canvas) : AirPool.Get(Location, Canvas));
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
                    if (next.State >= 3)
                    {
                        Pixel created = new Lightning(next.Location, Canvas);
                        SetPixel(Canvas, next.GetIndex(), created);
                        created.Update();
                    }
                    else
                    {
                        SetPixel(Canvas, Index, new Explosive(Location, Canvas));
                        return;
                    }
                }
                else
                {
                    SetPixel(Canvas, Index, new Explosive(Location, Canvas));
                    return;
                }
            }
            life += Canvas.Delta;
        }
    }
}
