using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public enum ToolType
    {
        None,
        ClueMovingTool,
        RedStringTool,
        StringTagTool,
        StringDeleteTool
    }
    public static ToolManager Instance { get; private set; }

    public ToolType currentTool = ToolType.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentTool(ToolType tool)
    {
        switch (tool)
        {
            case ToolType.ClueMovingTool:
                currentTool = ToolType.ClueMovingTool;
                break;
            case ToolType.RedStringTool:
                currentTool = ToolType.RedStringTool;
                break;
            case ToolType.StringTagTool:
                currentTool = ToolType.StringTagTool;
                break;
            case ToolType.StringDeleteTool:
                currentTool = ToolType.StringDeleteTool;
                break;
            default:
                currentTool = ToolType.None;
                break;
        }
    }

    public void SetToolClueMove()
    {
        SetCurrentTool(ToolType.ClueMovingTool);
    }
    public void SetToolRedString()
    {
        SetCurrentTool(ToolType.RedStringTool);
    }
    public void SetToolStringTag()
    {
        SetCurrentTool(ToolType.StringTagTool);
    }
    public void SetToolStringDelete()
    {
        SetCurrentTool(ToolType.StringDeleteTool);
    }
    
}