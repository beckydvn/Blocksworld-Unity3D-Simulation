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
            solvedInitialState = true;
            stateText.text = "solving for the goal...";
        }
        else
        {
            stateText.text = "shifting to the initial state...";
        }
        TakeActionBuffer();
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(3);
        index = 0;
        solvedInitialState = true;
        stateText.text = "solving for the goal...";
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
                StartCoroutine(Reset());
            }
            else
            {
                //trigger an event to spawn button in the gamemanager
                GameManager.allSolved.Invoke();
            }
        }
    }



    /*
     * iterate through the plan
     * "move" script has stack/unstack functions triggered by an event; you can
     * pass the stacking function 2 strings which correspond to the relevant blocks.
     * this move script will just be attached to an empty gameobject.
     * keep track of which index of the plan we are on.
     * "taking action" happens within a function also triggered by an event - 
     * first triggered when the solver is done, and only triggered again when the
     * previous action is finished (stack/unstack functions will invoke it again).
     * so basically this function and the stack/unstack functions play tag with
     * each other until the plan is complete.
    */


}
