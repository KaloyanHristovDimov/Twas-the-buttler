using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RedString : MonoBehaviour, IPointerClickHandler
{
    public HashSet<ClueData> clues = new HashSet<ClueData>();

    public ClueData.StringTag stringTag = ClueData.StringTag.None;
    public GameObject label;

    public HashSet<GameObject> connectedObjects = new HashSet<GameObject>();
    public Transform startNote;
    public Transform endNote;


    public void SetClueData(ClueData clueDataA, ClueData clueDataB)
    {
        clues.Add(clueDataA);
        clues.Add(clueDataB);
        
    }


    public void SetTag(ClueData.StringTag newTag)
    {
        if (stringTag != ClueData.StringTag.None)
        {
            InventoryManager.Instance.AddTagToInventory(stringTag);
        }
        stringTag = newTag;
        label = gameObject.transform.parent.GetComponentInChildren<TextMeshPro>().gameObject;
        label.GetComponent<TextMeshPro>().text = stringTag.ToString();
        
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

        label = transform.parent.GetComponentInChildren<TMPro.TextMeshPro>().gameObject.transform.parent.gameObject;
        label.transform.position = (startPos + endPos) * 0.5f;
        label.transform.position += new Vector3(0, 0, -0.1f); // Offset above the string
        //label.GetComponent<KeepScale>().ReturnScale();
        //label.transform.rotation = Quaternion.Euler(0, 0, 0);
        //Debug.Log("Reset rotation");

        startNote = start.transform;
        endNote = end.transform;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ToolManager.Instance.currentTool == ToolManager.ToolType.StringDeleteTool)
        {
            StringManager.Instance.RemoveRedString(this);
            if (stringTag != ClueData.StringTag.None)
            {
                InventoryManager.Instance.AddTagToInventory(stringTag);
            }
            Destroy(gameObject.transform.parent.gameObject);

        }
    }
}
