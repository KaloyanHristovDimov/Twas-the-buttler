using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SolutionCheck : MonoBehaviour
{
    public List<Solution> solutions;
    [SerializeField] private UnityEvent onAnswerCorrect;
    [SerializeField] private UnityEvent onAnswerInCorrect;
    private int correctStrings = 0;

    public void CheckSolution()
    {
        foreach (var solution in solutions)
        {       
                if (StringManager.Instance.HasString(solution.requiredClueA, solution.requiredClueB, solution.requiredStringTag))
                {
                    correctStrings++;
                }
        }
        if (correctStrings == solutions.Count)
        {
            onAnswerCorrect.Invoke();
        }
        else
        {
            onAnswerInCorrect.Invoke();
        }
        Debug.Log($"Correct Strings: {correctStrings} / {solutions.Count}");
        correctStrings = 0;
    }
}
