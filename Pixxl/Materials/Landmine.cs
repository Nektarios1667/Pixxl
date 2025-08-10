using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Landmine : Explosive
    {
        public Landmine(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Explosion = 200;
            Range = 3;
            Conductivity = 1f;
            Density = 1.2f;
            State = 2;
            Strength = 50;
            Melting = new Transformation(Int32.MaxValue, typeof(Landmine));
            Solidifying = new Transformation(Int32.MinValue, typeof(Landmine));
        }
        public override void Update()
        {
            base.Update();

            // Explode
            if (ExplodeCheck()) { Explode(); }
        }
        public override bool ExplodeCheck()
        {
            return Neighbors[0]?.Type != "Air";
        }

    }
}
