using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Snow : Pixel
    {
        // Constructor
        int tick = 0;
        public Snow(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = -40f;
            Conductivity = .04f;
            Density = 0.3f;
            State = 2;
            Strength = 20;
            Melting = new Transformation(32, typeof(Water));
            Solidifying = new Transformation(-250, typeof(Ice));
        }
        public override void Update()
        {
            // Reset
            UpdatePositions();
            GetNeighbors();

            // Heat transfer
            HeatTransfer();

            // For the possible moves including diagonals
            tick = (tick + 1) % 4;
            if (tick == 0) { Movements(); }

            // Spreading in the air
            Drift();
            UpdatePositions();

            // Check changes for melting, evaporating, plasmifying, deplasmifying, condensing, solidifying
            StateCheck();

            // Final
            Previous = Location;
            Array.Clear(Neighbors);
        }
        public virtual void Drift()
        {
            if ((Previous != Location && Canvas.Rand.Next(0, 20) == 0)) { FluidSpread(); }
        }
    }
}
