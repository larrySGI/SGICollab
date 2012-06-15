using UnityEngine;
using System.Collections;

public class buttonToDestroy : Photon.MonoBehaviour {
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerEnter(Collider other){
		print("Destroyed all built objects!");
		
		GameObject[] platformsCreated = GameObject.FindGameObjectsWithTag("PlacedPlatform");
		foreach(GameObject creation in platformsCreated){
			PhotonNetwork.Destroy(creation);
		}
		
		GameObject[] blocksCreated = GameObject.FindGameObjectsWithTag("PlacedBlock");
		foreach(GameObject creation in blocksCreated){
			PhotonNetwork.Destroy(creation);
		}
		
		ThirdPersonControllerNET.blockammo = 1;
		ThirdPersonControllerNET.plankammo = 5;
		
	}
}
