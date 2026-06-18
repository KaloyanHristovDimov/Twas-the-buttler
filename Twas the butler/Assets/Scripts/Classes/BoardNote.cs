using UnityEngine;
using UnityEngine.UI;

public class BoardNote : MonoBehaviour
{
    public Image image;
    public ClueData clueData;

    public void SetClueData(ClueData data)
    {
        clueData = data;
        UpdateNote();
    }

    public void UpdateNote()
    {
        if (clueData != null)
        {
            image.sprite = clueData.clueSprite;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
