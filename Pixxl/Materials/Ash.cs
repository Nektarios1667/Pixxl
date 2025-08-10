using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Ash : Pixel
    {
        // Constructor
        int tick = 0;
        public Ash(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 200f;
            Conductivity = .14f;
            Density = 1.3f;
            State = 2;
            Strength = 30;
            Melting = new Transformation(2000, typeof(Steam));
            Solidifying = new Transformation(Int32.MinValue, typeof(Ash));
        }
        public override void Update()
        {
            // Reset
            UpdatePositions();
            GetNeighbors();

            // Heat transfer
            HeatTransfer();

            // For the possible moves including diagonals
            tick = (tick + 1) % 3;
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
            if (Previous != Location && Canvas.ChancePerSecond(3)) { FluidSpread(); }
        }
    }
}
