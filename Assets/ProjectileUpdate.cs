using UnityEngine;
using System.Collections;

public class ProjectileUpdate : Photon.MonoBehaviour
{
	
	//Variables
	private Vector3 lastPosition;
	private Quaternion lastRotation;
	private Transform myTransform;
		
	// Use this for initialization
	[RPC]
	void updateMovement (Vector3 newPosition, Quaternion newRotation)
	{	
		Debug.Log("updating plank");
		transform.position = newPosition;
		
		transform.rotation = newRotation;
		

	}

	
	
	void Start(){

	}
	
	void Awake () 
	{
		
			myTransform = transform;
			
			photonView.RPC("updateMovement", PhotonTargets.OthersBuffered, myTransform.position, myTransform.rotation);
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		
		
		//If the player has moved at all then fire an RPC to update across the network\
		// rigidbody.isKinematic = false;
		//photonView.RPC("turnoffkinematics", PhotonTargets.OthersBuffered);
		GameObject SpawnManager = GameObject.Find("Code");
		GameManagerVik MoverTest = SpawnManager.GetComponent<GameManagerVik>();
		if(MoverTest.selectedClass == "Mover")
		{
		
			
			if(Vector3.Distance(myTransform.position, lastPosition) >=0.01)
			{
				lastPosition = myTransform.position;
	
				photonView.RPC("updateMovement", PhotonTargets.Others, myTransform.position, myTransform.rotation);
			}
			
			if(Quaternion.Angle(myTransform.rotation, lastRotation) >=1)
			{	
				//Capture the player's rotation before the RPC is fired
				//This det ermins if the player has turned in the previous if statement
				 
				lastRotation = myTransform.rotation;
				
				photonView.RPC("updateMovement", PhotonTargets.Others, myTransform.position, myTransform.rotation);
				
			}
		}else{
			enabled=false;
			rigidbody.isKinematic =true;
		}
		
	}
	
	

}
