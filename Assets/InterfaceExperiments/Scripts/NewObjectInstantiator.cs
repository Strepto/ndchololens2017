using System.Collections;
using UnityEngine;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Sharing;

public class NewObjectInstantiator : MonoBehaviour
{
    public GameObject InstatiatedPrefab;

    public void InstantiatePrefab()
    {
        StartCoroutine(DispatchAnimationEvent());
    }

    IEnumerator DispatchAnimationEvent()
    {
        var pos = gameObject.transform.position;
        pos.y = pos.y + 0.2F;

        yield return new WaitForEndOfFrame();
        var go = Instantiate(InstatiatedPrefab, pos, Quaternion.identity, GameObject.Find("_Dynamic").transform);
        // var syncDataModel = new SyncSpawnedObject();
        // GameObject.Find("_Dynamic").GetComponent<PrefabSpawnManager>().Spawn(syncDataModel, pos, Quaternion.identity, null, "ObstacleBox", false);
        //Instantiate(InstatiatedPrefab, pos, new Quaternion(), GameObject.Find("_Dynamic").transform);
        // syncDataModel.GameObject.AddComponent<TransformSynchronizer>();

    }
}
