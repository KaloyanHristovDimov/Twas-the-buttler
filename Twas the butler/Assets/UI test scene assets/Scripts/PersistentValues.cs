
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class PersistentValues : MonoBehaviour
{
    public static PersistentValues Instance;

    [Header("Stored Values")]
    [SerializeField] public List<bool> levelAvailable = new();

    [SerializeField] public List<float> levelTimes = new();

    [SerializeField] public UnityEvent OnValuesChanged = new();

    [SerializeField] public float timerlength = 7.5f;

    public int lastLevelPlayed = 0;




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

}
