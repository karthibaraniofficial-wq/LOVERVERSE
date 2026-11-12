using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Loveverse.Core
{
    public sealed class AuthService : MonoBehaviour, IAuthService
    {
        private const string TokenStorageKey = "loveverse_session_token";
        private const string UserIdStorageKey = "loveverse_user_id";
        private const string NameStorageKey = "loveverse_display_name";

        public bool IsInitialized { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public string CurrentUserId { get; private set; }
        public string DisplayName { get; private set; }
        public string SessionToken { get; private set; }

        public event Action<string> OnAuthenticated;
        public event Action OnLoggedOut;

        public Task InitializeAsync()
        {
            IsInitialized = true;

            // Attempt session restoration from persistent player preferences
            string savedToken = PlayerPrefs.GetString(TokenStorageKey, null);
            string savedId = PlayerPrefs.GetString(UserIdStorageKey, null);
            string savedName = PlayerPrefs.GetString(NameStorageKey, null);

            if (!string.IsNullOrEmpty(savedToken) && !string.IsNullOrEmpty(savedId))
            {
                SessionToken = savedToken;
                CurrentUserId = savedId;
                DisplayName = string.IsNullOrEmpty(savedName) ? "Player" : savedName;
                IsAuthenticated = true;
                Lg.Info($"Restored previous auth session for user: {CurrentUserId} ({DisplayName})");
            }

            return Task.CompletedTask;
        }

        public void Shutdown()
        {
            IsInitialized = false;
        }

        public Task<bool> AuthenticateGuestAsync(string displayName = null)
        {
            string generatedId = "usr_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            string generatedToken = "tok_" + Guid.NewGuid().ToString("N");
            string name = string.IsNullOrWhiteSpace(displayName) ? "Beloved Partner" : displayName.Trim();

            CurrentUserId = generatedId;
            SessionToken = generatedToken;
            DisplayName = name;
            IsAuthenticated = true;

            PlayerPrefs.SetString(TokenStorageKey, SessionToken);
            PlayerPrefs.SetString(UserIdStorageKey, CurrentUserId);
            PlayerPrefs.SetString(NameStorageKey, DisplayName);
            PlayerPrefs.Save();

            Lg.Info($"Guest authenticated successfully: {CurrentUserId} ({DisplayName})");
            OnAuthenticated?.Invoke(CurrentUserId);
            return Task.FromResult(true);
        }

        public Task<bool> AuthenticateSessionAsync(string sessionToken)
        {
            if (string.IsNullOrWhiteSpace(sessionToken))
            {
                return Task.FromResult(false);
            }

            SessionToken = sessionToken;
            if (string.IsNullOrEmpty(CurrentUserId))
            {
                CurrentUserId = "usr_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            }
            if (string.IsNullOrEmpty(DisplayName))
            {
                DisplayName = "Player";
            }
            IsAuthenticated = true;

            PlayerPrefs.SetString(TokenStorageKey, SessionToken);
            PlayerPrefs.SetString(UserIdStorageKey, CurrentUserId);
            PlayerPrefs.Save();

            OnAuthenticated?.Invoke(CurrentUserId);
            return Task.FromResult(true);
        }

        public void Logout()
        {
            CurrentUserId = null;
            SessionToken = null;
            DisplayName = null;
            IsAuthenticated = false;

            PlayerPrefs.DeleteKey(TokenStorageKey);
            PlayerPrefs.DeleteKey(UserIdStorageKey);
            PlayerPrefs.DeleteKey(NameStorageKey);
            PlayerPrefs.Save();

            Lg.Info("User logged out.");
            OnLoggedOut?.Invoke();
        }
    }
}
