using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RedString : MonoBehaviour
{
    public HashSet<ClueData> clues = new HashSet<ClueData>();
    public enum StringTag
    {
        None,
        Used,
        Killed
    }
    public StringTag stringTag = StringTag.None;

    private GameObject pinA;
    private GameObject pinB;


    public void SetClueData(ClueData clueDataA, ClueData clueDataB)
    {
        clues.Add(clueDataA);
        clues.Add(clueDataB);
        
    }


    public void SetTag(StringTag newTag)
    {
        stringTag = newTag;
        // Update visual representation based on the tag
        // For example, change color or material of the string
        switch (stringTag)
        {
            case StringTag.Used:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case StringTag.Killed:
                GetComponent<Renderer>().material.color = Color.black;
                break;
            default:
                GetComponent<Renderer>().material.color = Color.white;
                break;
        }
    }

    public void SetStartAndEndPoints(GameObject start, GameObject end)
    {
        // Set the start and end points of the string
        pinA = start.GetComponentsInChildren<Transform>()[1].gameObject; // Assuming the first child is the pin
        pinB = end.GetComponentsInChildren<Transform>()[1].gameObject; // Assuming the first child is the pin
        // Stretch the string between the two points
        transform.position = (pinA.transform.position + pinB.transform.position) / 2;
        

    }
}
