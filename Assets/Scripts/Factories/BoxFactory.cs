using UnityEngine;

public class BoxFactory
{
    public GameObject CreateInstance()
    {
        var BoxPrefab = Resources.Load<GameObject>("Prefabs/BoxObstacle");
        return Object.Instantiate(BoxPrefab);
    }
}
