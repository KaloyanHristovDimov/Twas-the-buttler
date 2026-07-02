using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerMinuteText;
    [SerializeField] private TextMeshProUGUI timerSecondText;

    [SerializeField] private float timerlength;

    private bool timerActive;

    private void Start()
    {
        timerActive = true;
        timerlength = PersistentValues.Instance.timerlength * 60;
        float minutes = Mathf.FloorToInt(timerlength / 60);
        float seconds = Mathf.FloorToInt(timerlength % 60);
        timerMinuteText.text = minutes.ToString();
        timerSecondText.text = seconds.ToString();
    }


    void Update()
    {
        if(timerActive)
        {
            if (timerlength > 0)
            { 
                timerlength -= Time.deltaTime;
                float minutes = Mathf.FloorToInt(timerlength / 60);
                float seconds = Mathf.FloorToInt(timerlength % 60);
                timerMinuteText.text = minutes.ToString();
                timerSecondText.text = seconds.ToString();
            }
            else
            {
                timerlength = 0;
                timerActive = false;
                float minutes = Mathf.FloorToInt(timerlength / 60);
                float seconds = Mathf.FloorToInt(timerlength % 60);
                timerMinuteText.text = minutes.ToString();
                timerSecondText.text = seconds.ToString();
            }
        }
    }

    public void StopTime(int level)
    {
        PersistentValues.Instance.levelTimes[level-1] = timerlength * 60;
    }

    public void ReduceTime(int amount)
    {
        timerlength -= amount;
    }

}
