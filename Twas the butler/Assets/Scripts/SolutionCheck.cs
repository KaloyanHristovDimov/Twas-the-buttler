using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SolutionCheck : MonoBehaviour
{
    public List<Solution> solutions;
    public void CheckSolution()
    {
        foreach (var solution in solutions)
        {
            if (solution.requiredClueA != null && solution.requiredClueB != null)
            {
                
                if (StringManager.Instance.HasString(solution.requiredClueA, solution.requiredClueB, solution.requiredStringTag))
                {
                    Debug.Log("Congratulations! You've solved the puzzle!");
                    return;
                }
            }
        }

        
    }
}
