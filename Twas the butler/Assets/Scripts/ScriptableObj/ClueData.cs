using UnityEngine;

[CreateAssetMenu(fileName = "New Clue Data", menuName = "Clue Data")]
public class ClueData : ScriptableObject
{
    public string clueName;
    public enum ClueType { Note, Object, Location, Person }
    public ClueType clueType;
    public Sprite clueSprite;
    public enum StringTag { None, Used, Killed, Family, Wore, Recieved, Harrased, Gave, About }
    public StringTag asosiatedStringTag;
}
