using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class ObstaclesScript : MonoBehaviour, IFocusable {

    public GameObject camera;

    void IFocusable.OnFocusEnter()
    {
    }

    void IFocusable.OnFocusExit()
    {
    }

    // Use this for initialization
    void Start () {
        //gameObject.GetComponentInChildren<Renderer>().material.SetColor("_main", Color.red);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
