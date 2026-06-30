using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BoardNote : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SpriteRenderer noteSpriteRenderer;
    public ClueData clueData;

    //public GameObject stringDragVisualPrefab;
    public GameObject StringPrefab;

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

                GameObject stringObject = Instantiate(StringPrefab, transform.position, Quaternion.identity);
                RedString redString = stringObject.GetComponentInChildren<RedString>();

                redString.SetClueData(clueData, targetNote.clueData);
                redString.SetStartAndEndPoints(this.gameObject, targetNote.gameObject);
                StringManager.Instance.AddRedString(redString);
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
                Vector3 worldPosition = hit.point;
                transform.position = worldPosition;
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
    }

    

}

