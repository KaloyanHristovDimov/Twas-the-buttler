using UnityEngine;

public class Clue : MonoBehaviour
{
    public ClueData clueData;

    public void OnClueClicked()
    {
        InventoryManager.Instance.AddClueToInventory(clueData);

    }
}
