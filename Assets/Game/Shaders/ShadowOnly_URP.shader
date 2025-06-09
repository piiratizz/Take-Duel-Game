Shader "Custom/ShadowOnly_URP"
{
    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline"
               "Queue"="Transparent"          // чтобы рисовался после обычной геометрии
               "RenderType"="Transparent" }   // но без сортировки по альфе

        // ───────────────────────────────────────── ShadowCaster ──
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            // Обязательные инклюды URP
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            // Вершины → клип-пространство
            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs pos = GetVertexPositionInputs(IN.positionOS);
                OUT.positionCS = pos.positionCS;
                return OUT;
            }

            // Фрагмент пустой: важно лишь присутствие pass’а
            void frag() {}
            ENDHLSL
        }

        // ───────────────────────────────────────── “пустой” рендер-пасс ──
        Pass
        {
            Name "Invisible"
            Tags { "LightMode"="UniversalForward" } // вызываться будет, но ничего не нарисует
            ColorMask 0       // не писать в буфер цвета
            ZWrite Off        // не вмешиваться в Z
        }
    }

    // Теням/глубине достаточно самого SubShader; fallback не нужен.
}
