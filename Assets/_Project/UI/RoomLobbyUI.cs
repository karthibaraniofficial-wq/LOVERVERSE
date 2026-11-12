using System;
using UnityEngine;
using Loveverse.Core;
using Loveverse.Networking;

namespace Loveverse.UI
{
    /// <summary>
    /// Multiplayer Room Lobby UI allowing players to create private 2-player rooms,
    /// enter 6-character room codes, view connection status, and launch into the world.
    /// Provides an explicitly labeled Offline Development Mode with automated partner bot simulation.
    /// </summary>
    public sealed class RoomLobbyUI : MonoBehaviour
    {
        [Header("Scene Navigation")]
        [SerializeField] private string _gameplayScene = "LoveTown";
        [SerializeField] private string _menuScene = "MainMenu";

        private INetworkService _networkService;
        private IBackendService _backendService;

        private string _inputRoomCode = "";
        private string _statusMessage = "Ready to connect.";
        private bool _isRoomCreated;
        private bool _isPartnerJoined;

        private void Start()
        {
            if (ServiceRegistry.Instance.TryResolve<INetworkService>(out var net))
            {
                _networkService = net;
                _networkService.OnPlayerJoined += OnPartnerJoined;
                _networkService.OnPlayerLeft += OnPartnerLeft;
            }

            if (ServiceRegistry.Instance.TryResolve<IBackendService>(out var backend))
            {
                _backendService = backend;
            }
        }

        private void OnDestroy()
        {
            if (_networkService != null)
            {
                _networkService.OnPlayerJoined -= OnPartnerJoined;
                _networkService.OnPlayerLeft -= OnPartnerLeft;
            }
        }

        private void OnPartnerJoined(string partnerId)
        {
            _isPartnerJoined = true;
            _statusMessage = $"Partner connected! ({partnerId})";
        }

        private void OnPartnerLeft(string partnerId)
        {
            _isPartnerJoined = false;
            _statusMessage = "Partner departed from private room.";
        }

        private void OnGUI()
        {
            float screenW = Screen.width;
            float screenH = Screen.height;

            float boxW = Mathf.Clamp(screenW * 0.5f, 320f, 540f);
            float boxH = Mathf.Clamp(screenH * 0.7f, 400f, 520f);
            float x = (screenW - boxW) * 0.5f;
            float y = (screenH - boxH) * 0.5f;

            GUI.Box(new Rect(x, y, boxW, boxH), "MULTIPLAYER PRIVATE ROOM LOBBY");

            GUILayout.BeginArea(new Rect(x + 20, y + 35, boxW - 40, boxH - 50));

            // Dev Mode Banner
            bool isOffline = _backendService == null || _backendService.IsOfflineDevMode;
            var modeStyle = new GUIStyle(GUI.skin.box);
            modeStyle.normal.textColor = isOffline ? new Color(1f, 0.75f, 0.2f) : new Color(0.2f, 1f, 0.4f);
            string modeText = isOffline
                ? "🟡 [OFFLINE DEV MODE — Local Partner Simulation Enabled]"
                : "🟢 [ONLINE — Photon Fusion 2 Cloud Connected]";
            GUILayout.Box(modeText, modeStyle, GUILayout.Height(30));

            GUILayout.Space(15);

            // Create Private Room Section
            GUILayout.Label("Host a Private 2-Player World:");
            if (GUILayout.Button("🔑  CREATE NEW ROOM CODE", GUILayout.Height(42)))
            {
                OnCreateRoomClicked();
            }

            if (_isRoomCreated && _networkService != null && _networkService.IsConnected && !string.IsNullOrEmpty(_networkService.CurrentRoomCode))
            {
                GUILayout.Space(8);
                var codeStyle = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 20,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter
                };
                codeStyle.normal.textColor = new Color(1f, 0.45f, 0.65f);
                GUILayout.Box($"INVITATION CODE: {_networkService.CurrentRoomCode}", codeStyle, GUILayout.Height(45));
                GUILayout.Label("Share this 6-character code with your partner to join!");
            }

            GUILayout.Space(15);

            // Join Room Section
            GUILayout.Label("Or Join Partner's Room:");
            GUILayout.BeginHorizontal();
            _inputRoomCode = GUILayout.TextField(_inputRoomCode.ToUpperInvariant(), 6, GUILayout.Height(35));
            if (GUILayout.Button("JOIN", GUILayout.Width(90), GUILayout.Height(35)))
            {
                OnJoinRoomClicked();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(12);

            // Status message
            string partnerStatus = _isPartnerJoined ? "💖 Partner Present" : "Waiting for partner...";
            GUILayout.Label($"Status: {_statusMessage} • [{partnerStatus}]");
            if (_networkService != null)
            {
                GUILayout.Label($"Players in Room: {_networkService.ConnectedPlayerCount} / 2");
            }

            GUILayout.FlexibleSpace();

            // Launch Game
            if (GUILayout.Button("🚀  START ADVENTURE TOGETHER", GUILayout.Height(48)))
            {
                OnStartAdventureClicked();
            }

            GUILayout.Space(8);
            if (GUILayout.Button("Back to Menu", GUILayout.Height(32)))
            {
                OnBackClicked();
            }

            GUILayout.EndArea();
        }

        private async void OnCreateRoomClicked()
        {
            if (_networkService == null) return;

            string roomCode = await _networkService.CreatePrivateRoomAsync();
            _isRoomCreated = true;
            _statusMessage = $"Created room: {roomCode}. Waiting for partner...";

            if (_backendService != null)
            {
                await _backendService.CreateRoomOnBackendAsync("local_player");
            }
        }

        private async void OnJoinRoomClicked()
        {
            if (_networkService == null) return;
            string code = _inputRoomCode.Trim();

            if (!RoomCodeGenerator.IsValidCodeFormat(code))
            {
                _statusMessage = "Error: Room code must be 6 alphanumeric characters.";
                return;
            }

            _statusMessage = $"Joining room {code}...";
            bool success = await _networkService.JoinPrivateRoomAsync(code);
            if (success)
            {
                _statusMessage = $"Joined room {code} successfully!";
                if (_backendService != null)
                {
                    await _backendService.JoinRoomOnBackendAsync(code, "local_player");
                }
            }
            else
            {
                _statusMessage = "Failed to join room. Verify the code and try again.";
            }
        }

        private async void OnStartAdventureClicked()
        {
            if (ServiceRegistry.Instance.TryResolve<ISceneService>(out var sceneService))
            {
                await sceneService.LoadSceneAsync(_gameplayScene);
            }
        }

        private async void OnBackClicked()
        {
            if (ServiceRegistry.Instance.TryResolve<ISceneService>(out var sceneService))
            {
                await sceneService.LoadSceneAsync(_menuScene);
            }
        }
    }
}
