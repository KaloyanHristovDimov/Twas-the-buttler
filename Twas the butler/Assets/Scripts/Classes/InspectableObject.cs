using UnityEngine;
using UnityEngine.EventSystems;

public class InspectableObject : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {gameObject.name}");
        ObjectInspector.Instance.StartInspection(gameObject);
    }
}
