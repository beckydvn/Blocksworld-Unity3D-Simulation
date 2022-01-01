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
    public static string[] generated_plan; // stores the plan

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

    private async Task Solve()
    {
        StreamReader reader = new StreamReader("Assets/PDDL Files/domain.pddl");
        string domain = reader.ReadToEnd();
        reader = new StreamReader("Assets/PDDL Files/updatedProblem.pddl");
        string problem = reader.ReadToEnd();
        PDDLData pddlData = new PDDLData(domain, problem);
        var url = "http://solver.planning.domains/solve";
        var client = new HttpClient();
        var json = JsonUtility.ToJson(pddlData);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, data);
        var result = response.Content.ReadAsStringAsync().Result;
        Debug.Log(result);
        Result converted = FullResponse.CreateFromJSON(result).result;
        generated_plan = new string[converted.plan.Length];
        for(int i = 0; i < generated_plan.Length; i++)
        {
            var name = converted.plan[i].name;
            generated_plan[i] = name.Substring(1, name.Length - 2);
        }
        GameManager.planReceived.Invoke();
    }

    // Start is called before the first frame update
    async void Start()
    {
        await Solve();
    }
}

