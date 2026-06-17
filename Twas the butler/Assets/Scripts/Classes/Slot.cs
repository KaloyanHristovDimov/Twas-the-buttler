using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    public int slotIndex;
    public ClueData clueData;

    public Image slotImage;


    private void Awake()
    {
        slotImage = GetComponent<Image>();
        ClearClueData();

    }

    public void SetClueData(ClueData data)
    {
        clueData = data;
    }

    public void ClearClueData()
    {
        clueData = null;
        UpdateSlot();
    }

    public bool HasClueData()
    {
        return clueData != null;
    }

    public void UpdateSlot()
    {
        if (HasClueData())
        {
              slotImage.sprite = clueData.clueSprite;
        }
        else
        {
            slotImage.sprite = null;
        }
    }

    public ClueData GetClueData()
    {
        return clueData;
    }

}
