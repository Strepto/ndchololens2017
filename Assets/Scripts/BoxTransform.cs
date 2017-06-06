using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class BoxTransform : MonoBehaviour, IFocusable, ISpeechHandler, IManipulationHandler
{
    Vector3? prevCumulativeDelta;
    private bool isManipulating = false;
    private ManipulationModes manipulationMode = ManipulationModes.Scale;


    void ISpeechHandler.OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        if (RecognizeKeyword(eventData, "remove target"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (isManipulating)
        {
            StopManipulating();
        }
    }


    private bool RecognizeKeyword(SpeechKeywordRecognizedEventData eventData, string keyword)
    {
        if (eventData.RecognizedText.Equals(keyword, System.StringComparison.OrdinalIgnoreCase) && (int)eventData.Confidence <= (int)UnityEngine.Windows.Speech.ConfidenceLevel.Medium)
        {
            eventData.Use();
            return true;
        }
        return false;
    }

    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        StartManipulating();
    }

    private void StartManipulating()
    {
		if(isManipulating){
			return;
		}
		isManipulating = true;
        prevCumulativeDelta = null;
        InputManager.Instance.PushModalInputHandler(gameObject);
    }


    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        var manipulationMode = BoxManipulationManager.Instance.GetManipulationMode();
        if (manipulationMode == ManipulationModes.Scale)
        {
            if (prevCumulativeDelta != null)
            {

                var delta = (eventData.CumulativeDelta.y - prevCumulativeDelta.Value.y);
                delta = delta * 6;
                gameObject.transform.localScale += new Vector3(delta, delta, delta);

            }
            prevCumulativeDelta = eventData.CumulativeDelta;
        }

        else if (manipulationMode == ManipulationModes.Move)
        {
            if (prevCumulativeDelta != null)
            {
                var delta = (eventData.CumulativeDelta - prevCumulativeDelta.Value);
                delta = delta * 5;
                gameObject.transform.localPosition += delta;
            }
            prevCumulativeDelta = eventData.CumulativeDelta;
        }
        else if (manipulationMode == ManipulationModes.Rotate)
        {
            if (prevCumulativeDelta != null)
            {
                var delta = (eventData.CumulativeDelta - prevCumulativeDelta.Value);

                float rotateSpeed = 6.0f;
                transform.RotateAround(transform.position, Camera.main.transform.up,
                    -delta.x * rotateSpeed);
                transform.RotateAround(transform.position, Camera.main.transform.forward,
                    delta.y * rotateSpeed);
                transform.RotateAround(transform.position, Camera.main.transform.right,
                    delta.z * rotateSpeed);
            }
            prevCumulativeDelta = eventData.CumulativeDelta;
        }
    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        StopManipulating();
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        StopManipulating();
    }

    private void StopManipulating()
    {
		if(!isManipulating){
			return;
		}
        isManipulating = false;
		InputManager.Instance.PopModalInputHandler();
        prevCumulativeDelta = null;
    }
    void IFocusable.OnFocusEnter()
    {

    }

    void IFocusable.OnFocusExit()
    {
    }
}
