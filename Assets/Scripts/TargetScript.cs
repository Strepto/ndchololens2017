using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class TargetScript : MonoBehaviour, IFocusable {

    ParticleSystem particlesSystem;
    AudioSource audioSource;
    private bool targetHasBeenFound = false;
    
    public event Action<GameObject> targetSeen;


    void IFocusable.OnFocusEnter()
    {
        if (targetHasBeenFound == false)
        {
            particlesSystem.Play();
            audioSource.Play();
            if(targetSeen != null)
            {
                targetSeen.Invoke(gameObject);
            }
            targetHasBeenFound = true;
            GetComponent<Renderer>().enabled = false;
        }
    }

    void IFocusable.OnFocusExit()
    {
    }



    // Use this for initialization
    void Start () {
        
        particlesSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
