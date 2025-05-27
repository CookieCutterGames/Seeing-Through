Shader "Custom/PeripheralShadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowIntensity("Shadow Intensity", Range(0,1)) = 0.5
        _GhostDistance("Ghost Distance", Float) = 5.0
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _ShadowIntensity;
            float _GhostDistance;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 center = float2(0.5, 0.5);
                float distFromCenter = distance(i.uv, center);
                
                // Dynamiczny efekt pulsowania
                float pulse = sin(_Time.y * 3) * 0.1 + 0.9;
                float shadow = smoothstep(0.7 * pulse, 1.0 * pulse, distFromCenter);
                
                // Zniekształcenie peryferyjne
                float2 distortedUV = i.uv + (i.uv - center) * shadow * 0.1 * (1 - _GhostDistance/10);
                
                fixed4 finalCol = tex2D(_MainTex, distortedUV);
                return lerp(finalCol, fixed4(0,0,0,1), shadow * _ShadowIntensity);
            }
            ENDCG
        }
    }
}