using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnObjectScript : MonoBehaviour
{

    [SerializeField] private GameObject rotatingObject;
    // Source - https://stackoverflow.com/a/37588536
    // Posted by Programmer, modified by community. See post 'Timeline' for change history
    // Retrieved 2026-06-16, License - CC BY-SA 3.0

    bool rotating = false;
    //public GameObject objectToRotate;
    
    //added:
    [SerializeField] private float turnDuration = 0.25f;
    //
    
    void Start()
    {
        //(I turned this line into a comment) StartCoroutine(rotateObject(objectToRotate, new Vector3(0, 0, 90), 3f));
    }

    
    IEnumerator rotateObject(GameObject gameObjectToMove, Vector3 eulerAngles, float duration)
    {
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Vector3 newRot = gameObjectToMove.transform.localEulerAngles + eulerAngles;

        Vector3 currentRot = gameObjectToMove.transform.localEulerAngles;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            gameObjectToMove.transform.localEulerAngles = Vector3.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
        rotating = false;
    }


    
    
    public void turnLeft()
    {
        StartCoroutine(rotateObject(rotatingObject, new Vector3(0, -90, 0), turnDuration));
    }
    public void turnRight()
    {
        StartCoroutine(rotateObject(rotatingObject, new Vector3(0, 90, 0), turnDuration));
    }
    
    
    
}
