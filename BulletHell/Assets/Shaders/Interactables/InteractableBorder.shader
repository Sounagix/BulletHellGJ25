Shader "Interactable/InteractableBorder"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        _BorderColor ("Border Color", Color) = (1,0,0,1)
        _Width ("Width", Range(0,1)) = 0.005
        [HideInInspector]_Color ("Tint", Color) = (1,1,1,1)
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
                float2 offsets[8] = {
                    float2(-1,  0), float2(1,  0),
                    float2(0, -1), float2(0,  1),
                    float2(-1, -1), float2(-1, 1),
                    float2(1, -1), float2(1,  1)
                };

                float centerAlpha = tex2D(_MainTex, uv).a;
                if (centerAlpha == 0) return 0.0;

                for (int i = 0; i < 8; i++)
                {
                    float2 offset = offsets[i] * _MainTex_TexelSize.xy * _Width * 100.0;
                    float alpha = tex2D(_MainTex, uv + offset).a;
                    if (alpha < 0.1) return 1.0;
                }

                return 0.0;
            }

            fixed3 ApplyBorderColor(fixed3 texColor, float edge)
            {
                return edge > 0.0 ? _BorderColor.rgb : texColor;
            }

            fixed ApplyBorderAlpha(fixed texAlpha, float edge)
            {
                return edge > 0.0 ? _BorderColor.a : texAlpha;
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
