using Quantum;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KitchenDemo.Gameplay
{
    public class GameplaySceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private QuantumEntityViewUpdater _entityViewUpdater;
        [SerializeField] private CharacterAssets _characterAssets;
        [SerializeField] private CameraView _cameraView;

        private GameplayLifetimeScope _scope;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(_entityViewUpdater).As<QuantumEntityViewUpdater>();
            builder.RegisterInstance(_characterAssets).As<CharacterAssets>();
            builder.RegisterInstance(_cameraView).As<CameraView>();
        }

        protected override void Awake()
        {
            base.Awake();

            if (_scope != null)
            {
                return;
            }

            _scope = CreateChild<GameplayLifetimeScope>(builder =>
            {
                builder.RegisterEntryPoint<GameplayEventBridge>().AsSelf();
                builder.RegisterEntryPoint<GameplayContext>().AsSelf();
                builder.RegisterEntryPoint<InputService>().AsSelf();
                builder.RegisterEntryPoint<CharacterViewController>().AsSelf();
                builder.RegisterEntryPoint<CameraController>().AsSelf();
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_scope != null)
            {
                _scope.Dispose();
            }

            _scope = null;
        }
    }
}