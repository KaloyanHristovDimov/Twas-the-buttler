using UnityEngine;
using UnityEngine.EventSystems;
public class Clue : MonoBehaviour, IPointerClickHandler
{
    public ClueData clueData;
    public bool destroyOnClick = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.AddClueToInventory(clueData);
        if (destroyOnClick) Destroy(gameObject);
        else Destroy(GetComponent<Clue>());
    }
}
