Shader "Custom/VertexDispl"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag 

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.uv = v.uv;
                float xMod = tex2Dlod(_MainTex, float4(o.uv.xy, 0, 1));
                xMod = xMod * 2 - 1;

                o.uv.x = sin(xMod * 10 + _Time.y);
                float3 vert = v.vertex;
                vert.y = o.uv.x * 1;
                o.uv.x = o.uv.x * 0.6 + 0.3;
                o.vertex = UnityObjectToClipPos(vert);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(0.1, 0.1, i.uv.x + 0.1, .3); 
            }
            ENDHLSL
        }
    }
}
