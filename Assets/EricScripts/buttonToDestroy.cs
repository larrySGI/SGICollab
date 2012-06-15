using UnityEngine;
using System.Collections;

public class buttonToDestroy : MonoBehaviour {
		
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
			Destroy(creation);
		}
		
		GameObject[] blocksCreated = GameObject.FindGameObjectsWithTag("PlacedBlock");
		foreach(GameObject creation in blocksCreated){
			Destroy(creation);
		}
		
		//GameObject builder = GameObject.FindGameObjectWithTag("Builder");
		//builder.GetComponent(ThirdPersonControllerNET);
		
		ThirdPersonControllerNET.blockammo = 1;
		ThirdPersonControllerNET.plankammo = 5;
		
	}
}
