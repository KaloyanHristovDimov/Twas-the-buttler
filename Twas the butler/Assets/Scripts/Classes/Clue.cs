using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class Clue : MonoBehaviour, IPointerClickHandler
{
    public ClueData clueData;
    public bool destroyOnClick = false;
    public UnityEvent OnCluePickedUpEvent;
    private GameObject originalClueGameObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.AddClueToInventory(clueData);
        OnCluePickedUpEvent.Invoke();
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
