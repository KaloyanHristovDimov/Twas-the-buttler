using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChildrenImageVizability : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    private void OnEnable()
    {
        foreach (Slot slot in InventoryManager.Instance.inventorySlots) 
        {
            Transform child = slot.transform;
            if (child.GetComponent<Image>() != null) child.GetComponent<Image>().enabled = true;
            if (child.GetComponentInChildren<TextMeshProUGUI>() != null) child.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }

        foreach (TagSlot slot in InventoryManager.Instance.tagSlots)
        {
            Transform child = slot.transform;
            if (child.GetComponent<Image>() != null) child.GetComponent<Image>().enabled = true;
            if (child.GetComponentInChildren<TextMeshProUGUI>() != null) child.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }
    }

    private void OnDisable()
    {
        foreach (Slot slot in InventoryManager.Instance.inventorySlots)
        {
            Transform child = slot.transform;
            if (child.GetComponent<Image>() != null) child.GetComponent<Image>().enabled = false;
            if (child.GetComponentInChildren<TextMeshProUGUI>() != null) child.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }

        foreach (TagSlot slot in InventoryManager.Instance.tagSlots)
        {
            Transform child = slot.transform;
            if (child.GetComponent<Image>() != null) child.GetComponent<Image>().enabled = false;
            if (child.GetComponentInChildren<TextMeshProUGUI>() != null) child.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }
    }
}
