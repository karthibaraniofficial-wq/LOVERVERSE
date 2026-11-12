using UnityEngine;
using Loveverse.Core;
using Loveverse.Movement;

namespace Loveverse.Camera
{
    public enum CameraMode
    {
        ThirdPersonExplore,
        Inspect,
        PhotoMode,
        PairedInteraction
    }

    /// <summary>
    /// Orbiting third-person camera controller with wall collision avoidance and multi-mode framing.
    /// </summary>
    public sealed class CameraController : MonoBehaviour
    {
        [Header("Target & Config")]
        [SerializeField] private Transform _target;
        [SerializeField] private CameraConfig _config;

        [Header("Mode & State")]
        [SerializeField] private CameraMode _currentMode = CameraMode.ThirdPersonExplore;

        private float _yaw;
        private float _pitch;
        private float _currentDistance;
        private float _targetDistance;
        private Vector3 _smoothVelocity;

        private IInputService _inputService;

        public CameraMode CurrentMode => _currentMode;

        private void Start()
        {
            if (ServiceRegistry.Instance.TryResolve<IInputService>(out var input))
            {
                _inputService = input;
            }

            _targetDistance = _config != null ? _config.DefaultDistance : 3.5f;
            _currentDistance = _targetDistance;

            var euler = transform.eulerAngles;
            _yaw = euler.y;
            _pitch = euler.x;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void SetCameraMode(CameraMode mode)
        {
            _currentMode = mode;
            switch (mode)
            {
                case CameraMode.Inspect:
                    _targetDistance = 1.8f;
                    break;
                case CameraMode.PairedInteraction:
                    _targetDistance = 2.8f;
                    break;
                case CameraMode.ThirdPersonExplore:
                default:
                    _targetDistance = _config.DefaultDistance;
                    break;
            }
        }

        private void LateUpdate()
        {
            if (_target == null || _config == null) return;

            HandleOrbit();
            HandleCollisionAndPosition();
        }

        private void HandleOrbit()
        {
            if (_inputService == null) return;

            var look = _inputService.LookInput;
            float sensX = Application.isMobilePlatform ? _config.TouchHorizontalSensitivity : _config.MouseHorizontalSensitivity;
            float sensY = Application.isMobilePlatform ? _config.TouchVerticalSensitivity : _config.MouseVerticalSensitivity;

            _yaw += look.x * sensX;
            _pitch -= look.y * sensY;
            _pitch = Mathf.Clamp(_pitch, _config.MinPitchAngle, _config.MaxPitchAngle);
        }

        private void HandleCollisionAndPosition()
        {
            var targetPivot = _target.position + _config.TargetOffset;
            var rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            var desiredDirection = rotation * Vector3.back;

            // Collision avoidance via SphereCast
            float desiredDistance = _targetDistance;
            if (Physics.SphereCast(targetPivot, _config.CollisionRadius, desiredDirection, out var hit, _targetDistance, _config.CollisionLayers, QueryTriggerInteraction.Ignore))
            {
                desiredDistance = Mathf.Clamp(hit.distance - _config.CollisionRadius, _config.MinDistance, _targetDistance);
            }

            _currentDistance = Mathf.Lerp(_currentDistance, desiredDistance, Time.deltaTime / _config.PositionDamping);
            var targetPosition = targetPivot + desiredDirection * _currentDistance;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _smoothVelocity, _config.PositionDamping);
            transform.LookAt(targetPivot);
        }
    }
}
