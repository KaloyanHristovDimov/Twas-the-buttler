using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class Clue : MonoBehaviour, IPointerClickHandler
{
    public List<ClueData> clueData;
    public bool destroyOnClick = false;
    public UnityEvent OnCluePickedUpEvent;
    private GameObject originalClueGameObject;
    [SerializeField] private PopupScript popupScript;

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (ClueData data in clueData)
        {
            InventoryManager.Instance.AddClueToInventory(data);
        }
        OnCluePickedUpEvent.Invoke();
        popupScript.StartPopupCoroutine(clueData);
        if (originalClueGameObject != null) Destroy(originalClueGameObject);
        if (destroyOnClick) Destroy(gameObject);
        else Destroy(GetComponent<Clue>());
    }

    public void HandleCopies(Clue originalClue)
    {
        originalClueGameObject = originalClue.gameObject;
        Debug.Log("GameObject Linked");
    }
}
