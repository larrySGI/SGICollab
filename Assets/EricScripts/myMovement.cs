using UnityEngine;
using System.Collections;

public class myMovement : MonoBehaviour
{
    public float movementSpeed = 10.0f;
	
    public float jumpSpeed = 5.0f;
	public bool canJump = true;
	private bool isJumping = false;
	public float maxJumpHeight = 5.0f;
	private float beforeJumpHeight;
	private float afterJumpHeight;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{	
        Vector3 movement = (Input.GetAxis("Horizontal") * -Vector3.left * movementSpeed) + (Input.GetAxis("Vertical") * Vector3.forward *movementSpeed);
        rigidbody.AddForce(movement, ForceMode.Force);
				
		if(canJump && !isJumping){
			if(Input.GetKeyDown(KeyCode.Space)){
	        	print ("jump!");
				canJump = false;
				isJumping = true;
				beforeJumpHeight = rigidbody.position.y;
				afterJumpHeight = beforeJumpHeight + maxJumpHeight;
			}
		}
		else if(isJumping)
			jumping();	
		else{
			if(rigidbody.position.y <= beforeJumpHeight)
				canJump = true;
		}
	}
	
	void jumping(){
		print("jumpinG");
		Vector3 jump = jumpSpeed * Vector3.up;
		rigidbody.AddForce(jump, ForceMode.Acceleration);
		//rigidbody.MovePosition(jump);
		
		if(rigidbody.position.y > afterJumpHeight)
			isJumping = false;
	}
}

