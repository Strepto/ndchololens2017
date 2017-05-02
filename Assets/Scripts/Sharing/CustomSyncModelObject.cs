using HoloToolkit.Sharing.SyncModel;
using UnityEngine;

[SyncDataClass]
public class CustomSyncModelObject : SyncObject
{
    //Shared Data
    [SyncData]
    public SyncTransform transform;

    //Local Data
    public GameObject GameObject { get; set; }
}
