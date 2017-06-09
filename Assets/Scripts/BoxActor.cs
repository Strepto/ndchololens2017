using System;
using HoloToolkit.Sharing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxActor : MonoBehaviour
{
    private Guid guid = Guid.NewGuid();
    public string GUID { get {return guid.ToString();} set {guid = new Guid(value);}}
    
    private CustomPrefabSpawner SpawnManager;

    void Start()
    {
        SpawnManager =  GameObject.Find("World").GetComponent<CustomPrefabSpawner>();
        SpawnManager.DataModelSourceSet += Connected;
        
    }

    private void Connected()
    {
        SpawnManager.CreateOrGetSyncObject(GUID, this.gameObject);
    }
}
