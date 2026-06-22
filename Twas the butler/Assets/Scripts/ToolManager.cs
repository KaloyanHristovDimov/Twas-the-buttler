using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public enum ToolType
    {
        None,
        ClueMovingTool,
        RedStringTool
    }
    public static ToolManager Instance { get; private set; }

    public ToolType currentTool { get; private set; } = ToolType.None;

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
            case ToolType. RedStringTool:
                currentTool = ToolType.RedStringTool;
                break;
            default:
                currentTool = ToolType.None;
                break;
        }
    }
}
