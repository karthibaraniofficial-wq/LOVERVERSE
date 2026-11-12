using UnityEngine;

namespace Loveverse.Core
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Loveverse/Config/MovementConfig")]
    public sealed class MovementConfig : ScriptableObject
    {
        [Header("Locomotion Speeds")]
        [Tooltip("Walking speed in meters per second.")]
        [SerializeField] private float _walkSpeed = 2.4f;

        [Tooltip("Running speed in meters per second.")]
        [SerializeField] private float _runSpeed = 5.2f;

        [Tooltip("Sprinting speed in meters per second.")]
        [SerializeField] private float _sprintSpeed = 7.8f;

        [Header("Acceleration & Rotation")]
        [SerializeField] private float _acceleration = 12f;
        [SerializeField] private float _deceleration = 16f;
        [SerializeField] private float _rotationSpeed = 720f;

        [Header("Physics & Jumping")]
        [SerializeField] private float _jumpHeight = 1.4f;
        [SerializeField] private float _gravity = -18.0f;
        [SerializeField] private float _groundCheckRadius = 0.28f;
        [SerializeField] private float _groundCheckOffset = 0.15f;
        [SerializeField] private LayerMask _groundLayers = ~0;

        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        public float SprintSpeed => _sprintSpeed;
        public float Acceleration => _acceleration;
        public float Deceleration => _deceleration;
        public float RotationSpeed => _rotationSpeed;
        public float JumpHeight => _jumpHeight;
        public float Gravity => _gravity;
        public float GroundCheckRadius => _groundCheckRadius;
        public float GroundCheckOffset => _groundCheckOffset;
        public LayerMask GroundLayers => _groundLayers;
    }
}
