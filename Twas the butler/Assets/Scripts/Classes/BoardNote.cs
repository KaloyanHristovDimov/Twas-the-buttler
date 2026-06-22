using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BoardNote : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SpriteRenderer noteSpriteRenderer;
    public ClueData clueData;

    public GameObject stringDragVisualPrefab;
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
                // Handle clue moving logic here
                Debug.Log("Dropped note: " + clueData.clueName);
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
            if (targetNote != null)
            {
                // Handle string connection logic here
                Debug.Log("Dragging over a valid string target: " + targetNote.name);

                GameObject stringObject = Instantiate(StringPrefab, transform.position, Quaternion.identity);
                RedString redString = stringObject.GetComponent<RedString>();

                redString.SetClueData(clueData, targetNote.clueData);
                redString.SetStartAndEndPoints(this.gameObject, targetNote.gameObject);
                StringManager.Instance.AddRedString(redString);
            }
        }
    }
}
