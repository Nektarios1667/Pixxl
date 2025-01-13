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
            if (Coords.Y == Consts.Screen.Grid[1] - 1 || Canvas.Pixels[Flat(Coords.X, Coords.Y + 1)].GetType().Name != "Air") { Explode(); }
        }

        public void Explode()
        {
            // Iterate pixels
            for (int y = 0; y < Consts.Screen.Grid[1]; y++)  // Loop through rows
            {
                for (int x = 0; x < Consts.Screen.Grid[0]; x++)  // Loop through columns
                {
                    // Pixel data
                    Pixel current = Canvas.Pixels[Flat(x, y)];
                    int dX = (int)(Coords.X - current.Coords.X);
                    int dY = (int)(Coords.Y - current.Coords.Y);
                    int dist = Math.Abs(dX) + Math.Abs(dY);

                    // Damage
                    if (dist <= Range)  
                    {
                        //    damage = -dxr^-1 + d where d = Damage, x = Distance, r = Range
                        float damage = (float)(Explosion * dist * (1 / Range) + Explosion);
                        if (damage >= current.Strength)
                        {
                            Pixel repl = new Air(current.Location, Canvas);
                            repl.Temperature = current.Temperature + damage / 2; repl.Velocity = current.Velocity;
                            Canvas.Pixels[Flat(current.Coords)] = repl;
                        } else if (current.GetType().Name == "Air" && Canvas.Rand.Next(0, (int)dist / Range) == 0)
                        {
                            Pixel repl = new Fire(current.Location, Canvas);
                            repl.Temperature = current.Temperature + damage / 2; repl.Velocity = current.Velocity;
                            Canvas.Pixels[Flat(current.Coords)] = repl;
                        }
                    }
                }
            }

            // Remove self
            Pixel self = new Air(Location, Canvas);
            self.Temperature = Temperature; self.Velocity = Velocity;
            Canvas.Pixels[Flat(Coords)] = self;
        }
    }
}
