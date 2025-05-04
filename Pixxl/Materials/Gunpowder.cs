using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Gunpowder : Explosive, IIgnitable
    {
        // Constructor
        public Gunpowder(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Explosion = 400;
            Range = 2;
            Conductivity = .14f;
            Density = 1.4f;
            State = 2;
            Strength = 40;
            Melting = new Transformation(3100, typeof(Air));
            Solidifying = new Transformation(-999999, typeof(Gunpowder));
        }
        public override bool ExplodeCheck()
        {
            foreach (Pixel? neighbor in Neighbors)
            {
                if (neighbor is IBurnable burnable && burnable.Lit)
                {
                    return true;
                }
            }
            return false;
        }

        public void Ignite()
        {
            Explode();
        }

        public void Snuff() {}
    }
}
