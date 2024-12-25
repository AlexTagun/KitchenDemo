using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.Kitchen
{
    [Preserve]
    public unsafe class PlayerInputSystem : SystemMainThreadFilter<PlayerInputSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerLink->PlayerRef);

            var movement = new Movement();

            if (input->MovementDirection == default)
            {
                movement.Direction = FPVector3.Zero;
                movement.Speed = FP._0;
            }
            else
            {
                var direction = new FPVector3(input->MovementDirection.X, 0, input->MovementDirection.Y);
                movement.Direction = direction;
                movement.Speed = FP._7;
            }

            f.Set(filter.Entity, movement);
        }
    }
}