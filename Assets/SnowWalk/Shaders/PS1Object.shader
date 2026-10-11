// PS1-style shader for objects (trees, ground, house).
// Vertex snapping makes models wobble, affine mapping makes textures warp.
// Lighting is based on the "Simple diffuse lighting" example in the Unity manual.
// Affine idea based on Daniel Ilett's ps1-affine-shader.
Shader "PS1/Object"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Colour", Color) = (1,1,1,1)
        _SnapAmount ("Vertex Snap (lower = more wobble)", Float) = 120
        _AffineAmount ("Texture Warp", Range(0, 1)) = 1

        // For leaves: cuts out see-through pixels
        [Toggle(_ALPHATEST_ON)] _AlphaClip ("Alpha Clip (leaves)", Float) = 0
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5

        // Off = draw both sides of each face (needed for flat leaf planes)
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull (Off = double sided)", Float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="AlphaTest" }
        Cull [_Cull]

        Pass
        {
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma shader_feature _ALPHATEST_ON
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 affineUV : TEXCOORD1;
                fixed3 light : COLOR0;
                UNITY_FOG_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _SnapAmount;
            float _AffineAmount;
            float _Cutoff;

            v2f vert (appdata v)
            {
                v2f o;

                // Vertex snapping: round the screen position so vertices jump between "pixels"
                float4 pos = UnityObjectToClipPos(v.vertex);
                pos.xy = pos.xy / pos.w;
                pos.xy = round(pos.xy * _SnapAmount) / _SnapAmount;
                pos.xy = pos.xy * pos.w;
                o.pos = pos;

                // Affine texture mapping: multiply UVs by w here, divide again in frag
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.affineUV = float3(o.uv * pos.w, pos.w);

                // Per-vertex lighting (sun + ambient)
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.light = nl * _LightColor0.rgb + ShadeSH9(half4(worldNormal, 1));

                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 affine = i.affineUV.xy / i.affineUV.z;
                float2 uv = lerp(i.uv, affine, _AffineAmount);

                fixed4 col = tex2D(_MainTex, uv) * _Color;

                // Throw away pixels that are more see-through than the cutoff
                #ifdef _ALPHATEST_ON
                clip(col.a - _Cutoff);
                #endif

                col.rgb *= i.light;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}