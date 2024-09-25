Shader "Custom/HexDecalShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _TileSize("Tile Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _BaseColor;
            float _TileSize;

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.worldPos = TransformObjectToWorld(v.positionOS);
                return o;
            }

            float hexPattern(float2 uv)
            {
                uv.x = uv.x / _TileSize;
                uv.y = uv.y / _TileSize;

                // 기본적인 육각형 패턴 계산 로직
                uv.x = uv.x - floor(uv.x) - 0.5;
                uv.y = uv.y - floor(uv.y) - 0.5;

                float3 q;
                q.x = uv.x - uv.y * 0.57735;  // sqrt(3) / 3
                q.y = uv.x + uv.y * 0.57735;
                q.z = uv.y * 1.1547;           // sqrt(3)
                q = abs(frac(q - 0.5) - 0.5);

                return min(q.x, min(q.y, q.z));
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 uv = i.worldPos.xz;
                float pattern = hexPattern(uv);

                if (pattern < 0.1)
                {
                    return _BaseColor;
                }
                
                return half4(0, 0, 0, 0);  // 투명 처리
            }
            ENDHLSL
        }
    }
}