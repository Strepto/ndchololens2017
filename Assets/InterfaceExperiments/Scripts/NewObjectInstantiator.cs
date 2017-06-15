using System.Collections;
using UnityEngine;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Sharing;
using HoloToolkit.Unity.SpatialMapping;

public class NewObjectInstantiator : MonoBehaviour
{
    public GameObject InstatiatedPrefab;
    public CustomPrefabSpawner SpawnManager;
    public SharingObjectType type;

    public void InstantiatePrefab()
    {
        StartCoroutine(DispatchAnimationEvent());
    }

    IEnumerator DispatchAnimationEvent()
    {
        var pos = gameObject.transform.position;
        pos.y = pos.y + 0.2F;

        yield return new WaitForEndOfFrame();
        var go = Instantiate(InstatiatedPrefab, pos, Quaternion.identity, GameObject.Find("Obstacles").transform);

        SpawnManager.SpawnObject(go, type);

        if (go.GetComponent<TargetScript>())
        {
            HoloAndSeekManager.Instance.AddTarget(go.GetComponent<TargetScript>());
        }

        if (go.GetComponent<TapToPlace>())
        {
            go.GetComponent<TapToPlace>().IsBeingPlaced = true;
        }
    }
}
