using System.Threading.Tasks;

namespace Loveverse.Core
{
    /// <summary>
    /// Base lifecycle contract for all modular game services in LOVEVERSE.
    /// </summary>
    public interface IGameService
    {
        bool IsInitialized { get; }
        Task InitializeAsync();
        void Shutdown();
    }
}
