using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveandUnactive : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjectsToToggle;

    public void ToggleActive()
    {
        foreach (GameObject gameObject in gameObjectsToToggle)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
