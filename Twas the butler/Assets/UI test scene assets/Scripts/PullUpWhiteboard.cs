using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullUpWhiteboard : MonoBehaviour
{
    
    [SerializeField] private GameObject whiteboard;
    
    [SerializeField] private float ammountOfYToMove;

    [SerializeField] private float movementDuration = .25f;

    [SerializeField] private List<GameObject> buttonsToToggleWithTheWiteboard;
    
    private bool moving = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator MoveObject(GameObject gameObjectToMove, float ammountOfYToMove, float duration)
    {
        if (moving)
        {
            yield break;
        }
        moving = true;

        Vector3 startPos = gameObjectToMove.transform.position;
        Vector3 targetPos = startPos + new Vector3(0f, ammountOfYToMove, 0f);

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;

            gameObjectToMove.transform.position = Vector3.Lerp(startPos, targetPos, counter / duration);
            
            yield return null;
        }
        moving = false;
    }
    
    
    

    public void PullUp()
    {
        ToggleUI();
        StartCoroutine(MoveObject(whiteboard, ammountOfYToMove, movementDuration));
        
    }

    public void PushDown()
    {
        ToggleUI();
        StartCoroutine(MoveObject(whiteboard, -ammountOfYToMove, movementDuration));

    }
    
    private void ToggleUI()
    {
        foreach (GameObject button in buttonsToToggleWithTheWiteboard)
        {
            button.SetActive(!button.activeSelf);
        }
    }
    
}
