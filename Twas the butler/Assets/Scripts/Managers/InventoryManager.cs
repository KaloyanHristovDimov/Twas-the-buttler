using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public Slot[] inventorySlots;
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
        
        Instantiate(slotPrefab, gameObject.GetComponentInChildren<VerticalLayoutGroup>(true).gameObject.transform).GetComponent<Slot>().SetClueData(clueData);
        if (clueData.asosiatedStringTags != null)
        {
            foreach (var tag in clueData.asosiatedStringTags)
            {
                AddTagToInventory(tag);
            }
        }
    }

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
            Instantiate(tagSlotPrefab, gameObject.GetComponentInChildren<VerticalLayoutGroup>().gameObject.transform).GetComponent<TagSlot>().SetTag(stringTag);
        }
        else 
        {
            GameObject tag = Instantiate(
                                tagSlotPrefab,
                                gameObject.GetComponentInChildren<VerticalLayoutGroup>().transform
                                );

            tag.GetComponent<TagSlot>().SetTag(stringTag);

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