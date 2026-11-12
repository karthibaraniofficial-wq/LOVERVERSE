using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loveverse.Characters
{
    [Serializable]
    public sealed class ColorPaletteOption
    {
        public string Id;
        public string DisplayName;
        public string ColorHex;
        public Color Color => ColorUtility.TryParseHtmlString(ColorHex, out var c) ? c : Color.white;
    }

    [CreateAssetMenu(fileName = "EyeRegistry", menuName = "Loveverse/Customization/Registries/Eyes")]
    public sealed class EyeRegistry : ScriptableObject
    {
        [SerializeField] private List<ColorPaletteOption> _eyeColors = new List<ColorPaletteOption>();
        public IReadOnlyList<ColorPaletteOption> EyeColors => _eyeColors;
    }

    [CreateAssetMenu(fileName = "SkinRegistry", menuName = "Loveverse/Customization/Registries/Skins")]
    public sealed class SkinRegistry : ScriptableObject
    {
        [SerializeField] private List<ColorPaletteOption> _skinTones = new List<ColorPaletteOption>();
        public IReadOnlyList<ColorPaletteOption> SkinTones => _skinTones;
    }

    [CreateAssetMenu(fileName = "OutfitRegistry", menuName = "Loveverse/Customization/Registries/Outfits")]
    public sealed class OutfitRegistry : ScriptableObject
    {
        [SerializeField] private List<CustomizationItem> _tops = new List<CustomizationItem>();
        [SerializeField] private List<CustomizationItem> _bottoms = new List<CustomizationItem>();
        [SerializeField] private List<CustomizationItem> _shoes = new List<CustomizationItem>();

        public IReadOnlyList<CustomizationItem> Tops => _tops;
        public IReadOnlyList<CustomizationItem> Bottoms => _bottoms;
        public IReadOnlyList<CustomizationItem> Shoes => _shoes;
    }
}
