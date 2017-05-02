using HoloToolkit.Sharing.Spawning;
using UnityEngine;
using HoloToolkit.Sharing;

public class CustomPrefabSpawner : SpawnManager<CustomSyncModelObject>
{
    private CustomSyncRoot _syncRoot;
    
    public void SpawnObject()
    {
        CustomSyncModelObject _syncObject = CreateSyncModelObject();

        BoxFactory boxFactory = new BoxFactory();

        GameObject instance = boxFactory.CreateInstance();

        _syncObject.GameObject = instance;

        instance.GetComponent<CustomTransformSynchronizer>().TransformDataModel = _syncObject.transform;

        if(SyncSource != null) SyncSource.AddObject(_syncObject);
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
        if (addedObject.GameObject != null) return;

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
    }
}
