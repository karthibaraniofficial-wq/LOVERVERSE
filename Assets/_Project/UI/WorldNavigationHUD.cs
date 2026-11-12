using System;
using UnityEngine;
using Loveverse.Core;
using Loveverse.Romance;
using Loveverse.Camera;
using Loveverse.Home;

namespace Loveverse.UI
{
    /// <summary>
    /// In-game master HUD providing region fast-travel, romance interaction triggers,
    /// photo mode, home decoration mode, and return to main menu across all 6 world scenes.
    /// </summary>
    public sealed class WorldNavigationHUD : MonoBehaviour
    {
        [Header("Current Region Info")]
        [SerializeField] private string _currentRegionName = "Love Town";

        private bool _showRegionMap;
        private bool _showRomanceMenu;
        private string _statusNotification = "";
        private float _notificationTimer = 0f;

        private InteractionCoordinator _romanceCoordinator;
        private PhotoModeController _photoModeController;
        private HomePlacementService _homePlacementService;

        private readonly (string Name, string Scene)[] _regions = new[]
        {
            ("🏡 Our Home", "OurHome"),
            ("🌸 Love Town Plaza", "LoveTown"),
            ("🌅 Sunset Beach", "SunsetBeach"),
            ("🌲 Dream Forest", "DreamForest"),
            ("🏔️ Sky Mountain", "SkyMountain"),
            ("✨ Starlight City", "StarlightCity")
        };

        private void Start()
        {
            _romanceCoordinator = FindAnyObjectByType<InteractionCoordinator>();
            _photoModeController = FindAnyObjectByType<PhotoModeController>();
            _homePlacementService = FindAnyObjectByType<HomePlacementService>();
        }

        private void Update()
        {
            if (_notificationTimer > 0f)
            {
                _notificationTimer -= Time.deltaTime;
                if (_notificationTimer <= 0f)
                {
                    _statusNotification = "";
                }
            }

            // Keyboard shortcut helpers for desktop
            if (Input.GetKeyDown(KeyCode.M))
            {
                _showRegionMap = !_showRegionMap;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                _showRomanceMenu = !_showRomanceMenu;
            }
        }

        public void ShowNotification(string message, float duration = 3.5f)
        {
            _statusNotification = message;
            _notificationTimer = duration;
        }

        private void OnGUI()
        {
            // Do not render HUD when Photo Mode is actively capturing
            if (_photoModeController != null && _photoModeController.IsPhotoModeActive)
            {
                DrawPhotoModeOverlay();
                return;
            }

            float screenW = Screen.width;
            float screenH = Screen.height;

            // 1. Top Bar: Current Region Pill & Status
            var pillStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            pillStyle.normal.textColor = Color.white;

            float pillW = Mathf.Clamp(screenW * 0.32f, 200f, 320f);
            GUI.Box(new Rect((screenW - pillW) * 0.5f, 15, pillW, 36), $"📍 {_currentRegionName}", pillStyle);

            if (!string.IsNullOrEmpty(_statusNotification))
            {
                var notifStyle = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 14,
                    alignment = TextAnchor.MiddleCenter
                };
                notifStyle.normal.textColor = new Color(1f, 0.85f, 0.4f);
                GUI.Box(new Rect((screenW - 400f) * 0.5f, 56, 400f, 30), _statusNotification, notifStyle);
            }

            // 2. Top-Right Quick Action Icons
            float btnSize = 44f;
            float pad = 10f;
            float rightX = screenW - btnSize - 15f;

            // Travel Map Button (M)
            if (GUI.Button(new Rect(rightX, 15, btnSize, btnSize), "🗺️"))
            {
                _showRegionMap = !_showRegionMap;
                _showRomanceMenu = false;
            }

            // Romance Wheel Button (B)
            if (GUI.Button(new Rect(rightX - (btnSize + pad), 15, btnSize, btnSize), "💖"))
            {
                _showRomanceMenu = !_showRomanceMenu;
                _showRegionMap = false;
            }

            // Photo Mode Button (P)
            if (GUI.Button(new Rect(rightX - (btnSize + pad) * 2, 15, btnSize, btnSize), "📷"))
            {
                if (_photoModeController != null)
                {
                    _photoModeController.TogglePhotoMode();
                }
            }

            // Home Decorating Button (Only in OurHome)
            if (_homePlacementService != null)
            {
                if (GUI.Button(new Rect(rightX - (btnSize + pad) * 3, 15, btnSize, btnSize), "🛋️"))
                {
                    _homePlacementService.ToggleEditMode();
                    ShowNotification(_homePlacementService.IsEditModeActive ? "Home Decoration Mode: ON" : "Home Decoration Mode: OFF");
                }
            }

            // Return to Menu Button
            if (GUI.Button(new Rect(15, 15, 90, 36), "◀ Menu"))
            {
                OnReturnToMenuClicked();
            }

