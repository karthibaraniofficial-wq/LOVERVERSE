using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Loveverse.Core
{
    public sealed class BackendService : MonoBehaviour, IBackendService
    {
        [SerializeField] private string _baseApiUrl = "http://localhost:3000";
        [SerializeField] private bool _forceOfflineMode = false;

        public bool IsInitialized { get; private set; }
        public string BaseApiUrl
        {
            get => _baseApiUrl;
            set => _baseApiUrl = value;
        }

        public bool IsBackendAvailable { get; private set; }
        public bool IsOfflineDevMode
        {
            get => _forceOfflineMode || !IsBackendAvailable;
            set => _forceOfflineMode = value;
        }

        public async Task InitializeAsync()
        {
            IsInitialized = true;
            Lg.Info($"BackendService initialized with BaseUrl: {_baseApiUrl}");

            if (!_forceOfflineMode)
            {
                await CheckHealthAsync();
            }
            else
            {
                Lg.Info("BackendService started in explicit OFFLINE DEV MODE.");
            }
        }

        public void Shutdown()
        {
            IsInitialized = false;
        }

        public async Task<bool> CheckHealthAsync()
        {
            string url = $"{_baseApiUrl}/health";
            try
            {
                using var request = UnityWebRequest.Get(url);
                request.timeout = 2; // Fast timeout for responsiveness
                var op = request.SendWebRequest();

                while (!op.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    IsBackendAvailable = true;
                    Lg.Info("Connected to LOVEVERSE Fastify backend server successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Lg.Warning($"Backend health check failed: {ex.Message}. Falling back to offline dev mode.");
            }

            IsBackendAvailable = false;
            return false;
        }

        public async Task<string> CreateRoomOnBackendAsync(string hostUserId)
        {
            if (IsOfflineDevMode)
            {
                string devCode = "LV-" + Guid.NewGuid().ToString("N").Substring(0, 4).ToUpperInvariant();
                Lg.Info($"[Offline Dev Mode] Created local private room: {devCode}");
                return devCode;
            }

            string url = $"{_baseApiUrl}/api/v1/rooms";
            string payload = $"{{\"hostUserId\":\"{hostUserId}\"}}";

            try
            {
                using var request = new UnityWebRequest(url, "POST");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(payload);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.timeout = 3;

                var op = request.SendWebRequest();
                while (!op.isDone) await Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var responseText = request.downloadHandler.text;
                    // Extract roomCode simply without heavy JSON parser dependency
                    int idx = responseText.IndexOf("\"roomCode\":\"", StringComparison.Ordinal);
                    if (idx != -1)
                    {
                        int start = idx + 12;
                        int end = responseText.IndexOf("\"", start, StringComparison.Ordinal);
                        if (end != -1)
                        {
                            return responseText.Substring(start, end - start);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lg.Warning($"CreateRoomOnBackend failed ({ex.Message}), falling back to local generation.");
            }

            // Fallback code
            return "LV-" + Guid.NewGuid().ToString("N").Substring(0, 4).ToUpperInvariant();
        }

        public async Task<bool> JoinRoomOnBackendAsync(string roomCode, string guestUserId)
        {
            if (IsOfflineDevMode)
            {
                Lg.Info($"[Offline Dev Mode] Joined local private room: {roomCode}");
                return true;
            }

            string url = $"{_baseApiUrl}/api/v1/rooms/join";
            string payload = $"{{\"roomCode\":\"{roomCode}\",\"guestUserId\":\"{guestUserId}\"}}";

            try
            {
                using var request = new UnityWebRequest(url, "POST");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(payload);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.timeout = 3;

                var op = request.SendWebRequest();
                while (!op.isDone) await Task.Yield();

                return request.result == UnityWebRequest.Result.Success;
            }
            catch (Exception ex)
            {
                Lg.Warning($"JoinRoomOnBackend failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SyncWorldLayoutAsync(string worldId, string layoutJson)
        {
            if (IsOfflineDevMode)
            {
                Lg.Info($"[Offline Dev Mode] Saved world layout locally: {worldId}");
                return true;
            }

            string url = $"{_baseApiUrl}/api/v1/worlds/{worldId}";
            try
            {
                using var request = new UnityWebRequest(url, "PATCH");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(layoutJson);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.timeout = 3;

                var op = request.SendWebRequest();
                while (!op.isDone) await Task.Yield();

                return request.result == UnityWebRequest.Result.Success;
            }
            catch (Exception ex)
            {
                Lg.Warning($"SyncWorldLayout failed: {ex.Message}");
                return false;
            }
        }
    }
}
