using System.Threading.Tasks;

namespace Loveverse.Core
{
    public interface IBackendService : IGameService
    {
        string BaseApiUrl { get; set; }
        bool IsBackendAvailable { get; }
        bool IsOfflineDevMode { get; set; }

        Task<bool> CheckHealthAsync();
        Task<string> CreateRoomOnBackendAsync(string hostUserId);
        Task<bool> JoinRoomOnBackendAsync(string roomCode, string guestUserId);
        Task<bool> SyncWorldLayoutAsync(string worldId, string layoutJson);
    }
}
