using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Romance
{
    /// <summary>
    /// Master coordinator for romantic interactions connecting detector, permissions, consent requests, and animation anchors.
    /// </summary>
    public sealed class InteractionCoordinator : MonoBehaviour
    {
        [SerializeField] private InteractionDetector _detector;
        [SerializeField] private InteractionPermissionService _permissions;
        [SerializeField] private InteractionRequestService _requestService;
        [SerializeField] private PairedAnimationController _pairedController;

        private void Awake()
        {
            if (_detector == null) _detector = GetComponent<InteractionDetector>();
            if (_requestService == null) _requestService = GetComponent<InteractionRequestService>();
            if (_permissions == null) _permissions = GetComponent<InteractionPermissionService>() ?? gameObject.AddComponent<InteractionPermissionService>();
            if (_pairedController == null) _pairedController = GetComponent<PairedAnimationController>() ?? gameObject.AddComponent<PairedAnimationController>();
        }

        private void Start()
        {
            if (_requestService != null)
            {
                _requestService.OnRequestAccepted += HandleRequestAccepted;
            }
        }

        private void OnDestroy()
        {
            if (_requestService != null)
            {
                _requestService.OnRequestAccepted -= HandleRequestAccepted;
            }
        }

        public bool TryRequestInteraction(InteractionType type)
        {
            if (_detector == null || !_detector.HasPartnerInRange)
            {
                Lg.Warning("Cannot request interaction: No partner in range.");
                return false;
            }

            var partner = _detector.NearbyPartner;
            if (!_permissions.CanInitiateInteraction(transform, partner, type, out var reason))
            {
                Lg.Warning($"Interaction denied: {reason}");
                return false;
            }

            _requestService.SendRequest("local_player", partner.name, type);
            return true;
        }

        private void HandleRequestAccepted(InteractionRequest request)
        {
            if (_detector != null && _detector.NearbyPartner != null)
            {
                _permissions.TriggerCooldown(request.Type);
                _pairedController.StartPairedInteraction(_detector.NearbyPartner, request.Type, true);
                EventBus.Publish(new RomanceInteractionTriggeredEvent(request.InitiatorId, request.TargetId, request.Type.ToString()));
            }
        }

        public void CancelCurrentInteraction()
        {
            _requestService?.CancelActive();
            _pairedController?.StopPairedInteraction();
        }
    }
}
