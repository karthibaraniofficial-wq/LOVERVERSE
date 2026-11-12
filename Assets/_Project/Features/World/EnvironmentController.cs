using UnityEngine;
using Loveverse.Core;

namespace Loveverse.World
{
    /// <summary>
    /// Simulates a 24-hour day/night cycle, rotates sun light, and interpolates ambient colors.
    /// </summary>
    public sealed class EnvironmentController : MonoBehaviour
    {
        [SerializeField] private WorldConfig _config;
        [SerializeField] private Light _sunLight;

        [Range(0f, 24f)]
        [SerializeField] private float _currentTimeHours = 17.5f; // Golden Hour default

        public float CurrentTimeHours => _currentTimeHours;

        private void Start()
        {
            if (_config != null)
            {
                _currentTimeHours = _config.InitialTimeOfDayHours;
            }
        }

        private void Update()
        {
            if (_config == null) return;

            // Advance time
            float dayDuration = _config.DayCycleDurationSeconds > 0 ? _config.DayCycleDurationSeconds : 1200f;
            _currentTimeHours += (24f / dayDuration) * Time.deltaTime;
            if (_currentTimeHours >= 24f)
            {
                _currentTimeHours -= 24f;
            }

            UpdateLighting();
        }

        private void UpdateLighting()
        {
            if (_sunLight == null) return;

            // Sun rotation (0h = midnight, 6h = sunrise, 12h = noon, 18h = sunset)
            float sunAngle = (_currentTimeHours / 24f) * 360f - 90f;
            _sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

            // Ambient color interpolation
            if (_currentTimeHours >= 6f && _currentTimeHours < 16.5f)
            {
                // Full Day
                RenderSettings.ambientLight = _config.DayAmbientColor;
                _sunLight.intensity = 1.2f;
            }
            else if (_currentTimeHours >= 16.5f && _currentTimeHours < 19.5f)
            {
                // Golden Hour / Sunset
                float t = Mathf.InverseLerp(16.5f, 19.5f, _currentTimeHours);
                RenderSettings.ambientLight = Color.Lerp(_config.DayAmbientColor, _config.SunsetAmbientColor, t);
                _sunLight.intensity = Mathf.Lerp(1.2f, 0.6f, t);
            }
            else
            {
                // Night
                RenderSettings.ambientLight = _config.NightAmbientColor;
                _sunLight.intensity = 0.1f;
            }
        }

        public void SetTimeOfDay(float hours)
        {
            _currentTimeHours = Mathf.Clamp(hours, 0f, 24f);
            UpdateLighting();
        }
    }
}
