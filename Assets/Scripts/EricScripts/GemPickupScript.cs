using UnityEngine;
using System.Collections;

public class GemPickupScript : MonoBehaviour {
	private bool addedGem = false;
	
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
			
			if(!addedGem){
				GameManagerVik.gemsCollected++;
				addedGem = true;
			}
			if(other.transform.GetComponent<ThirdPersonNetworkVik>().photonView.isMine){
				if(collabAnalytics.sendAnalytics(other.transform, "gemcollect", true)){				
					Destroy(this.gameObject);					
				}
			}
					
		}
	}
}
