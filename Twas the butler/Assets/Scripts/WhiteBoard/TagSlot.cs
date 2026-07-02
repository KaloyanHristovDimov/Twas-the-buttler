using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TagSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("This is the UI Representation of a tag in players possession")]
    public ClueData.StringTag stringTag;

    public TMPro.TextMeshProUGUI textMesh;

    private GameObject dragVisual;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetTag(ClueData.StringTag tagToSet)
    {
        stringTag = tagToSet;
        Debug.Log(stringTag);
        textMesh.text = stringTag.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (stringTag == ClueData.StringTag.None)
            return;

        // Make original slot semi-transparent
        Color c = textMesh.color;
        c.a = 0.5f;
        textMesh.color = c;

        // Create drag visual
        //dragVisual = new GameObject("DragVisual");
        //dragVisual.transform.SetParent(parentCanvas.transform, false);
        //dragVisual.transform.SetAsLastSibling();

        //Image img = dragVisual.AddComponent<Image>();
        //img.sprite = clueData.clueSprite;
        //img.raycastTarget = false;

        //RectTransform rt = dragVisual.GetComponent<RectTransform>();
        //rt.sizeDelta = slotImage.rectTransform.sizeDelta;
        //rt.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore slot appearance
        Color c = textMesh.color;
        c.a = 1f;
        textMesh.color = c;

        if (dragVisual != null)
        {
            Destroy(dragVisual);
        }

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            RedString redString = hit.collider.GetComponent<RedString>();

            if (redString != null)
            {
                redString.SetTag(stringTag);

                InventoryManager.Instance.tagSlots.Remove(this);
                Destroy(gameObject);
                
            }
        }
    }
}
