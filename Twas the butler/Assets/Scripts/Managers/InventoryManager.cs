using UnityEngine;
using NaughtyAttributes;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
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
        Instantiate(slotPrefab, gameObject.transform).GetComponent<Slot>().SetClueData(clueData);
        if (clueData.asosiatedStringTag != ClueData.StringTag.None)
        {
            AddTagToInventory(clueData.asosiatedStringTag);
        }
    }

    public void AddTagToInventory(ClueData.StringTag stringTag)
    {

    }

    public void RemoveClueFromInventory(ClueData clueData)
    {
        foreach (Slot slot in inventorySlots)
        {
            if (slot.HasClueData() && slot.GetClueData() == clueData)
            {
                slot.ClearClueData();
                return;
            }
        }
    }

    [Button]
    private void TestAddClue()
    {
        
        AddClueToInventory(testClue);
    }

}