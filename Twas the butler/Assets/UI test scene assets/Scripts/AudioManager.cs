using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;


    public UnityEvent onClueDown;
    public UnityEvent onDragStart;
    public UnityEvent onDragStop;



    public void ClueDown()
    {
        onClueDown.Invoke();
    }

    public void DragStart()
    {
        onDragStart.Invoke();
    }

    public void DragEnd()
    {
        onDragStop.Invoke();
    }


}