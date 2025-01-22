using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Consts = Pixxl.Constants;

namespace Pixxl.Materials
{
    public class Explosive : Pixel
    {
        // Constructor
        public int Explosion { get; set; }
        public int Range { get; set; }
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
            Xna.Vector2 coords = Coords;
            // Iterate pixels
            for (int y = 0; y < Consts.Screen.Grid[1]; y++)  // Loop through rows
            {
                for (int x = 0; x < Consts.Screen.Grid[0]; x++)  // Loop through columns
                {
                    // Pre checks
                    if (Math.Abs(coords.X - x) > Range || Math.Abs(coords.Y - y) > Range) { continue; }

                    // Pixel data
                    Pixel current = Canvas.Pixels[Flat(x, y)];
                    int idx = current.Index;
                    int dX = (int)(coords.X - current.Coords.X);
                    int dY = (int)(coords.Y - current.Coords.Y);
                    float dist = (float)Math.Sqrt(dX*dX + dY*dY);

                    // Damage
                    if (dist <= Range)  
                    {
                        // damage = -dxr^-1 + d where d = Damage, x = Distance, r = Range
                        float damage = (Explosion / Range) * (Range - dist);
                        if (damage >= current.Strength || (current.GetType().Name == "Air" && Canvas.Rand.Next(0, (int)dist / Range) == 0))
                        {
                            Fire repl = new Fire(current.Location, Canvas);
                            repl.Temperature = damage * 2;
                            repl.Lifespan -= Canvas.Rand.NextSingle();
                            current.Skip = true;
                            Canvas.Pixels[current.Index] = repl;
                        }
                    }
                }
            }

            // Remove self
            Pixel self = new Air(Location, Canvas);
            self.Temperature = Temperature;
            Canvas.Pixels[Index] = self;
        }
    }
}
