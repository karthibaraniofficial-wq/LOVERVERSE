using UnityEngine;
using Loveverse.Core;

namespace Loveverse.World
{
    public enum WeatherType
    {
        Clear,
        Cloudy,
        Rain,
        LightFog
    }

    /// <summary>
    /// Controls weather particle emissions, fog density, and sky occlusion.
    /// </summary>
    public sealed class WeatherController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _rainParticles;
        [SerializeField] private WeatherType _currentWeather = WeatherType.Clear;

        public WeatherType CurrentWeather => _currentWeather;

        public void SetWeather(WeatherType weather)
        {
            _currentWeather = weather;

            if (_rainParticles != null)
            {
                if (weather == WeatherType.Rain)
                {
                    _rainParticles.Play();
                }
                else
                {
                    _rainParticles.Stop();
                }
            }

            RenderSettings.fog = (weather == WeatherType.LightFog);
            if (weather == WeatherType.LightFog)
            {
                RenderSettings.fogDensity = 0.025f;
            }

            Lg.Info($"Weather transition complete: {weather}");
        }
    }

    /// <summary>
    /// Smoothly transitions contextual background ambient audio based on time and weather.
    /// </summary>
    public sealed class AmbientAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _dayAmbienceSource;
        [SerializeField] private AudioSource _nightAmbienceSource;
        [SerializeField] private AudioSource _rainAmbienceSource;

        public void UpdateAmbience(float timeOfDayHours, WeatherType weather)
        {
            bool isDay = timeOfDayHours >= 6f && timeOfDayHours < 18f;

            if (_dayAmbienceSource != null)
            {
                _dayAmbienceSource.volume = isDay ? 0.6f : 0f;
            }

            if (_nightAmbienceSource != null)
            {
                _nightAmbienceSource.volume = !isDay ? 0.6f : 0f;
            }

            if (_rainAmbienceSource != null)
            {
                _rainAmbienceSource.volume = (weather == WeatherType.Rain) ? 0.8f : 0f;
            }
        }
    }
}
