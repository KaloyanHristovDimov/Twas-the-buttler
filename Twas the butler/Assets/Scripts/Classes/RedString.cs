using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RedString : MonoBehaviour, IPointerClickHandler
{
    public HashSet<ClueData> clues = new HashSet<ClueData>();
    public enum StringTag
    {
        None,
        Used,
        Killed
    }
    public StringTag stringTag = StringTag.None;
    public GameObject label;

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
        Vector3 startPos = start.transform.GetChild(0).position;
        Vector3 endPos = end.transform.GetChild(0).position;

        // Place in the middle
        transform.position = (startPos + endPos) * 0.5f;

        // Direction from start to end
        Vector3 direction = endPos - startPos;

        // Rotate so the capsule's Y axis points along the direction
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        // Stretch along Y (capsule height)
        float distance = direction.magnitude;

        Vector3 scale = transform.localScale;
        scale.y = distance * 0.5f; // Default capsule height is 2 units
        transform.localScale = scale;

        label = GetComponentInChildren<TMPro.TextMeshPro>().gameObject;
        label.transform.position = (startPos + endPos) * 0.5f;
        label.transform.position += new Vector3(0, 0, -0.2f); // Offset above the string
        label.transform.rotation = Quaternion.Euler(0, 0, 0);
        label.GetComponent<KeepScale>().ReturnScale();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ToolManager.Instance.currentTool == ToolManager.ToolType.StringDeleteTool)
        {
            Destroy(gameObject);
        }
    }
}
