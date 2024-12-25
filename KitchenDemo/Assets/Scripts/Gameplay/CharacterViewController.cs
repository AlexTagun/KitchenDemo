using System;
using System.Collections.Generic;
using Quantum;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace KitchenDemo.Gameplay
{
    public unsafe class CharacterViewController : IInitializable, IDisposable, ILateTickable
    {
        private readonly Dictionary<EntityRef, CharacterView> _views = new();
        private readonly GameplayEventBridge _gameplayEventBridge;
        private readonly QuantumEntityViewUpdater _entityViewUpdater;
        private readonly CharacterAssets _characterAssets;
        private readonly GameplayContext _gameplayContext;

        [Inject]
        public CharacterViewController(GameplayEventBridge gameplayEventBridge,
            QuantumEntityViewUpdater entityViewUpdater, CharacterAssets characterAssets,
            GameplayContext gameplayContext)
        {
            _gameplayEventBridge = gameplayEventBridge;
            _entityViewUpdater = entityViewUpdater;
            _characterAssets = characterAssets;
            _gameplayContext = gameplayContext;
        }

        void IInitializable.Initialize()
        {
            _gameplayEventBridge.EntityInstantiated += GameplayEventBridge_EntityInstantiated;
            // _gameplayEventBridge.EntityDestroyed += GameplayEventBridge_EntityDestroyed;
        }

        void IDisposable.Dispose()
        {
            _gameplayEventBridge.EntityInstantiated -= GameplayEventBridge_EntityInstantiated;
            // _gameplayEventBridge.EntityDestroyed -= GameplayEventBridge_EntityDestroyed;
        }

        void ILateTickable.LateTick()
        {
            var frame = _gameplayContext.PredictedFrame;
            
            if (frame == null)
            {
                return;
            }

            foreach (var (entity, view) in _views)
            {
                float movementSpeed = 0;
                if (frame.Unsafe.TryGetPointer(entity, out Movement* movement))
                {
                    movementSpeed = movement->Speed.AsFloat;
                }

                view.SetAnimatorParameters(movementSpeed);
            }
        }

        private void GameplayEventBridge_EntityInstantiated(EventEntityInstantiated data)
        {
            var game = data.Game;
            var frame = game?.Frames?.Predicted;
            if (frame == null)
            {
                return;
            }

            var entity = data.Entity;

            if (_views.ContainsKey(entity) || !frame.TryGet<Character>(entity, out var character))
            {
                return;
            }

            var entityView = _entityViewUpdater.GetView(entity);
            if (entityView == null)
            {
                throw new Exception($"Not supported character without {nameof(EntityView)}");
            }

            var characterView = Object.Instantiate(_characterAssets.CharacterView, entityView.transform);
            _views.Add(entity, characterView);
        }
    }
}