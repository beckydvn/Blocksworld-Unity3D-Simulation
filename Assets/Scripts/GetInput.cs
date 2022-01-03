using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GetInput : MonoBehaviour
{
    private TMPro.TextMeshProUGUI bannerText;
    private string goalText = "set goal";
    private StreamWriter writer;
    private StreamReader reader;
    private string initDelim = "(:INIT ";
    private string goalDelim = "(:goal ";
    private string andDelim = "(AND ";
    private List<string> initialProbSplit;
    private List<string> fullProbSplit;
    private string baseInitialState;
    public static bool basicStart = false;
    public static Button btn;

    // Start is called before the first frame update
    void Start()
    {
        //always write to the initial file first in case of corruption
        reader = new StreamReader("Assets/PDDL Files/sampleProblem.pddl");
        string data = reader.ReadToEnd();
        WriteToFile(data, "Assets/PDDL Files/initialProblem.pddl");

        btn = GameObject.FindGameObjectWithTag("Submit").GetComponent<Button>();
        bannerText = GameObject.FindGameObjectWithTag("Initial State Label").GetComponent<TMPro.TextMeshProUGUI>();
        btn.onClick.AddListener(TaskOnClick);  
        initialProbSplit = SplitFile("Assets/PDDL Files/initialProblem.pddl");
        baseInitialState = initialProbSplit[1];
        fullProbSplit = initialProbSplit.ToList();
    }

    private void WriteToFile(string newData, string path)
    {
        reader.Close();
        writer = new StreamWriter(path, false);
        writer.Write(newData);
        writer.Close();
    }

    private List<string> SplitFile(string path)
    {
        List<string> probSplit;
        reader = new StreamReader(path);
        string initialProb = reader.ReadToEnd();
        reader.Close();
        probSplit = initialProb.Split(new string[] { initDelim }, System.StringSplitOptions.None).ToList();
        foreach (var element in probSplit[1].Split(new string[] { goalDelim }, System.StringSplitOptions.None).ToList())
        {
            probSplit.Add(element);
        }
        // remove the duplicate
        probSplit.RemoveAt(1);

        return probSplit.ToList();
    }

    private void UpdateProblemFile(string resultStr)
    {
        // overwrite the initial state
        if (bannerText.text != goalText)
        {
            // set up the new string (at 1 will be the new initial state, at 2 will be the goal)
            fullProbSplit[1] = "\n" + initDelim + resultStr + ")";
            // check if initial start is the same as the basic one
            if(initialProbSplit[1].Trim("\n ".ToCharArray()).Contains(resultStr.Trim("\n ".ToCharArray())))
            {
                basicStart = true;
            }
            initialProbSplit[1] = "\n" + initDelim + baseInitialState;
            initialProbSplit[2] = "\n" + goalDelim + andDelim + resultStr + ")))";
        }
        // overwrite the goal
        else
        {
            // set up the new strings (at 1 will be the new initial state, at 2 will be the goal)
            
            fullProbSplit[2] = "\n" + goalDelim + andDelim + resultStr + ")))";
            // write to the files
            WriteToFile(string.Join(" ", initialProbSplit), "Assets/PDDL Files/initialProblem.pddl");
            WriteToFile(string.Join(" ", fullProbSplit), "Assets/PDDL Files/fullProblem.pddl");

            btn.interactable = false;
            GameManager.inputReceived.Invoke();
        }
        bannerText.text = goalText;
    }

    private string ConvertUIToString(GameObject[] ui, bool isCheckBox)
    {
        // convert results to a list of strings
        string[] results = new string[ui.Length];
        bool varTrue;
        for (int i = 0; i < ui.Length; i++)
        {
            varTrue = true;
            if (isCheckBox)
            {
                if (!ui[i].GetComponent<Toggle>().isOn)
                {
                    varTrue = false;
                }
            }
            if (varTrue)
            {
                results[i] = ui[i].GetComponent<TMPro.TextMeshProUGUI>().text;
            }
        }
        return string.Join(" ", results);
    }

    private void TaskOnClick()
    {
        string dropDownResults = ConvertUIToString(GameObject.FindGameObjectsWithTag("Dropdown"), false);
        string clearResults = ConvertUIToString(GameObject.FindGameObjectsWithTag("Clear"), true);
        string handemptyResult = ConvertUIToString(GameObject.FindGameObjectsWithTag("Handempty"), true);

        // update the problem file with the changes
        UpdateProblemFile(dropDownResults + clearResults + handemptyResult);
    }
}
