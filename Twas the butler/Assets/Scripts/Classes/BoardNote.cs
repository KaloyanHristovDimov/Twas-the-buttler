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
        // Handle begin drag event
        // Create drag visual
        Debug.Log("Begin dragging note: " + clueData.clueName);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Handle end drag event
        // Destroy drag visual
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

    public void OnDrag(PointerEventData eventData)
    {
        // Handle drag event
        // Update drag visual position

        
    }
}
