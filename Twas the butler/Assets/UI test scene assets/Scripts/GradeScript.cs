using TMPro;
using UnityEngine;

public class GradeScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI minuteText;
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private int criteriaA;
    [SerializeField] private int criteriaB;
    [SerializeField] private int criteriaC;
    [SerializeField] private int criteriaD;
    [SerializeField] private int criteriaE;
    [SerializeField] private GameObject gradeScreen;


    private float time;

    void Start()
    {
        if (PersistentValues.Instance.lastLevelPlayed != 0)
        {
            gradeScreen.SetActive(true);

            time = PersistentValues.Instance.levelTimes[PersistentValues.Instance.lastLevelPlayed];
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);
            minuteText.text = minutes.ToString();
            secondText.text = seconds.ToString();

            gradeText.text = CheckGrade(time);
        }
    }

    private string CheckGrade(float time)
    {
        if (time == 0)
        {
            return "F";
        }
        else if (time == criteriaE)
        {
            return "E";
        }
        else if (time == criteriaD)
        {
            return "E";
        }
        else if (time == criteriaC)
        {
            return "C";
        }
        else if (time == criteriaB)
        {
            return "B";
        }
        else if (time == criteriaA)
        {
            return "A";
        }
        return "No grade available";
    }

}
