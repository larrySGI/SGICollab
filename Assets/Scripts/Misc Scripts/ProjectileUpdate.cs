using UnityEngine;
using System.Collections;

public class ProjectileUpdate : Photon.MonoBehaviour
{
	
	//Variables
	public LayerMask doNotIntersectWithLayer;
	
	
	private Vector3 lastPosition;
	private Quaternion lastRotation;
	private Transform myTransform;

	
	private float minimumExtent; 
	private float partialExtent; 
	private float sqrMinimumExtent; 
	
	
	private GameManagerVik MoverTest;
	
	// Use this for initialization
	[RPC]
	void updateMovement (Vector3 newPosition, Quaternion newRotation)
	{			
		transform.position = newPosition;		
		transform.rotation = newRotation;
	}

	
	
	void Start(){

	}
	
	void Awake () 
	{
		
			myTransform = transform;
			lastPosition = myTransform.position;
			lastRotation = myTransform.rotation;
			minimumExtent = Mathf.Min(Mathf.Min(collider.bounds.extents.x, collider.bounds.extents.y), collider.bounds.extents.z); 
		   partialExtent = minimumExtent; 
		   sqrMinimumExtent = minimumExtent * minimumExtent; 
		
		
			GameObject SpawnManager = GameObject.Find("Code");
			if (SpawnManager)
				MoverTest = SpawnManager.GetComponent<GameManagerVik>();
			else
			{
				Debug.Log("No code object");	
			}
	
			photonView.RPC("updateMovement", PhotonTargets.OthersBuffered, myTransform.position, myTransform.rotation);
		
			rigidbody.isKinematic =true;
			
	
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (MoverTest == null) 
		{
			Debug.Log("No Code Object!");
			return;
		}
		
		
		//If the player has moved at all then fire an RPC to update across the network\
		// rigidbody.isKinematic = false;
		//photonView.RPC("turnoffkinematics", PhotonTargets.OthersBuffered);
		if(MoverTest.selectedClass == "Mover")
		{	
			//enabled = true;
			Vector3 movementThisStep = myTransform.position - lastPosition; 
			
			if (movementThisStep.magnitude > 0.0f)
			{
			 	//float movementSqrMagnitude = movementThisStep.sqrMagnitude;
				 //float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
	
				RaycastHit hitInfo; 
 	
	  	 	   //check for obstructions we might have missed 
	   		    if (Physics.Raycast(lastPosition, movementThisStep, out hitInfo, movementThisStep.magnitude, doNotIntersectWithLayer.value)) 
		  		{			
	         			myTransform.position = hitInfo.point - (movementThisStep.normalized)*partialExtent; 
		 		}
				
				lastPosition = myTransform.position;
	
				//photonView.RPC("updateMovement", PhotonTargets.Others, myTransform.position, myTransform.rotation);
			}
			
			if(Quaternion.Angle(myTransform.rotation, lastRotation) >=1)
			{	
				//Capture the player's rotation before the RPC is fired
				//This det ermins if the player has turned in the previous if statement
				 
				lastRotation = myTransform.rotation;
				
				
			}
		
			photonView.RPC("updateMovement", PhotonTargets.Others, myTransform.position, myTransform.rotation);

		}else{
			enabled=false;
			//rigidbody.AddForce (new Vector3(0,0,0), ForceMode.VelocityChange);
		}
		
	}
	
	

}
