using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Movement
{
    /// <summary>
    /// Adapts input handling dynamically: activates touch joystick on Android/iOS and WASD/Mouse on Windows/Linux.
    /// Also supports universal gamepads and controllers.
    /// </summary>
    public sealed class PlatformInputAdapter : MonoBehaviour
    {
        [Header("UI Overlays")]
        [SerializeField] private GameObject _mobileTouchCanvas;

        private IPlatformService _platformService;
        private IInputService _inputService;

        private void Start()
        {
            ServiceRegistry.Instance.TryResolve(out _platformService);
            ServiceRegistry.Instance.TryResolve(out _inputService);

            ConfigureInputForPlatform();
        }

        public void ConfigureInputForPlatform()
        {
            bool isMobile = _platformService != null ? _platformService.IsMobilePlatform : Application.isMobilePlatform;

            if (_mobileTouchCanvas != null)
            {
                _mobileTouchCanvas.SetActive(isMobile);
            }

            Lg.Info($"PlatformInputAdapter configured: Touch Canvas active = {isMobile}");
        }

        public static bool IsGamepadConnected()
        {
            var joystickNames = Input.GetJoystickNames();
            foreach (var j in joystickNames)
            {
                if (!string.IsNullOrWhiteSpace(j))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
