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
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionHCS : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        TEXTURE2D(_MainTex);
        float4 _MainTex_TexelSize;
        float4 _MainTex_ST;

        SamplerState sampler_point_clamp;

        uniform float2 _BlockCount;
        uniform float2 _BlockSize;
        uniform float2 _HalfBlockSize;
        uniform float4 _Time; // Using Unity's built-in time vector

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
                // Apply a sine wave distortion to the UV coordinates for a wavy effect.
                // You can adjust these values to change the frequency, speed, and amplitude.
                float waveFrequency = 20.0;
                float waveSpeed = 3.0;
                float waveAmplitude = 0.005;
                float wave = sin(IN.uv.y * waveFrequency + _Time.y * waveSpeed) * waveAmplitude;
                float2 distortedUV = IN.uv + float2(wave, 0.0);

                // Now perform the pixelation on the distorted UVs.
                float2 blockPos = floor(distortedUV * _BlockCount);
                float2 blockCenter = blockPos * _BlockSize + _HalfBlockSize;
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockCenter);
                return tex;
            }
            ENDHLSL
        }
    }
}
