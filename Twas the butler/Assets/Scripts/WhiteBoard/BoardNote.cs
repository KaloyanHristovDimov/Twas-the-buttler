using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.Events;

public class BoardNote : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SpriteRenderer noteSpriteRenderer;
    public ClueData clueData;

    //public GameObject stringDragVisualPrefab;
    public GameObject StringPrefab;


    /// <summary>
    /// Sets the clue data to be displayed by this component.
    /// </summary>
    /// <param name="data">The clue data to associate with this component. Cannot be null.</param>
    public void SetClueData(ClueData data)
    {
        clueData = data;
        noteSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        UpdateNote();
    }

    public void UpdateNote()
    {
        if (clueData != null)
        {
            noteSpriteRenderer.sprite = clueData.clueSprite;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        switch(ToolManager.Instance.currentTool)
        {
            case ToolManager.ToolType.ClueMovingTool:
                // Handle clue moving logic here
                Debug.Log("Started dragging note: " + clueData.clueName);
                break;
            case ToolManager.ToolType.RedStringTool:
                // Handle red string dragging logic here
                Debug.Log("Started dragging note for red string: " + clueData.clueName);
                //AudioManager.Instance.DragStart();
                break;
            default:
                Debug.Log("Started dragging note with no tool: " + clueData.clueName);
                break;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Handle end drag event
        // Destroy drag visual
        switch(ToolManager.Instance.currentTool)
        {
            case ToolManager.ToolType.ClueMovingTool:
                HandleNoteMove(eventData);
                break;
            case ToolManager.ToolType.RedStringTool:
                HandleRedStringConnection(eventData);
                //AudioManager.Instance.DragEnd();
                break;
            default:
                Debug.Log("No tool selected, dropping note: " + clueData.clueName);
                break;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        switch(ToolManager.Instance.currentTool)
        {
            case ToolManager.ToolType.ClueMovingTool:
                // Handle clue moving logic here
                Debug.Log("Dragging note: " + clueData.clueName);
                break;
            case ToolManager.ToolType.RedStringTool:
                // Handle red string dragging logic here
                Debug.Log("Dragging note for red string: " + clueData.clueName);
                break;
            default:
                Debug.Log("Dragging note with no tool: " + clueData.clueName);
                break;
        }


    }

    private void HandleRedStringConnection(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the hit object is a valid target for the string
            BoardNote targetNote = hit.collider.GetComponent<BoardNote>();
            if (targetNote != null && targetNote != this && !StringManager.Instance.HasDuplicate(this, targetNote))
            {
                // Handle string connection logic here
                Debug.Log("Dragging over a valid string target: " + targetNote.name);

                GameObject stringObject = Instantiate(StringPrefab, transform.position, Quaternion.identity, gameObject.transform.parent.transform);
                RedString redString = stringObject.GetComponentInChildren<RedString>();
                redString.InitializeTheString(this.gameObject, targetNote.gameObject);

            }
        }
    }

    private void HandleNoteMove(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Board board = hit.collider.GetComponent<Board>();
            if (board != null)
            {
                // Move the note to the new position on the board
                GameObject rotationObject = gameObject.transform.parent.transform.parent.gameObject;
                transform.localEulerAngles = Vector3.zero;
                Vector3 worldPosition = hit.point;
                Vector3 newPosition = Vector3.zero + worldPosition;
                float xOffset = gameObject.transform.parent.gameObject.GetComponent<Board>().xOffset;
                switch (rotationObject.transform.rotation.eulerAngles.y)
                {
                    case 90:
                        newPosition = new Vector3(worldPosition.x + xOffset, worldPosition.y, worldPosition.z);
                        break;
                    case 180:
                        newPosition = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z - xOffset);
                        break;
                    case -90:
                        newPosition = new Vector3(worldPosition.x - xOffset, worldPosition.y, worldPosition.z);
                        break;
                    case 0:
                        newPosition = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z + xOffset);
                        break;
                }
                transform.position = newPosition;
                //AudioManager.Instance.ClueDown();
                Debug.Log("Moved note to new position: " + worldPosition);
            }
        }
        else if (Physics.Raycast(ray, out RaycastHit uiHit))
        {
            // Check if we hit a UI element (like a slot)
            Slot slot = uiHit.collider.GetComponent<Slot>();
            if (slot != null)
            {
                // Handle moving the note back to the slot
                InventoryManager.Instance.AddClueToInventory(clueData);
                Destroy(gameObject);
                Debug.Log("Moved note back to slot: " + slot.name);
            }
        }
        StringManager.Instance.UpdateString(this);
    }

    

}

