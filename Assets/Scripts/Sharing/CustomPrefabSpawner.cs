using HoloToolkit.Sharing.Spawning;
using UnityEngine;
using HoloToolkit.Sharing;
using System;
using HoloToolkit.Sharing.SyncModel;
using System.Collections;
using System.Collections.Generic;

public enum ObjectType
{
    BoxMex = 0,
    Box = 1,
    Cactus = 2,
    Hat = 3,
    Spider = 4,
    Target = 5,
    ApplicationState = 6
}



public class CustomPrefabSpawner : SpawnManager<CustomSyncModelObject>
{
    public List<GameObject> Objects; //Order matters on this, I know... Great code indeed
    private CustomSyncRoot _syncRoot;

    public void SpawnObject(GameObject instance, ObjectType type)
    {
        CustomSyncModelObject syncObject = CreateSyncModelObject(type, instance);

        syncObject.GameObject = instance;

        if (instance.GetComponent<TargetScript>())
        {
            var targetScript = instance.GetComponent<TargetScript>();
            syncObject.isActive.BooleanValueChanged += targetScript.TargetRemoteStateChanged;
        }


        instance.GetComponent<SyncObjectAccessor>().SyncObject = syncObject;
        instance.GetComponent<CustomTransformSynchronizer>().TransformDataModel = syncObject.transform;

        if (SyncSource != null) SyncSource.AddObject(syncObject);
    }

    public void SpawnApplicationStateSyncObject()
    {
        CustomSyncModelObject syncObject = new CustomSyncModelObject();
        syncObject.Type.Value = (int)ObjectType.ApplicationState;

        if (SyncSource != null) SyncSource.AddObject(syncObject);
    }

    private CustomSyncModelObject CreateSyncModelObject(ObjectType type, GameObject instance)
    {
        CustomSyncModelObject syncObject = new CustomSyncModelObject();

        syncObject.transform.Position.Value = instance.transform.localPosition;
        syncObject.transform.Rotation.Value = instance.transform.localRotation; 
        syncObject.transform.Scale.Value = instance.transform.localScale;

        syncObject.Type.Value = (int)type;

        return syncObject;
    }

    public override void Delete(CustomSyncModelObject objectToDelete)
    {
        if (SyncSource != null) SyncSource.RemoveObject(objectToDelete);
    }

    protected override void InstantiateFromNetwork(CustomSyncModelObject addedObject)
    {
        if (addedObject.GameObject != null) return;

        if((ObjectType)addedObject.Type.Value == ObjectType.ApplicationState)
        {
            HoloAndSeekManager.Instance.SetSyncObject(addedObject);
            return;
        }

        int type = addedObject.Type.Value;

        var instance = Instantiate(Objects[type], GameObject.Find("Obstacles").transform);

        instance.transform.localPosition = addedObject.transform.Position.Value;
        instance.transform.localRotation = addedObject.transform.Rotation.Value;
        instance.transform.localScale = addedObject.transform.Scale.Value;

        addedObject.GameObject = instance;

        if (instance.GetComponent<TargetScript>())
        {
            var targetScript = instance.GetComponent<TargetScript>();
            HoloAndSeekManager.Instance.AllTargets.Add(targetScript);
            addedObject.isActive.BooleanValueChanged += targetScript.TargetRemoteStateChanged;
        }

        instance.GetComponent<SyncObjectAccessor>().SyncObject = addedObject;
        instance.GetComponent<CustomTransformSynchronizer>().TransformDataModel = addedObject.transform;
    }

    protected override void RemoveFromNetwork(CustomSyncModelObject removedObject)
    {
        if (removedObject.GameObject != null)
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
        //So I create my own custom root

        _syncRoot = new CustomSyncRoot(rootObjectElement);
        SyncSource = _syncRoot.InstantiatedModels;
    }
}
