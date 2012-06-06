using UnityEngine;
using System.Collections;

public class BoxUpdate : Photon.MonoBehaviour
{
	
	//Variables
//	private Vector3 lastPosition;
//	private Quaternion lastRotation;
	private Transform myTransform;
	
	private GameManagerVik manager;
	private bool isCarried = false;
		
	// Use this for initialization
	[RPC]
	void updateMovement (Vector3 newPosition, Quaternion newRotation)
	{	
		transform.position = newPosition;	
		transform.rotation = newRotation;
	}
	/*
	[RPC]
	void turnOnKinematics()
	{	
		rigidbody.isKinematic = true;
	}
	[RPC]
	void turnOffKinematics()
	{	
		rigidbody.isKinematic = false;
		

	}
	*/
	public void setCarry(bool Carry)
	{
		isCarried = Carry;
	}
	
	void Start()
	{
		GameObject SpawnManager = GameObject.Find("Code");
		manager = SpawnManager.GetComponent<GameManagerVik>();
	
	}
	
	void Awake () 
	{
		
		//	myTransform = transform;
			
			photonView.RPC("updateMovement", PhotonTargets.OthersBuffered, myTransform.position, myTransform.rotation);
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		if(isCarried)
		{
			rigidbody.mass = 2;
			rigidbody.isKinematic = true;
		//	if(Vector3.Distance(myTransform.position, lastPosition) >=0.01)
		//	{
				
				//if raycast hits lift, set position to 
				
				//lastPosition = myTransform.position;
			photonView.RPC("updateMovement", PhotonTargets.Others,transform.position, transform.rotation);		
					
				
				
			//}
			
			//if(Quaternion.Angle(myTransform.rotation, lastRotation) >=1)
			//{	
				//Capture the player's rotation before the RPC is fired
				//This det ermins if the player has turned in the previous if statement
				 
				//lastRotation = myTransform.rotation;
				
				photonView.RPC("updateMovement", PhotonTargets.Others, transform.position, transform.rotation);
				
			//}
		}
		else
		{
			rigidbody.isKinematic= false;
			rigidbody.mass = 9999;
		}
			
	}
	/*		
	public void OnKinematics()
	{
		photonView.RPC("turnOnKinematics", PhotonTargets.Others);
	}
	*/
	

}
