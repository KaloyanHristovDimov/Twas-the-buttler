using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour
{
    public GameObject notePrefab;
    public float xOffSet = -0.1f;

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

    public void PlaceNote(ClueData clueData, Vector3 worldPosition)
    {
        GameObject noteObject = Instantiate(
            notePrefab,
            new Vector3(worldPosition.x + xOffSet, worldPosition.y, worldPosition.z),
            Quaternion.identity,
            transform.parent);

        noteObject.GetComponent<BoardNote>().SetClueData(clueData);
    }

}
