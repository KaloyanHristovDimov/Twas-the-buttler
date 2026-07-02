using System.Collections.Generic;
using UnityEngine;

public class StringManager : MonoBehaviour
{
    [SerializeField]
    public HashSet<RedString> redStrings = new HashSet<RedString>();
    public static StringManager Instance { get; private set; }

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

    /// <summary>
    /// Adds the specified RedString to the collection.
    /// </summary>
    /// <param name="redString">The RedString instance to add to the collection. Cannot be null.</param>
    public void AddRedString(RedString redString)
    {
        redStrings.Add(redString);
    }
    /// <summary>
    /// Removes the specified RedString instance from the collection.
    /// </summary>
    /// <param name="redString">The RedString instance to remove from the collection. Cannot be null.</param>
    public void RemoveRedString(RedString redString)
    {
        redStrings.Remove(redString);
    }

    public bool HasString(ClueData clueA, ClueData clueB, ClueData.StringTag stringTag)
    {
        foreach (var stringToCheck in redStrings)
        {
            Debug.Log($"Checking string with tag {stringToCheck.stringTag} between clues: {stringToCheck.clues.Count}");
            if (stringToCheck.clues.Contains(clueA) && stringToCheck.clues.Contains(clueB) && stringToCheck.stringTag == stringTag)
            {
                Debug.Log($"Found string between {clueA.clueName} and {clueB.clueName} with tag {stringTag}");
                return true;

            }
        }
        return false;
    }

    public bool HasDuplicate(BoardNote boardNoteA, BoardNote boardNoteB)
    {
        foreach(var redString in redStrings)
        {
            if((redString.startNote == boardNoteA.transform || redString.endNote == boardNoteA.transform) && (redString.startNote == boardNoteB.transform || redString.endNote == boardNoteB.transform))
                { return true; }
        }
        return false;
    }

    public void UpdateString(BoardNote boardNote)
    {
        foreach (var stringToUpdate in redStrings)
        {
            
            if (stringToUpdate.startNote == boardNote.transform || stringToUpdate.endNote == boardNote.transform)
            {
                stringToUpdate.UpdateVisual();
            }
        }
    }
}