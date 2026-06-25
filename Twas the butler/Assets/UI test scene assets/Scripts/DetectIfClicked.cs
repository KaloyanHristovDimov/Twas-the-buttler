using UnityEngine;
using UnityEngine.Events;

public class DetectIfClicked : MonoBehaviour
{
    [SerializeField] private BoxCollider clickableObject;

    [SerializeField] private UnityEvent OnDetected;

    
    public void Detect(Vector3 mousePosition)
    {
        if (clickableObject.bounds.Contains(mousePosition))
        {
            OnDetected.Invoke();
        }
    }
    
}
