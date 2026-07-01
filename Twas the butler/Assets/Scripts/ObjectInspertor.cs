using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ObjectInspector : MonoBehaviour, IDragHandler
{
    public static ObjectInspector Instance { get; private set; }
    [SerializeField]
    private bool copyObject = false;

    private GameObject selectedObject;
    private GameObject copyOfSelectedObject;

    public float rotationSpeed = 1f;
    private Vector3 originalObjectPosition;
    private Quaternion originalObjectRotation;

    public UnityEvent onStartInspection;

    private Clue originalClue;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void StartInspection(GameObject gameObject, float inspectDistance)
    {
        originalClue = gameObject.GetComponentInChildren<Clue>();
        if (selectedObject != null) return;
        if (copyObject)
        {
            copyOfSelectedObject = Instantiate(gameObject);
            copyOfSelectedObject.name = "Copy of inspected object";
            
            
            selectedObject = copyOfSelectedObject;
        }
        else selectedObject = gameObject;
        originalObjectPosition = selectedObject.transform.position;
        originalObjectRotation = selectedObject.transform.rotation;
        selectedObject.transform.SetParent(transform);
        Vector3 objectScale = selectedObject.transform.localScale;
        if (copyObject) selectedObject.transform.localScale = new Vector3(objectScale.x * 40, objectScale.y * 40, objectScale.z * 40);
        else selectedObject.transform.localScale = new Vector3(objectScale.x / 5, objectScale.y / 5, objectScale.z / 5);
        selectedObject.transform.localPosition = new Vector3(inspectDistance / 10, 0f, 0f);
        Clue copyClue = copyOfSelectedObject.GetComponentInChildren<Clue>();
        if (copyClue != null)
        {
            copyClue.HandleCopies(gameObject.GetComponentInChildren<Clue>());
        }

        onStartInspection.Invoke();


    }

    public void OnDrag(PointerEventData eventData)
    {

        float rotY = -eventData.delta.x * rotationSpeed;
        float rotX = eventData.delta.y * rotationSpeed;

        selectedObject.transform.Rotate(
            Camera.main.transform.up,
            rotY,
            Space.World);

        selectedObject.transform.Rotate(
            Camera.main.transform.right,
            rotX,
            Space.World);
    }

    public void StopInspection()
    {
        if (selectedObject == null) return;
        Debug.Log("Stopping inspection");
        if (originalClue != null)
        {
            originalClue.enabled = true;
            originalClue = null;
        }
        
        
        if (copyObject)
        {
            Debug.Log("copy object");
            copyOfSelectedObject = GameObject.Find("Copy of inspected object");
            Destroy(copyOfSelectedObject);
            Debug.Log("Destroyed copy object");
            return;
        }
        Debug.Log("Skipped copy object");
        selectedObject.transform.SetParent(null);
        Vector3 objectScale = selectedObject.transform.localScale;
        selectedObject.transform.localScale = new Vector3(objectScale.x * 2, objectScale.y * 2, objectScale.z * 2);
        selectedObject.transform.position = originalObjectPosition;
        selectedObject.transform.rotation = originalObjectRotation;
        selectedObject = null;
        
    }
}
