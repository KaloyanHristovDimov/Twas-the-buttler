using UnityEngine;

public class eitherenableOrEnableObject : MonoBehaviour
{

    [SerializeField] private GameObject uiScreen;

    public void setActive()
    {
        uiScreen.SetActive(true);
    }

    public void setInactive()
    {
        uiScreen.SetActive(false);
    }
}
