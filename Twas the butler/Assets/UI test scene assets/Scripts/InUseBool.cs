using UnityEngine;

public class InUseBool : MonoBehaviour
{
    public bool inUse = false;

    public void SetInUse()
    {
        inUse = true;
    }

    public void SetOutOfUse()
    {
        inUse = false;
    }
}

