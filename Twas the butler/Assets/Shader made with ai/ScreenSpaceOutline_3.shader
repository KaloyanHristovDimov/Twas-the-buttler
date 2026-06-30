Shader "Custom/ScreenSpaceOutline"
{
    // ------------------------------------------------------------------
    // Full-screen post-process shader. Detects edges by comparing depth
    // and normals between neighboring pixels across the WHOLE rendered
    // image, then draws an outline color over those edges.
    //
    // Because this works on the final image (not per-mesh), it:
    //  - never leaves gaps at sharp corners or between separate objects
    //  - never touches or replaces your existing materials/textures
    // ------------------------------------------------------------------
    Properties
    {
        _OutlineColor    ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness("Outline Thickness", Range(0, 5)) = 1
        _DepthThreshold   ("Depth Sensitivity", Range(0, 1)) = 0.05
        _NormalThreshold  ("Normal Sensitivity", Range(0, 1)) = 0.4

        [Header(Debug)]
        // 0 = normal output, 1 = show raw edge mask (white=edge,black=none),
        // 2 = show linear depth, 3 = show world normals as color
        _DebugView ("Debug View (0-3)", Range(0,3)) = 0
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

            float SampleDepth(float2 uv)
            {
                return SampleSceneDepth(uv);
            }

            float3 SampleNormal(float2 uv)
            {
                return SampleSceneNormals(uv);
            }

            float4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;
                float2 texel = _BlitTexture_TexelSize.xy * _OutlineThickness;

                // sample center + 4 neighbors
                float  depthC = SampleDepth(uv);
                float3 normC  = SampleNormal(uv);

                float depthL = SampleDepth(uv - float2(texel.x, 0));
                float depthR = SampleDepth(uv + float2(texel.x, 0));
                float depthU = SampleDepth(uv + float2(0, texel.y));
                float depthD = SampleDepth(uv - float2(0, texel.y));

                float3 normL = SampleNormal(uv - float2(texel.x, 0));
                float3 normR = SampleNormal(uv + float2(texel.x, 0));
                float3 normU = SampleNormal(uv + float2(0, texel.y));
                float3 normD = SampleNormal(uv - float2(0, texel.y));

                // depth edge: linearize so the threshold behaves consistently
                // near and far from the camera
                float linC = LinearEyeDepth(depthC, _ZBufferParams);
                float linL = LinearEyeDepth(depthL, _ZBufferParams);
                float linR = LinearEyeDepth(depthR, _ZBufferParams);
                float linU = LinearEyeDepth(depthU, _ZBufferParams);
                float linD = LinearEyeDepth(depthD, _ZBufferParams);

                float depthDiff = abs(linL - linC) + abs(linR - linC) + abs(linU - linC) + abs(linD - linC);
                float depthEdge = step(_DepthThreshold, depthDiff);

                // normal edge: catches edges depth alone misses (e.g. a flat
                // wall meeting a flat floor at the same distance)
                float normalDiff = (1 - dot(normL, normC)) + (1 - dot(normR, normC))
                                  + (1 - dot(normU, normC)) + (1 - dot(normD, normC));
                float normalEdge = step(_NormalThreshold, normalDiff);

                // SAFETY: if the normals texture isn't actually bound, every
                // sample reads back as a zero-length vector. dot() of two
                // zero vectors is 0, so (1 - 0) = 1 per neighbor, which is
                // a guaranteed "edge" everywhere on screen -- this is what
                // causes the whole image to go solid outline-color black.
                // Skip normal-based detection entirely when that happens.
                float normalLengthC = length(normC);
                if (normalLengthC < 0.01)
                    normalEdge = 0;

                // SAFETY: skip the background/skybox (max depth) so the
                // outline doesn't outline "nothing" against itself.
                float rawDepthIsBackground = step(depthC, 0.0001);

                float edge = saturate(depthEdge + normalEdge) * (1 - rawDepthIsBackground);

                float4 sceneColor = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                // ---- Debug views: bypass normal output to show raw data ----
                if (_DebugView > 0.5 && _DebugView < 1.5)
                    return float4(edge.xxx, 1); // white = detected edge, black = none
                if (_DebugView > 1.5 && _DebugView < 2.5)
                    return float4(saturate(linC / 20).xxx, 1); // depth, brighter = farther
                if (_DebugView > 2.5)
                    return float4(normC * 0.5 + 0.5, 1); // normals as RGB

                return lerp(sceneColor, _OutlineColor, edge * _OutlineColor.a);
            }
            ENDHLSL
        }
    }
}
