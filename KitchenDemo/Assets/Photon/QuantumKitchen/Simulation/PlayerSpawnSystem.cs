namespace Quantum.Kitchen
{
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerSpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var data = f.GetPlayerData(player);

            // Create a ship entity from the provided prototype or the default prototype from the RuntimeConfig
            var playerAvatarAssetRef =
                data.PlayerAvatar.IsValid ? data.PlayerAvatar : f.RuntimeConfig.DefaultPlayerAvatar;
            var playerPrototypeAsset = f.FindAsset(playerAvatarAssetRef);
            var playerEntity = f.Create(playerPrototypeAsset);

            // Set player link component to mark this entity as player controller
            f.Set(playerEntity, new PlayerLink { PlayerRef = player });
            f.Set(playerEntity, new Character());

            // Spawn the ship
            f.Signals.PlayerSpawned(playerEntity);
            f.Events.EntityInstantiated(playerEntity);
        }
    }
}