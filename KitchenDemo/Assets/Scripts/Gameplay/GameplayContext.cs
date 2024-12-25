using Photon.Deterministic;
using Quantum;

namespace KitchenDemo.Gameplay
{
    public unsafe class GameplayContext
    {
        public QuantumGame QuantumGame => QuantumRunner.Default != null ? QuantumRunner.Default.Game : null;

        public Frame VerifiedFrame => QuantumGame?.Frames.Verified;
        public Frame PredictedFrame => QuantumGame?.Frames.Predicted;
        public DeterministicSession Session => QuantumGame?.Session;

        public bool IsLocalPlayer(EntityRef entityRef) 
            => PredictedFrame.Unsafe.TryGetPointer(entityRef, out PlayerLink* link) &&
               QuantumGame.PlayerIsLocal(link->PlayerRef);
    }
}