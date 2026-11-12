using System;
using System.Collections;
using UnityEngine;
using Loveverse.Core;
using Loveverse.Movement;

namespace Loveverse.Camera
{
    /// <summary>
    /// Photo Mode controller allowing players to compose, filter, and capture screenshots.
    /// </summary>
    public sealed class PhotoModeController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _photoCamera;
        [SerializeField] private CanvasGroup _hudCanvasGroup;

        private bool _isPhotoModeActive;
        private float _originalFov;
        private IInputService _inputService;

        public bool IsPhotoModeActive => _isPhotoModeActive;

        public event Action<bool> OnPhotoModeToggled;
        public event Action<Texture2D> OnPhotoCaptured;

        private void Start()
        {
            if (_photoCamera == null)
            {
                _photoCamera = UnityEngine.Camera.main;
            }

            if (_photoCamera != null)
            {
                _originalFov = _photoCamera.fieldOfView;
            }

            if (ServiceRegistry.Instance.TryResolve<IInputService>(out var input))
            {
                _inputService = input;
            }
        }

        private void Update()
        {
            if (_inputService != null && _inputService.IsPhotoModeTriggered)
            {
                TogglePhotoMode();
            }
        }

        public void TogglePhotoMode()
        {
            _isPhotoModeActive = !_isPhotoModeActive;

            if (_hudCanvasGroup != null)
            {
                _hudCanvasGroup.alpha = _isPhotoModeActive ? 0f : 1f;
                _hudCanvasGroup.blocksRaycasts = !_isPhotoModeActive;
            }

            if (!_isPhotoModeActive && _photoCamera != null)
            {
                _photoCamera.fieldOfView = _originalFov;
            }

            OnPhotoModeToggled?.Invoke(_isPhotoModeActive);
            Lg.Info($"Photo Mode active: {_isPhotoModeActive}");
        }

        public void SetFov(float fov)
        {
            if (_photoCamera != null && _isPhotoModeActive)
            {
                _photoCamera.fieldOfView = Mathf.Clamp(fov, 20f, 85f);
            }
        }

        public void CaptureScreenshot()
        {
            StartCoroutine(CaptureRoutine());
        }

        private IEnumerator CaptureRoutine()
        {
            yield return new WaitForEndOfFrame();

            int width = Screen.width;
            int height = Screen.height;
            var texture = new Texture2D(width, height, TextureFormat.RGB24, false);

            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();

            Lg.Info($"Photo captured successfully: {width}x{height}");
            OnPhotoCaptured?.Invoke(texture);
        }
    }
}
