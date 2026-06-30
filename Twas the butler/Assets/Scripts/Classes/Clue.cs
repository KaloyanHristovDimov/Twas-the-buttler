using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class Clue : MonoBehaviour, IPointerClickHandler
{
    public ClueData clueData;
    public bool destroyOnClick = false;
    public UnityEvent OnCluePickedUpEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.AddClueToInventory(clueData);
        OnCluePickedUpEvent.Invoke();
        if (destroyOnClick) Destroy(gameObject);
        else Destroy(GetComponent<Clue>());
    }
}
