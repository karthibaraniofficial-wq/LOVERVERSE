using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Movement
{
    /// <summary>
    /// Smoothly rotates character towards motion trajectory aligned with active camera view.
    /// </summary>
    public sealed class CharacterRotationController : MonoBehaviour
    {
        [SerializeField] private MovementConfig _config;

        public void RotateTowards(Vector3 targetDirection, float deltaTime)
        {
            if (_config == null || targetDirection.sqrMagnitude < 0.001f) return;

            targetDirection.y = 0f;
            var targetRotation = Quaternion.LookRotation(targetDirection.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _config.RotationSpeed * deltaTime);
        }
    }
}
