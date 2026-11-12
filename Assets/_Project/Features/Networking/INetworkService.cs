using System;
using System.Threading.Tasks;
using Loveverse.Core;

namespace Loveverse.Networking
{
    public interface INetworkService : IGameService
    {
        bool IsConnected { get; }
        string CurrentRoomCode { get; }
        int ConnectedPlayerCount { get; }

        event Action<string> OnPlayerJoined;
        event Action<string> OnPlayerLeft;
        event Action OnDisconnected;

        Task<string> CreatePrivateRoomAsync();
        Task<bool> JoinPrivateRoomAsync(string roomCode);
        Task LeaveRoomAsync();
    }
}
