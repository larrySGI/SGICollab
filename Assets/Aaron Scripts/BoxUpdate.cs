using UnityEngine;
using System.Collections;

public class BoxUpdate : Photon.MonoBehaviour
{
	
	//Variables
	private Vector3 lastPosition;
	private Quaternion lastRotation;
	private Transform myTransform;
		
	// Use this for initialization
	[RPC]
	void updateMovement (Vector3 newPosition, Quaternion newRotation)
	{	
		Debug.Log("updating block");
		transform.position = newPosition;
		
		transform.rotation = newRotation;
		

	}
	[RPC]
	void turnOnKinematics()
	{	
		Debug.Log("turning on kinematics");
		rigidbody.isKinematic = true;
	}
	[RPC]
	void turnOffKinematics()
	{	
		Debug.Log("turning off kinematics");
		rigidbody.isKinematic = false;
		

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
		GameObject SpawnManager = GameObject.Find("Code");
		GameManagerVik MoverTest = SpawnManager.GetComponent<GameManagerVik>();
		if(MoverTest.selectedClass == "Mover")
		{
			rigidbody.mass = 2;
			
			if(Vector3.Distance(myTransform.position, lastPosition) >=0.01)
			{
				
				//if raycast hits lift, set position to 
				
				lastPosition = myTransform.position;
				photonView.RPC("updateMovement", PhotonTargets.Others,myTransform.position, myTransform.rotation);		
					
				
				
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
			
	public void OnKinematics()
	{
		photonView.RPC("turnOnKinematics", PhotonTargets.Others);
	}
	
	

}
