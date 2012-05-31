using UnityEngine;
using System.Collections;

public class myMovement : MonoBehaviour
{
    public float movementSpeed = 6.0f;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{	
        Vector3 movement = (Input.GetAxis("Horizontal") * -Vector3.left * movementSpeed) + (Input.GetAxis("Vertical") * Vector3.forward *movementSpeed);
        rigidbody.AddForce(movement, ForceMode.Force);
	}
}

