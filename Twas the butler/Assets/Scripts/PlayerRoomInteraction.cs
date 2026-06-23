using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRoomInteraction : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            
            if (hit.collider.CompareTag("Clue"))
            {
                Clue clue = hit.collider.GetComponent<Clue>();
                if (clue != null)
                {
                    clue.OnClueClicked();
                }
            }
            else if (hit.collider.CompareTag("InspectableObject"))
            {
                InspectableObject inspectableObject = hit.collider.GetComponent<InspectableObject>();
                if (inspectableObject != null)
                {
                    inspectableObject.StartRotate();
                }
            }
        }
    }
}
