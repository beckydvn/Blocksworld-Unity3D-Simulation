using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject getInput;
    public GameObject solver;
    public GameObject executePlan;
    public GameObject canvas; // to delete the canvas when finished
    public GameObject stateCanvas;
    public static UnityEvent inputReceived;
    public static UnityEvent plansReceived;
    public static UnityEvent noPlan;
    private GameObject instantiatedSolver;
    private GameObject instantiatedCanvas;
    private GameObject instantiatedGetInput;
    private GameObject instantiatedExecutePlan;
    private GameObject instantiatedStateCanvas;

    // Start is called before the first frame update
    void Start()
    {
        inputReceived = new UnityEvent();
        plansReceived = new UnityEvent();
        noPlan = new UnityEvent();
        inputReceived.AddListener(CallSolver);
        plansReceived.AddListener(CallExecutePlan);
        noPlan.AddListener(GetInputAgain);
        instantiatedCanvas = Instantiate(canvas);
        instantiatedGetInput = Instantiate(getInput);
    }
    // TODO: solve how to get from initial setup to initial state first (will need to change some params)...
    // destroy the canvas and configure the blocks in that position
    // instantiate the canvas and do the same, now getting the goal
    // destroy the canvas and configure the blocks in that position
    void CallSolver()
    {
        instantiatedSolver = Instantiate(solver);
    }

    void CallExecutePlan()
    {
        Destroy(instantiatedCanvas);
        Destroy(instantiatedGetInput);
        instantiatedExecutePlan = Instantiate(executePlan);
        instantiatedStateCanvas = Instantiate(stateCanvas);
    }

    void GetInputAgain()
    {
        GameObject.FindGameObjectWithTag("Initial State Label").GetComponent<TMPro.TextMeshProUGUI>().text = "set initial state";
        Debug.Log("Getting input again...");
        Destroy(instantiatedSolver);
    }
}
 