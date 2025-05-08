Shader "Border/BorderNeonEffect"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1, 0, 0, 1)
        _NeonColor ("Neon Color", Color) = (0, 1, 1, 1)
        _CutoffFactor ("Cutoff Factor", Range(0, 0.5)) = 0.4
        _GlowStrength ("Glow Strength", Range(0, 4)) = 1.5
        _GlowWidth ("Glow Width", Range(0, 5)) = 1.0
        _Speed ("Scroll Speed", Range(0, 5)) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _NeonColor;
            float _CutoffFactor;
            float _GlowStrength;
            float _GlowWidth;
            float _Speed;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float GetScrollCenter()
            {
                return 0.5 + 0.5 * sin(_Time.y * _Speed * 2.0 * 3.14159);
            }

            float ComputeGlow(float dist)
            {
                float glow = saturate(1.0 - (dist * _GlowWidth));
                return pow(glow, _GlowStrength * 2.0);
            }

            fixed4 ApplyNeonEffect(float glow)
            {
                fixed4 color = lerp(_Color, _NeonColor, glow);
                color.a = max(_Color.a, _NeonColor.a * glow);
                return color;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 centeredUV = abs(i.uv - 0.5);
                if (centeredUV.x < _CutoffFactor && centeredUV.y < _CutoffFactor)
                    return fixed4(0, 0, 0, 0);

                float scrollCenter = GetScrollCenter();
                float dist = abs(i.uv.x - scrollCenter);
                float glow = ComputeGlow(dist);
                fixed4 color = ApplyNeonEffect(glow);

                return color;
            }

            ENDCG
        }
    }
    FallBack "Sprites/Default"
}
