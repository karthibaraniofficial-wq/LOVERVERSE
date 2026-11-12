using UnityEngine;
using Loveverse.Romance;

namespace Loveverse.Networking
{
    /// <summary>
    /// Replicates active paired romantic interaction state across network clients.
    /// </summary>
    public sealed class NetworkInteractionState : MonoBehaviour
    {
        [SerializeField] private bool _isInteracting;
        [SerializeField] private int _interactionTypeIndex;
        [SerializeField] private string _partnerPlayerId;

        public bool IsInteracting => _isInteracting;
        public InteractionType CurrentInteractionType => (InteractionType)_interactionTypeIndex;
        public string PartnerPlayerId => _partnerPlayerId;

        public void SetInteraction(InteractionType type, string partnerId)
        {
            _isInteracting = true;
            _interactionTypeIndex = (int)type;
            _partnerPlayerId = partnerId;
        }

        public void ClearInteraction()
        {
            _isInteracting = false;
            _interactionTypeIndex = -1;
            _partnerPlayerId = string.Empty;
        }
    }
}
