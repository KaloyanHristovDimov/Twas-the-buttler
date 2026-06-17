using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public Slot[] inventorySlots;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        inventorySlots = GetComponentsInChildren<Slot>();
    }

    public void AddClueToInventory(ClueData clueData)
    {
            foreach (Slot slot in inventorySlots)
            {
                if (!slot.HasClueData())
                {
                    slot.SetClueData(clueData);
                    slot.UpdateSlot();
                    return;
                }
            }
            Slot firstSlot = inventorySlots[0];
            Instantiate(firstSlot, firstSlot.transform.parent).GetComponent<Slot>().SetClueData(clueData);
    }

    
}