using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveandUnactive : MonoBehaviour
{
    public List<GameObject> gameObjectsToToggle;
    
    [SerializeField] private List<GameObject> gameObjectsToAddToList;
    
    [SerializeField] private List<GameObject> gameObjectsToRemoveFromList;
    
    [SerializeField] private ToggleActiveandUnactive scriptWhoseListWillChange;

    [SerializeField] public List<GameObject> gameObjectsToRemoveFromRemoveList;
    
    public void ToggleActive()
    {
        foreach (GameObject gameObject in gameObjectsToToggle)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public void AddToRemoveList()
    {
        foreach (GameObject gameObject in gameObjectsToRemoveFromRemoveList)
        {
            scriptWhoseListWillChange.gameObjectsToRemoveFromList.Add(gameObject);

        }
    }

    public void AddToList()
    {
        foreach (var gameObject in gameObjectsToAddToList)
        {
            scriptWhoseListWillChange.gameObjectsToToggle.Add(gameObject);
        }
    }

    public void RemoveFromList()
    {
        foreach (var gameObject in gameObjectsToRemoveFromList)
        {
            if (gameObjectsToToggle.Contains(gameObject))
            {
                scriptWhoseListWillChange.gameObjectsToToggle.Remove(gameObject);
            }
        }
    }
}
