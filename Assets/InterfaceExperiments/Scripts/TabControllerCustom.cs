using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabControllerCustom : MonoBehaviour {
    public GameObject DisplayArea;
	// Use this for initialization
	void Start () {
		
	}
	

    public void TextureOffset(float offsetX)
    {
        DisplayArea.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
