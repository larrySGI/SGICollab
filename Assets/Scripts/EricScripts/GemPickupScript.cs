using UnityEngine;
using System.Collections;

public class GemPickupScript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Builder" ||
			other.tag == "Mover" ||
				other.tag == "Viewer" ||
					other.tag == "Jumper"){
			
			if(other.transform.GetComponent<ThirdPersonNetworkVik>().photonView.isMine){
				bool sentAnalytics = false;
				
				while(!sentAnalytics){
					sentAnalytics = collabAnalytics.sendAnalytics(other.transform, "gemcollect", true);
				}
				
//			other.transform.GetChild(0).audio.PlayOneShot(other.GetComponentInChildren<MusicScript>().pickupSFX);
//			Debug.Log("AHA");
			}
			
			
			GameManagerVik.gemsCollected++;
			GameObject.Destroy(this.gameObject);							
		}
	}
}
