using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class InspectableObject : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private float inspectDistance = 0f;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {gameObject.name}");
        ObjectInspector.Instance.StartInspection(gameObject.transform.parent.gameObject, inspectDistance);
    }
}
