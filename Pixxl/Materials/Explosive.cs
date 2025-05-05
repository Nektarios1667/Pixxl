using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Consts = Pixxl.Constants;
using System.Text;

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
            Melting = new Transformation(999999, typeof(Explosive));
            Solidifying = new Transformation(-999999, typeof(Explosive));
        }

        public override void Update()
        {
            base.Update();
            
            if (ExplodeCheck()) { Explode(); }
        }
        public virtual bool ExplodeCheck()
        {
            Xna.Vector2 coords = Coords;
            if (coords.Y == Consts.Screen.Grid[1] - 1 || Canvas.Pixels[Flat(coords.X, coords.Y + 1)].GetType().Name != "Air") { return true; }
            return false;
        }
        public virtual void Explode()
        {
            if (exploded || Skip) { return; }
            exploded = true;

            Xna.Vector2 coords = Coords;
            rangeSq = Range * Range;

            // Get scan range
            int minX = Math.Max(0, (int)(coords.X - Range));
            int maxX = Math.Min(Consts.Screen.Grid[0] - 1, (int)(coords.X + Range));
            int minY = Math.Max(0, (int)(coords.Y - Range));
            int maxY = Math.Min(Consts.Screen.Grid[1] - 1, (int)(coords.Y + Range));

            // Iterate pixels
            for (int y = minY; y <= maxY; y++) // Rows
            {
                for (int x = minX; x <= maxX; x++) { // Columns
                    // Pixel data
                    Pixel current = Canvas.Pixels[Flat(x, y)];
                    int idx = current.GetIndex();
                    int dX = (int)(coords.X - current.GetCoords().X);
                    int dY = (int)(coords.Y - current.GetCoords().Y);
                    float distSq = dX*dX + dY*dY;

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
                                repl.Temperature = (damage * 2);
                                repl.Lifespan += (float)Canvas.Rand.NextDouble();
                                Canvas.Pixels[idx] = repl;
                                current.Skip = true;
                            }
                        }
                    }
                }
            }

            // Remove self
            Pixel self = new Air(Location, Canvas);
            self.Temperature = Explosion * 2;
            self.Skip = true;
            Canvas.Pixels[Index] = self;
            Skip = true;
        }
    }
}
