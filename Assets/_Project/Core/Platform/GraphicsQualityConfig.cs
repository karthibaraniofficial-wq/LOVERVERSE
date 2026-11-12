using System;
using UnityEngine;

namespace Loveverse.Core
{
    [Serializable]
    public sealed class QualityPresetData
    {
        public QualityTier Tier;
        public float RenderScale = 1.0f;
        public int ShadowDistance = 50;
        public float LodBias = 1.0f;
        public int AnisotropicFiltering = 2; // 0=Disable, 1=Enable, 2=ForceEnable
        public bool SoftShadows = true;
        public bool PostProcessingEnabled = true;
    }

    [CreateAssetMenu(fileName = "GraphicsQualityConfig", menuName = "Loveverse/Config/GraphicsQualityConfig")]
    public sealed class GraphicsQualityConfig : ScriptableObject
    {
        [SerializeField] private QualityPresetData _low = new QualityPresetData
        {
            Tier = QualityTier.Low,
            RenderScale = 0.75f,
            ShadowDistance = 20,
            LodBias = 0.7f,
            AnisotropicFiltering = 0,
            SoftShadows = false,
            PostProcessingEnabled = false
        };

        [SerializeField] private QualityPresetData _medium = new QualityPresetData
        {
            Tier = QualityTier.Medium,
            RenderScale = 0.9f,
            ShadowDistance = 35,
            LodBias = 1.0f,
            AnisotropicFiltering = 1,
            SoftShadows = true,
            PostProcessingEnabled = true
        };

        [SerializeField] private QualityPresetData _high = new QualityPresetData
        {
            Tier = QualityTier.High,
            RenderScale = 1.0f,
            ShadowDistance = 60,
            LodBias = 1.5f,
            AnisotropicFiltering = 2,
            SoftShadows = true,
            PostProcessingEnabled = true
        };

        [SerializeField] private QualityPresetData _ultra = new QualityPresetData
        {
            Tier = QualityTier.Ultra,
            RenderScale = 1.0f,
            ShadowDistance = 100,
            LodBias = 2.0f,
            AnisotropicFiltering = 2,
            SoftShadows = true,
            PostProcessingEnabled = true
        };

        public QualityPresetData GetPreset(QualityTier tier)
        {
            return tier switch
            {
                QualityTier.Low => _low,
                QualityTier.Medium => _medium,
                QualityTier.High => _high,
                QualityTier.Ultra => _ultra,
                _ => _medium
            };
        }
    }
}
