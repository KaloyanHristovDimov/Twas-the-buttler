using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedBool : MonoBehaviour
{
    [SerializeField] private bool unlocked;

    [SerializeField] private int level;

    [SerializeField] private GameObject parentPanel;

    [SerializeField] private PersistentValues persistentValuesScript;

    [SerializeField] private GameObject button;

    [SerializeField] private GameObject unlockedImage;

    [SerializeField] private GameObject lockedImage;


    public void CheckIfUnlocked()
    {
        unlocked = persistentValuesScript.GetBool(level - 1);
        if (unlocked)
        {
            button.SetActive(true);
            //unlockedImage.SetActive(true);
            lockedImage.SetActive(false);
        }
        else
        {
            button.SetActive(false);
            //unlockedImage.SetActive(false);
            lockedImage.SetActive(true);
        }
    }
    
}
