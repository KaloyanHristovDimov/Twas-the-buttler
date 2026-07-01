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


    public void AddRedString(RedString redString)
    {
        redStrings.Add(redString);
    }
    public void RemoveRedString(RedString redString)
    {
        redStrings.Remove(redString);
    }

    public bool HasString(ClueData clueA, ClueData clueB, ClueData.StringTag stringTag)
    {
        foreach (var redString in redStrings)
        {
            if (redString.clues.Contains(clueA) && redString.clues.Contains(clueB) && redString.stringTag == stringTag)
            {
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
        foreach (var redString in redStrings)
        {
            if (redString.startNote == boardNote.transform || redString.endNote == boardNote.transform)
            {
                redString.UpdateVisual();
            }
        }
    }
}