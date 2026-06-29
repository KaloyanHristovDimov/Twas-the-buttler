using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ObjectInspector : MonoBehaviour, IDragHandler 
{
    public static ObjectInspector Instance { get; private set; }

    private GameObject selectedObject;

    public float rotationSpeed = 1f;
    private Vector3 originalObjectPosition;
    private Quaternion originalObjectRotation;

    public UnityEvent onStartInspection;

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
        if (selectedObject != null) return;
        selectedObject = gameObject;
        originalObjectPosition = selectedObject.transform.position;
        originalObjectRotation = selectedObject.transform.rotation;
        selectedObject.transform.SetParent(transform);
        Vector3 objectScale = selectedObject.transform.localScale;
        selectedObject.transform.localScale = new Vector3(objectScale.x/2, objectScale.y / 2, objectScale.z / 2);
        selectedObject.transform.localPosition = new Vector3(inspectDistance/2, 0f, 0f);
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
        selectedObject.transform.SetParent(null);
        Vector3 objectScale = selectedObject.transform.localScale;
        selectedObject.transform.localScale = new Vector3(objectScale.x * 2, objectScale.y * 2, objectScale.z * 2);
        selectedObject.transform.position = originalObjectPosition;
        selectedObject.transform.rotation = originalObjectRotation;
        selectedObject = null;
    }
}
