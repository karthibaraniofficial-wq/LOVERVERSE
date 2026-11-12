using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Movement
{
    /// <summary>
    /// Production third-person character controller supporting both Android touch and PC input.
    /// Drives CharacterController with slope snapping, camera-relative movement, and animation states.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class ThirdPersonController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private MovementConfig _config;
        [SerializeField] private UnityEngine.Camera _mainCamera;
        [SerializeField] private Animator _animator;

        [Header("State")]
        [SerializeField] private LocomotionState _currentState = LocomotionState.Idle;
        public LocomotionState CurrentState => _currentState;

        private CharacterController _characterController;
        private IInputService _inputService;

        private Vector3 _velocity;
        private float _currentSpeed;
        private bool _isGrounded;

        // Cached Animator Parameter Hashes
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
        private static readonly int StateHash = Animator.StringToHash("LocomotionState");

        public bool IsControlLocked { get; set; }

        private void Awake()
        {
            if (_config == null)
            {
                _config = ScriptableObject.CreateInstance<MovementConfig>();
            }

            _characterController = GetComponent<CharacterController>();
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }

            if (_mainCamera == null)
            {
                _mainCamera = UnityEngine.Camera.main;
            }
        }

        private void Start()
        {
            if (ServiceRegistry.Instance.TryResolve<IInputService>(out var input))
            {
                _inputService = input;
            }
            else
            {
                _inputService = gameObject.AddComponent<InputReader>();
                ServiceRegistry.Instance.Register<IInputService>(_inputService);
            }
        }

        private void Update()
        {
            CheckGrounded();

            if (IsControlLocked)
            {
                ApplyGravity();
                _characterController.Move(_velocity * Time.deltaTime);
                UpdateAnimator(0f);
                return;
            }

            HandleMovement();
            HandleJumpAndGravity();
            UpdateAnimator(_currentSpeed);
        }

        private void CheckGrounded()
        {
            var checkPos = transform.position + Vector3.down * _config.GroundCheckOffset;
            _isGrounded = Physics.CheckSphere(checkPos, _config.GroundCheckRadius, _config.GroundLayers, QueryTriggerInteraction.Ignore);

            if (_isGrounded && _velocity.y < 0f)
            {
                _velocity.y = -2f; // Slight downward snap
            }
        }

        private void HandleMovement()
        {
            var moveInput = _inputService.MoveInput;
            var hasInput = moveInput.sqrMagnitude > 0.01f;

            // Camera-relative direction
            var camForward = _mainCamera != null ? _mainCamera.transform.forward : Vector3.forward;
            var camRight = _mainCamera != null ? _mainCamera.transform.right : Vector3.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            var targetDirection = (camRight * moveInput.x + camForward * moveInput.y).normalized;

            // Determine target speed
            float targetSpeed = 0f;
            if (hasInput)
            {
                if (_inputService.IsSprintPressed)
                {
                    targetSpeed = _config.SprintSpeed;
                    _currentState = LocomotionState.Sprint;
                }
                else if (moveInput.magnitude > 0.7f)
                {
                    targetSpeed = _config.RunSpeed;
                    _currentState = LocomotionState.Run;
                }
                else
                {
                    targetSpeed = _config.WalkSpeed;
                    _currentState = LocomotionState.Walk;
                }
            }
            else
            {
                _currentState = LocomotionState.Idle;
            }

            // Smooth acceleration / deceleration
            float accelRate = hasInput ? _config.Acceleration : _config.Deceleration;
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, accelRate * Time.deltaTime);

            // Rotate towards movement direction
            if (hasInput && targetDirection.sqrMagnitude > 0.001f)
            {
                var targetRot = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, _config.RotationSpeed * Time.deltaTime);
            }

            var horizontalMovement = targetDirection * _currentSpeed;
            _characterController.Move((horizontalMovement + _velocity) * Time.deltaTime);
        }

        private void HandleJumpAndGravity()
        {
            if (_isGrounded)
            {
                if (_inputService.IsJumpTriggered)
                {
                    // v = sqrt(2 * h * -g)
                    _velocity.y = Mathf.Sqrt(2f * _config.JumpHeight * Mathf.Abs(_config.Gravity));
                    _currentState = LocomotionState.Jump;
                }
            }
            else
            {
                _currentState = _velocity.y > 0 ? LocomotionState.InAir : LocomotionState.Fall;
            }

            ApplyGravity();
        }

        private void ApplyGravity()
        {
            _velocity.y += _config.Gravity * Time.deltaTime;
        }

        private void UpdateAnimator(float speed)
        {
            if (_animator == null) return;

            _animator.SetFloat(SpeedHash, speed);
            _animator.SetBool(IsGroundedHash, _isGrounded);
            _animator.SetBool(IsJumpingHash, !_isGrounded && _velocity.y > 0);
            _animator.SetInteger(StateHash, (int)_currentState);
        }

        public void Teleport(Vector3 position, Quaternion rotation)
        {
            _characterController.enabled = false;
            transform.position = position;
            transform.rotation = rotation;
            _characterController.enabled = true;
        }

        private void OnDrawGizmosSelected()
        {
            if (_config == null) return;
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            var checkPos = transform.position + Vector3.down * _config.GroundCheckOffset;
            Gizmos.DrawWireSphere(checkPos, _config.GroundCheckRadius);
        }
    }
}
