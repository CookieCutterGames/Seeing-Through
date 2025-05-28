Shader "Unlit/CircularMaskFixed"
{
    Properties
    {
        _Color("Overlay Color", Color) = (0,0,0,1)
        _Radius("Hole Radius", Float) = 0.25
        _Softness("Edge Softness", Float) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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

            fixed4 _Color;
            float _Radius;
            float _Softness;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 center = float2(0.5, 0.5);

                // Get screen aspect ratio
                float aspect = _ScreenParams.x / _ScreenParams.y;

                // Scale UV so distance calculation becomes aspect-ratio aware
                float2 scaledUV = float2((uv.x - center.x) * (aspect), uv.y - center.y);
                float dist = length(scaledUV);

                float alpha = smoothstep(_Radius, _Radius + _Softness, dist);
                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}
