using System;
using Consts = Pixxl.Constants;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    interface IExplosive
    {
        public int Explosion { get; set; }
        public int Range { get; set; }
        public bool ExplodeCheck();
        public void Explode();
    }
    public class Explosive : Pixel, IExplosive
    {
        // Constructor
        public int Explosion { get; set; }
        public int Range { get; set; }
        private bool exploded = false;
        private int rangeSq;
        public Explosive(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Explosion = 600;
            Range = 3;
            Conductivity = 1f;
            Density = 1.2f;
            State = 2;
            Strength = 50;
            Melting = new Transformation(Int32.MaxValue, typeof(Explosive));
            Solidifying = new Transformation(Int32.MinValue, typeof(Explosive));
        }

        public override void Update()
        {
            base.Update();

            if (ExplodeCheck()) { Explode(); }
        }
        public virtual bool ExplodeCheck()
        {
            if (Coords.Y == Consts.Screen.Grid[1] - 1 || Canvas.Pixels[Flat(Coords.X, Coords.Y + 1)].GetType().Name != "Air") { return true; }
            return false;
        }
        public virtual void Explode()
        {
            int roomTemp = Consts.Game.RoomTemp;

            if (exploded || Skip) { return; }
            exploded = true;

            rangeSq = Range * Range;

            // Get scan range
            int minX = Math.Max(0, (int)(Coords.X - Range));
            int maxX = Math.Min(Consts.Screen.Grid[0] - 1, (int)(Coords.X + Range));
            int minY = Math.Max(0, (int)(Coords.Y - Range));
            int maxY = Math.Min(Consts.Screen.Grid[1] - 1, (int)(Coords.Y + Range));

            // Iterate pixels
            for (int y = minY; y <= maxY; y++) // Rows
            {
                for (int x = minX; x <= maxX; x++)
                { // Columns
                    // Pixel data
                    Pixel current = Canvas.Pixels[Flat(x, y)];
                    int idx = current.GetIndex();
                    int dX = (int)(Coords.X - current.GetCoords().X);
                    int dY = (int)(Coords.Y - current.GetCoords().Y);
                    float distSq = dX * dX + dY * dY;

                    // Damage
                    if (distSq <= rangeSq)
                    {
                        // damage = x * (1 - d/r)
                        float damage = Explosion * (1f - (distSq / rangeSq));
                        if (damage >= current.Strength || current.Type == "Air")
                        {
                            if (current is IExplosive explosive)
                            {
                                explosive.Explode();
                            }
                            else
                            {
                                Fire repl = new(current.Location, Canvas);
                                repl.Temperature = Math.Max(damage * 2, roomTemp);
                                repl.Lifespan += (float)Canvas.Rand.NextDouble();
                                Canvas.Pixels[idx] = repl;
                                current.Skip = true;
                            }
                        }
                    }
                }
            }

            // Remove self
            Pixel self = AirPool.Get(Location, Canvas);
            self.Temperature = Explosion * 2;
            self.Skip = true;
            Canvas.Pixels[Index] = self;
            Skip = true;
        }
    }
}
