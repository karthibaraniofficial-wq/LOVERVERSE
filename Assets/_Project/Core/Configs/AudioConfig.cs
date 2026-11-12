using UnityEngine;

namespace Loveverse.Core
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Loveverse/Config/AudioConfig")]
    public sealed class AudioConfig : ScriptableObject
    {
        [Header("Volume Defaults (0.0 - 1.0)")]
        [Range(0f, 1f)] [SerializeField] private float _masterVolume = 1.0f;
        [Range(0f, 1f)] [SerializeField] private float _musicVolume = 0.85f;
        [Range(0f, 1f)] [SerializeField] private float _sfxVolume = 1.0f;
        [Range(0f, 1f)] [SerializeField] private float _ambientVolume = 0.75f;

        [Header("3D Spatialization")]
        [SerializeField] private float _minDistance = 1.0f;
        [SerializeField] private float _maxDistance = 25.0f;
        [SerializeField] private AudioRolloffMode _rolloffMode = AudioRolloffMode.Logarithmic;

        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SFXVolume => _sfxVolume;
        public float AmbientVolume => _ambientVolume;
        public float MinDistance => _minDistance;
        public float MaxDistance => _maxDistance;
        public AudioRolloffMode RolloffMode => _rolloffMode;
    }
}
