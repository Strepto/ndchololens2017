using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

public enum GameState
{
    Sharing,
    Configuration,
    Playing,
    Ended
}
public class GameStateManager : Singleton<GameStateManager>
{

    [SerializeField]
    private GameState currentGameState = GameState.Configuration;

    public CustomPrefabSpawner prefabSpawner;

    public Action<GameState> gameStateChangedEvent;

    public GameState CurrentGameState
    {
        get
        {
            return currentGameState;
        }
        set
        {
            currentGameState = value;
            if (gameStateChangedEvent != null)
            {
                gameStateChangedEvent.Invoke(currentGameState);
            }
        }
    }

    public void SetGameStatePlaying()
    {
        SetGameState(GameState.Playing);
    }

    public void SetGameStateConfiguration()
    {
        SetGameState(GameState.Configuration);
    }

    public void SetGameState(GameState gameState)
    {
        CurrentGameState = gameState;
    }

    public void ToggleGameStatePlayingConfiguration()
    {
        if (currentGameState == GameState.Configuration)
        {
            if (HoloAndSeekManager.Instance._syncModel == null)
                prefabSpawner.SpawnApplicationStateSyncObject();

            CurrentGameState = GameState.Playing;
        }
        else
        {
            CurrentGameState = GameState.Configuration;
        }
    }

    void Start()
    {

    }

}
