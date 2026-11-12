using UnityEngine;

namespace Loveverse.Networking
{
    /// <summary>
    /// Tracks unique identity, user ID, and display name for network player entities.
    /// </summary>
    public sealed class NetworkPlayerIdentity : MonoBehaviour
    {
        [SerializeField] private string _userId;
        [SerializeField] private string _displayName;
        [SerializeField] private bool _isLocalPlayer;

        public string UserId => _userId;
        public string DisplayName => _displayName;
        public bool IsLocalPlayer => _isLocalPlayer;

        public void Initialize(string userId, string displayName, bool isLocal)
        {
            _userId = userId;
            _displayName = displayName;
            _isLocalPlayer = isLocal;
        }
    }
}
