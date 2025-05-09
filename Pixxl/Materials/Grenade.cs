using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Grenade : Explosive
    {
        // Constructor
        private float Fuse { get; set; }
        public Grenade(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Explosion = 300;
            Range = 5;
            Conductivity = 1f;
            Density = 1.2f;
            State = 2;
            Strength = 50;
            Melting = new Transformation(999999, typeof(Explosive));
            Solidifying = new Transformation(-999999, typeof(Explosive));
            Fuse = 5f;
        }
        public override void Update()
        {
            base.Update();

            // Timer
            Fuse -= Canvas.Delta;

            // Explode
            if (ExplodeCheck()) { Explode(); }
        }
        public override bool ExplodeCheck()
        {
            if (Fuse <= 0) { return true; }
            return false;
        }

    }
}
