Shader "Custom/InvisibleButCastsShadows_URP"
{
    //Tags { "RenderPipeline" = "UniversalRenderPipeline" }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        // ─── Основной пасс: ничего не рендерим ─────────────────────
        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            ColorMask 0   // Не писать в цветовой буфер
            ZWrite Off    // Не писать в глубину (если нужно — можно включить)

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                VertexPositionInputs pos = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = pos.positionCS;
                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                discard; // Прозрачный пиксель
                return float4(0, 0, 0, 0);
            }
            ENDHLSL
        }

        // ─── ShadowCaster пасс ─────────────────────────────────────
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs pos = GetVertexPositionInputs(input.positionOS);
                output.positionCS = pos.positionCS;
                return output;
            }

            float4 frag() : SV_Target
            {
                return 0; // Тени не требуют цвета
            }
            ENDHLSL
        }
    }
}
