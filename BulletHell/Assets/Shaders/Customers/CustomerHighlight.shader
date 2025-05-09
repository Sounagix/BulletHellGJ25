Shader "Customers/CustomerHighlight"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        _BorderColor ("Border Color", Color) = (1,0,0,1)
        _Width ("Width", Range(0,1)) = 0.005
        [HideInInspector]_Color ("Tint", Color) = (1,1,1,1)
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 2.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" }

        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            fixed4 _Color;
            fixed4 _BorderColor;
            float _Width;
            float _PulseSpeed;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 GetTexColor(v2f IN)
            {
                return tex2D(_MainTex, IN.texcoord) * IN.color;
            }

            float GetEdgeFactor(float2 uv)
            {
                float centerAlpha = tex2D(_MainTex, uv).a;
                if (centerAlpha == 0) return 0.0;

                float2 scale = _MainTex_TexelSize.xy * _Width * 100.0;

                if (tex2D(_MainTex, uv + float2(-1, 0) * scale).a < 0.1) return 1.0;
                if (tex2D(_MainTex, uv + float2(1, 0) * scale).a < 0.1) return 1.0;
                if (tex2D(_MainTex, uv + float2(0, -1) * scale).a < 0.1) return 1.0;
                if (tex2D(_MainTex, uv + float2(0, 1) * scale).a < 0.1) return 1.0;
                if (tex2D(_MainTex, uv + float2(-1, -1) * scale).a < 0.1) return 1.0;
                if (tex2D(_MainTex, uv + float2(-1, 1) * scale).a < 0.1) return 1.0;
                if (tex2D(_MainTex, uv + float2(1, -1) * scale).a < 0.1) return 1.0;
                if (tex2D(_MainTex, uv + float2(1, 1) * scale).a < 0.1) return 1.0;

                return 0.0;
            }

            fixed3 ApplyBorderColor(fixed3 texColor, float edge)
            {
                return edge > 0.0 ? _BorderColor.rgb : texColor;
            }

            fixed ApplyBorderAlpha(fixed texAlpha, float edge)
            {
                if (edge > 0.0)
                {
                    float pulse = 0.5 + 0.5 * sin(_Time.y * _PulseSpeed);
                    return _BorderColor.a * pulse;
                }
                else
                {
                    return texAlpha;
                }
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 texColor = GetTexColor(IN);
                float edge = GetEdgeFactor(IN.texcoord);

                texColor.rgb = ApplyBorderColor(texColor.rgb, edge);
                texColor.a = ApplyBorderAlpha(texColor.a, edge);

                return texColor;
            }

            ENDCG
        }
    }
}
