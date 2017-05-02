﻿using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesScript : MonoBehaviour {


    public BaseRandomCubeScript cubePrefab;

	// Use this for initialization
	void Start () {
		
	}
	
    public void CreateObstacle(int type = 0)
    {
        var cube = Instantiate<BaseRandomCubeScript>(cubePrefab, GameObject.Find("_Dynamic").transform);
        cube.GetComponent<TapToPlace>().IsBeingPlaced = true;
    }


	// Update is called once per frame
	void Update () {
		
	}
}
