using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.Kitchen
{
    [Preserve]
    public unsafe class PlayerMovementSystem : SystemMainThreadFilter<PlayerMovementSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Movement* Movement;
            public Transform3D* Transform;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            if (filter.Movement->Speed <= 0)
            {
                return;
            }

            var direction = f.DeltaTime * filter.Movement->Speed * filter.Movement->Direction;
            filter.Transform->Position += direction;
            filter.Transform->Rotation = FPQuaternion.LookRotation(direction);
        }
    }
}