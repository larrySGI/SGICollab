using UnityEngine;
using System.Collections;

public class createNewBlock : Photon.MonoBehaviour {
	public bool created=false;
	// Use this for initialization
	void Start () {
		
		
		
	}
	void createBlock(){
	var builtBlock = PhotonNetwork.Instantiate("pBlock", transform.position + transform.forward, transform.rotation, 0);
						builtBlock.tag = "PlacedBlock";
	}
	// Update is called once per frame
	void Update () {
	
	}		
}
