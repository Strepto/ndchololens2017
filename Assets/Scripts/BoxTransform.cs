using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;

public class BoxTransform : MonoBehaviour, IFocusable, ISpeechHandler, IManipulationHandler
{
    Vector3? prevCumulativeDelta;
    private bool isManipulating = false;
    private ManipulationModes manipulationMode = ManipulationModes.Scale;
    private WorldAnchorManager anchorManager;


	void Start(){
		anchorManager = WorldAnchorManager.IsInitialized ? WorldAnchorManager.Instance : null;
	}

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
        if(GameStateManager.Instance.CurrentGameState != GameState.Configuration)
        {
            return;
        }
		if(isManipulating){
			return;
		}
		isManipulating = true;
        prevCumulativeDelta = null;
        InputManager.Instance.PushModalInputHandler(gameObject);
		if(anchorManager != null){
			anchorManager.RemoveAnchor(gameObject);
		}

    }


    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (!isManipulating)
        {
            return;
        }

        var manipulationMode = BoxManipulationManager.Instance.GetManipulationMode();
        if (manipulationMode == ManipulationModes.Scale)
        {
            if (prevCumulativeDelta != null)
            {
                var delta = (eventData.CumulativeDelta.y - prevCumulativeDelta.Value.y);
                delta = delta * 6;
                gameObject.transform.localScale += Vector3.one * delta;
            }
        }
        else if (manipulationMode == ManipulationModes.Move)
        {
            if (prevCumulativeDelta != null)
            {
                var delta = (eventData.CumulativeDelta - prevCumulativeDelta.Value );
                delta = delta * 5;
                gameObject.transform.position += delta;
            }
        }
        else if (manipulationMode == ManipulationModes.Rotate)
        {
            if (prevCumulativeDelta != null)
            {
                var delta = (eventData.CumulativeDelta - prevCumulativeDelta.Value);

                float multiplier = 1.0f;
                float cameraLocalYRotation = Camera.main.transform.localRotation.eulerAngles.y;

                if (cameraLocalYRotation > 270 || cameraLocalYRotation < 90)
                    multiplier = -1.0f;

                var rotation = new Vector3(eventData.CumulativeDelta.y * -multiplier, eventData.CumulativeDelta.x * multiplier);
                transform.Rotate(rotation * 10, Space.World);
            }
        }
        prevCumulativeDelta = eventData.CumulativeDelta;
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
		if(!isManipulating)
        {
			return;
		}
        isManipulating = false;
		if(anchorManager != null){
			anchorManager.AttachAnchor(gameObject, gameObject.GetComponent<TapToPlace>().SavedAnchorFriendlyName);
		}
		InputManager.Instance.PopModalInputHandler();
        prevCumulativeDelta = null;
    }
    void IFocusable.OnFocusEnter()
    {
        // Take focus events.
    }

    void IFocusable.OnFocusExit()
    {
    }
}
