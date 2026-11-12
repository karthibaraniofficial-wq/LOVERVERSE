using System;
using System.Collections;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Romance
{
    public sealed class InteractionRequest
    {
        public string RequestId { get; } = Guid.NewGuid().ToString("N");
        public string InitiatorId { get; }
        public string TargetId { get; }
        public InteractionType Type { get; }
        public float TimeRemaining { get; set; }

        public InteractionRequest(string initiatorId, string targetId, InteractionType type, float timeoutSeconds)
        {
            InitiatorId = initiatorId;
            TargetId = targetId;
            Type = type;
            TimeRemaining = timeoutSeconds;
        }
    }

    /// <summary>
    /// Coordinates two-player consent workflow for romantic paired interactions.
    /// </summary>
    public sealed class InteractionRequestService : MonoBehaviour
    {
        [SerializeField] private RomanceConfig _config;

        private InteractionRequest _activeRequest;
        public InteractionRequest ActiveRequest => _activeRequest;

        public event Action<InteractionRequest> OnRequestReceived;
        public event Action<InteractionRequest> OnRequestAccepted;
        public event Action<InteractionRequest> OnRequestRejected;
        public event Action<InteractionRequest> OnRequestTimedOut;

        private void Update()
        {
            if (_activeRequest != null)
            {
                _activeRequest.TimeRemaining -= Time.deltaTime;
                if (_activeRequest.TimeRemaining <= 0f)
                {
                    var req = _activeRequest;
                    _activeRequest = null;
                    Lg.Info($"Interaction request timed out: {req.Type}");
                    OnRequestTimedOut?.Invoke(req);
                }
            }
        }

        public InteractionRequest SendRequest(string initiatorId, string targetId, InteractionType type)
        {
            float timeout = _config != null ? _config.RequestTimeoutSeconds : 15f;
            _activeRequest = new InteractionRequest(initiatorId, targetId, type, timeout);
            Lg.Info($"Sent interaction request: {type} to {targetId}");
            return _activeRequest;
        }

        public void ReceiveRequest(InteractionRequest request)
        {
            _activeRequest = request;
            Lg.Info($"Received interaction request: {request.Type} from {request.InitiatorId}");
            OnRequestReceived?.Invoke(_activeRequest);
        }

        public void AcceptRequest()
        {
            if (_activeRequest == null) return;
            var req = _activeRequest;
            _activeRequest = null;
            Lg.Info($"Accepted interaction request: {req.Type}");
            OnRequestAccepted?.Invoke(req);
        }

        public void RejectRequest()
        {
            if (_activeRequest == null) return;
            var req = _activeRequest;
            _activeRequest = null;
            Lg.Info($"Rejected interaction request: {req.Type}");
            OnRequestRejected?.Invoke(req);
        }

        public void CancelActive()
        {
            _activeRequest = null;
        }
    }
}
