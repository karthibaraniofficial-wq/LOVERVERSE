using System;
using UnityEngine;
using Loveverse.Core;
using Loveverse.Characters;

namespace Loveverse.UI
{
    /// <summary>
    /// Interactive Character Customizer UI supporting real-time preview, color swatches,
    /// hairstyle / outfit cycling, and persistent profile saving.
    /// </summary>
    public sealed class CharacterCreatorUI : MonoBehaviour
    {
        [Header("Scene Navigation")]
        [SerializeField] private string _nextScene = "RoomLobby";
        [SerializeField] private string _menuScene = "MainMenu";

        [Header("Preview Binding")]
        [SerializeField] private CharacterVisualApplier _previewVisualApplier;

        private CharacterAppearance _appearance = new CharacterAppearance();
        private ICharacterCustomizationService _customService;

        private readonly string[] _hairstyles = { "hair_long_01", "hair_short_01", "hair_ponytail_01", "hair_messy_01" };
        private readonly string[] _hairColors = { "#2B1D0C", "#5C4033", "#D4AF37", "#B22222", "#E6E6FA" };
        private readonly string[] _hairColorNames = { "Raven", "Brunette", "Blonde", "Crimson", "Silver" };

        private readonly string[] _eyeColors = { "#3A75C4", "#2E8B57", "#8A2BE2", "#8B4513", "#4682B4" };
        private readonly string[] _eyeColorNames = { "Sapphire", "Emerald", "Violet", "Hazel", "Slate" };

        private readonly string[] _outfits = { "casual_chic_01", "romantic_date_01", "comfort_knit_01", "summer_breeze_01" };
        private readonly string[] _outfitNames = { "Casual Chic", "Romantic Date", "Comfort Knit", "Summer Breeze" };

        private int _hairIndex = 0;
        private int _hairColorIndex = 0;
        private int _eyeColorIndex = 0;
        private int _outfitIndex = 0;

        private void Start()
        {
            if (ServiceRegistry.Instance.TryResolve<ICharacterCustomizationService>(out var custom))
            {
                _customService = custom;
                if (custom.CurrentAppearance != null)
                {
                    _appearance = custom.CurrentAppearance.Clone();
                }
            }

            if (_previewVisualApplier == null)
            {
                _previewVisualApplier = FindAnyObjectByType<CharacterVisualApplier>();
            }

            UpdatePreview();
        }

        private void UpdatePreview()
        {
            _appearance.HairStyleId = _hairstyles[_hairIndex];
            _appearance.HairColorHex = _hairColors[_hairColorIndex];
            _appearance.EyeColorHex = _eyeColors[_eyeColorIndex];
            _appearance.OutfitTopId = _outfits[_outfitIndex];

            if (_previewVisualApplier != null)
            {
                _previewVisualApplier.ApplyAppearance(_appearance);
            }
        }

        private void OnGUI()
        {
            float screenW = Screen.width;
            float screenH = Screen.height;

            float panelW = Mathf.Clamp(screenW * 0.35f, 280f, 420f);
            float panelH = screenH * 0.85f;
            float x = 30f;
            float y = (screenH - panelH) * 0.5f;

            GUI.Box(new Rect(x, y, panelW, panelH), "CHARACTER CREATOR");

            GUILayout.BeginArea(new Rect(x + 15, y + 30, panelW - 30, panelH - 45));

            // Gender Selection
            GUILayout.Label("Gender Preset:");
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(_appearance.Gender == GenderPreset.Female, "Female", "Button", GUILayout.Height(35)))
            {
                if (_appearance.Gender != GenderPreset.Female)
                {
                    _appearance.Gender = GenderPreset.Female;
                    UpdatePreview();
                }
            }
            if (GUILayout.Toggle(_appearance.Gender == GenderPreset.Male, "Male", "Button", GUILayout.Height(35)))
            {
                if (_appearance.Gender != GenderPreset.Male)
                {
                    _appearance.Gender = GenderPreset.Male;
                    UpdatePreview();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(12);

            // Hairstyle
            GUILayout.Label($"Hairstyle: {_hairstyles[_hairIndex]}");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("◀ Prev", GUILayout.Height(30)))
            {
                _hairIndex = (_hairIndex - 1 + _hairstyles.Length) % _hairstyles.Length;
                UpdatePreview();
            }
            if (GUILayout.Button("Next ▶", GUILayout.Height(30)))
            {
                _hairIndex = (_hairIndex + 1) % _hairstyles.Length;
                UpdatePreview();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(12);

            // Hair Color
            GUILayout.Label("Hair Color Tint:");
            int newHairCol = GUILayout.SelectionGrid(_hairColorIndex, _hairColorNames, 3, GUILayout.Height(50));
            if (newHairCol != _hairColorIndex)
            {
                _hairColorIndex = newHairCol;
                UpdatePreview();
            }

            GUILayout.Space(12);

            // Eye Color
            GUILayout.Label("Eye Color Tint:");
            int newEyeCol = GUILayout.SelectionGrid(_eyeColorIndex, _eyeColorNames, 3, GUILayout.Height(50));
            if (newEyeCol != _eyeColorIndex)
            {
                _eyeColorIndex = newEyeCol;
                UpdatePreview();
            }

            GUILayout.Space(12);

            // Outfit
            GUILayout.Label("Outfit Style:");
            int newOutfit = GUILayout.SelectionGrid(_outfitIndex, _outfitNames, 2, GUILayout.Height(55));
            if (newOutfit != _outfitIndex)
            {
                _outfitIndex = newOutfit;
                UpdatePreview();
            }

            GUILayout.FlexibleSpace();

            // Action Buttons
            if (GUILayout.Button("💾  SAVE & CONTINUE", GUILayout.Height(45)))
            {
                OnSaveAndContinueClicked();
            }

            GUILayout.Space(8);
            if (GUILayout.Button("Back to Menu", GUILayout.Height(32)))
            {
                OnBackClicked();
            }

            GUILayout.EndArea();
        }

        private async void OnSaveAndContinueClicked()
        {
            if (_customService != null)
            {
                _customService.SetAppearance(_appearance);
                Lg.Info("Custom appearance saved to profile.");
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
