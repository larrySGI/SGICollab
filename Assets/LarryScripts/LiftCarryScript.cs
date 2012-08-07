using UnityEngine;
using System.Collections;

public class LiftCarryScript : MonoBehaviour {

	// Use this for initialization
	Vector3 lastPosition;
	Vector3 thisPosition;
	Vector3 liftMovement;
	public float liftCarryMultiplier = 1.0f;
	
	void Start () 
	{
		lastPosition = thisPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		lastPosition = thisPosition;
		thisPosition = transform.position;
		
		liftMovement = thisPosition - lastPosition;
	}
	/*
	void OnTriggerEnter(Collider col)
	{
    		// col.transform.parent = transform.parent; 
			col.rigidbody.isKinematic = true;
	}

	void OnTriggerExit(Collider col)
	{
       		//col.transform.parent = null; 
			col.rigidbody.isKinematic = false;

	}*/
	
	void OnTriggerStay (Collider col)
	{
		//HACK: I can't use kinematics, so I must adjust here. Hopefully this will apply to all scales and sizes.	
		col.transform.position += (liftMovement * liftCarryMultiplier);	
	}
}
