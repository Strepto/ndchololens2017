using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMexType : MonoBehaviour {

    [Tooltip("-1 is random. 0-n are specified values from MeshAndMatOptions.")]
    public int boxType = -1;


    [System.Serializable]
    public class BoxMexMaterialFaces
    {
        public Material front;
        public Material top;
        public Material back;
    }

	[Tooltip("Use this to rotate around a common axis")]
	public Transform AlignmentObject;

    public List<BoxMexMaterialFaces> materialCombinations = new List<BoxMexMaterialFaces>();

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
            SetMeshAndMaterial(Random.Range(0, materialCombinations.Count));
            AlignmentObject.transform.Rotate(RandomizeRotation());
        }
        else
        {
            SetMeshAndMaterial(boxType);
            //Not rotating randomly.
        }

        //Randomize rotation in increments of 90 degrees, to avoid completely identical looking boxes.
    }



    private void SetMeshAndMaterial(int meshAndMatIndex)
    {
        var meshAndMat = materialCombinations[meshAndMatIndex];
		var sharedMats = meshRenderer.sharedMaterials;
        sharedMats[8] = meshAndMat.front;
        sharedMats[7] = meshAndMat.back;
        sharedMats[6] = meshAndMat.top;
		meshRenderer.sharedMaterials = sharedMats;
    }

    private static Vector3 RandomizeRotation()
    {
        return new Vector3(0f, Random.Range(0, 8) * 45f, 0f);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
