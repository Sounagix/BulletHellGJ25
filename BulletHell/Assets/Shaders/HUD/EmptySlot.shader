Shader "HUD/EmptySlot"
{
    Properties
    {
        _Color ("Color", Color) = (1,0,0,1)
        _CutoffFactor ("Cutoff Factor", Range(0,0.5)) = 0.4
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
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

            fixed4 _Color;
            float _CutoffFactor;

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

            fixed4 frag (v2f i) : SV_Target
            {
                float2 centeredUV = abs(i.uv - 0.5);
                if (centeredUV.x < _CutoffFactor && centeredUV.y < _CutoffFactor)
                    return fixed4(0, 0, 0, 0);

                return _Color;
            }
            ENDCG
        }
    }
}
