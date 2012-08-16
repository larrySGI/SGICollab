using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {
	
	float timeDied;
	float respawnTime = 0.5f;
	
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
			
			Debug.Log("Died, waiting for respawn");
			timeDied = Time.time;
		}
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag == "Builder" ||
			other.tag == "Mover" ||
				other.tag == "Viewer" ||
					other.tag == "Jumper"){
			
			if(Time.time - timeDied > respawnTime)
				other.transform.position = other.GetComponent<ThirdPersonControllerNET>().lastRespawn;
		}
	}
}
