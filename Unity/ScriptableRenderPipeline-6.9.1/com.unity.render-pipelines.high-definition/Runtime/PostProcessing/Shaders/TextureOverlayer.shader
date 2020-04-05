Shader "Custom/TextureOverlayer"
{
    Properties
    {
        _OverlayTex("Texture", 2D) = "white" {}
        _SecondOverlayTex("Texture", 2D) = "white" {}
        _TintColor("Color", Color) = (1,1,1,1)
    }
    HLSLINCLUDE
        #pragma target 4.5
        #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

        TEXTURE2D_X(_InputTexture);

        struct Attributes
        {
            uint vertexID : SV_VertexID;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float2 texcoord   : TEXCOORD0;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        Varyings Vert(Attributes input)
        {
            Varyings output;
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
            output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
            output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
            return output;
        }

        sampler2D _OverlayTex;
        sampler2D _SecondOverlayTex;
        float4 _TintColor;

        float4 Frag(Varyings input) : SV_Target
        {
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

            float2 positionNDC = input.texcoord;
            uint2 positionSS = input.texcoord * _ScreenSize.xy;

            float4 uvTransform = float4(1.0, 1.0, 0.0, 0.0);
            // Flip logic
            positionSS = positionSS * uvTransform.xy + uvTransform.zw * (_ScreenSize.xy - 1.0);
            positionNDC = positionNDC * uvTransform.xy + uvTransform.zw;

            #if defined(BILINEAR) || defined(CATMULL_ROM_4) || defined(LANCZOS)
            float3 outColor = UpscaledResult(positionNDC.xy);
            #else
            float3 outColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;
            #endif

            float2 uv = input.texcoord;
            uv.x = 1 - uv.x;
            uv.y = 1 - uv.y;
            uv.x *= 3.0;
            uv.y *= 2.25;

            float2 uv2 = input.texcoord;
            uv2.x = 1 - uv2.x;
            uv2.y = uv2.y;
            uv2.x *= 3.0;
            uv2.y *= 2.25;

            float4 col = tex2D(_OverlayTex, uv);
            float4 secondCol = tex2D(_SecondOverlayTex, uv2);

            float4 resultCol;
            if(uv.x < 1.0 && uv.y < 1.0)
            {
                resultCol = col * _TintColor;
            }
            else if(uv2.x < 1.0 && uv2.y < 1.0)
            {
                resultCol = secondCol;
            }
            else
            {
                resultCol = float4(outColor, 1);
            }

            return resultCol;
        }
    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }

        Pass
        {
            ZWrite On ZTest Always Blend Off Cull Off

            HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment Frag
            ENDHLSL
        }
    }
    Fallback Off
    }
