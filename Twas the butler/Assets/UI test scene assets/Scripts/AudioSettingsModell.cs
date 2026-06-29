using UnityEngine;
using UnityEngine.Events;

public class AudioSettingsModel : MonoBehaviour
{
    [Header("Current values (0..1)")]
    public float master = 1f;
    public float bgm = 1f;
    public float sfx = 1f;

    public UnityEvent<float> OnMasterChanged = new UnityEvent<float>();
    public UnityEvent<float> OnBgmChanged = new UnityEvent<float>();
    public UnityEvent<float> OnSfxChanged = new UnityEvent<float>();


    private void Start()
    {
        SetMaster(PersistentValues.Instance.master);
        SetBgm(PersistentValues.Instance.bgm);
        SetSfx(PersistentValues.Instance.sfx);
    }

    private void OnDisable()
    {
        PersistentValues.Instance.master = master;
        PersistentValues.Instance.bgm = bgm;
        PersistentValues.Instance.sfx = sfx;
    }


    public void SetMaster(float v)
    {
        v = Mathf.Clamp01(v);
        if (Mathf.Approximately(master, v)) return;
        master = v;
        OnMasterChanged.Invoke(master);
    }

    public void SetBgm(float v)
    {
        v = Mathf.Clamp01(v);
        if (Mathf.Approximately(bgm, v)) return;
        bgm = v;
        OnBgmChanged.Invoke(bgm);
    }

    public void SetSfx(float v)
    {
        v = Mathf.Clamp01(v);
        if (Mathf.Approximately(sfx, v)) return;
        sfx = v;
        OnSfxChanged.Invoke(sfx);
    }
}