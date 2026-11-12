using System;
using UnityEngine;

namespace Loveverse.Core
{
    [Serializable]
    public sealed class PlatformSettingEntry
    {
        public PlatformType Platform;
        public int TargetFrameRate = 60;
        public QualityTier DefaultQualityTier = QualityTier.High;
        public bool EnableDynamicResolution = false;
        public bool EnableVirtualJoystick = false;
        public bool SupportController = true;
    }

    [CreateAssetMenu(fileName = "PlatformConfiguration", menuName = "Loveverse/Config/PlatformConfiguration")]
    public sealed class PlatformConfiguration : ScriptableObject
    {
        [SerializeField] private PlatformSettingEntry _androidSettings = new PlatformSettingEntry
        {
            Platform = PlatformType.Android,
            TargetFrameRate = 60,
            DefaultQualityTier = QualityTier.Medium,
            EnableDynamicResolution = true,
            EnableVirtualJoystick = true,
            SupportController = true
        };

        [SerializeField] private PlatformSettingEntry _iosSettings = new PlatformSettingEntry
        {
            Platform = PlatformType.iOS,
            TargetFrameRate = 60,
            DefaultQualityTier = QualityTier.High,
            EnableDynamicResolution = true,
            EnableVirtualJoystick = true,
            SupportController = true
        };

        [SerializeField] private PlatformSettingEntry _windowsSettings = new PlatformSettingEntry
        {
            Platform = PlatformType.Windows,
            TargetFrameRate = 120,
            DefaultQualityTier = QualityTier.Ultra,
            EnableDynamicResolution = false,
            EnableVirtualJoystick = false,
            SupportController = true
        };

        [SerializeField] private PlatformSettingEntry _linuxSettings = new PlatformSettingEntry
        {
            Platform = PlatformType.Linux,
            TargetFrameRate = 120,
            DefaultQualityTier = QualityTier.High,
            EnableDynamicResolution = false,
            EnableVirtualJoystick = false,
            SupportController = true
        };

        public PlatformSettingEntry GetSettingsForPlatform(PlatformType platform)
        {
            return platform switch
            {
                PlatformType.Android => _androidSettings,
                PlatformType.iOS => _iosSettings,
                PlatformType.Windows => _windowsSettings,
                PlatformType.Linux => _linuxSettings,
                _ => _linuxSettings
            };
        }
    }
}
