using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour {

private const string SAVED_GAMESTATE_JSON = "SaveGameJson";
	public GameObject Obstacles;
	public CustomPrefabSpawner prefabSpawner;

	public ButtonScript RestoreSavedButton;

	[Serializable]
	class StorableJson
	{
		public Storable[] storables;
	}

	void Start () {
		if(!Obstacles || !prefabSpawner){
			Debug.LogError("Missing assignments in GameSaveManager");
			return;
		}		

		if(PlayerPrefs.GetString(SAVED_GAMESTATE_JSON, null) == null){
			RestoreSavedButton.SetInteractable(false);
        }
        else
        {
            RestoreSavedButton.SetInteractable(true);
        }
	}

	public void SaveObstacles(){
		List<Storable> allStorableStates = new List<Storable>();
		foreach (StorableObj child in Obstacles.GetComponentsInChildren<StorableObj>()){
			allStorableStates.Add(child.GetStorableRepresentation());
		}

		var json = JsonUtility.ToJson(new StorableJson { storables = allStorableStates.ToArray()});
		Debug.Log(json);

		PlayerPrefs.SetString(SAVED_GAMESTATE_JSON, json);
		Debug.Log("Saved game state");
		RestoreSavedButton.SetInteractable(true);
	}


	public void RestoreSavedState(){
	 	var storablejson = JsonUtility.FromJson<StorableJson>(PlayerPrefs.GetString(SAVED_GAMESTATE_JSON));
		foreach(Storable storable in storablejson.storables){
			var prefab = prefabSpawner.Objects[(int)storable.Type];
			
			
			var go = Instantiate(prefabSpawner.Objects[(int)storable.Type], Obstacles.transform, false);
			
			go.transform.localPosition = storable.localPosition;
			go.transform.localRotation = storable.localRotation;
			go.transform.localScale = storable.localScale;
			if(go.GetComponentInChildren<IBox>() != null && storable.BoxType != -1){
				go.GetComponentInChildren<IBox>().SetBoxType(storable.BoxType);
			}
			if (go.GetComponent<TargetScript>())
			{
				HoloAndSeekManager.Instance.AddTarget(go.GetComponent<TargetScript>());
			}

			prefabSpawner.SpawnObject(go, storable.Type);
		}
	}
}
