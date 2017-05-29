using HoloToolkit.Sharing.SyncModel;
using UnityEngine;

[SyncDataClass]
public class CustomSyncModelObject : SyncObject
{
    //Shared Data
    [SyncData]
    public SyncTransform transform;

    [SyncData]
    public SyncString GUID;

    //Local Data
    public GameObject GameObject { get; set; }

    public CustomSyncModelObject() { }

    public CustomSyncModelObject(string GUID)
    {
        this.GUID.Value = GUID;
    }
}
