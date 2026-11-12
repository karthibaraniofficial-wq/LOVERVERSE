using System;
using UnityEngine;
using Loveverse.Core;
using Loveverse.Memories;

namespace Loveverse.UI
{
    /// <summary>
    /// Master Main Menu controller handling Play, Character Customization, Multiplayer,
    /// Settings, Memories gallery, and application Exit.
    /// Provides responsive visual UI with both mobile touch and desktop click support.
    /// </summary>
    public sealed class MainMenuController : MonoBehaviour
    {
        [Header("Scene Navigation")]
        [SerializeField] private string _gameplayScene = "LoveTown";
        [SerializeField] private string _characterCreatorScene = "CharacterCreator";
        [SerializeField] private string _multiplayerScene = "RoomLobby";

        private bool _showSettingsModal;
        private bool _showMemoriesModal;

        private float _musicVolume = 0.8f;
        private float _sfxVolume = 1.0f;
        private int _selectedQualityPreset = 2; // 0: Low, 1: Medium, 2: High, 3: Ultra
        private readonly string[] _qualityNames = { "Low (Mobile)", "Medium", "High (PC)", "Ultra" };

        private void Start()
        {
            // Ensure audio listener exists
            if (FindAnyObjectByType<AudioListener>() == null)
            {
                var cam = UnityEngine.Camera.main;
                if (cam != null && cam.GetComponent<AudioListener>() == null)
                {
                    cam.gameObject.AddComponent<AudioListener>();
                }
            }
        }

        public async void OnPlayClicked()
        {
            if (ServiceRegistry.Instance.TryResolve<IAuthService>(out var auth))
            {
                if (!auth.IsAuthenticated)
                {
                    await auth.AuthenticateGuestAsync("Player");
                }
            }

            if (ServiceRegistry.Instance.TryResolve<ISceneService>(out var sceneService))
            {
                Lg.Info($"MainMenu: Launching Game World -> {_gameplayScene}");
                await sceneService.LoadSceneAsync(_gameplayScene);
            }
        }

        public async void OnCharacterCustomizationClicked()
        {
            if (ServiceRegistry.Instance.TryResolve<ISceneService>(out var sceneService))
            {
                Lg.Info("MainMenu: Opening Character Creator");
                await sceneService.LoadSceneAsync(_characterCreatorScene);
            }
        }

        public async void OnMultiplayerClicked()
        {
            if (ServiceRegistry.Instance.TryResolve<ISceneService>(out var sceneService))
            {
                Lg.Info("MainMenu: Opening Room Lobby");
                await sceneService.LoadSceneAsync(_multiplayerScene);
            }
        }

        public void OnSettingsClicked()
        {
            _showSettingsModal = !_showSettingsModal;
            _showMemoriesModal = false;
        }

        public void OnMemoriesClicked()
        {
            _showMemoriesModal = !_showMemoriesModal;
            _showSettingsModal = false;
        }

        public void OnExitClicked()
        {
            Lg.Info("Exiting LOVEVERSE application.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnGUI()
        {
            // Custom responsive GUI for immediate Play Mode testing across all 4 platforms
            float screenW = Screen.width;
            float screenH = Screen.height;

            // Title Header Banner
            var titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.Clamp(Mathf.RoundToInt(screenH * 0.055f), 24, 48),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            titleStyle.normal.textColor = new Color(1f, 0.45f, 0.65f); // Pastel Romantic Pink

            var subStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.Clamp(Mathf.RoundToInt(screenH * 0.024f), 12, 20),
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleCenter
            };
            subStyle.normal.textColor = new Color(0.9f, 0.9f, 1f, 0.9f);

            GUI.Label(new Rect(0, screenH * 0.08f, screenW, screenH * 0.07f), "💖 LOVEVERSE", titleStyle);
            GUI.Label(new Rect(0, screenH * 0.15f, screenW, screenH * 0.04f), "A World Made for Two", subStyle);

            if (_showSettingsModal)
            {
                DrawSettingsModal(screenW, screenH);
                return;
            }

            if (_showMemoriesModal)
            {
                DrawMemoriesModal(screenW, screenH);
                return;
            }

            // Main Menu Buttons Layout
            float menuW = Mathf.Clamp(screenW * 0.35f, 260f, 420f);
            float btnH = Mathf.Clamp(screenH * 0.065f, 44f, 65f);
            float spacing = btnH * 0.22f;
            float startY = screenH * 0.26f;
            float startX = (screenW - menuW) * 0.5f;

            var btnStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = Mathf.Clamp(Mathf.RoundToInt(btnH * 0.38f), 14, 22),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            if (GUI.Button(new Rect(startX, startY, menuW, btnH), "▶  PLAY GAME", btnStyle))
            {
                OnPlayClicked();
            }

            if (GUI.Button(new Rect(startX, startY + (btnH + spacing) * 1, menuW, btnH), "👗  CUSTOMIZE AVATAR", btnStyle))
            {
                OnCharacterCustomizationClicked();
            }

            if (GUI.Button(new Rect(startX, startY + (btnH + spacing) * 2, menuW, btnH), "🌐  MULTIPLAYER ROOMS", btnStyle))
            {
                OnMultiplayerClicked();
            }

            if (GUI.Button(new Rect(startX, startY + (btnH + spacing) * 3, menuW, btnH), "📸  SHARED MEMORIES", btnStyle))
            {
                OnMemoriesClicked();
            }

            if (GUI.Button(new Rect(startX, startY + (btnH + spacing) * 4, menuW, btnH), "⚙️  SETTINGS", btnStyle))
            {
                OnSettingsClicked();
            }

            if (GUI.Button(new Rect(startX, startY + (btnH + spacing) * 5, menuW, btnH), "✖  EXIT", btnStyle))
            {
                OnExitClicked();
            }

            // Footer Platform Info
            var footerStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                alignment = TextAnchor.LowerCenter
            };
            footerStyle.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 0.6f);
            string platformTag = Application.platform.ToString();
            GUI.Label(new Rect(0, screenH - 30, screenW, 25), $"LOVEVERSE Universal Engine v1.0 • Running on {platformTag}", footerStyle);
        }

