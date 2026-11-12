using UnityEngine;
using Loveverse.Core;

namespace Loveverse.UI
{
    public sealed class AuthenticationUI : MonoBehaviour
    {
        [SerializeField] private string _nextScene = "CharacterCreator";
        [SerializeField] private string _menuScene = "MainMenu";

        private string _playerNameInput = "Partner";
        private string _statusMessage = "";

        private void Start()
        {
            if (ServiceRegistry.Instance.TryResolve<IAuthService>(out var auth))
            {
                if (auth.IsAuthenticated)
                {
                    _statusMessage = $"Signed in as: {auth.DisplayName}";
                    _playerNameInput = auth.DisplayName;
                }
            }
        }

        private void OnGUI()
        {
            float screenW = Screen.width;
            float screenH = Screen.height;

            float boxW = Mathf.Clamp(screenW * 0.45f, 300f, 500f);
            float boxH = Mathf.Clamp(screenH * 0.45f, 260f, 360f);
            float x = (screenW - boxW) * 0.5f;
            float y = (screenH - boxH) * 0.5f;

            GUI.Box(new Rect(x, y, boxW, boxH), "LOVEVERSE PROFILE AUTHENTICATION");

            GUILayout.BeginArea(new Rect(x + 20, y + 35, boxW - 40, boxH - 50));

            GUILayout.Label("Enter Your In-Game Partner Name:", GUILayout.Height(25));
            _playerNameInput = GUILayout.TextField(_playerNameInput, 24, GUILayout.Height(35));

            GUILayout.Space(15);
            if (GUILayout.Button("Continue as Guest / Enter World", GUILayout.Height(45)))
            {
                OnContinueClicked();
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Back to Main Menu", GUILayout.Height(35)))
            {
                OnBackClicked();
            }

            if (!string.IsNullOrEmpty(_statusMessage))
            {
                GUILayout.Space(10);
                GUILayout.Label(_statusMessage);
            }

            GUILayout.EndArea();
        }

        private async void OnContinueClicked()
        {
            if (ServiceRegistry.Instance.TryResolve<IAuthService>(out var auth))
            {
                _statusMessage = "Authenticating profile...";
                await auth.AuthenticateGuestAsync(_playerNameInput);
            }

            if (ServiceRegistry.Instance.TryResolve<ISceneService>(out var sceneService))
            {
                await sceneService.LoadSceneAsync(_nextScene);
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
