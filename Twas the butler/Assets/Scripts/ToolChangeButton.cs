using UnityEngine;
using UnityEngine.EventSystems;

public class ToolChangeButtons : MonoBehaviour, IPointerClickHandler
{
    public ToolManager.ToolType toolTypeToSet;

    public void OnPointerClick(PointerEventData eventData)
    {
        ToolManager.Instance.SetCurrentTool(toolTypeToSet);
    }
}
