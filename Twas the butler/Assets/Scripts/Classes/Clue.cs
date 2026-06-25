using UnityEngine;
using UnityEngine.EventSystems;
public class Clue : MonoBehaviour, IPointerClickHandler
{
    public ClueData clueData;

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.AddClueToInventory(clueData);
        Destroy(gameObject);

    }
}
