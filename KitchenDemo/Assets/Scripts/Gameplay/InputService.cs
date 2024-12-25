using Photon.Deterministic;
using Quantum;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Input = UnityEngine.Input;

namespace KitchenDemo.Gameplay
{
    public class InputService : IInitializable
    {
        private readonly LifetimeScope _parentScope;

        private GameObject _listener;

        [Inject]
        public InputService(LifetimeScope parentScope)
        {
            _parentScope = parentScope;
        }

        void IInitializable.Initialize()
        {
            _listener = new GameObject($"{nameof(InputService)}_Listener");
            _listener.transform.SetParent(_parentScope.transform);

            QuantumCallback.Subscribe(_listener, (CallbackPollInput callback) => PollInput(callback));
        }

        private void PollInput(CallbackPollInput callback)
        {
            var movement = FPVector2.Zero;
            if (Input.GetKey(KeyCode.W))
            {
                movement += FPVector2.Up;
            }

            if (Input.GetKey(KeyCode.A))
            {
                movement -= FPVector2.Right;
            }

            if (Input.GetKey(KeyCode.S))
            {
                movement -= FPVector2.Up;
            }

            if (Input.GetKey(KeyCode.D))
            {
                movement += FPVector2.Right;
            }

            var i = new Quantum.Input
            {
                MovementDirection = movement
            };
            callback.SetInput(i, DeterministicInputFlags.Repeatable);
        }
    }
}