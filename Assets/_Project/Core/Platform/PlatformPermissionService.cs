using System.Threading.Tasks;
using UnityEngine;

namespace Loveverse.Core
{
    /// <summary>
    /// Cross-platform permission handler abstracting Android runtime requests, iOS privacy prompts, and desktop environments.
    /// </summary>
    public static class PlatformPermissionService
    {
        public static Task<bool> RequestCameraPermissionAsync()
        {
            var platform = PlatformDetector.DetectPlatform();

            if (platform == PlatformType.Windows || platform == PlatformType.Linux)
            {
                // Desktop platforms do not require mobile runtime permissions
                return Task.FromResult(true);
            }

#if UNITY_ANDROID
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
            {
                var tcs = new TaskCompletionSource<bool>();
                var callbacks = new UnityEngine.Android.PermissionCallbacks();
                callbacks.PermissionGranted += _ => tcs.TrySetResult(true);
                callbacks.PermissionDenied += _ => tcs.TrySetResult(false);
                callbacks.PermissionDeniedAndDontAskAgain += _ => tcs.TrySetResult(false);
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera, callbacks);
                return tcs.Task;
            }
            return Task.FromResult(true);
#elif UNITY_IOS
            return Task.FromResult(Application.HasUserAuthorization(UserAuthorization.WebCam));
#else
            return Task.FromResult(true);
#endif
        }

        public static Task<bool> RequestStoragePermissionAsync()
        {
            var platform = PlatformDetector.DetectPlatform();
            if (platform == PlatformType.Windows || platform == PlatformType.Linux)
            {
                return Task.FromResult(true);
            }

#if UNITY_ANDROID
            // Android 13+ (API 33+) uses granular media permissions, persistentDataPath requires no permission
            return Task.FromResult(true);
#elif UNITY_IOS
            return Task.FromResult(true);
#else
            return Task.FromResult(true);
#endif
        }
    }
}

