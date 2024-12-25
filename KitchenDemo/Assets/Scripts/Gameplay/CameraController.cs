using System;
using Quantum;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KitchenDemo.Gameplay
{
    public class CameraController : IInitializable, IDisposable, ILateTickable
    {
        private readonly CameraView _cameraView;
        private readonly GameplayEventBridge _gameplayEventBridge;
        private readonly GameplayContext _gameplayContext;
        private readonly QuantumEntityViewUpdater _entityViewUpdater;

        private Transform _target;

        [Inject]
        public CameraController(CameraView cameraView, GameplayEventBridge gameplayEventBridge,
            GameplayContext gameplayContext, QuantumEntityViewUpdater entityViewUpdater)
        {
            _cameraView = cameraView;
            _gameplayEventBridge = gameplayEventBridge;
            _gameplayContext = gameplayContext;
            _entityViewUpdater = entityViewUpdater;
        }

        void IInitializable.Initialize()
        {
            _gameplayEventBridge.EntityInstantiated += OnEntityInstantiated;
            _gameplayEventBridge.EntityDestroyed += OnEntityDestroyed;
        }

        void IDisposable.Dispose()
        {
            _gameplayEventBridge.EntityInstantiated -= OnEntityInstantiated;
            _gameplayEventBridge.EntityDestroyed -= OnEntityDestroyed;
        }

        void ILateTickable.LateTick()
        {
            if (_target is null)
            {
                return;
            }

            _cameraView.AnchorPosition = _target.position;
        }

        private void OnEntityInstantiated(EventEntityInstantiated data)
        {
            if (_gameplayContext.IsLocalPlayer(data.Entity))
            {
                _target = _entityViewUpdater.GetView(data.Entity).Transform;
            }
        }

        private void OnEntityDestroyed(EventEntityDestroyed data)
        {
            if (_gameplayContext.IsLocalPlayer(data.Entity))
            {
                _target = null;
            }
        }
    }
}