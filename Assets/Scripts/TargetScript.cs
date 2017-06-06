using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class TargetScript : MonoBehaviour, IFocusable
{

    ParticleSystem particlesSystem;
    AudioSource audioSource;
    private bool targetIsActive = true;
    private GameStateManager gameStateManager;

    public event Action<TargetScript> TargetSeenEvent;



    void Start()
    {
        particlesSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        gameStateManager = GameStateManager.Instance;
    }

    public void Reset()
    {
        targetIsActive = true;
        GetComponent<Renderer>().enabled = true;
    }

    void Update()
    {

    }


    void IFocusable.OnFocusEnter()
    {
        if (gameStateManager.CurrentGameState == GameState.Playing)
        {
            if (targetIsActive == true)
            {
                particlesSystem.Play();
                audioSource.Play();
                if (TargetSeenEvent != null)
                {
                    TargetSeenEvent.Invoke(this);
                    TargetSeenEvent = null;
                }
                targetIsActive = false;
                GetComponent<Renderer>().enabled = false;
            }
        }

    }

    void IFocusable.OnFocusExit()
    {
    }

    internal void RemoveTarget()
    {
        
        targetIsActive = false;
        TargetSeenEvent = null;
        GetComponent<Renderer>().enabled = false;
    }
}
