Shader "HUD/ImageBorder"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        _BorderColor ("Border Color", Color) = (1,0,0,1)
        _Width ("Width", Range(0,0.4)) = 0.0
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

            float GetBorderFactor(float2 texcoord)
            {
                if (_Width <= 0.0)
                    return 0.0;

                float borderThreshold = _Width;

                bool inX = texcoord.x < borderThreshold || texcoord.x > 1 - borderThreshold;
                bool inY = texcoord.y < borderThreshold || texcoord.y > 1 - borderThreshold;

                if (inX || inY)
                    return _BorderColor.a;

                return 0.0;
            }

            fixed3 ApplyBorderColor(fixed3 texColor, float borderFactor)
            {
                return borderFactor > 0.0 ? _BorderColor.rgb : texColor;
            }

            fixed ApplyBorderAlpha(fixed texAlpha, float borderFactor)
            {
                return borderFactor > 0.0 ? _BorderColor.a : texAlpha;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 texColor = GetTexColor(IN);

                if (texColor.a == 0.0 || _Width == 0.0)
                    return texColor;

                float borderFactor = GetBorderFactor(IN.texcoord);

                texColor.rgb = ApplyBorderColor(texColor.rgb, borderFactor);
                texColor.a = ApplyBorderAlpha(texColor.a, borderFactor);

                return texColor;
            }

            ENDCG
        }
    }
}
