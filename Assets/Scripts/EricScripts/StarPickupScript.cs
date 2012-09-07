using UnityEngine;
using System.Collections;

public class StarPickupScript : MonoBehaviour {

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
			
			GameManagerVik.starsCollected++;
			Destroy(this.gameObject);
		}
	}
}
