using System.Collections;
using UnityEngine;
using Loveverse.Core;
using Loveverse.Movement;

namespace Loveverse.Romance
{
    /// <summary>
    /// Calculates anchor offsets and smoothly aligns two characters into position for paired animations.
    /// </summary>
    public sealed class PairedAnimationController : MonoBehaviour
    {
        [SerializeField] private RomanceConfig _config;
        [SerializeField] private ThirdPersonController _controller;
        [SerializeField] private Animator _animator;

        private InteractionState _state = InteractionState.Idle;
        private Transform _partnerTransform;
        private InteractionType _activeType;

        public InteractionState State => _state;
        public bool IsPerformingInteraction => _state == InteractionState.Active || _state == InteractionState.Aligning;

        private static readonly int PairedActionHash = Animator.StringToHash("PairedAction");
        private static readonly int PairedTypeHash = Animator.StringToHash("PairedType");

        public void StartPairedInteraction(Transform partner, InteractionType type, bool isInitiator)
        {
            if (partner == null) return;

            _partnerTransform = partner;
            _activeType = type;
            _state = InteractionState.Aligning;

            if (_controller != null)
            {
                _controller.IsControlLocked = true;
            }

            StartCoroutine(AlignmentRoutine(isInitiator));
        }

        private IEnumerator AlignmentRoutine(bool isInitiator)
        {
            float duration = _config != null ? _config.AlignmentDuration : 0.35f;
            float elapsed = 0f;

            var startPos = transform.position;
            var startRot = transform.rotation;

            // Target positioning based on interaction type
            float offsetDist = GetOffsetDistance(_activeType);
            Vector3 midPoint = (_partnerTransform.position + transform.position) * 0.5f;

            Vector3 targetPos;
            Quaternion targetRot;

            if (_activeType == InteractionType.HoldHands)
            {
                // Side-by-side facing forward
                Vector3 forward = (_partnerTransform.forward + transform.forward).normalized;
                Vector3 right = Vector3.Cross(Vector3.up, forward);
                targetPos = midPoint + (isInitiator ? -right : right) * (offsetDist * 0.5f);
                targetRot = Quaternion.LookRotation(forward, Vector3.up);
            }
            else
            {
                // Face-to-face (Hug, Dance, Wave, etc.)
                Vector3 toPartner = (_partnerTransform.position - transform.position).normalized;
                toPartner.y = 0f;
                targetPos = midPoint - toPartner * (offsetDist * 0.5f);
                targetRot = Quaternion.LookRotation(toPartner, Vector3.up);
            }

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                transform.position = Vector3.Lerp(startPos, targetPos, t);
                transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
                yield return null;
            }

            transform.position = targetPos;
            transform.rotation = targetRot;

            _state = InteractionState.Active;
            TriggerPairedAnimation();
        }

        private void TriggerPairedAnimation()
        {
            if (_animator != null)
            {
                _animator.SetInteger(PairedTypeHash, (int)_activeType);
                _animator.SetTrigger(PairedActionHash);
            }
            Lg.Info($"Paired animation started: {_activeType}");
        }

        public void StopPairedInteraction()
        {
            if (_state == InteractionState.Idle) return;

            _state = InteractionState.Idle;
            _partnerTransform = null;

            if (_controller != null)
            {
                _controller.IsControlLocked = false;
            }

            if (_animator != null)
            {
                _animator.ResetTrigger(PairedActionHash);
            }

            Lg.Info("Paired interaction stopped.");
        }

        private float GetOffsetDistance(InteractionType type)
        {
            if (_config == null) return 0.7f;
            return type switch
            {
                InteractionType.Hug => _config.HugDistance,
                InteractionType.HoldHands => _config.HandHoldDistance,
                InteractionType.Dance => _config.DanceDistance,
                InteractionType.SitTogether => _config.SitTogetherDistance,
                _ => 1.2f
            };
        }
    }
}
