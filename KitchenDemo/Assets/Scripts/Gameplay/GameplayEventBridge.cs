using System;
using Quantum;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace KitchenDemo.Gameplay
{
    public class GameplayEventBridge : IInitializable, IDisposable
    {
        public event Action<EventEntityInstantiated> EntityInstantiated = delegate { };
        public event Action<EventEntityDestroyed> EntityDestroyed = delegate { };

        private readonly LifetimeScope _parentScope;

        private GameObject _listener;

        [Inject]
        public GameplayEventBridge(LifetimeScope parentScope) => _parentScope = parentScope;

        void IInitializable.Initialize()
        {
            _listener = new GameObject("GameplayEventBridge_Listener");
            _listener.transform.SetParent(_parentScope.transform);

            QuantumEvent.Subscribe<EventEntityInstantiated>(_listener, QuantumEvent_EntityInstantiated);
            QuantumEvent.Subscribe<EventEntityDestroyed>(_listener, QuantumEvent_EntityDestroyed);
        }

        void IDisposable.Dispose()
        {
            QuantumEvent.UnsubscribeListener(_listener);
            Object.Destroy(_listener);
        }

        private void QuantumEvent_EntityInstantiated(EventEntityInstantiated data) => EntityInstantiated(data);
        private void QuantumEvent_EntityDestroyed(EventEntityDestroyed data) => EntityDestroyed(data);
    }
}