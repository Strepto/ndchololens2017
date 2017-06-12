using HoloToolkit.Sharing.SyncModel;
using System;

public class EventSyncInteger : SyncInteger {

    public event Action<int> IntegerValueChanged;

    public EventSyncInteger(string field) : base(field) { }

    public override void UpdateFromRemote(int remoteValue)
    {
        base.UpdateFromRemote(remoteValue);

        if (IntegerValueChanged != null)
        {
            IntegerValueChanged.Invoke(remoteValue);
        }
    }
}
