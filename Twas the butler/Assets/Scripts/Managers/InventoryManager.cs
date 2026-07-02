using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Slot> inventorySlots;
    public List<TagSlot> tagSlots;
    public GameObject slotPrefab;
    public GameObject tagSlotPrefab;

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
        foreach (Transform child in transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null)
            {
                inventorySlots.Add(slot);
            }
        }
    }

    /// <summary>
    /// Adds a clue to the inventory by finding the first empty slot and setting its ClueData. 
    /// It instantiates a new slot prefab and adds it to the inventory.
    /// Additionally, if the clue has associated string tags, it adds those tags to the inventory as well.
    /// </summary>
    /// <param name="clueData"></param>
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

        GameObject newSlotObj = Instantiate(slotPrefab, gameObject.GetComponentInChildren<VerticalLayoutGroup>(true).gameObject.transform);
        Slot newSlot = newSlotObj.GetComponent<Slot>();
        newSlot.SetClueData(clueData);
        inventorySlots.Add(newSlot);
        if (clueData.asosiatedStringTags != null)
        {
            foreach (var tag in clueData.asosiatedStringTags)
            {
                AddTagToInventory(tag);
            }
        }
    }

    /// <summary>
    /// Adds the specified string tag to the inventory UI, creating a new tag slot for it.
    /// </summary>
    /// <param name="stringTag">The string tag to add to the inventory. Represents the tag data to be displayed or stored.</param>
    public void AddTagToInventory(ClueData.StringTag stringTag)
    {
        GameObject canvas = GameObject.Find("Main Canvas");
        string childName = "Push Witeboard Down";
        Transform[] children = canvas.transform.GetComponentsInChildren<Transform>(true);
        bool showTag = false;
        foreach (Transform child in children)
        {
            Transform[] grandChildren = child.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform grandChild in grandChildren)
            {
                if (grandChild.name == childName)
                {
                    showTag = child.gameObject.activeInHierarchy;
                }
            }
        }
        
        if (showTag)
        {
            GameObject tag = Instantiate(tagSlotPrefab, gameObject.GetComponentInChildren<VerticalLayoutGroup>().gameObject.transform);
            TagSlot tagSlot = tag.GetComponent<TagSlot>();
            tagSlot.SetTag(stringTag);
            tagSlots.Add(tagSlot);
        }
        else 
        {
            GameObject tag = Instantiate(
                                tagSlotPrefab,
                                gameObject.GetComponentInChildren<VerticalLayoutGroup>().transform
                                );

            TagSlot tagSlot = tag.GetComponent<TagSlot>();
            tagSlot.SetTag(stringTag);
            tagSlots.Add(tagSlot);

            if (tag.GetComponent<Image>() != null) tag.GetComponent<Image>().enabled = false;
            if (tag.GetComponentInChildren<TextMeshProUGUI>() != null) tag.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }
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