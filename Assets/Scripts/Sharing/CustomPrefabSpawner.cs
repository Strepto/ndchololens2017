using HoloToolkit.Sharing.Spawning;
using UnityEngine;
using HoloToolkit.Sharing;
using System;
using HoloToolkit.Sharing.SyncModel;
using System.Collections;

public class CustomPrefabSpawner : SpawnManager<CustomSyncModelObject>
{
    private CustomSyncRoot _syncRoot;

    public event Action DataModelSourceSet;

    private int NumberOfLocalEntities;

    protected override void Start()
    {
        base.Start();
    }

    public void SpawnObject()
    {
        CustomSyncModelObject syncObject = CreateSyncModelObject();

        BoxFactory boxFactory = new BoxFactory();

        GameObject instance = boxFactory.CreateInstance();

        syncObject.GameObject = instance;

        instance.GetComponent<CustomTransformSynchronizer>().TransformDataModel = syncObject.transform;

        if(SyncSource != null) SyncSource.AddObject(syncObject);
    }

    public void CreateOrGetSyncObject(string GUID, GameObject instance)
    {
        foreach (CustomSyncModelObject syncObject in _syncRoot.InstantiatedModels)
        {
            if (syncObject.GUID.Value == GUID)
            {
                syncObject.GameObject = instance;
                instance.GetComponent<CustomTransformSynchronizer>().TransformDataModel = syncObject.transform;
                return;
            }
        }
        
        CustomSyncModelObject createdSyncObject = new CustomSyncModelObject(GUID);

        createdSyncObject.transform.Position.Value = instance.transform.localPosition;
        createdSyncObject.transform.Rotation.Value = instance.transform.localRotation;
        createdSyncObject.transform.Scale.Value = instance.transform.localScale;

        createdSyncObject.GameObject = instance;

        instance.GetComponent<CustomTransformSynchronizer>().TransformDataModel = createdSyncObject.transform;

        
        if (SyncSource != null) SyncSource.AddObject(createdSyncObject);

    }

    private CustomSyncModelObject CreateSyncModelObject()
    {
        CustomSyncModelObject syncObject = new CustomSyncModelObject();
        syncObject.transform.Position.Value = Vector3.zero;
        syncObject.transform.Rotation.Value = Quaternion.identity;
        syncObject.transform.Scale.Value = Vector3.one;

        return syncObject;
    }

    public override void Delete(CustomSyncModelObject objectToDelete)
    {
        if(SyncSource != null) SyncSource.RemoveObject(objectToDelete);
    }

    protected override void InstantiateFromNetwork(CustomSyncModelObject addedObject)
    {
        if (addedObject.GameObject != null || addedObject.GUID.Value != "") return;

        BoxFactory boxFactory = new BoxFactory();

        GameObject instance = boxFactory.CreateInstance();

        addedObject.GameObject = instance;

        instance.GetComponent<CustomTransformSynchronizer>().TransformDataModel = addedObject.transform;
    }

    protected override void RemoveFromNetwork(CustomSyncModelObject removedObject)
    {
        if(removedObject.GameObject != null)
        {
            Destroy(removedObject.GameObject);
            removedObject.GameObject = null;
        }
    }

    protected override void SetDataModelSource()
    {
        var rootObjectElement = SharingStage.Instance.Manager.GetRootSyncObject();

        //SharingStage.SyncRoot is hardwired to SyncSpawnedObject... 
        //I have no idea why...
        //So i create my own custom root

        _syncRoot = new CustomSyncRoot(rootObjectElement);
        SyncSource = _syncRoot.InstantiatedModels;

        StartCoroutine(WaitForPopulatedSyncSource());
    }

    private IEnumerator WaitForPopulatedSyncSource()
    {
        yield return new WaitForSeconds(2);

        if (SyncSource.GetDataArray().Length == 0)
        {
            if (DataModelSourceSet != null) DataModelSourceSet.Invoke();
            Debug.Log("Create SyncObjects");
        }
        else
        {
            NumberOfLocalEntities = gameObject.GetComponentsInChildren<BoxActor>().Length;

            if (SyncSource.GetDataArray().Length == NumberOfLocalEntities)
            {
                if (DataModelSourceSet != null) DataModelSourceSet.Invoke();
            }
            else
            {
                Debug.Log("Fuck...");
            }
        }
    }
}
