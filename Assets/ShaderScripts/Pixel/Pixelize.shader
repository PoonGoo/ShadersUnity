Shader "Hidden/Pixelize"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white"
    }

        SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
        }

        HLSLINCLUDE
        #pragma vertex vert
        #pragma fragment frag

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        struct Attributes
        {
            float4 positionOS : POSITION;
            float2 uv         : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionHCS : SV_POSITION;
            float2 uv         : TEXCOORD0;
        };

        TEXTURE2D(_MainTex);
        float4 _MainTex_TexelSize;
        float4 _MainTex_ST;

        SamplerState sampler_point_clamp;

        // Pixelization grid
        uniform float2 _BlockCount;
        uniform float2 _BlockSize;
        uniform float2 _HalfBlockSize;

        // Wave distortion uniforms
        uniform float _WaveFrequency;
        uniform float _WaveSpeed;
        uniform float _WaveAmplitude;

        // Glitch uniforms
        uniform float _GlitchIntensity;
        uniform float _GlitchFrequency;
        uniform float _EnableGlitch;

        // RGB Split uniforms
        uniform float _RGBSplitAmount;
        uniform float _EnableRGBSplit;

        Varyings vert(Attributes IN)
        {
            Varyings OUT;
            OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
            OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
            return OUT;
        }

        ENDHLSL

        Pass
        {
            Name "Pixelation"

            HLSLPROGRAM
            half4 frag(Varyings IN) : SV_TARGET
            {
                // Apply a horizontal wave distortion based on the vertical UV coordinate
                float wave = sin(IN.uv.y * _WaveFrequency + _Time.y * _WaveSpeed) * _WaveAmplitude;
                float2 distortedUV = IN.uv + float2(wave, 0.0);

                // Optionally apply a glitch effect (random horizontal offset)
                float2 finalUV = distortedUV;
                if (_EnableGlitch > 0.5)
                {
                    // Generate a pseudo-random value based on the UVs
                    float randomValue = frac(sin(dot(distortedUV, float2(12.9898, 78.233))) * 43758.5453);
                    // Trigger glitch based on the glitch frequency threshold
                    float glitchTrigger = step(1.0 - _GlitchFrequency, randomValue);
                    // Create a random horizontal offset in the range [-_GlitchIntensity, _GlitchIntensity]
                    float randomOffset = (frac(sin(_Time.y * 10.0 + randomValue * 100.0)) - 0.5) * 2.0 * _GlitchIntensity;
                    finalUV += glitchTrigger * float2(randomOffset, 0.0);
                }

                half4 tex;
                // If RGB split is enabled, sample each channel with a slight horizontal offset
                if (_EnableRGBSplit > 0.5)
                {
                    float2 offset = float2(_RGBSplitAmount, 0.0);

                    float2 blockPosR = floor((finalUV + offset) * _BlockCount);
                    float2 blockCenterR = blockPosR * _BlockSize + _HalfBlockSize;
                    half4 colR = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockCenterR);

                    float2 blockPosG = floor(finalUV * _BlockCount);
                    float2 blockCenterG = blockPosG * _BlockSize + _HalfBlockSize;
                    half4 colG = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockCenterG);

                    float2 blockPosB = floor((finalUV - offset) * _BlockCount);
                    float2 blockCenterB = blockPosB * _BlockSize + _HalfBlockSize;
                    half4 colB = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockCenterB);

                    tex = float4(colR.r, colG.g, colB.b, 1.0);
                }
                else
                {
                    float2 blockPos = floor(finalUV * _BlockCount);
                    float2 blockCenter = blockPos * _BlockSize + _HalfBlockSize;
                    tex = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockCenter);
                }
                return tex;
            }
            ENDHLSL
        }
    }
}
