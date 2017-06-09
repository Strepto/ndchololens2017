using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity;
using UnityEngine;

public class FoundTarget
{
    GameObject target;
    int timestamp;
    int number;
    int points;
    int pointsTotal;
}

public class HoloAndSeekManager : Singleton<HoloAndSeekManager>
{
    public GameObject Score3DTextPrefab;
    public GameObject Remaining3DTextPrefab;

    public GameObject ConfigurationMenu;
    public GameObject GameOverMenu;

    public List<TargetScript> AllTargets;
    public List<TargetScript> SessionTargets = new List<TargetScript>();
    public List<TargetScript> UsedTargets = new List<TargetScript>();
    int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            
            Score3DTextPrefab.GetComponent<TextMesh>().text = "Score:" + _score;
        }
    }
    float timestampStart;
    List<FoundTarget> targetSequence;
    private GameStateManager gameStateManager;
    private int numTargets = 2;

    // Use this for initialization
    void Start()
    {
        if(!ConfigurationMenu || !GameOverMenu){
            Debug.LogError("Missing some required paramters.");
        }
        gameStateManager = GameStateManager.Instance;
        gameStateManager.gameStateChangedEvent += OnGameStateChanged;
    }

    public void ResetGame(bool startGame){
        AllTargets.ForEach(t => t.ResetTarget());
        UsedTargets.Clear();
        Score = 0;
        if(startGame){
            timestampStart = Time.unscaledTime;
            SeedNewTargets();
        }
    }

    private void SeedNewTargets()
    {
        var targetPool = new List<TargetScript>(AllTargets);
        SessionTargets = new List<TargetScript>();
        numTargets = (targetPool.Count > numTargets) ? numTargets : targetPool.Count;
        for (int i = 0; i < numTargets; i++)
        {
            var n = Random.Range(0, targetPool.Count);
            SessionTargets.Add(targetPool[n]);
            targetPool.RemoveAt(n);
        }

        foreach (var t in SessionTargets)
        {
            t.TargetSeenEvent += FoundTarget;
        };
        foreach(var t in targetPool){
            t.RemoveTarget();
            t.TargetSeenEvent -= FoundTarget;
        }

        
        Remaining3DTextPrefab.GetComponent<TextMesh>().text = UsedTargets.Count + " / " + SessionTargets.Count;
    }

    private void OnGameStateChanged(GameState changedGameState)
    {
        
        if (changedGameState == GameState.Configuration)
        {
            ResetGame(false);
        }

        if(changedGameState == GameState.Configuration){
            ShowConfigurationMenu();
        }
        else{
            HideConfigurationMenu();
        }


        if(changedGameState == GameState.Playing){
            ResetGame(true);
        }

        if(changedGameState == GameState.Ended){
            ShowGameOverMenu();
        }
        else {
            HideGameOverMenu();
        }
    }

    private void HideConfigurationMenu()
    {
        SetMainMenuActive(false);
    }

    private void ShowConfigurationMenu()
    {
        SetMainMenuActive(true);
    }

    private void SetMainMenuActive(bool active){
        ConfigurationMenu.SetActive(active);
    }

    private void ShowGameOverMenu()
    {
        SetGameOverMenuActive(true);
    }

    private void HideGameOverMenu()
    {
        SetGameOverMenuActive(false);
    }

    private void SetGameOverMenuActive(bool active){
        GameOverMenu.GetComponent<GameOverMenu>().UpdateGameOverScreen(Score, System.TimeSpan.FromSeconds(Time.unscaledTime - timestampStart), 4, 10);
        GameOverMenu.SetActive(active);
    }

    public void FoundTarget(TargetScript target)
    {
        UsedTargets.Add(target);

        Remaining3DTextPrefab.GetComponent<TextMesh>().text = UsedTargets.Count + " / " + SessionTargets.Count;

        target.TargetSeenEvent -= FoundTarget;

        Score += 10 + Random.Range(0, 5);

        if(UsedTargets.Count == SessionTargets.Count){

            System.TimeSpan timeSpent = System.TimeSpan.FromSeconds(Time.unscaledTime - timestampStart);
            Score += (int)((10 *numTargets) / timeSpent.TotalSeconds);
            Debug.Log("Time spent: "  + timeSpent);
            
            GameStateManager.Instance.CurrentGameState = GameState.Ended;
        }
    }
}
