using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Clue Data", menuName = "Clue Data")]
public class ClueData : ScriptableObject
{
    public string clueName;
    [Header("Visual for inventory and note")]
    public Sprite clueSprite;
    public enum StringTag { None, Used, Killed, Family, Wore, Recieved, Harrased, Gave, About }
    public List<StringTag> asosiatedStringTags;
}
