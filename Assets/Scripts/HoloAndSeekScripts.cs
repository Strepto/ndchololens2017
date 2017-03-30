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
            ts.targetSeen += FoundTarget;
            //t.SetActive(false);
        };
        startedTime = Time.time;
    }

    public void FoundTarget(GameObject target)
    {
        Remaining3DTextPrefab.GetComponent<TextMesh>().text = UsedTargets.Count + " / " + AllTargets.Count;
        target.GetComponent<TargetScript>().targetSeen -= FoundTarget;
        target.GetComponent<Renderer>().enabled = false;
        UsedTargets.Add(target);
        var allUnusedTargets = AllTargets.Except(UsedTargets);
        score = score + 10 + rng.Next(0,5);
        Score3DTextPrefab.GetComponent<TextMesh>().text = "Score:" + score;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
