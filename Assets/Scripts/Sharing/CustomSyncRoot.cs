using HoloToolkit.Sharing;
using HoloToolkit.Sharing.SyncModel;

public class CustomSyncRoot : SyncObject
{
    [SyncData]
    public SyncArray<CustomSyncModelObject> InstantiatedModels;

    public CustomSyncRoot(ObjectElement rootElement)
    {
        Element = rootElement;
        FieldName = Element.GetName().GetString();

        SyncSettings.Instance.Initialize();

        InstantiatedModels.InitializeLocal(Element);
    }
}
