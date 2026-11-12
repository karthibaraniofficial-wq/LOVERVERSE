using System;
using System.Collections.Generic;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Romance
{
    /// <summary>
    /// Validates eligibility, range, and cooldowns for romantic interactions.
    /// </summary>
    public sealed class InteractionPermissionService : MonoBehaviour
    {
        [SerializeField] private RomanceConfig _config;
        private readonly Dictionary<string, float> _cooldowns = new Dictionary<string, float>();

        public bool CanInitiateInteraction(Transform player, Transform partner, InteractionType type, out string failureReason)
        {
            if (player == null || partner == null)
            {
                failureReason = "Target player not found.";
                return false;
            }

            // Check distance
            float maxRange = _config != null ? _config.InteractionRange : 2.5f;
            float distance = Vector3.Distance(player.position, partner.position);
            if (distance > maxRange)
            {
                failureReason = $"Target is too far away ({distance:F1}m > {maxRange:F1}m).";
                return false;
            }

            // Check Cooldown
            var key = $"{type}";
            if (_cooldowns.TryGetValue(key, out var expiry) && Time.time < expiry)
            {
                failureReason = "Interaction on cooldown. Please wait.";
                return false;
            }

            failureReason = string.Empty;
            return true;
        }

        public void TriggerCooldown(InteractionType type, float durationSeconds = 3.0f)
        {
            _cooldowns[$"{type}"] = Time.time + durationSeconds;
        }
    }
}
