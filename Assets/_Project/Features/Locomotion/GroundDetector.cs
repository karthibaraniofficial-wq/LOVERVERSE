using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Movement
{
    /// <summary>
    /// Evaluates ground contact, surface normal, and slope steepness for character locomotion.
    /// </summary>
    public sealed class GroundDetector : MonoBehaviour
    {
        [SerializeField] private MovementConfig _config;

        public bool IsGrounded { get; private set; }
        public Vector3 GroundNormal { get; private set; } = Vector3.up;
        public float SlopeAngle { get; private set; }

        public void EvaluateGround(Vector3 characterPosition)
        {
            if (_config == null) return;

            var origin = characterPosition + Vector3.down * _config.GroundCheckOffset;
            if (Physics.SphereCast(origin, _config.GroundCheckRadius, Vector3.down, out var hit, 0.2f, _config.GroundLayers, QueryTriggerInteraction.Ignore))
            {
                IsGrounded = true;
                GroundNormal = hit.normal;
                SlopeAngle = Vector3.Angle(Vector3.up, GroundNormal);
            }
            else
            {
                IsGrounded = false;
                GroundNormal = Vector3.up;
                SlopeAngle = 0f;
            }
        }
    }
}
