using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.Events;

//call the solver.
//note that no attempt is made (yet?) to catch any solver issues.
public class CallSolver : MonoBehaviour
{
    public static string[] initialPlan; // stores the plan from start to initial state
    public static string[] fullPlan; // stores the plan from initial state to goal
    private string domain;
    private string problem;

    [System.Serializable]
    class Actions
    {
        public string name;
        public static Result CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Result>(jsonString);
        }
    }

    [System.Serializable]
    class Result
    {
        public Actions[] plan;
        public static Result CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Result>(jsonString);
        }
    }

    [System.Serializable]
    class FullResponse
    {
        public string status;
        public Result result;
        public static FullResponse CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<FullResponse>(jsonString);
        }
    }

    [System.Serializable]
    class PDDLData
    {
        public string domain;
        public string problem;
        public PDDLData(string domain, string problem)
        {
            this.domain = domain;
            this.problem = problem;
        }
    }

    private async Task<string[]> Solve(string domain, string problem, string[] plan)
    {
        PDDLData pddlData = new PDDLData(domain, problem);
        var url = "http://solver.planning.domains/solve";
        var client = new HttpClient();
        var json = JsonUtility.ToJson(pddlData);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, data);
        var result = response.Content.ReadAsStringAsync().Result;
        Debug.Log(result);
        Result converted = FullResponse.CreateFromJSON(result).result;

        plan = new string[converted.plan.Length];
        for (int i = 0; i < plan.Length; i++)
        {
            var name = converted.plan[i].name;
            plan[i] = name.Substring(1, name.Length - 2);
        }
        return plan;
    }

    // Start is called before the first frame update
    async void Start()
    {
        StreamReader reader = new StreamReader("Assets/PDDL Files/domain.pddl");
        domain = reader.ReadToEnd();

        try
        {
            if (!GetInput.basicStart)
            {
                reader = new StreamReader("Assets/PDDL Files/initialProblem.pddl");
                problem = reader.ReadToEnd();
                initialPlan = await Solve(domain, problem, initialPlan);
            }
            reader = new StreamReader("Assets/PDDL Files/fullProblem.pddl");
            problem = reader.ReadToEnd();
            fullPlan = await Solve(domain, problem, fullPlan);
            GameManager.plansReceived.Invoke();
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Solver was not able to generate a plan.");
            GetInput.basicStart = false;
            GameManager.noPlan.Invoke();
        }
        finally
        {
            reader.Close();
        }
        GetInput.btn.interactable = true;
        reader.Close();
    }
}

