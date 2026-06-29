using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChildrenImageVizability : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    private void OnEnable()
    {
        foreach (Transform child in parent.transform) 
        {
            if(child.GetComponent<Image>() != null) child.GetComponent<Image>().enabled = true;
            if (child.GetComponentInChildren<TextMeshProUGUI>() != null) child.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }
    }

    private void OnDisable()
    {
        foreach (Transform child in parent.transform)
        {
            if (child.GetComponent<Image>() != null) child.GetComponent<Image>().enabled = false;
            if (child.GetComponentInChildren<TextMeshProUGUI>() != null) child.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }
    }
}
