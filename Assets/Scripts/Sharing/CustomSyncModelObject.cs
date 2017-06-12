using HoloToolkit.Sharing.SyncModel;
using UnityEngine;

[SyncDataClass]
public class CustomSyncModelObject : SyncObject
{
    //Shared Data
    [SyncData]
    public SyncTransform transform;
    
    [SyncData]
    public SyncInteger Type;

    [SyncData]
    public EventSyncBool isActive;

    [SyncData]
    public EventSyncInteger gameStage;

    [SyncData]
    public EventSyncInteger targetsFound;

    //Local Data
    public GameObject GameObject { get; set; }
}
