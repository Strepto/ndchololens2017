using HoloToolkit.Sharing.SyncModel;
using System;


public class EventSyncBool : SyncBool
{
    public event Action<bool> BooleanValueChanged;

    public EventSyncBool(string field) : base(field) { }

    public override void UpdateFromRemote(bool remoteValue)
    {
        base.UpdateFromRemote(remoteValue);

        if (BooleanValueChanged != null)
        {
            BooleanValueChanged.Invoke(remoteValue);
        }
    }
}
