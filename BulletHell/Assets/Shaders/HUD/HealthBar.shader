Shader "HUD/HealthBar"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        _LifePoints ("Life Points", Int) = 3
        _MaxLifePoints ("Max Life Points", Int) = 5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            int _LifePoints;
            int _MaxLifePoints;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                int totalStripes = max(_MaxLifePoints * 2 - 1, 1);
                float stripeWidth = 1.0 / totalStripes;
                float x = i.texcoord.x;

                int stripeIndex = floor(x / stripeWidth);

                if (stripeIndex % 2 == 1)         // transparent stripe
                    discard;

                if (stripeIndex / 2 >= _LifePoints) // more than current life
                    discard;

                return tex2D(_MainTex, i.texcoord) * i.color;
            }

            ENDCG
        }
    }
}
