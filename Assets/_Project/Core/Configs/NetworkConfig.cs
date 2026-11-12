using UnityEngine;

namespace Loveverse.Core
{
    [CreateAssetMenu(fileName = "NetworkConfig", menuName = "Loveverse/Config/NetworkConfig")]
    public sealed class NetworkConfig : ScriptableObject
    {
        [Header("Photon Fusion Configuration")]
        [SerializeField] private string _appId = "YOUR_FUSION_APP_ID";
        [SerializeField] private string _region = "asia";
        [SerializeField] private int _tickRate = 60;

        [Header("Private Room Rules")]
        [SerializeField] private int _maxRoomCapacity = 2;
        [SerializeField] private float _roomCodeExpirationMinutes = 60f;
        [SerializeField] private float _reconnectTimeoutSeconds = 30f;

        [Header("Interpolation & Bandwidth")]
        [SerializeField] private float _positionInterpolationSpeed = 15f;
        [SerializeField] private float _rotationInterpolationSpeed = 15f;
        [SerializeField] private int _packetsPerSecond = 20;

        public string AppId => _appId;
        public string Region => _region;
        public int TickRate => _tickRate;
        public int MaxRoomCapacity => _maxRoomCapacity;
        public float RoomCodeExpirationMinutes => _roomCodeExpirationMinutes;
        public float ReconnectTimeoutSeconds => _reconnectTimeoutSeconds;
        public float PositionInterpolationSpeed => _positionInterpolationSpeed;
        public float RotationInterpolationSpeed => _rotationInterpolationSpeed;
        public int PacketsPerSecond => _packetsPerSecond;
    }
}
