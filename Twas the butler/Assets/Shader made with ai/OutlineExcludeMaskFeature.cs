using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class OutlineExcludeMaskFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public LayerMask excludeLayer = 0;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    public Settings settings = new Settings();
    private ExcludeMaskPass maskPass;
    private Material whiteMaterial;

    public override void Create()
    {
        Shader s = Shader.Find("Hidden/Custom/SolidWhite");
        if (s == null)
        {
            Debug.LogWarning("[OutlineExcludeMaskFeature] Cannot find 'Hidden/Custom/SolidWhite'.");
            return;
        }
        whiteMaterial = CoreUtils.CreateEngineMaterial(s);
        maskPass = new ExcludeMaskPass(whiteMaterial, settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (whiteMaterial == null || maskPass == null) return;
        maskPass.renderPassEvent = settings.renderPassEvent;
        renderer.EnqueuePass(maskPass);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(whiteMaterial);
    }

    private class PassData
    {
        public LayerMask excludeLayer;
        public Material whiteMat;
        public TextureHandle maskTex;
    }

    private class ExcludeMaskPass : ScriptableRenderPass
    {
        private readonly Material whiteMat;
        private readonly Settings settings;
        private static readonly int MaskId = Shader.PropertyToID("_OutlineExcludeMask");

        public ExcludeMaskPass(Material whiteMat, Settings settings)
        {
            this.whiteMat = whiteMat;
            this.settings = settings;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

            var desc = cameraData.cameraTargetDescriptor;
            desc.colorFormat = RenderTextureFormat.R8;
            desc.depthBufferBits = 0;
            desc.msaaSamples = 1;

            TextureHandle maskTex = UniversalRenderer.CreateRenderGraphTexture(
                renderGraph, desc, "_OutlineExcludeMask", true, FilterMode.Point);

            using (var builder = renderGraph.AddUnsafePass<PassData>("OutlineExcludeMask", out var passData))
            {
                passData.excludeLayer = settings.excludeLayer;
                passData.whiteMat     = whiteMat;
                passData.maskTex      = maskTex;

                builder.UseTexture(maskTex, AccessFlags.Write);
                builder.AllowPassCulling(false);
                builder.SetGlobalTextureAfterPass(maskTex, MaskId);

                builder.SetRenderFunc((PassData data, UnsafeGraphContext ctx) =>
                {
                    CommandBuffer cmd = CommandBufferHelpers.GetNativeCommandBuffer(ctx.cmd);

                    cmd.SetRenderTarget(data.maskTex);
                    cmd.ClearRenderTarget(false, true, Color.black);

                    var allRenderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
                    foreach (var r in allRenderers)
                    {
                        if (!r.isVisible) continue;
                        if ((data.excludeLayer.value & (1 << r.gameObject.layer)) == 0) continue;

                        if (r is MeshRenderer mr)
                        {
                            var mf = mr.GetComponent<MeshFilter>();
                            if (mf == null || mf.sharedMesh == null) continue;
                            for (int i = 0; i < mf.sharedMesh.subMeshCount; i++)
                                cmd.DrawMesh(mf.sharedMesh, mr.localToWorldMatrix, data.whiteMat, i, 0);
                        }
                        else if (r is SkinnedMeshRenderer smr)
                        {
                            Mesh baked = new Mesh();
                            smr.BakeMesh(baked);
                            for (int i = 0; i < baked.subMeshCount; i++)
                                cmd.DrawMesh(baked, smr.localToWorldMatrix, data.whiteMat, i, 0);
                        }
                    }
                });
            }
        }
    }
}