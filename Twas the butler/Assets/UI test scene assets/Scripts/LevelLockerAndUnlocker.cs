using System.Collections.Generic;
using UnityEngine;

public class LevelLockerAndUnlocker : MonoBehaviour
{
    [SerializeField] private List<UnlockedBool> levelSelectorsOnScreen;

    private void OnEnable()
    {
        foreach (UnlockedBool levelSelector in levelSelectorsOnScreen)
        {
            levelSelector.CheckIfUnlocked();
        }
    }
}
