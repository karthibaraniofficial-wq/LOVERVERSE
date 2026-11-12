using System;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Romance
{
    /// <summary>
    /// Scans surroundings for nearby partner characters within interaction range and angle.
    /// </summary>
    public sealed class InteractionDetector : MonoBehaviour
    {
        [SerializeField] private RomanceConfig _config;
        [SerializeField] private LayerMask _playerLayer = ~0;

        private Transform _nearbyPartner;
        public Transform NearbyPartner => _nearbyPartner;
        public bool HasPartnerInRange => _nearbyPartner != null;

        public event Action<Transform> OnPartnerEnteredRange;
        public event Action OnPartnerExitedRange;

        private void Update()
        {
            if (_config == null) return;

            var colliders = Physics.OverlapSphere(transform.position, _config.InteractionRange, _playerLayer, QueryTriggerInteraction.Ignore);
            Transform closest = null;
            float closestDist = float.MaxValue;

            foreach (var col in colliders)
            {
                if (col.transform == transform || col.transform.IsChildOf(transform)) continue;

                var dir = (col.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, dir);

                if (angle <= _config.MaxAngleDegrees)
                {
                    float dist = Vector3.Distance(transform.position, col.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = col.transform;
                    }
                }
            }

            if (closest != _nearbyPartner)
            {
                _nearbyPartner = closest;
                if (_nearbyPartner != null)
                {
                    OnPartnerEnteredRange?.Invoke(_nearbyPartner);
                }
                else
                {
                    OnPartnerExitedRange?.Invoke();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_config == null) return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _config.InteractionRange);
        }
    }
}
