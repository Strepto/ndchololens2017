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

    
    [SyncData]
    public SyncInteger BoxType;

    //Local Data
    public GameObject GameObject { get; set; }

    public CustomSyncModelObject() { }

    public CustomSyncModelObject(string GUID, int boxType)
    {
        this.GUID.Value = GUID;
        this.BoxType.Value = boxType;
    }
}
