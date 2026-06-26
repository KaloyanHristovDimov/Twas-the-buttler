using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullUpWhiteboard : MonoBehaviour
{
    
    [SerializeField] private GameObject whiteboard;

    [SerializeField] private GameObject inventoryUI;

    [SerializeField] private float ammountOfYToMoveInventoryUI;

    [SerializeField] private float ammountOfYToMove;

    [SerializeField] private float movementDuration = .25f;

    [SerializeField] private List<GameObject> buttonsToToggleWithTheWiteboard;
    
    
    
    IEnumerator MoveObject(GameObject gameObjectToMove, float ammountOfYToMove, float duration)
    {
        
        Vector3 startPos = gameObjectToMove.transform.position;
        Vector3 targetPos = startPos + new Vector3(0f, ammountOfYToMove, 0f);

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;

            gameObjectToMove.transform.position = Vector3.Lerp(startPos, targetPos, counter / duration);
            
            yield return null;
        }
    }
    
    
    

    public void PullUp()
    {
        ToggleUI();
        StartCoroutine(MoveObject(whiteboard, ammountOfYToMove, movementDuration));
        StartCoroutine(MoveObject(inventoryUI, ammountOfYToMoveInventoryUI, movementDuration));
    }

    public void PushDown()
    {
        ToggleUI();
        StartCoroutine(MoveObject(whiteboard, -ammountOfYToMove, movementDuration));
        StartCoroutine(MoveObject(inventoryUI, -ammountOfYToMoveInventoryUI, movementDuration));
    }

    private void ToggleUI()
    {
        foreach (GameObject button in buttonsToToggleWithTheWiteboard)
        {
            button.SetActive(!button.activeSelf);
        }
    }
    
}
