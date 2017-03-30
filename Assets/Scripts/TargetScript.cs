using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class TargetScript : MonoBehaviour, IFocusable {

    ParticleSystem particleSystem;
    AudioSource audioSource;
    private bool targetHasBeenFound = false;
    public UnityEvent targetSeen = new UnityEvent();


    void IFocusable.OnFocusEnter()
    {
        if (targetHasBeenFound == false)
        {
            particleSystem.Play();
            audioSource.Play();

            targetSeen.Invoke();
            targetHasBeenFound = true;
        }
    }

    void IFocusable.OnFocusExit()
    {
    }



    // Use this for initialization
    void Start () {
        particleSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
