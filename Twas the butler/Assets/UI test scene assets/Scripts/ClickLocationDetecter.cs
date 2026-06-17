
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ClickLocationDetector : MonoBehaviour
{
    [SerializeField] private Vector3 clickPosition;
    
    [SerializeField] private UnityEvent<Vector3> OnClick;
    
    
    void Update() { 
        // Get the mouse click position in world space 
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log(Mouse.current.position.ReadValue()); 
            Ray mouseRay = Camera.main.ScreenPointToRay( Mouse.current.position.ReadValue() ); 

            if (Physics.Raycast( mouseRay, out RaycastHit hitInfo )) { 
                Vector3 clickWorldPosition = hitInfo.point; 
                Debug.Log(hitInfo.point);
                
                clickPosition = clickWorldPosition;
                
                Vector3 direction = mouseRay.direction;
                Debug.DrawRay(mouseRay.origin, hitInfo.point - mouseRay.origin, Color.red, 5f);

                //activate trigger and send it the coordinates of the click
                
                OnClick.Invoke(clickPosition);
            }
        }
        

    }
}
