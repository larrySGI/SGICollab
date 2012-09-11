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
			
			GameManagerVik.gemsCollected++;
			
			if(PhotonNetwork.isMasterClient)
				collabAnalytics.sendAnalytics(other.transform, "gemcollect");
			
			Destroy(this.gameObject);
		}
	}
}
