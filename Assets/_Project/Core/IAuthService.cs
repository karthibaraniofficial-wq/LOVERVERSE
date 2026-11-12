using System;
using System.Threading.Tasks;

namespace Loveverse.Core
{
    public interface IAuthService : IGameService
    {
        bool IsAuthenticated { get; }
        string CurrentUserId { get; }
        string DisplayName { get; }
        string SessionToken { get; }

        event Action<string> OnAuthenticated;
        event Action OnLoggedOut;

        Task<bool> AuthenticateGuestAsync(string displayName = null);
        Task<bool> AuthenticateSessionAsync(string sessionToken);
        void Logout();
    }
}
