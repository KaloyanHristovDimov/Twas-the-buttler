using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

// ----------------------------------------------------------------------
// Add this to your URP Renderer asset (the one referenced by your
// Render Pipeline Asset) via the "Add Renderer Feature" button at the
// bottom of the Renderer asset's Inspector.
//
// Settings below appear directly on the Renderer Feature in that
// Inspector -- NOT on a Material. Your object's existing material and
// textures are never touched.
// ----------------------------------------------------------------------
public class ScreenSpaceOutlineFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class OutlineSettings
    {
        [Tooltip("Where in the frame this draws. AfterRenderingTransparents is usually correct.")]
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

        [Header("Outline Look")]
        public Color outlineColor = Color.black;
        [Range(0f, 5f)] public float outlineThickness = 1f;

        [Header("Edge Detection Sensitivity")]
        [Tooltip("Lower = catches more depth edges (can get noisy up close).")]
        [Range(0f, 1f)] public float depthThreshold = 0.05f;
        [Tooltip("Lower = catches more angle changes (e.g. cube corners).")]
        [Range(0f, 1f)] public float normalThreshold = 0.4f;

        [Header("Debug")]
        [Tooltip("Use this to diagnose a blank/black screen. EdgeMask shows white=detected edge, black=none. Depth/Normals show the raw textures the shader is reading.")]
        public DebugMode debugView = DebugMode.Off;
    }

    public enum DebugMode { Off = 0, EdgeMask = 1, Depth = 2, Normals = 3 }

    public OutlineSettings settings = new OutlineSettings();

    private Material outlineMaterial;
    private OutlinePass outlinePass;

    private const string ShaderName = "Hidden/Custom/ScreenSpaceOutline";

    public override void Create()
    {
        Shader shader = Shader.Find(ShaderName);
        if (shader == null)
        {
            Debug.LogWarning($"[ScreenSpaceOutlineFeature] Could not find shader '{ShaderName}'. Make sure ScreenSpaceOutline.shader is in the project.");
            return;
        }

        outlineMaterial = CoreUtils.CreateEngineMaterial(shader);
        outlinePass = new OutlinePass(outlineMaterial, settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (outlineMaterial == null || outlinePass == null)
            return;

        outlinePass.renderPassEvent = settings.renderPassEvent;
        renderer.EnqueuePass(outlinePass);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(outlineMaterial);
    }

    private class OutlinePass : ScriptableRenderPass
    {
        private readonly Material material;
        private readonly OutlineSettings settings;

        private static readonly int ColorId = Shader.PropertyToID("_OutlineColor");
        private static readonly int ThicknessId = Shader.PropertyToID("_OutlineThickness");
        private static readonly int DepthThreshId = Shader.PropertyToID("_DepthThreshold");
        private static readonly int NormalThreshId = Shader.PropertyToID("_NormalThreshold");
        private static readonly int DebugViewId = Shader.PropertyToID("_DebugView");

        private class PassData
        {
            public TextureHandle source;
            public Material material;
        }

        public OutlinePass(Material material, OutlineSettings settings)
        {
            this.material = material;
            this.settings = settings;
            renderPassEvent = settings.renderPassEvent;

            // We need the depth + normal prepass textures available to sample from.
            ConfigureInput(ScriptableRenderPassInput.Normal | ScriptableRenderPassInput.Depth);
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

            if (resourceData.isActiveTargetBackBuffer)
                return;

            material.SetColor(ColorId, settings.outlineColor);
            material.SetFloat(ThicknessId, settings.outlineThickness);
            material.SetFloat(DepthThreshId, settings.depthThreshold);
            material.SetFloat(NormalThreshId, settings.normalThreshold);
            material.SetFloat(DebugViewId, (float)settings.debugView);

            TextureHandle source = resourceData.activeColorTexture;

            TextureDesc destinationDesc = renderGraph.GetTextureDesc(source);
            destinationDesc.name = "_OutlinePassTexture";
            destinationDesc.clearBuffer = false;
            destinationDesc.depthBufferBits = 0;
            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

            using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Screen Space Outline", out PassData passData))
            {
                passData.source = source;
                passData.material = material;

                builder.UseTexture(source, AccessFlags.Read);
                builder.SetRenderAttachment(destination, 0, AccessFlags.Write);
                builder.AllowPassCulling(false);

                builder.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
                {
                    Blitter.BlitTexture(ctx.cmd, data.source, new Vector4(1, 1, 0, 0), data.material, 0);
                });
            }

            // Feed the result back into the pipeline so subsequent passes
            // (UI, post-processing, etc.) draw on top of it correctly.
            resourceData.cameraColor = destination;
        }
    }
}