        private void DrawSettingsModal(float screenW, float screenH)
        {
            float modalW = Mathf.Clamp(screenW * 0.6f, 340f, 600f);
            float modalH = Mathf.Clamp(screenH * 0.65f, 380f, 520f);
            float x = (screenW - modalW) * 0.5f;
            float y = (screenH - modalH) * 0.5f;

            GUI.Box(new Rect(x, y, modalW, modalH), "SETTINGS");

            GUILayout.BeginArea(new Rect(x + 20, y + 35, modalW - 40, modalH - 50));

            GUILayout.Label("Graphics Quality Preset:", GUILayout.Height(25));
            int newPreset = GUILayout.SelectionGrid(_selectedQualityPreset, _qualityNames, 2, GUILayout.Height(60));
            if (newPreset != _selectedQualityPreset)
            {
                _selectedQualityPreset = newPreset;
                QualitySettings.SetQualityLevel(_selectedQualityPreset, true);
                Lg.Info($"Applied graphics quality preset: {_qualityNames[_selectedQualityPreset]}");
            }

            GUILayout.Space(15);
            GUILayout.Label($"Music Volume: {Mathf.RoundToInt(_musicVolume * 100)}%");
            _musicVolume = GUILayout.HorizontalSlider(_musicVolume, 0f, 1f);

            GUILayout.Space(10);
            GUILayout.Label($"SFX Volume: {Mathf.RoundToInt(_sfxVolume * 100)}%");
            _sfxVolume = GUILayout.HorizontalSlider(_sfxVolume, 0f, 1f);

            GUILayout.Space(25);
            if (GUILayout.Button("Close Settings", GUILayout.Height(40)))
            {
                _showSettingsModal = false;
            }

            GUILayout.EndArea();
        }

        private void DrawMemoriesModal(float screenW, float screenH)
        {
            float modalW = Mathf.Clamp(screenW * 0.7f, 360f, 700f);
            float modalH = Mathf.Clamp(screenH * 0.7f, 380f, 540f);
            float x = (screenW - modalW) * 0.5f;
            float y = (screenH - modalH) * 0.5f;

            GUI.Box(new Rect(x, y, modalW, modalH), "SHARED MEMORIES ALBUM");

            GUILayout.BeginArea(new Rect(x + 20, y + 35, modalW - 40, modalH - 50));

            if (ServiceRegistry.Instance.TryResolve<IMemoryService>(out var memoryService) && memoryService.LocalMemories.Count > 0)
            {
                GUILayout.Label($"Preserved Moments ({memoryService.LocalMemories.Count}):");
                for (int i = 0; i < memoryService.LocalMemories.Count; i++)
                {
                    var item = memoryService.LocalMemories[i];
                    GUILayout.Label($"📸 {item.Caption} • [{item.LocationId}] • {item.CreatedAt}");
                }
            }
            else
            {
                GUILayout.Label("\nNo photos captured yet.\n\nExplore Love Town or Sunset Beach with your partner and use Photo Mode (P or Camera Icon) to capture eternal memories!", GUILayout.Height(100));
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Back to Menu", GUILayout.Height(40)))
            {
                _showMemoriesModal = false;
            }

            GUILayout.EndArea();
        }
    }
}
