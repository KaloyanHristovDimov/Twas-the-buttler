using UnityEngine;
using NaughtyAttributes;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public Slot[] inventorySlots;
    public GameObject slotPrefab;

    public ClueData testClue;

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
        slotPrefab = inventorySlots[0].gameObject;
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
        Instantiate(slotPrefab, slotPrefab.transform.parent).GetComponent<Slot>().SetClueData(clueData);
    }

    [Button]
    private void TestAddClue()
    {
        
        AddClueToInventory(testClue);
    }

}