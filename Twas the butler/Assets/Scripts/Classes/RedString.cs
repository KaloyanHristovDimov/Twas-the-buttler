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

    public HashSet<GameObject> connectedObjects = new HashSet<GameObject>();


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
        connectedObjects.Add(start.GetComponentInChildren<Transform>().gameObject);
        connectedObjects.Add(end.GetComponentInChildren<Transform>().gameObject);
        // Stretch the string between the two points
        transform.position = (start.transform.position + end.transform.position) / 2;
        transform.LookAt(end.transform);
        float distance = Vector3.Distance(start.transform.position, end.transform.position);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance);



    }
}
