// Screen filter for the low-res render texture (used on the RawImage material).
// Lowers saturation, cuts down the number of colours and adds a dither pattern.
Shader "PS1/Screen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColourSteps ("Colour Steps", Float) = 32
        _DitherAmount ("Dither Amount", Range(0, 1)) = 0.5
        _Saturation ("Saturation", Range(0, 1)) = 0.6
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off
        ZWrite Off
        ZTest Always

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // z = width, w = height of the render texture
            float _ColourSteps;
            float _DitherAmount;
            float _Saturation;

            // 4x4 Bayer dither pattern
            static const float dither[16] =
            {
                 0,  8,  2, 10,
                12,  4, 14,  6,
                 3, 11,  1,  9,
                15,  7, 13,  5
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 col = tex2D(_MainTex, i.uv).rgb;

                // Desaturate (mix towards grey)
                float grey = dot(col, float3(0.299, 0.587, 0.114));
                col = lerp(float3(grey, grey, grey), col, _Saturation);

                // Which pixel of the 4x4 pattern are we on?
                int x = (int)fmod(i.uv.x * _MainTex_TexelSize.z, 4);
                int y = (int)fmod(i.uv.y * _MainTex_TexelSize.w, 4);
                float d = dither[y * 4 + x] / 16.0 - 0.5;

                // Add the dither, then round the colour to a limited number of steps
                col += d * _DitherAmount / _ColourSteps;
                col = round(col * _ColourSteps) / _ColourSteps;

                return fixed4(col, 1);
            }
            ENDCG
        }
    }
}
