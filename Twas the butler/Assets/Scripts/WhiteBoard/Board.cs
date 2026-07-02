using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject notePrefab;
    public float xOffset = -0.1f;
    [SerializeField]
    private List<ClueData> cluesToAddOnStart;
 

    private void Start()
    {
        foreach (ClueData clueData in cluesToAddOnStart) 
        {
            InventoryManager.Instance.AddClueToInventory(clueData);
        }
    }

    public void PlaceNote(ClueData clueData, Vector3 worldPosition)
    {
        GameObject rotationObject = gameObject.transform.parent.gameObject;
        Vector3 newPosition = Vector3.zero;

        switch (rotationObject.transform.rotation.eulerAngles.y) 
        {
            case 90:
                newPosition = new Vector3(worldPosition.x + xOffset, worldPosition.y, worldPosition.z);
                break;
            case 180:
                newPosition = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z - xOffset);
                break;
            case 270:
                newPosition = new Vector3(worldPosition.x - xOffset, worldPosition.y, worldPosition.z);
                break;
            case 0:
                newPosition = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z + xOffset);
                break;
            default:
                newPosition = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z + xOffset);
                break;
        }

        GameObject noteObject = Instantiate(
            notePrefab,
            newPosition,
            Quaternion.identity,
            transform);
        noteObject.transform.localEulerAngles = Vector3.zero;
        noteObject.GetComponent<BoardNote>().SetClueData(clueData);
    }

}
