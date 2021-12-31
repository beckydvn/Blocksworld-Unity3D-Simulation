using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GetInput : MonoBehaviour
{
    public GameObject submitButton;
    public GameObject banner; // to get/change the banner text
    public GameObject canvas; // to delete the canvas when finished
    public GameObject solver; // solver to instantiate

    private TMPro.TextMeshProUGUI bannerText;
    private string goalText = "set goal";
    private StreamWriter writer;
    private StreamReader reader;
    private string initDelim = "(:INIT ";
    private string goalDelim = "(:goal ";
    private List<string> initialProbSplit;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = submitButton.GetComponent<Button>();
        bannerText = banner.GetComponent<TMPro.TextMeshProUGUI>();
        initialProbSplit = SplitFile();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void WriteToFile(string newData)
    {
        writer = new StreamWriter("Assets/Scripts/updatedProblem.pddl", false);
        writer.Write(newData);
        writer.Close();
    }

    private List<string> SplitFile()
    {
        reader = new StreamReader("Assets/Scripts/problem.pddl");
        string initialProb = reader.ReadToEnd();
        reader.Close();
        initialProbSplit = initialProb.Split(new string[] { initDelim }, System.StringSplitOptions.None).ToList();
        foreach (var element in initialProbSplit[1].Split(new string[] { goalDelim }, System.StringSplitOptions.None).ToList())
        {
            initialProbSplit.Add(element);
        }
        // remove the duplicate
        initialProbSplit.RemoveAt(1);

        return initialProbSplit;
    }

    private void UpdateProblemFile(string resultStr)
    {
        // overwrite the initial state
        if (bannerText.text != goalText)
        {
            // set up the new string (at 1 will be the new initial state, at 2 will be the goal)
            initialProbSplit[1] = "\n" +initDelim + resultStr + ")";
            // write to the file
            WriteToFile(string.Join(" ", initialProbSplit));
        }
        // overwrite the goal
        else
        {
            // set up the new string (at 1 will be the new initial state, at 2 will be the goal)
            initialProbSplit[2] = "\n" + goalDelim + "(AND " + resultStr + ")))";
            // write to the file
            WriteToFile(string.Join(" ", initialProbSplit));
            Destroy(canvas);
            Instantiate(solver);
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

        // TODO: CONVERT TO STRING BUT ONLY ADD TO STRING IF CHECKBOX CHECKED 
        string clearResults = ConvertUIToString(GameObject.FindGameObjectsWithTag("Clear"), true);

        // TODO: CHECK HANDEMPTY
        string handemptyResult = ConvertUIToString(GameObject.FindGameObjectsWithTag("Handempty"), true);

        // update the problem file with the changes
        UpdateProblemFile(dropDownResults + clearResults + handemptyResult);
    }
}
