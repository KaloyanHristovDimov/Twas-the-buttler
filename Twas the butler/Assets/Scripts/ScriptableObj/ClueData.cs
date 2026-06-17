using UnityEngine;

[CreateAssetMenu(fileName = "New Clue Data", menuName = "Clue Data")]
public class ClueData : ScriptableObject
{
    public string clueName;
    public enum ClueType { Note, Object, Location, Person }
    public Sprite clueSprite;
}
