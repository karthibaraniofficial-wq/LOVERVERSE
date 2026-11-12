using UnityEngine;

namespace Loveverse.Core
{
    public readonly struct PlayerSpawnedEvent
    {
        public readonly string PlayerId;
        public readonly bool IsLocalPlayer;
        public readonly Vector3 SpawnPosition;

        public PlayerSpawnedEvent(string playerId, bool isLocalPlayer, Vector3 spawnPosition)
        {
            PlayerId = playerId;
            IsLocalPlayer = isLocalPlayer;
            SpawnPosition = spawnPosition;
        }
    }

    public readonly struct SceneLoadedEvent
    {
        public readonly string SceneName;

        public SceneLoadedEvent(string sceneName)
        {
            SceneName = sceneName;
        }
    }

    public readonly struct RomanceInteractionTriggeredEvent
    {
        public readonly string InitiatorId;
        public readonly string TargetId;
        public readonly string InteractionId;

        public RomanceInteractionTriggeredEvent(string initiatorId, string targetId, string interactionId)
        {
            InitiatorId = initiatorId;
            TargetId = targetId;
            InteractionId = interactionId;
        }
    }

    public readonly struct RoomStateChangedEvent
    {
        public readonly string RoomCode;
        public readonly int PlayerCount;
        public readonly bool IsConnected;

        public RoomStateChangedEvent(string roomCode, int playerCount, bool isConnected)
        {
            RoomCode = roomCode;
            PlayerCount = playerCount;
            IsConnected = isConnected;
        }
    }
}
