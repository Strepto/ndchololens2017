using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHide : MonoBehaviour
{
    GameStateManager _gameStateManager;

    void Start()
    {
        _gameStateManager = GameStateManager.Instance;

        _gameStateManager.gameStateChangedEvent += GameStateChanged;

        if(_gameStateManager.CurrentGameState != GameState.Configuration)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void GameStateChanged(GameState obj)
    {
        if(_gameStateManager.CurrentGameState == GameState.Configuration)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
