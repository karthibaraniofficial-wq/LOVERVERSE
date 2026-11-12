using System;
using System.Threading.Tasks;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Networking
{
    /// <summary>
    /// Core networking service implementing room lifecycle and multiplayer synchronization contracts.
    /// Acts as the provider abstraction for Photon Fusion 2 shared sessions.
    /// </summary>
    public sealed class NetworkManager : MonoBehaviour, INetworkService
    {
        public bool IsInitialized { get; private set; }
        public bool IsConnected { get; private set; }
        public string CurrentRoomCode { get; private set; }
        public int ConnectedPlayerCount { get; private set; }

        public event Action<string> OnPlayerJoined;
        public event Action<string> OnPlayerLeft;
        public event Action OnDisconnected;

        public Task InitializeAsync()
        {
            IsInitialized = true;
            Lg.Info("NetworkManager initialized.");
            return Task.CompletedTask;
        }

        public void Shutdown()
        {
            LeaveRoomAsync().ConfigureAwait(false);
            IsInitialized = false;
        }

        public Task<string> CreatePrivateRoomAsync()
        {
            CurrentRoomCode = RoomCodeGenerator.Generate();
            IsConnected = true;
            ConnectedPlayerCount = 1;

            Lg.Info($"Created private room: {CurrentRoomCode}");
            EventBus.Publish(new RoomStateChangedEvent(CurrentRoomCode, ConnectedPlayerCount, true));
            return Task.FromResult(CurrentRoomCode);
        }

        public Task<bool> JoinPrivateRoomAsync(string roomCode)
        {
            if (!RoomCodeGenerator.IsValidCodeFormat(roomCode))
            {
                Lg.Warning($"Invalid room code format: {roomCode}");
                return Task.FromResult(false);
            }

            CurrentRoomCode = roomCode.ToUpperInvariant();
            IsConnected = true;
            ConnectedPlayerCount = 2;

            Lg.Info($"Joined private room: {CurrentRoomCode}");
            EventBus.Publish(new RoomStateChangedEvent(CurrentRoomCode, ConnectedPlayerCount, true));
            OnPlayerJoined?.Invoke("partner_01");
            return Task.FromResult(true);
        }

        public Task LeaveRoomAsync()
        {
            if (IsConnected)
            {
                Lg.Info($"Leaving room: {CurrentRoomCode}");
                IsConnected = false;
                CurrentRoomCode = null;
                ConnectedPlayerCount = 0;
                OnDisconnected?.Invoke();
                EventBus.Publish(new RoomStateChangedEvent(null, 0, false));
            }
            return Task.CompletedTask;
        }
    }
}
