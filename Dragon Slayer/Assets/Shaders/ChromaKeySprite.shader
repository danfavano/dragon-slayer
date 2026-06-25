Shader "Custom/ChromaKeySprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ChromaKey ("Chroma Key Color", Color) = (0, 1, 0, 1)
        _Threshold ("Threshold", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
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
            float4 _MainTex_ST;
            float4 _ChromaKey;
            float _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Calculate color difference
                float3 diff = abs(col.rgb - _ChromaKey.rgb);
                float maxDiff = max(diff.r, max(diff.g, diff.b));
                
                // If color is close to chroma key, make transparent
                col.a = (maxDiff > _Threshold) ? col.a : 0;
                
                return col;
            }
            ENDCG
        }
    }
}
