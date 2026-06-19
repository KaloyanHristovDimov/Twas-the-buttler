using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int slotIndex;
    public ClueData clueData;

    public Image slotImage;

    private GameObject dragVisual;
    private Canvas parentCanvas;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void SetClueData(ClueData data)
    {
        clueData = data;
        UpdateSlot();
    }

    public void ClearClueData()
    {
        clueData = null;
        UpdateSlot();
    }

    public bool HasClueData()
    {
        return clueData != null;
    }

    public void UpdateSlot()
    {
        if (HasClueData())
        {
              slotImage.sprite = clueData.clueSprite;
        }
        else
        {
            if (dragVisual != null)
            {
                Destroy(dragVisual);
            }
            Destroy(gameObject);
        }
    }

    public ClueData GetClueData()
    {
        return clueData;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!HasClueData())
            return;

        // Make original slot semi-transparent
        Color c = slotImage.color;
        c.a = 0.5f;
        slotImage.color = c;

        // Create drag visual
        dragVisual = new GameObject("DragVisual");
        dragVisual.transform.SetParent(parentCanvas.transform, false);
        dragVisual.transform.SetAsLastSibling();

        Image img = dragVisual.AddComponent<Image>();
        img.sprite = clueData.clueSprite;
        img.raycastTarget = false;

        RectTransform rt = dragVisual.GetComponent<RectTransform>();
        rt.sizeDelta = slotImage.rectTransform.sizeDelta;
        rt.position = eventData.position;
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
        Color c = slotImage.color;
        c.a = 1f;
        slotImage.color = c;

        if (dragVisual != null)
        {
            Destroy(dragVisual);
        }
    }


}