using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;



[RequireComponent(typeof(Animator))]
public class TabularButton : MonoBehaviour, IFocusable, IInputClickHandler {


    public GameObject NormalTextIcon;
    public GameObject CheckedTextIcon;
    public AudioSource PressedAudioSource;

    //Exposing UnityEvent with parameter to the Editor GUI requires this hack as of U5.5.1
    [System.Serializable]
    public class BooleanUnityEvent : UnityEvent<bool> {}

    Animator animator;

    [SerializeField]
    private bool _Checked = false;

    private bool IsHovering = false;

    public bool Checked {
        get
        {
            return _Checked;
        }

        private set
        {
            if(value != _Checked)
            {
                this._Checked = value;
                if (ButtonStateCheckedListener != null)
                {
                    ButtonStateCheckedListener.Invoke(_Checked);
                }

                if (this._Checked)
                {
                    animator.SetTrigger(PressedTriggerName);
                    PressedAudioSource.Play();
                }
                else
                {
                    animator.SetTrigger(CheckedPressedTriggerName);
                    PressedAudioSource.Play();
                }
                SwitchIcon();
                animator.SetBool("CheckedState", this._Checked);
            }
        }
    }


    private void SwitchIcon()
    {
        if(NormalTextIcon != null && CheckedTextIcon != null)
        {
            if (Checked)
            {
                NormalTextIcon.SetActive(false);
                CheckedTextIcon.SetActive(true);
            }
            else
            {

                NormalTextIcon.SetActive(true);
                CheckedTextIcon.SetActive(false);
            }
        }
    }

    [Header("Toogle Changed Event")]
    [SerializeField]
    public BooleanUnityEvent ButtonStateCheckedListener;

    

    [Header("Animation Trigger Names")]
    public string NormalTriggerName = "Normal";
    public string PointerOverTriggerName = "PointerOver";
    public string PressedTriggerName = "Pressed";
    public string DisabledTriggerName = "Disabled";
    public string CheckedTriggerName = "Checked";
    public string CheckedPointerOverTriggerName = "CheckedPointerOver";
    public string CheckedPressedTriggerName = "CheckedPressed";
    public string CheckedDisabledTriggerName = "CheckedDisabled";
    public string IsHoveringParameterName = "IsHovering";



    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        if (this.Checked)
        {
            animator.SetTrigger(PressedTriggerName);
        }
    }
    

    public void Uncheck()
    {
        Checked = false;
    }

    public void Check()
    {
        Checked = true;
    }

    public void SetChecked(bool checkedState)
    {
        Checked = checkedState;
    }

    public bool ToggleChecked()
    {
        SetChecked(!Checked);
        return Checked;
    }

    void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
    {
        Check();
    }

    void IFocusable.OnFocusEnter()
    {
        IsHovering = true;
        animator.SetBool(IsHoveringParameterName, IsHovering);
        if (Checked)
        {
            animator.SetTrigger(CheckedPointerOverTriggerName);
        }else
        {
            animator.SetTrigger(PointerOverTriggerName);
        }
    }

    void IFocusable.OnFocusExit()
    {
        IsHovering = false;
        animator.SetBool(IsHoveringParameterName, IsHovering);
        if (Checked)
        {
            animator.SetTrigger(CheckedTriggerName);
        }
        else
        {
            animator.SetTrigger(NormalTriggerName);
        }

    }
}
