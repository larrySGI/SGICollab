using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
		
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
			
			Debug.Log("Hit a checkpoint");
			other.GetComponent<ThirdPersonControllerNET>().lastRespawn = this.transform.position;
		}
	}
}
