using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SolutionCheck : MonoBehaviour
{
    [Header("Connections required to solve")]
    public List<Solution> solutions;
    [SerializeField] private UnityEvent onAnswerCorrect;
    [SerializeField] private UnityEvent onAnswerInCorrect;
    private int correctStrings = 0;

    /// <summary>
    /// Evaluates all solutions and invokes the appropriate event based on whether all required strings are present.
    /// </summary>
    /// <remarks>This method checks each solution to determine if the required clues and string tag are
    /// present using the StringManager. If all solutions are correct, the onAnswerCorrect event is invoked; otherwise,
    /// the onAnswerInCorrect event is invoked.</remarks>
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
