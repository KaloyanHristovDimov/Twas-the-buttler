using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class InspectableObject : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private string Requirement = "This gameobject needs to be on the water layer to work";


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {gameObject.name}");
        ObjectInspector.Instance.StartInspection(gameObject);
    }
}
