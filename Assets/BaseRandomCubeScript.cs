using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class BaseRandomCubeScript : MonoBehaviour, ISpeechHandler, IManipulationHandler {

    [Range(0, 5)]
    public int boxType = 0;


    [System.Serializable]
    public class MeshAndMat
    {
        public Mesh mesh;
        public Material material;
    }

    public List<MeshAndMat> meshAndMatOptions = new List<MeshAndMat>();

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private ManipualtionModes manipulationMode = ManipualtionModes.Scale;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        if(meshRenderer == null)
        {

            meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshFilter = GetComponentInChildren<MeshFilter>();
        }
        if (boxType == 0)
        {
            SetMeshAndMaterial(Random.Range(0, meshAndMatOptions.Count));
            gameObject.transform.Rotate(RandomizeRotation90Degrees());
        }
        else
        {
            SetMeshAndMaterial(boxType);
            gameObject.transform.Rotate(RandomizeRotation90Degrees());

        }

        ////Randomize rotation in increments of 90 degrees, to avoid completely identical looking boxes.
    }



    private void SetMeshAndMaterial(int meshAndMatIndex)
    {
        var meshAndMat = meshAndMatOptions[meshAndMatIndex];

        meshRenderer.material = meshAndMat.material;
        meshFilter.mesh = meshAndMat.mesh;
    }

    private static Vector3 RandomizeRotation90Degrees()
    {
        return new Vector3(Random.Range(0, 4) * 90f, Random.Range(0, 4) * 90f, Random.Range(0, 4) * 90f);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void ISpeechHandler.OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        if (RecognizeKeyword(eventData, "scale cube"))
        {
            manipulationMode = ManipualtionModes.Scale;
        }

        if (RecognizeKeyword(eventData, "move cube"))
        {
            manipulationMode = ManipualtionModes.Drag;
        }

        if (RecognizeKeyword(eventData, "remove"))
        {
            Destroy(gameObject);
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
        prevCumulativeDelta = null;
    }
    enum ManipualtionModes
    {
        None, 
        Drag,
        Scale,
        Rotate
    }

    Vector3? prevCumulativeDelta;
    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (manipulationMode == ManipualtionModes.Scale)
        {
            if (prevCumulativeDelta != null)
            {
            
                var delta = (eventData.CumulativeDelta.y - prevCumulativeDelta.Value.y);
                delta = delta * 6;
                gameObject.transform.localScale += new Vector3(delta, delta, delta);

            }
            prevCumulativeDelta = eventData.CumulativeDelta;
        }

        else if(manipulationMode == ManipualtionModes.Drag)
        {
            if (prevCumulativeDelta != null)
            {
                var delta = (eventData.CumulativeDelta - prevCumulativeDelta.Value);
                delta = delta * 5;
                gameObject.transform.localPosition += delta;
            }
            prevCumulativeDelta = eventData.CumulativeDelta;
        }
    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        prevCumulativeDelta = null;
        //throw new NotImplementedException();
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        prevCumulativeDelta = null;
        //throw new NotImplementedException();
    }
}
