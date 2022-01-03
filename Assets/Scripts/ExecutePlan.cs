using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExecutePlan : MonoBehaviour
{
    public static UnityEvent actionFinished;
    private int index = 0;
    private string[] initialPlan = CallSolver.initialPlan;
    private string[] fullPlan = CallSolver.fullPlan;
    private string[] splitCurAct;
    private bool solvedInitialState = false;
    private TMPro.TextMeshProUGUI stateText;

    private void Start()
    {
        stateText = GameObject.FindGameObjectWithTag("State of Solving Label").GetComponent<TMPro.TextMeshProUGUI>();
        actionFinished = new UnityEvent();
        actionFinished.AddListener(TakeActionBuffer);
        if(GetInput.basicStart)
        {
            StartCoroutine(FullPlanStart());
        }
        else
        {
            StartCoroutine(InitialPlanStart());
        }
        
    }

    IEnumerator InitialPlanStart()
    {
        stateText.text = "shifting to the initial state...";
        yield return new WaitForSeconds(3);
        TakeActionBuffer();
    }

    IEnumerator FullPlanStart()
    {
        stateText.text = "now solving for the goal...";
        yield return new WaitForSeconds(3);
        index = 0;
        solvedInitialState = true;
        TakeAction(fullPlan);
    }

    private void TakeActionBuffer()
    {
        if(solvedInitialState)
        {
            TakeAction(fullPlan);
        }
        else
        {
            TakeAction(initialPlan);
        }
    }

    private void TakeAction(string[] plan)
    {
        if(index < plan.Length)
        {
            splitCurAct = plan[index].Split(' ');
            index++;
            switch (splitCurAct[0])
            {
                case "pick-up":
                    MoveBlock.pickUpEvent.Invoke(splitCurAct[1]);
                    break;
                case "put-down":
                    MoveBlock.putDownEvent.Invoke(splitCurAct[1]);
                    break;
                case "stack":
                    MoveBlock.stackEvent.Invoke(splitCurAct[1], splitCurAct[2]);
                    break;
                case "unstack":
                    MoveBlock.unstackEvent.Invoke(splitCurAct[1], splitCurAct[2]);
                    break;
            }
        }
        else
        {
            if(!solvedInitialState)
            {
                //wait 5 seconds
                StartCoroutine(FullPlanStart());
            }
            else
            {
                //trigger an event to spawn button in the gamemanager
                solvedInitialState = false;
                GetInput.basicStart = false;
                GameManager.allSolved.Invoke();
            }
        }
    }
}
