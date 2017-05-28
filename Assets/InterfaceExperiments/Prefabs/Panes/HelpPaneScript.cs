using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

[RequireComponent(typeof(Animator))]
public class HelpPaneScript : MonoBehaviour, IFocusable {
    Animator animator;

    //void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
    //{
    //    animator.SetTrigger("Collapse");
    //}

    public void DestroyGameObject()
    {
        
        Destroy(gameObject);
    }

    void IFocusable.OnFocusEnter()
    {
        
    }

    void IFocusable.OnFocusExit()
    {
        
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
