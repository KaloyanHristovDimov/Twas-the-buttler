using UnityEngine;

public class ToolManager : MonoBehaviour
{
    /// <summary>
    /// Defines how the player can interact with notes and strings in the game.
    /// </summary>
    public enum ToolType
    {
        None,
        ClueMovingTool,
        RedStringTool,
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
            case ToolType.StringDeleteTool:
                currentTool = ToolType.StringDeleteTool;
                break;
            default:
                currentTool = ToolType.None;
                break;
        }
    }
    
}