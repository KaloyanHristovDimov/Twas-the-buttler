
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
}
