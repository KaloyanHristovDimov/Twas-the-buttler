using UnityEngine;

public class SetLastLevelScript : MonoBehaviour
{
    [SerializeField] private int level;
    void Start()
    {
        PersistentValues.Instance.lastLevelPlayed = level;
    }

   
}
