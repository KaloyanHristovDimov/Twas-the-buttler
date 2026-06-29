using UnityEngine;

[CreateAssetMenu(fileName = "New Solution", menuName = "Solution")]
public class Solution : ScriptableObject
{
    public ClueData requiredClueA;
    public ClueData requiredClueB;

    public ClueData.StringTag requiredStringTag;

}
