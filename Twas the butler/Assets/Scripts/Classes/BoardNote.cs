using UnityEngine;
using UnityEngine.UI;

public class BoardNote : MonoBehaviour
{
    public SpriteRenderer noteSpriteRenderer;
    public ClueData clueData;

    public void SetClueData(ClueData data)
    {
        clueData = data;
        noteSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        UpdateNote();
    }

    public void UpdateNote()
    {
        if (clueData != null)
        {
            noteSpriteRenderer.sprite = clueData.clueSprite;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
