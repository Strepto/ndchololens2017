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

    private Vector3 preStartScale;
    private Vector3 targetExplodeScale;

    [Range(1f,10f)]
    public float targetExplodeScaleFactor = 1.5f;
    private bool isFocused;
    float scaleT = 0f;

    [Range(0.01f, 10f)]
    public float timeToExplode = 1f;

    void Start()
    {
        particlesSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        gameStateManager = GameStateManager.Instance;
        preStartScale = transform.localScale;
    }

    public void ResetTarget(bool isRemote = false)
    {

        targetIsActive = true;
        GetComponent<Renderer>().enabled = true;
        preStartScale = transform.localScale;
        targetExplodeScale = preStartScale * targetExplodeScaleFactor;
        scaleT = 0f;

        if(!isRemote)
            this.gameObject.GetComponent<SyncObjectAccessor>().SyncObject.isActive.Value = true;
    }

    public void TargetRemoteStateChanged(bool activated)
    {
        if(activated)
        {
            ResetTarget(true);
        }
        else
        {
            RemoveTarget();
        }
    }

    void Update()
    {
        if (isFocused)
        {
            if (gameStateManager.CurrentGameState == GameState.Playing)
            {
                if (targetIsActive == true)
                {
                    if (scaleT == 1f)
                    {
                        particlesSystem.Play();
                        audioSource.Play();
                        if (TargetSeenEvent != null)
                        {
                            TargetSeenEvent.Invoke(this);
                        }

                        this.gameObject.GetComponent<SyncObjectAccessor>().SyncObject.isActive.Value = false;

                        targetIsActive = false;
                        transform.localScale = preStartScale;
                        GetComponent<Renderer>().enabled = false;
                    }
                    else
                    {
                        if(scaleT < 1f) {
                            scaleT += Time.deltaTime / timeToExplode;
                        }
                        else
                        {
                            scaleT = 1f;
                        }
                        transform.localScale = Vector3.Slerp(preStartScale, targetExplodeScale, scaleT * scaleT);
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (isFocused)
        {
            isFocused = false;
        }
    }

    void IFocusable.OnFocusEnter()
    {
        isFocused = true;
    }

    void IFocusable.OnFocusExit()
    {
        isFocused = false;
    }

    internal void RemoveTarget(bool isRemote = false)
    {

        targetIsActive = false;
        GetComponent<Renderer>().enabled = false;

        if (!isRemote)
            this.gameObject.GetComponent<SyncObjectAccessor>().SyncObject.isActive.Value = false;
    }
}
