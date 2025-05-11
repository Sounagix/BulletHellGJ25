Shader "Interactable/GlitchEffect"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        _NoiseStrength ("Noise Strength", Range(0, 1)) = 0.5
        _CellCount ("Cell Count (per side)", Range(1, 10)) = 3
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
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

            sampler2D _MainTex;
            float _NoiseStrength;
            float _CellCount;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
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
                o.uv = v.texcoord;
                return o;
            }

            float rand(float2 co)
            {
                return frac(sin(dot(co, float2(12.9898, 78.233))) * 43758.5453);
            }

            float3 GetColorNoise(float2 gridCoord, float seedTime)
            {
                float rNoise = rand(gridCoord + float2(1.0, 0.0) + seedTime);
                float gNoise = rand(gridCoord + float2(5.0, 3.0) + seedTime);
                float bNoise = rand(gridCoord + float2(8.0, 7.0) + seedTime);
                return float3(rNoise, gNoise, bNoise);
            }

            float GetAlphaNoise(float2 gridCoord)
            {
                return step(0.3, rand(gridCoord + _Time.xy));
            }

            float2 GetGridSizeAndCoord(float2 uv)
            {
                float2 gridSize = float2(1.0 / _CellCount, 1.0 / _CellCount);
                float2 gridCoord = floor(uv / gridSize);
                return gridCoord;
            }

            float ApplyNoise(float color, float noise)
            {
                return noise * _NoiseStrength + color * (1 - _NoiseStrength);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 gridCoord = GetGridSizeAndCoord(i.uv);
                float seedTime = _Time.y * 10.0;

                float3 colorNoise = GetColorNoise(gridCoord, seedTime);
                col.r = ApplyNoise(col.r, colorNoise.x);
                col.g = ApplyNoise(col.g, colorNoise.y);
                col.b = ApplyNoise(col.b, colorNoise.z);

                col.a = GetAlphaNoise(gridCoord);

                return col;
            }

            ENDCG
        }
    }
}
