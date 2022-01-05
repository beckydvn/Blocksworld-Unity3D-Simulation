using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject getInput;
    public GameObject solver;
    public GameObject executePlan;
    public GameObject canvas; // to delete the canvas when finished
    public GameObject stateCanvas;
    public GameObject redoButton;
    public static UnityEvent inputReceived;
    public static UnityEvent plansReceived;
    public static UnityEvent noPlan;
    public static UnityEvent allSolved;
    private GameObject instantiatedSolver;
    private GameObject instantiatedCanvas;
    private GameObject instantiatedGetInput;
    private GameObject instantiatedExecutePlan;
    private GameObject instantiatedStateCanvas;
    private GameObject instantiateRedoButton;
    public static Dictionary<string, Vector3> blockPositions = new Dictionary<string, Vector3>();
    private string[] blockTags = {"red", "orange", "yellow", "green", "cyan", "blue", "purple", "pink", "white", "black"};
    private Dictionary <string, GameObject> blocks = new Dictionary<string, GameObject>();
    public static TMPro.TextMeshProUGUI stateText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("quitting");
            Application.Quit();
        }
    }

        // Start is called before the first frame update
        void Start()
    {
        // get the initial positions of the blocks
        foreach(string tag in blockTags)
        {
            blocks[tag] = GameObject.FindGameObjectWithTag(tag);
            blockPositions.Add(tag, blocks[tag].transform.position);
        }

        inputReceived = new UnityEvent();
        plansReceived = new UnityEvent();
        noPlan = new UnityEvent();
        allSolved = new UnityEvent();
        inputReceived.AddListener(CallSolver);
        plansReceived.AddListener(CallExecutePlan);
        noPlan.AddListener(GetInputAgain);
        allSolved.AddListener(TryAgain);
        instantiatedCanvas = Instantiate(canvas);
        instantiatedGetInput = Instantiate(getInput);
    }

    void CallSolver()
    {
        instantiatedSolver = Instantiate(solver);
        if(instantiatedStateCanvas == null)
        {
            instantiatedStateCanvas = Instantiate(stateCanvas);
        }
        stateText = GameObject.FindGameObjectWithTag("State of Solving Label").GetComponent<TMPro.TextMeshProUGUI>();
        stateText.text = "solving...";
    }

    void CallExecutePlan()
    {
        Destroy(instantiatedCanvas);
        Destroy(instantiatedGetInput);
        instantiatedExecutePlan = Instantiate(executePlan);
    }

    void GetInputAgain()
    {
        GameObject.FindGameObjectWithTag("Initial State Label").GetComponent<TMPro.TextMeshProUGUI>().text = "set initial state";
        stateText.text = "Couldn't find a plan, please try different parameters.";
        Destroy(instantiatedSolver);
    }
    void TryAgain()
    {
        instantiateRedoButton = Instantiate(redoButton);
        Button btn = GameObject.FindGameObjectWithTag("Redo").GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    IEnumerator ResetToOriginalPos()
    {
        Debug.Log("starting couroutine...");
        foreach(string tag in blockTags)
        {
            Rigidbody rigidBody = blocks[tag].GetComponent<Rigidbody>();
            rigidBody.useGravity = false;
        }

        foreach(string tag in blockTags)
        {
            Debug.Log("moving block " + tag);
            blocks[tag].transform.position = blockPositions[tag];
            yield return new WaitForSeconds(5);
        }

    }
   
    void TaskOnClick()
    {
        Destroy(instantiatedSolver);
        Destroy(instantiateRedoButton);
        Destroy(instantiatedStateCanvas);
        Destroy(instantiatedExecutePlan);
        instantiatedCanvas = Instantiate(canvas);
        instantiatedGetInput = Instantiate(getInput);
        GameObject.FindGameObjectWithTag("Initial State Label").GetComponent<TMPro.TextMeshProUGUI>().text = "set initial state";
        // reset blocks
        Debug.Log("calling couroutine...");
        //StartCoroutine(ResetToOriginalPos());
        foreach (string tag in blockTags)
        {
            Rigidbody rigidBody = blocks[tag].GetComponent<Rigidbody>();
            rigidBody.useGravity = false;
        }

        foreach (string tag in blockTags)
        {
            Debug.Log("moving block " + tag);
            blocks[tag].transform.position = blockPositions[tag];
            //yield return new WaitForSeconds(5);
        }

        foreach (string tag in blockTags)
        {
            Rigidbody rigidBody = blocks[tag].GetComponent<Rigidbody>();
            rigidBody.useGravity = true;
        }
    }
}
 