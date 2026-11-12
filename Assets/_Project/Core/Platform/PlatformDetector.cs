using UnityEngine;

namespace Loveverse.Core
{
    /// <summary>
    /// Detects runtime platform and form factor across Android, iOS, Windows, and Linux.
    /// </summary>
    public static class PlatformDetector
    {
        public static PlatformType DetectPlatform()
        {
#if UNITY_ANDROID
            return PlatformType.Android;
#elif UNITY_IOS
            return PlatformType.iOS;
#elif UNITY_STANDALONE_WIN
            return PlatformType.Windows;
#elif UNITY_STANDALONE_LINUX
            return PlatformType.Linux;
#else
            // Fallback runtime evaluation
            return Application.platform switch
            {
                RuntimePlatform.Android => PlatformType.Android,
                RuntimePlatform.IPhonePlayer => PlatformType.iOS,
                RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor => PlatformType.Windows,
                RuntimePlatform.LinuxPlayer or RuntimePlatform.LinuxEditor => PlatformType.Linux,
                _ => PlatformType.Linux
            };
#endif
        }

        public static DeviceFormFactor DetectFormFactor()
        {
            var platform = DetectPlatform();
            if (platform == PlatformType.Windows || platform == PlatformType.Linux)
            {
                return DeviceFormFactor.Desktop;
            }

            // Mobile detection: screen diagonal in inches
            float dpi = Screen.dpi > 0 ? Screen.dpi : 160f;
            float widthInches = Screen.width / dpi;
            float heightInches = Screen.height / dpi;
            float diagonalInches = Mathf.Sqrt(widthInches * widthInches + heightInches * heightInches);

            // Greater than 7 inches is treated as Tablet (iPad or Android tablet)
            return diagonalInches >= 6.8f ? DeviceFormFactor.Tablet : DeviceFormFactor.MobilePhone;
        }

        public static bool IsMobile(PlatformType platform)
        {
            return platform == PlatformType.Android || platform == PlatformType.iOS;
        }

        public static bool IsDesktop(PlatformType platform)
        {
            return platform == PlatformType.Windows || platform == PlatformType.Linux;
        }
    }
}

