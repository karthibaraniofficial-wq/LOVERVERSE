using UnityEngine;

namespace Loveverse.Core
{
    [CreateAssetMenu(fileName = "WorldConfig", menuName = "Loveverse/Config/WorldConfig")]
    public sealed class WorldConfig : ScriptableObject
    {
        [Header("Day & Night Simulation")]
        [Tooltip("Length of a full in-game 24h day cycle in real-world seconds.")]
        [SerializeField] private float _dayCycleDurationSeconds = 1200f; // 20 minutes

        [Range(0f, 24f)]
        [SerializeField] private float _initialTimeOfDayHours = 17.5f; // 5:30 PM (Golden Hour)

        [Header("Atmosphere")]
        [SerializeField] private Color _dayAmbientColor = new Color(0.85f, 0.90f, 1.0f);
        [SerializeField] private Color _sunsetAmbientColor = new Color(1.0f, 0.65f, 0.45f);
        [SerializeField] private Color _nightAmbientColor = new Color(0.18f, 0.20f, 0.35f);

        public float DayCycleDurationSeconds => _dayCycleDurationSeconds;
        public float InitialTimeOfDayHours => _initialTimeOfDayHours;
        public Color DayAmbientColor => _dayAmbientColor;
        public Color SunsetAmbientColor => _sunsetAmbientColor;
        public Color NightAmbientColor => _nightAmbientColor;
    }
}
