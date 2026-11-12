using UnityEngine;

namespace Loveverse.Movement
{
    /// <summary>
    /// Bridges runtime locomotion parameters to the character Animator controller.
    /// </summary>
    public sealed class MovementAnimationBridge : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
        private static readonly int StateHash = Animator.StringToHash("LocomotionState");

        public void UpdateParameters(float speed, bool isGrounded, bool isJumping, LocomotionState state)
        {
            if (_animator == null) return;

            _animator.SetFloat(SpeedHash, speed);
            _animator.SetBool(IsGroundedHash, isGrounded);
            _animator.SetBool(IsJumpingHash, isJumping);
            _animator.SetInteger(StateHash, (int)state);
        }
    }
}
