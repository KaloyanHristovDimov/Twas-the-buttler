using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class Clue : MonoBehaviour, IPointerClickHandler
{
    public List<ClueData> clueData;
    public bool destroyOnClick = false;
    public UnityEvent OnCluePickedUpEvent;
    private Clue originalClueGameObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (ClueData data in clueData)
        {
            InventoryManager.Instance.AddClueToInventory(data);
        }
        OnCluePickedUpEvent.Invoke();
        PopupScript.Instance.StartPopupCoroutine(clueData);
        if (originalClueGameObject != null)
        {
            originalClueGameObject.enabled = true;
            Destroy(originalClueGameObject);
        }
        if (destroyOnClick) Destroy(gameObject);
        else Destroy(GetComponent<Clue>());
    }

    public void HandleCopies(Clue originalClue)
    {
        originalClueGameObject = originalClue;
        originalClue.enabled = false;
        Debug.Log("GameObject Linked");
    }
}
