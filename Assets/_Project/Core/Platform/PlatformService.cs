using System.Threading.Tasks;
using UnityEngine;

namespace Loveverse.Core
{
    /// <summary>
    /// Core implementation of IPlatformService registered with ServiceRegistry.
    /// Manages framerate targets, safe-area queries, windowing, and permissions.
    /// </summary>
    public sealed class PlatformService : MonoBehaviour, IPlatformService
    {
        public bool IsInitialized { get; private set; }

        public PlatformType ActivePlatform => PlatformDetector.DetectPlatform();
        public DeviceFormFactor FormFactor => PlatformDetector.DetectFormFactor();
        public bool IsMobilePlatform => PlatformDetector.IsMobile(ActivePlatform);
        public bool IsDesktopPlatform => PlatformDetector.IsDesktop(ActivePlatform);

        public Rect SafeArea => Screen.safeArea;
        public string PersistentStoragePath => PlatformStorageAdapter.GetSafeStoragePath();
        public string CacheStoragePath => PlatformStorageAdapter.GetCacheDirectory();

        [SerializeField] private PlatformConfiguration _configuration;

        public Task InitializeAsync()
        {
            ApplyPlatformDefaults();
            IsInitialized = true;
            Lg.Info($"PlatformService initialized. Active Platform: {ActivePlatform}, Form Factor: {FormFactor}");
            return Task.CompletedTask;
        }

        public void Shutdown()
        {
            IsInitialized = false;
        }

        private void ApplyPlatformDefaults()
        {
            if (_configuration != null)
            {
                var settings = _configuration.GetSettingsForPlatform(ActivePlatform);
                SetTargetFrameRate(settings.TargetFrameRate);
            }
            else
            {
                SetTargetFrameRate(IsMobilePlatform ? 60 : 120);
            }
        }

        public void SetTargetFrameRate(int targetFps)
        {
            Application.targetFrameRate = targetFps;
            Lg.Info($"Target framerate set to: {targetFps} FPS");
        }

        public void SetFullscreen(bool isFullscreen, int width = 1920, int height = 1080)
        {
            if (IsDesktopPlatform)
            {
                Screen.SetResolution(width, height, isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
                Lg.Info($"Window display updated: {width}x{height}, Fullscreen: {isFullscreen}");
            }
        }

        public Task<bool> RequestCameraPermissionAsync()
        {
            return PlatformPermissionService.RequestCameraPermissionAsync();
        }

        public Task<bool> RequestPhotoStoragePermissionAsync()
        {
            return PlatformPermissionService.RequestStoragePermissionAsync();
        }
    }
}
