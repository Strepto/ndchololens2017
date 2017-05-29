using System;
using HoloToolkit.Sharing;
using UnityEngine;

public class BoxActor : MonoBehaviour
{
    public string GUID;

    private CustomPrefabSpawner SpawnManager;

    void Start()
    {
        SpawnManager = this.transform.parent.transform.parent.GetComponent<CustomPrefabSpawner>();
        SpawnManager.DataModelSourceSet += Connected;
        
    }

    private void Connected()
    {
        SpawnManager.CreateOrGetSyncObject(GUID, this.gameObject);
    }
}
