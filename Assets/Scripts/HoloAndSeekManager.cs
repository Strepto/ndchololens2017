using System;
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
    public TextMesh TimeRemaining3DTextPrefab;

    public int GameMaxDurationInSeconds = 99;

    public GameObject ConfigurationMenu;
    public GameObject GameOverMenu;

    public int MaxTargetsToFind = 10;

    public List<TargetScript> AllTargets = new List<TargetScript>();
    private List<TargetScript> SessionTargets = new List<TargetScript>();
    private List<TargetScript> UsedTargets = new List<TargetScript>();
    int _score;

    [HideInInspector]
    public CustomSyncModelObject _syncModel;

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


    private float timeLeft = 99;

    // Use this for initialization
    void Start()
    {
        if (!ConfigurationMenu || !GameOverMenu || !TimeRemaining3DTextPrefab)
        {
            Debug.LogError("Missing some required paramters.");
        }
        gameStateManager = GameStateManager.Instance;
        gameStateManager.gameStateChangedEvent += OnGameStateChanged;
        Remaining3DTextPrefab.GetComponent<TextMesh>().text = "Num Targets: " + AllTargets.Count + "/" + MaxTargetsToFind;
    }

    public void ResetGame(bool startGame, bool remoteInitiated = false)
    {
        if (AllTargets.Count == 0) return;

        AllTargets.ForEach(t => t.ResetTarget());
        UsedTargets.Clear();
        Score = 0;
        if (startGame)
        {
            timestampStart = Time.unscaledTime;
            timeLeft = GameMaxDurationInSeconds;
            if (!remoteInitiated)
                SeedNewTargets();
        }
    }

    private void SeedNewTargets()
    {
        var targetPool = new List<TargetScript>(AllTargets);
        SessionTargets.Clear();
        var numTargets = (targetPool.Count > MaxTargetsToFind) ? MaxTargetsToFind : targetPool.Count;


        foreach (var t in targetPool)
        {
            t.TargetSeenEvent -= FoundTarget;
        }


        for (int i = 0; i < numTargets; i++)
        {
            var n = UnityEngine.Random.Range(0, targetPool.Count);
            SessionTargets.Add(targetPool[n]);
            targetPool.RemoveAt(n);
        }

        foreach (var t in SessionTargets)
        {
            t.TargetSeenEvent += FoundTarget;
        }

        foreach (var t in targetPool)
        {
            t.RemoveTarget();
        }

        Remaining3DTextPrefab.GetComponent<TextMesh>().text = UsedTargets.Count + " / " + SessionTargets.Count;
    }

    private void Update()
    {
        if (gameStateManager.CurrentGameState == GameState.Playing)
        {
            timeLeft -= Time.deltaTime;
            TimeRemaining3DTextPrefab.text = string.Format("Time left: {0:000.00}", TimeSpan.FromSeconds(timeLeft).TotalSeconds);

            if (timeLeft < 0)
            {
                gameStateManager.SetGameState(GameState.Ended);
            }
        }

    }

    internal void SetSyncObject(CustomSyncModelObject syncModel)
    {
        _syncModel = syncModel;
        _syncModel.gameStage.IntegerValueChanged += RemoteGameStageChanged;
    }

    private void RemoteGameStageChanged(int gameState)
    {
        GameStateManager.Instance.CurrentGameState = (GameState)gameState;
    }

    private void OnGameStateChanged(GameState changedGameState)
    {
        if (_syncModel != null && (GameState)_syncModel.gameStage.Value != changedGameState)
        {
            _syncModel.gameStage.Value = (int)changedGameState;
        }

        if (changedGameState == GameState.Configuration)
        {
            ResetGame(false);
            ShowConfigurationMenu();
        }
        else
        {
            HideConfigurationMenu();
        }

        if (changedGameState == GameState.Playing)
        {
            ResetGame(true);
        }

        if (changedGameState == GameState.Ended)
        {
            ShowGameOverMenu();
        }
        else
        {
            HideGameOverMenu();
        }
    }

    public void AddTarget(TargetScript targetScript)
    {
        AllTargets.Add(targetScript);
        Remaining3DTextPrefab.GetComponent<TextMesh>().text = "Num Targets: " + AllTargets.Count + "/" + MaxTargetsToFind;
    }

    public void IncreaseMaxTargets()
    {
        MaxTargetsToFind += 1; 
        Remaining3DTextPrefab.GetComponent<TextMesh>().text = "Num Targets: " + AllTargets.Count + "/" + MaxTargetsToFind;

    }

    public void DecreaseMaxTargets()
    {
        MaxTargetsToFind -= 1; 
        Remaining3DTextPrefab.GetComponent<TextMesh>().text = "Num Targets: " + AllTargets.Count + "/" + MaxTargetsToFind;

    }

    private void HideConfigurationMenu()
    {
        SetMainMenuActive(false);
    }

    private void ShowConfigurationMenu()
    {
        SetMainMenuActive(true);
    }

    private void SetMainMenuActive(bool active)
    {
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

    private void SetGameOverMenuActive(bool active)
    {
        GameOverMenu.GetComponent<GameOverMenu>().UpdateGameOverScreen(Score, System.TimeSpan.FromSeconds(Time.unscaledTime - timestampStart), 4, 10);
        GameOverMenu.SetActive(active);
    }

    public void FoundTarget(TargetScript target)
    {
        UsedTargets.Add(target);

        int scoreCounter;

        if (_syncModel != null)
        {
            _syncModel.targetsFound.Value = _syncModel.targetsFound.Value + 1;
            scoreCounter = _syncModel.targetsFound.Value;
        }
        else
        {
            scoreCounter = UsedTargets.Count;
        }

        Remaining3DTextPrefab.GetComponent<TextMesh>().text = UsedTargets.Count + " / " + SessionTargets.Count;

        target.TargetSeenEvent -= FoundTarget;

        Score += 10 + UnityEngine.Random.Range(0, 5);

        if (scoreCounter == SessionTargets.Count)
        {

            System.TimeSpan timeSpent = System.TimeSpan.FromSeconds(Time.unscaledTime - timestampStart);
            Score += (int)((10 * SessionTargets.Count) / timeSpent.TotalSeconds);
            Debug.Log("Time spent: " + timeSpent);

            if (_syncModel != null)
            {
                _syncModel.targetsFound.Value = 0;
            }

            GameStateManager.Instance.CurrentGameState = GameState.Ended;
        }
    }
}
