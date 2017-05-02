using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class CreateSharingTest : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    private CustomPrefabSpawner _prefabSpawner;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        _prefabSpawner.SpawnObject();
    }
    
}
