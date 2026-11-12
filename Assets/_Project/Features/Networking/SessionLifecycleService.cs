using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Networking
{
    public enum SessionStatus
    {
        Disconnected,
        Connecting,
        InRoom,
        Reconnecting
    }

    /// <summary>
    /// Coordinates connection lifecycle, heartbeat health, and automatic reconnection for private worlds.
    /// </summary>
    public sealed class SessionLifecycleService : MonoBehaviour, IGameService
    {
        public bool IsInitialized { get; private set; }
        public SessionStatus Status { get; private set; } = SessionStatus.Disconnected;

        public event Action<SessionStatus> OnStatusChanged;

        public Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public void Shutdown()
        {
            Status = SessionStatus.Disconnected;
            IsInitialized = false;
        }

        public void SetStatus(SessionStatus newStatus)
        {
            if (Status == newStatus) return;
            Status = newStatus;
            Lg.Info($"Session status changed to: {Status}");
            OnStatusChanged?.Invoke(Status);
        }

        public void HandleConnectionLoss()
        {
            if (Status == SessionStatus.InRoom)
            {
                SetStatus(SessionStatus.Reconnecting);
                StartCoroutine(ReconnectWindowRoutine(30f));
            }
        }

        private IEnumerator ReconnectWindowRoutine(float timeoutSeconds)
        {
            float elapsed = 0f;
            while (elapsed < timeoutSeconds && Status == SessionStatus.Reconnecting)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (Status == SessionStatus.Reconnecting)
            {
                Lg.Warning("Reconnection window expired. Terminating session.");
                SetStatus(SessionStatus.Disconnected);
            }
        }
    }
}
