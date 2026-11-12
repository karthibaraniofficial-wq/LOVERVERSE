using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Loveverse.Core
{
    public interface IPlatformService : IGameService
    {
        PlatformType ActivePlatform { get; }
        DeviceFormFactor FormFactor { get; }
        bool IsMobilePlatform { get; }
        bool IsDesktopPlatform { get; }
        Rect SafeArea { get; }

        string PersistentStoragePath { get; }
        string CacheStoragePath { get; }

        void SetTargetFrameRate(int targetFps);
        void SetFullscreen(bool isFullscreen, int width = 1920, int height = 1080);
        Task<bool> RequestCameraPermissionAsync();
        Task<bool> RequestPhotoStoragePermissionAsync();
    }
}
