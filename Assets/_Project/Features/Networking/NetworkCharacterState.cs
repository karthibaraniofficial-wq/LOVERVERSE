using UnityEngine;
using Loveverse.Core;
using Loveverse.Movement;
using Loveverse.Romance;

namespace Loveverse.Networking
{
    /// <summary>
    /// Synchronizes character transform and animation states across the network using interpolation.
    /// </summary>
    public sealed class NetworkCharacterState : MonoBehaviour
    {
        [Header("Authority")]
        [SerializeField] private bool _hasInputAuthority = true;
        public bool HasInputAuthority => _hasInputAuthority;

        [Header("Components")]
        [SerializeField] private ThirdPersonController _controller;
        [SerializeField] private PairedAnimationController _pairedController;
        [SerializeField] private Animator _animator;

        [Header("Interpolation Settings")]
        [SerializeField] private float _positionLerpSpeed = 15f;
        [SerializeField] private float _rotationLerpSpeed = 15f;

        // Replicated network buffers
        private Vector3 _networkPosition;
        private Quaternion _networkRotation;
        private float _networkSpeed;
        private int _networkLocomotionState;

        private void Start()
        {
            _networkPosition = transform.position;
            _networkRotation = transform.rotation;
        }

        private void Update()
        {
            if (_hasInputAuthority)
            {
                // Local Authority: Capture state to replicate over network
                _networkPosition = transform.position;
                _networkRotation = transform.rotation;
                if (_controller != null)
                {
                    _networkLocomotionState = (int)_controller.CurrentState;
                }
            }
            else
            {
                // Remote Proxy: Interpolate transform toward network state
                transform.position = Vector3.Lerp(transform.position, _networkPosition, Time.deltaTime * _positionLerpSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, _networkRotation, Time.deltaTime * _rotationLerpSpeed);

                if (_animator != null)
                {
                    _animator.SetInteger("LocomotionState", _networkLocomotionState);
                }
            }
        }

        /// <summary>
        /// Called when remote network packet arrives with updated snapshot.
        /// </summary>
        public void ReceiveSnapshot(Vector3 position, Quaternion rotation, float speed, int state)
        {
            if (_hasInputAuthority) return;

            _networkPosition = position;
            _networkRotation = rotation;
            _networkSpeed = speed;
            _networkLocomotionState = state;
        }
    }
}
