using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExecutePlan : MonoBehaviour
{
    public static UnityEvent actionFinished;
    private int index = 0;
    private string[] plan = CallSolver.generated_plan;
    private string[] splitCurAct;

    private void Start()
    {
        Debug.Log("executing plan...");
        actionFinished = new UnityEvent();
        actionFinished.AddListener(TakeAction);
        TakeAction();
    }

    private void TakeAction()
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
