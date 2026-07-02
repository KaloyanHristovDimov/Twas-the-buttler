using UnityEngine;

[CreateAssetMenu(menuName = "Documentation/Guide")]
public class NewMonoBehaviourScript : ScriptableObject
{
    [TextArea(50, 10)]
    public string documentation;
}
