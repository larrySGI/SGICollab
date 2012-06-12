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
	
	[RPC] 
	void setCarryNetwork (bool Carry)
	{
		isCarried = Carry;
	}
	
	public void setCarry(bool Carry) 
	{
		isCarried = Carry;
		photonView.RPC("setCarryNetwork", PhotonTargets.Others, isCarried);
	}
	
	void Start()
	{
		GameObject SpawnManager = GameObject.Find("Code");
		manager = SpawnManager.GetComponent<GameManagerVik>();
	
	}
	
	void Awake () 
	{
		
		photonView.RPC("updateMovement", PhotonTargets.Others, transform.position, transform.rotation);
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		if(isCarried)
		{
			rigidbody.mass = 2;
			rigidbody.isKinematic = true;
			
			//the mover is the one who updates the position only when he's carrying it. In all other instances, this doesn't matter.
			if (manager.selectedClass == "Mover")
			{
				photonView.RPC("updateMovement", PhotonTargets.Others,transform.position, transform.rotation);		
				
				photonView.RPC("updateMovement", PhotonTargets.Others, transform.position, transform.rotation);
				
			}
		}
		else
		{
			//the box weighs 9999 and can only be pushed at this point. This logic allows the box to obey the laws of physics even if a Mover is not present,
			//the weight should prevent the box from being moved by random elements. 
			rigidbody.isKinematic= false;
			rigidbody.mass = 9999;
			//this is the same way a viking can remain on the lift - we force an AddForce calculation so the thing updates.
			rigidbody.AddForce (new Vector3(0,0,0), ForceMode.VelocityChange);
		}
			
	}

	

}
