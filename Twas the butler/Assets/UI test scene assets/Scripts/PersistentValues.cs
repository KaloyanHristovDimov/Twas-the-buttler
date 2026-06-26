
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PersistentValues : MonoBehaviour
{
    public static PersistentValues Instance;

    [Header("Stored Values")]
    [SerializeField] public List<bool> levelAvailable = new();

    [SerializeField] public UnityEvent OnValuesChanged = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBool(int index, bool value)
    {
        EnsureSize(levelAvailable, index + 1);

        if (levelAvailable[index] == value)
            return;

        levelAvailable[index] = value;
        OnValuesChanged.Invoke();
    }

    public bool GetBool(int index)
    {
        return index < levelAvailable.Count ? levelAvailable[index] : false;
    }

    private void EnsureSize<T>(List<T> list, int size)
    {
        while (list.Count < size)
            list.Add(default);
    }


    [Header("Current values (0..1)")]
    public float master = 1f;
    public float bgm = 1f;
    public float sfx = 1f;

    public UnityEvent<float> OnMasterChanged = new UnityEvent<float>();
    public UnityEvent<float> OnBgmChanged = new UnityEvent<float>();
    public UnityEvent<float> OnSfxChanged = new UnityEvent<float>();

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