            // 3. Region Fast Travel Modal
            if (_showRegionMap)
            {
                DrawRegionTravelModal(screenW, screenH);
            }

            // 4. Romance Actions Modal
            if (_showRomanceMenu)
            {
                DrawRomanceMenuModal(screenW, screenH);
            }

            // 5. Desktop Keymap Hint (Bottom Right)
            if (!Application.isMobilePlatform)
            {
                var hintStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 11,
                    alignment = TextAnchor.LowerRight
                };
                hintStyle.normal.textColor = new Color(1f, 1f, 1f, 0.5f);
                GUI.Label(new Rect(screenW - 350, screenH - 25, 335, 20), "WASD: Move • Shift: Sprint • E: Interact • B: Emote • P: Photo", hintStyle);
            }
        }

        private void DrawRegionTravelModal(float screenW, float screenH)
        {
            float modalW = Mathf.Clamp(screenW * 0.45f, 280f, 440f);
            float modalH = Mathf.Clamp(screenH * 0.55f, 320f, 440f);
            float x = (screenW - modalW) * 0.5f;
            float y = (screenH - modalH) * 0.5f;

            GUI.Box(new Rect(x, y, modalW, modalH), "TRAVEL TO WORLD REGION");

            GUILayout.BeginArea(new Rect(x + 15, y + 35, modalW - 30, modalH - 50));

            for (int i = 0; i < _regions.Length; i++)
            {
                var r = _regions[i];
                if (GUILayout.Button(r.Name, GUILayout.Height(40)))
                {
                    _showRegionMap = false;
                    TravelToRegion(r.Scene);
                }
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close Map", GUILayout.Height(32)))
            {
                _showRegionMap = false;
            }

            GUILayout.EndArea();
        }

        private void DrawRomanceMenuModal(float screenW, float screenH)
        {
            float modalW = Mathf.Clamp(screenW * 0.4f, 260f, 380f);
            float modalH = Mathf.Clamp(screenH * 0.5f, 300f, 400f);
            float x = (screenW - modalW) * 0.5f;
            float y = (screenH - modalH) * 0.5f;

            GUI.Box(new Rect(x, y, modalW, modalH), "ROMANTIC INTERACTIONS");

            GUILayout.BeginArea(new Rect(x + 15, y + 35, modalW - 30, modalH - 50));

            if (GUILayout.Button("🤝  HOLD HANDS", GUILayout.Height(42)))
            {
                TriggerRomanceAction(InteractionType.HoldHands);
            }
            if (GUILayout.Button("🤗  WARM HUG", GUILayout.Height(42)))
            {
                TriggerRomanceAction(InteractionType.Hug);
            }
            if (GUILayout.Button("💃  ROMANTIC WALTZ", GUILayout.Height(42)))
            {
                TriggerRomanceAction(InteractionType.Dance);
            }
            if (GUILayout.Button("🪑  SIT TOGETHER", GUILayout.Height(42)))
            {
                TriggerRomanceAction(InteractionType.SitTogether);
            }
            if (GUILayout.Button("✨  STARGAZE", GUILayout.Height(42)))
            {
                TriggerRomanceAction(InteractionType.Stargaze);
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close", GUILayout.Height(30)))
            {
                _showRomanceMenu = false;
            }

            GUILayout.EndArea();
        }

        private void DrawPhotoModeOverlay()
        {
            float screenW = Screen.width;
            float screenH = Screen.height;

            GUI.Box(new Rect(15, 15, 220, 90), "📷 PHOTO MODE ACTIVE");
            if (GUI.Button(new Rect(25, 45, 95, 45), "📸 SNAP"))
            {
                if (_photoModeController != null)
                {
                    _photoModeController.CaptureScreenshot();
                    ShowNotification("Photo saved to Memories album!");
                }
            }
            if (GUI.Button(new Rect(125, 45, 95, 45), "EXIT"))
            {
                if (_photoModeController != null)
                {
                    _photoModeController.TogglePhotoMode();
                }
            }
        }

        private async void TravelToRegion(string sceneName)
        {
            if (ServiceRegistry.Instance.TryResolve<ISceneService>(out var sceneService))
            {
                ShowNotification($"Traveling to {sceneName}...");
                await sceneService.LoadSceneAsync(sceneName);
            }
        }

        private void TriggerRomanceAction(InteractionType type)
        {
            _showRomanceMenu = false;
            if (_romanceCoordinator != null)
            {
                bool sent = _romanceCoordinator.TryRequestInteraction(type);
                ShowNotification(sent ? $"Requested {type} with partner! Waiting for response..." : $"Partner is not close enough for {type}. Walk closer together!");
            }
            else
            {
                ShowNotification($"Performed {type} in romantic harmony!");
            }
        }

        private async void OnReturnToMenuClicked()
        {
            if (ServiceRegistry.Instance.TryResolve<ISceneService>(out var sceneService))
            {
                await sceneService.LoadSceneAsync("MainMenu");
            }
        }
    }
}
