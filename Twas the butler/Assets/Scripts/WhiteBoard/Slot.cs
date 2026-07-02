using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("This is the UI Representation of a clue")]

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
            if (ParentHasOtherActiveImage())
                slotImage.sprite = clueData.clueSprite;
            else 
            {
                if (slotImage == null) Debug.Log("No slot image");
                slotImage.sprite = clueData.clueSprite;
                slotImage.enabled = false;
            }
        }
        else
        {
            if (dragVisual != null)
            {
                Destroy(dragVisual);
            }
            InventoryManager.Instance.inventorySlots.Remove(this);
            Destroy(gameObject);
        }
    }

    public bool ParentHasOtherActiveImage()
    {
        Transform parent = transform.parent;

        if (parent == null)
            return false;

        foreach (Transform child in parent)
        {
            if (child == transform)
                continue;

            Image image = child.GetComponent<Image>();

            if (image != null && image.enabled && child.gameObject.activeInHierarchy)
            {
                return true;
            }
        }

        return false;
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

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Board board = hit.collider.GetComponent<Board>();

            if (board != null)
            {
                board.PlaceNote(clueData, hit.point);

                ClearClueData();
            }
        }
    }


}