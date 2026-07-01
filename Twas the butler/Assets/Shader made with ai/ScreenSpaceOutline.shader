Shader "Custom/ScreenSpaceOutline"
{
    Properties
    {
        [Header(Base)]
        _OutlineColor    ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness("Outline Thickness", Range(0, 5)) = 1

        [Header(Edge Detection Sensitivity)]
        _DepthThreshold  ("Depth Sensitivity", Range(0, 1)) = 0.05
        _NormalThreshold ("Normal Sensitivity", Range(0, 1)) = 0.4

        [Header(Debug)]
        // 0=normal, 1=edge mask, 2=depth, 3=normals, 4=exclusion mask
        _DebugView ("Debug View (0-4)", Range(0,4)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "OutlineDetection"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            float4 _OutlineColor;
            float  _OutlineThickness;
            float  _DepthThreshold;
            float  _NormalThreshold;
            float  _DebugView;

            // Set globally by OutlineExcludeMaskFeature each frame.
            // White pixels = objects on the NoOutline layer = skip outline there.
            TEXTURE2D(_OutlineExcludeMask);
            SAMPLER(sampler_OutlineExcludeMask);

            float4 Frag(Varyings input) : SV_Target
            {
                float2 uv    = input.texcoord;
                float2 texel = _BlitTexture_TexelSize.xy * _OutlineThickness;

                // --- sample center + 4 neighbours ---
                float  depthC = SampleSceneDepth(uv);
                float3 normC  = SampleSceneNormals(uv);

                float depthL = SampleSceneDepth(uv - float2(texel.x, 0));
                float depthR = SampleSceneDepth(uv + float2(texel.x, 0));
                float depthU = SampleSceneDepth(uv + float2(0, texel.y));
                float depthD = SampleSceneDepth(uv - float2(0, texel.y));

                float3 normL = SampleSceneNormals(uv - float2(texel.x, 0));
                float3 normR = SampleSceneNormals(uv + float2(texel.x, 0));
                float3 normU = SampleSceneNormals(uv + float2(0, texel.y));
                float3 normD = SampleSceneNormals(uv - float2(0, texel.y));

                // --- depth edges ---
                float linC = LinearEyeDepth(depthC, _ZBufferParams);
                float linL = LinearEyeDepth(depthL, _ZBufferParams);
                float linR = LinearEyeDepth(depthR, _ZBufferParams);
                float linU = LinearEyeDepth(depthU, _ZBufferParams);
                float linD = LinearEyeDepth(depthD, _ZBufferParams);

                float depthDiff = abs(linL-linC) + abs(linR-linC) + abs(linU-linC) + abs(linD-linC);
                float depthEdge = step(_DepthThreshold, depthDiff);

                // --- normal edges ---
                float normalDiff = (1 - dot(normL, normC)) + (1 - dot(normR, normC))
                                 + (1 - dot(normU, normC)) + (1 - dot(normD, normC));
                float normalEdge = step(_NormalThreshold, normalDiff);
                if (length(normC) < 0.01) normalEdge = 0;

                // --- background safety ---
                float isBackground = step(depthC, 0.0001);

                // --- exclusion mask: sample center + neighbours so the
                //     silhouette edge against other objects is also suppressed ---
                float exC = SAMPLE_TEXTURE2D(_OutlineExcludeMask, sampler_OutlineExcludeMask, uv).r;
                float exL = SAMPLE_TEXTURE2D(_OutlineExcludeMask, sampler_OutlineExcludeMask, uv - float2(texel.x, 0)).r;
                float exR = SAMPLE_TEXTURE2D(_OutlineExcludeMask, sampler_OutlineExcludeMask, uv + float2(texel.x, 0)).r;
                float exU = SAMPLE_TEXTURE2D(_OutlineExcludeMask, sampler_OutlineExcludeMask, uv + float2(0, texel.y)).r;
                float exD = SAMPLE_TEXTURE2D(_OutlineExcludeMask, sampler_OutlineExcludeMask, uv - float2(0, texel.y)).r;
                float isExcluded = saturate(exC + exL + exR + exU + exD);

                float edge = saturate(depthEdge + normalEdge)
                           * (1 - isBackground)
                           * (1 - isExcluded);

                float4 sceneColor = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                // --- debug views ---
                if (_DebugView > 0.5 && _DebugView < 1.5) return float4(edge.xxx, 1);
                if (_DebugView > 1.5 && _DebugView < 2.5) return float4(saturate(linC / 20).xxx, 1);
                if (_DebugView > 2.5 && _DebugView < 3.5) return float4(normC * 0.5 + 0.5, 1);
                if (_DebugView > 3.5) return float4(isExcluded.xxx, 1);

                return lerp(sceneColor, _OutlineColor, edge * _OutlineColor.a);
            }
            ENDHLSL
        }
    }
}
