
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ClickLocationDetector : MonoBehaviour
{
    [SerializeField] private Vector3 clickPosition;
    
    [SerializeField] private UnityEvent<Vector3> OnClick;

    void Start()
    {
        Debug.Log("started");
        Debug.Log(Pointer.current);
        Debug.Log(Touchscreen.current);
        Debug.Log("started fully");
    }

    void Update() { 
        // Get the mouse click position in world space 
        if (Pointer.current.press.isPressed)
        {
            CastRayForDetection(Pointer.current.position.ReadValue());
            // Debug.Log(Pointer.current.position.ReadValue());
        }
        
        else if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Debug.Log("touchPosition:");
            // Debug.Log(Touchscreen.current.primaryTouch.position.ReadValue());
            CastRayForDetection(Touchscreen.current.primaryTouch.position.ReadValue());
        }
        
        

    }

    private void CastRayForDetection(Vector2 screenPosition)
    {
        // Debug.Log(screenPosition);
        Ray mouseRay = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo))
        {
            Vector3 clickWorldPosition = hitInfo.point;
            // Debug.Log(hitInfo.point);

            clickPosition = clickWorldPosition;

            Vector3 direction = mouseRay.direction;
            Debug.DrawRay(mouseRay.origin, hitInfo.point - mouseRay.origin, Color.red, 5f);

            //activate trigger and send it the coordinates of the click

            OnClick.Invoke(clickPosition);
        }
    }


}
