using UnityEngine;
using System.Collections;

public class MenuBG : MonoBehaviour {
	
	public Texture menuTexture;
	// Use this for initialization
	void Start () {
				
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Z))
			renderer.material.mainTexture = menuTexture;
	}
}
