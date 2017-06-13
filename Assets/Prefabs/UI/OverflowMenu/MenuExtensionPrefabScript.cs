using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuExtensionPrefabScript : MonoBehaviour {

	public void DeleteAllDynamicObjects(){
		var parent = GameObject.Find("Obstacles");
		var children = new List<GameObject>();
        foreach(Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(child => Destroy(child));
	}

    public void SetGameStateSharing(){
        GameStateManager.Instance.SetGameStateSharing();
    }
}
