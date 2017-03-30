using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoundTarget
{
    GameObject target;
    int timestamp;
    int number;
    int points;
    int pointsTotal;
}

public class HoloAndSeekScripts : MonoBehaviour
{
    public GameObject Score3DTextPrefab;
    public GameObject Remaining3DTextPrefab;
    
    public List<GameObject> AllTargets;
    public List<GameObject> UsedTargets = new List<GameObject>();
    int score;
    float startedTime;
    List<FoundTarget> targetSequence;
    GameObject currrentTarget;
    System.Random rng = new System.Random();

    // Use this for initialization
    void Start()
    {
        Remaining3DTextPrefab.GetComponent<TextMesh>().text = UsedTargets.Count + " / " + AllTargets.Count;
        foreach (var t in AllTargets)
        {
            var ts = t.GetComponent<TargetScript>();
            if (ts == null)
            {
                throw new System.Exception("Target does not have a target Script");
            }
            
            //TargetScript ts = t.AddComponent<TargetScript>();
            ts.targetSeen.AddListener(FoundTarget);
            t.SetActive(false);
        };
        currrentTarget = AllTargets[rng.Next(0, AllTargets.Count)];
        currrentTarget.SetActive(true);

        UsedTargets.Add(currrentTarget);
        startedTime = Time.time;
    }

    public void FoundTarget()
    {
        Remaining3DTextPrefab.GetComponent<TextMesh>().text = UsedTargets.Count + " / " + AllTargets.Count;
        currrentTarget.GetComponent<TargetScript>().targetSeen.RemoveListener(FoundTarget);
        currrentTarget.GetComponent<Renderer>().enabled = false;
        UsedTargets.Add(currrentTarget);
        var allUnusedTargets = AllTargets.Except(UsedTargets);
        currrentTarget = allUnusedTargets.ElementAt(rng.Next(0, allUnusedTargets.Count()));
        currrentTarget.SetActive(true);
        score = score + 10 + rng.Next(0,5);
        Score3DTextPrefab.GetComponent<TextMesh>().text = "Score:" + score;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
