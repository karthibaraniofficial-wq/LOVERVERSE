using UnityEngine;

namespace Loveverse.Core
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Loveverse/Config/CameraConfig")]
    public sealed class CameraConfig : ScriptableObject
    {
        [Header("Distance & Offsets")]
        [SerializeField] private float _defaultDistance = 3.8f;
        [SerializeField] private float _minDistance = 1.5f;
        [SerializeField] private float _maxDistance = 6.5f;
        [SerializeField] private Vector3 _targetOffset = new Vector3(0f, 1.4f, 0f);

        [Header("Sensitivity")]
        [SerializeField] private float _mouseHorizontalSensitivity = 2.0f;
        [SerializeField] private float _mouseVerticalSensitivity = 1.5f;
        [SerializeField] private float _touchHorizontalSensitivity = 1.2f;
        [SerializeField] private float _touchVerticalSensitivity = 1.0f;

        [Header("Pitch Clamping")]
        [SerializeField] private float _minPitchAngle = -30f;
        [SerializeField] private float _maxPitchAngle = 65f;

        [Header("Collision & Smoothing")]
        [SerializeField] private float _collisionRadius = 0.2f;
        [SerializeField] private float _positionDamping = 0.15f;
        [SerializeField] private LayerMask _collisionLayers = ~0;

        public float DefaultDistance => _defaultDistance;
        public float MinDistance => _minDistance;
        public float MaxDistance => _maxDistance;
        public Vector3 TargetOffset => _targetOffset;
        public float MouseHorizontalSensitivity => _mouseHorizontalSensitivity;
        public float MouseVerticalSensitivity => _mouseVerticalSensitivity;
        public float TouchHorizontalSensitivity => _touchHorizontalSensitivity;
        public float TouchVerticalSensitivity => _touchVerticalSensitivity;
        public float MinPitchAngle => _minPitchAngle;
        public float MaxPitchAngle => _maxPitchAngle;
        public float CollisionRadius => _collisionRadius;
        public float PositionDamping => _positionDamping;
        public LayerMask CollisionLayers => _collisionLayers;
    }
}
