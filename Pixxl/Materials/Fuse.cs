using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Fuse : Fuel
    {
        // Constructor
        public Fuse(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Ashes = false;
            Internal = true;
            Lifetime = .5f;
            Conductivity = .05f;
            Density = .8f;
            State = 0;
            Strength = 50;
            Melting = new Transformation(1000, typeof(BlueFire));
            Solidifying = new Transformation(Int32.MinValue, typeof(Fuse));
            Gravity = false;
        }

        // Update
        public override void Update()
        {
            base.Update();

            // Burned out
            if (Burned >= Lifetime)
            {
                Pixel creation = new Fire(Location, Canvas);
                Canvas.Pixels[Index] = creation;

                // Ignite next fuse
                foreach (Pixel? neighbor in Neighbors)
                {
                    if (neighbor != null && neighbor.Type == "Fuse" && neighbor is IIgnitable ignitable)
                    {
                        ignitable.Ignite();
                    }

                }

                return;
            }

            // Burn
            if (Lit)
            {
                Burned += Canvas.Delta;
                // Neighbors
                foreach (Pixel? neighbor in Neighbors)
                {
                    if (neighbor == null) { continue; }
                    if (neighbor.Type == "Air")
                    {
                        neighbor.Skip = true;
                        AirPool.Return((Air)neighbor);
                        Canvas.Pixels[neighbor.GetIndex()] = new Fire(neighbor.Location, Canvas);
                    }
                    else if (Canvas.ChancePerSecond(3) && neighbor is IIgnitable ignitable && neighbor.Type != "Fuse")
                    {
                        ignitable.Ignite();
                    }
                }
            }
        }
    }
}
