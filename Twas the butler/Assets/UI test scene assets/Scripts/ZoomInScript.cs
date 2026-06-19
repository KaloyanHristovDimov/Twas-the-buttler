using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoomInScript : MonoBehaviour
{
    [SerializeField] private GameObject selectableObject;
    
    [SerializeField] private GameObject mainParentOfObjectYouZoomInOn;
    [SerializeField] private GameObject tempParentOfObjectYouZoomInOn;
    [SerializeField] private InUseBool tempParentOfObjectYouZoomInOnInUseBoolScript;
    
    [SerializeField] private List<GameObject> listOfObjectsToToggleActivity;
    
    
    

    public void ZoomInCamera()
    {
        if (tempParentOfObjectYouZoomInOnInUseBoolScript.inUse)
        {return;}
        ChangeToTempParent();
        ToggleListOfObjectsToToggleActivity();
        tempParentOfObjectYouZoomInOnInUseBoolScript.SetInUse();
    }
    
    public void ZoomOutCamera()
    {
        if (!tempParentOfObjectYouZoomInOnInUseBoolScript.inUse)
        {return;}
        ChangeToMainParent();
        ToggleListOfObjectsToToggleActivity();
        tempParentOfObjectYouZoomInOnInUseBoolScript.SetOutOfUse();
    }

    private void ToggleListOfObjectsToToggleActivity()
    {
        foreach (GameObject objectToToggleActivity in listOfObjectsToToggleActivity)
        {
            objectToToggleActivity.SetActive(!objectToToggleActivity.activeSelf);
        }
    }
    
    private void ChangeToTempParent()
    {
        selectableObject.transform.SetParent(tempParentOfObjectYouZoomInOn.transform);
        tempParentOfObjectYouZoomInOn.transform.localPosition = new Vector3(0,0,selectableObject.GetComponent<SphereCollider>().radius + 1);
        selectableObject.transform.localPosition = new Vector3(0,0,0);
    }
    private void ChangeToMainParent()
    {
        selectableObject.transform.SetParent(mainParentOfObjectYouZoomInOn.transform);
        selectableObject.transform.localPosition = new Vector3(0,0,0);
    }
}
