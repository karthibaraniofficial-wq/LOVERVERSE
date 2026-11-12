Shader "Loveverse/AnimeToonCharacter"
{
    Properties
    {
        _BaseMap ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Tint", Color) = (0.72, 0.65, 0.78, 1)
        _CelThreshold ("Cel Shadow Threshold", Range(0, 1)) = 0.45
        _CelSoftness ("Cel Shadow Softness", Range(0.001, 0.2)) = 0.05
        _RimColor ("Rim Light Color", Color) = (1, 0.9, 0.95, 1)
        _RimPower ("Rim Light Exponent", Range(1, 10)) = 4.0
        _RimIntensity ("Rim Intensity", Range(0, 2)) = 0.8
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                float2 uv         : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float4 _ShadowColor;
                float4 _RimColor;
                float  _CelThreshold;
                float  _CelSoftness;
                float  _RimPower;
                float  _RimIntensity;
            CBUFFER_END

            Varyings Vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normInputs = GetVertexNormalInputs(input.normalOS);

                output.positionCS = posInputs.positionCS;
                output.positionWS = posInputs.positionWS;
                output.normalWS = NormalizeNormalPerVertex(normInputs.normalWS);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                float3 normalWS = normalize(input.normalWS);
                float3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);

                // Main directional light
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(input.positionWS));
                float NdotL = dot(normalWS, mainLight.direction);
                float halfLambert = NdotL * 0.5 + 0.5;

                // Cel shading step
                float celStep = smoothstep(_CelThreshold - _CelSoftness, _CelThreshold + _CelSoftness, halfLambert * mainLight.shadowAttenuation);
                half3 diffuseLighting = lerp(_ShadowColor.rgb, half3(1, 1, 1), celStep) * mainLight.color;

                // Stylized Rim Light (Fresnel)
                float NdotV = 1.0 - saturate(dot(normalWS, viewDirWS));
                float rim = pow(NdotV, _RimPower) * _RimIntensity * celStep;
                half3 rimLighting = _RimColor.rgb * rim;

                half3 finalColor = albedo.rgb * diffuseLighting + rimLighting;
                return half4(finalColor, albedo.a);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
