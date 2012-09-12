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
		if (other.attachedRigidbody.name.Contains("Builder")){
			print("Destroyed all built objects!");
			//   print("Starting " + Time.time);
	        StartCoroutine(destroyLater(1.0F));
	       // print("Before WaitAndPrint Finishes " + Time.time);
	    
	  
			GameObject[] platformsCreated = GameObject.FindGameObjectsWithTag("PlacedPlatform");
			foreach(GameObject creation in platformsCreated){
				creation.transform.position += Vector3.up * 100.0F;
				creation.renderer.enabled = false;
			}
			
			GameObject[] blocksCreated = GameObject.FindGameObjectsWithTag("PlacedBlock");
			foreach(GameObject creation in blocksCreated){
				creation.transform.position += Vector3.up * 100.0F;
				creation.renderer.enabled = false;
			}
			
			ThirdPersonControllerNET.blockammo = ThirdPersonControllerNET.currentMaxBlocks;
			ThirdPersonControllerNET.plankammo = ThirdPersonControllerNET.currentMaxPlanks;
			
//			other.transform.GetChild(0).audio.PlayOneShot(other.GetComponentInChildren<MusicScript>().destroySFX);
		}
	}
  IEnumerator destroyLater(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        GameObject[] platformsCreated = GameObject.FindGameObjectsWithTag("PlacedPlatform");
		foreach(GameObject creation in platformsCreated){
			PhotonNetwork.Destroy(creation);
		}
		
		GameObject[] blocksCreated = GameObject.FindGameObjectsWithTag("PlacedBlock");
		foreach(GameObject creation in blocksCreated){
			PhotonNetwork.Destroy(creation);
		}		
    }
}
