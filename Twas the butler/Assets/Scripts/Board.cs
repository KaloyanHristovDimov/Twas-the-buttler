using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject notePrefab;
    public float xOffset = -0.1f;
    [SerializeField]
    private List<ClueData> cluesToAddOnStart;
    //UI version
    //public void OnDrop(PointerEventData eventData)
    //{
    //    Slot slot = eventData.pointerDrag.GetComponent<Slot>();
    //    if (slot == null || !slot.HasClueData())
    //        return;

    //    ClueData clueData = slot.GetClueData();
    //    GameObject noteObject = Instantiate(notePrefab, transform, false);

    //    RectTransform noteRect = noteObject.GetComponent<RectTransform>();
    //    RectTransform boardRect = GetComponent<RectTransform>();
    //    noteObject.GetComponent<BoardNote>().SetClueData(clueData);

    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(boardRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

    //    noteRect.anchoredPosition = localPoint;

    //    noteObject.GetComponent<BoardNote>().SetClueData(clueData);

    //    slot.ClearClueData();
    //}

    private void Start()
    {
        foreach (ClueData clueData in cluesToAddOnStart) 
        {
            InventoryManager.Instance.AddClueToInventory(clueData);
        }
    }

    public void PlaceNote(ClueData clueData, Vector3 worldPosition)
    {
        GameObject rotationObject = gameObject.transform.parent.gameObject;
        Vector3 newPosition = Vector3.zero;

        switch (rotationObject.transform.rotation.eulerAngles.y) 
        {
            case 90:
                newPosition = new Vector3(worldPosition.x + xOffset, worldPosition.y, worldPosition.z);
                break;
            case 180:
                newPosition = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z - xOffset);
                break;
            case 270:
                newPosition = new Vector3(worldPosition.x - xOffset, worldPosition.y, worldPosition.z);
                break;
            case 0:
                newPosition = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z + xOffset);
                break;
        }

        GameObject noteObject = Instantiate(
            notePrefab,
            newPosition,
            Quaternion.identity,
            transform);
        noteObject.transform.localEulerAngles = Vector3.zero;
        noteObject.GetComponent<BoardNote>().SetClueData(clueData);
    }

}
