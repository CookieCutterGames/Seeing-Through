Shader "UI/ShockwaveEffect"
{
    Properties
    {
        _WaveColor("Wave Color", Color) = (1,1,1,1)
        _WaveWidth("Wave Width", Float) = 0.05
        _WaveSpeed("Wave Speed", Float) = 2
        _WaveTime("Wave Time", Float) = 0
        _WaveOrigin("Wave Origin (UV)", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _WaveColor;
            float _WaveWidth;
            float _WaveSpeed;
            float _WaveTime;
            float4 _WaveOrigin;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float dist = distance(uv, _WaveOrigin.xy);
                float waveRadius = _WaveTime * _WaveSpeed;

                float alpha = 1.0 - abs(dist - waveRadius) / _WaveWidth;
                alpha = saturate(alpha);

                return fixed4(_WaveColor.rgb, _WaveColor.a * alpha);
            }
            ENDCG
        }
    }
}
