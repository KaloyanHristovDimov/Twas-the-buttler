using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class InspectableObject : MonoBehaviour, IPointerClickHandler
{
   
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {gameObject.name}");
        ObjectInspector.Instance.StartInspection(gameObject);
    }
}
