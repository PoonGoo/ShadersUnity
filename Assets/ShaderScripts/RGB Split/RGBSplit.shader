Shader "Hidden/RGBSplit"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white"
        _RGBSplitAmount("RGB Split Amount", Float) = 0.002
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            Name "RGBSplitPass"
            HLSLPROGRAM
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

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            SamplerState sampler_PointClamp;
            TEXTURE2D(_MainTex);

            float _RGBSplitAmount;

            half4 frag(Varyings IN) : SV_Target
            {
                // Apply a slight horizontal offset to sample each channel separately
                float2 offset = float2(_RGBSplitAmount, 0.0);
                half red = SAMPLE_TEXTURE2D(_MainTex, sampler_PointClamp, IN.uv + offset).r;
                half green = SAMPLE_TEXTURE2D(_MainTex, sampler_PointClamp, IN.uv).g;
                half blue = SAMPLE_TEXTURE2D(_MainTex, sampler_PointClamp, IN.uv - offset).b;
                return half4(red, green, blue, 1.0);
            }
            ENDHLSL
        }
    }
}
