using UnityEngine;

namespace Loveverse.Core
{
    [CreateAssetMenu(fileName = "RomanceConfig", menuName = "Loveverse/Config/RomanceConfig")]
    public sealed class RomanceConfig : ScriptableObject
    {
        [Header("Detection & Proximity")]
        [SerializeField] private float _interactionRange = 2.4f;
        [SerializeField] private float _maxAngleDegrees = 85f;
        [SerializeField] private float _requestTimeoutSeconds = 15f;

        [Header("Anchor Alignment")]
        [SerializeField] private float _alignmentDuration = 0.35f;
        [SerializeField] private float _handHoldDistance = 0.75f;
        [SerializeField] private float _hugDistance = 0.55f;
        [SerializeField] private float _danceDistance = 0.90f;
        [SerializeField] private float _sitTogetherDistance = 0.65f;

        public float InteractionRange => _interactionRange;
        public float MaxAngleDegrees => _maxAngleDegrees;
        public float RequestTimeoutSeconds => _requestTimeoutSeconds;
        public float AlignmentDuration => _alignmentDuration;
        public float HandHoldDistance => _handHoldDistance;
        public float HugDistance => _hugDistance;
        public float DanceDistance => _danceDistance;
        public float SitTogetherDistance => _sitTogetherDistance;
    }
}
