using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using HoloToolkit.Unity;

[RequireComponent(typeof(Animator))]
public class ButtonScript : MonoBehaviour, IFocusable, IInputClickHandler {
    Animator animator;
    public AudioSource ClickedAudioSource;
    //Can the button be interacted with?
    [Tooltip("Can the button be interacted with?")]
    public bool Interactable = true;
    [Header("Animation Trigger Names")]
    public string IdleTrigger = "Idle";
    public string HighlightTrigger = "Highlight";
    public string PressedTrigger = "Pressed";
    public string DisabledTrigger = "Disabled";

    [Header("Animation Trigger Event")]
    public UnityEvent OnClickEvent;

    void IFocusable.OnFocusEnter()
    {
        if (Interactable)
        {
            animator.SetTrigger(HighlightTrigger);
            animator.SetBool("IsHovering", true);
        }
    }

    void IFocusable.OnFocusExit()
    {
        if (Interactable)
        {
            animator.SetTrigger(IdleTrigger);
        }
        animator.SetBool("IsHovering", false);

    }

    void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
    {
        PressButton();
    }



    public void PressButton()
    {
        if (Interactable) {
            animator.SetTrigger(PressedTrigger);
        }
    }

    public void PressedTriggerStarted()
    {
        if (OnClickEvent != null)
        {
            OnClickEvent.Invoke();
        }
        if (ClickedAudioSource != null)
        {
            ClickedAudioSource.GetComponent<AudioSource>().Play();
        }
    }


    public void SetInteractable(bool isInteractable)
    {
        Interactable = isInteractable;
        if (Interactable)
        {
            animator.SetTrigger(IdleTrigger);
        }
        else
        {
            animator.SetTrigger(DisabledTrigger);
        }
    }


    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator>();
        if (!Interactable)
        {
            animator.SetTrigger(DisabledTrigger);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
