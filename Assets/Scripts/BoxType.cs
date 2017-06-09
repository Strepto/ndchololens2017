using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
[ExecuteInEditMode]
public class BoxType : MonoBehaviour {

    [Tooltip("-1 is random. 0-n are specified values from MeshAndMatOptions.")]
    public int boxType = -1;


    [System.Serializable]
    public class MeshAndMat
    {
        public Mesh mesh;
        public Material material;
    }

    public List<MeshAndMat> meshAndMatOptions = new List<MeshAndMat>();

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        if(meshRenderer == null)
        {

            meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshFilter = GetComponentInChildren<MeshFilter>();
        }
        if (boxType == -1)
        {
            SetMeshAndMaterial(Random.Range(0, meshAndMatOptions.Count));
            //Randomize rotation in increments of 90 degrees, to avoid completely identical looking boxes.
            gameObject.transform.Rotate(RandomizeRotation90Degrees());
        }
        else
        {
            SetMeshAndMaterial(boxType);
            //Not rotating randomly.
        }
        
    }

    public void ChangeBoxType(int boxType){
        this.boxType = boxType;
        SetMeshAndMaterial(boxType);
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

}
