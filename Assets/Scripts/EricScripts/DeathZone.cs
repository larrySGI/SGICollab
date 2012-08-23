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
		else{
			other.transform.position = other.GetComponent<Checkpoint>().originalPosition;
		}
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag == "Builder" ||
			other.tag == "Mover" ||
				other.tag == "Viewer" ||
					other.tag == "Jumper"){
			
			if(Time.time - timeDied > respawnTime)
			{
				//respawn at the Code object if last respawn is not at origin /Larry
				if (other.GetComponent<ThirdPersonControllerNET>().lastRespawn.magnitude > 0)
					
					other.transform.position = other.GetComponent<ThirdPersonControllerNET>().lastRespawn;
				else
				{
					
					GameObject SpawnManager = GameObject.Find("Code");
					other.transform.position = SpawnManager.transform.position;
						
				}
			}
		}
	}
}
