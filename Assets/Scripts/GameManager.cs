using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject getInput;
    public GameObject solver;
    public GameObject executePlan;
    public static UnityEvent inputReceived;
    public static UnityEvent planReceived;

    // Start is called before the first frame update
    void Start()
    {
        inputReceived = new UnityEvent();
        planReceived = new UnityEvent();
        inputReceived.AddListener(CallSolver);
        planReceived.AddListener(CallExecutePlan);
    }
    // TODO: solve how to get from initial setup to initial state first (will need to change some params)...
    // destroy the canvas and configure the blocks in that position
    // instantiate the canvas and do the same, now getting the goal
    // destroy the canvas and configure the blocks in that position
    void CallSolver()
    {
        Instantiate(solver);
    }

    void CallExecutePlan()
    {
        Instantiate(executePlan);
    }

}
