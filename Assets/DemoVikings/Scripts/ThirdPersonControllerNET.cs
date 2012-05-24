using UnityEngine;
using System.Collections;

public delegate void JumpDelegate ();
enum GravityGunState { Free, Catch, Occupied, Charge, Release};
public class ThirdPersonControllerNET : Photon.MonoBehaviour
{
	
	//added a line here
	public Rigidbody target;
	public int blockammo;
	public int plankammo;
	private GravityGunState gravityGunState =0;
	private float holdDistance = 1.0f;
	private Rigidbody rigid;
		// The object we're steering
	public float speed = 1.0f, walkSpeedDownscale = 1.0f, turnSpeed = 2.0f, mouseTurnSpeed = 0.3f, jumpSpeed = 1.0f;
		// Tweak to ajust character responsiveness
	public LayerMask groundLayers = -1;
		// Which layers should be walkable?
		// NOTICE: Make sure that the target collider is not in any of these layers!
	public float groundedCheckOffset = 0.7f;
		// Tweak so check starts from just within target footing
	public bool
		showGizmos = true,
			// Turn this off to reduce gizmo clutter if needed
		requireLock = true,
			// Turn this off if the camera should be controllable even without cursor lock
		controlLock = false;
			// Turn this on if you want mouse lock controlled by this script
	public JumpDelegate onJump = null;
		// Assign to this delegate to respond to the controller jumping
	
	
	private const float inputThreshold = 0.01f,
		groundDrag = 5.0f,
		directionalJumpFactor = 0.7f;
		// Tweak these to adjust behaviour relative to speed
	private const float groundedDistance = 0.5f;
		// Tweak if character lands too soon or gets stuck "in air" often
		
	
	private bool grounded, walking;

    private bool isRemotePlayer = true;
	
	public bool Grounded
	// Make our grounded status available for other components
	{
		get
		{
			return grounded;
		}
	}

    public void SetIsRemotePlayer(bool val)
    {
        isRemotePlayer = val;
    }

	void Reset ()
	// Run setup on component attach, so it is visually more clear which references are used
	{
		Setup ();
	}
	
	
	void Setup ()
	// If target is not set, try using fallbacks
	{
		blockammo =1 ;
		plankammo = 5;
		if (target == null)
		{
			target = GetComponent<Rigidbody> ();
		}
	}
	
		
	void Start ()
	// Verify setup, configure rigidbody
	{
		Setup ();
		
			// Retry setup if references were cleared post-add
		
		if (target == null)
		{
			Debug.LogError ("No target assigned. Please correct and restart.");
			enabled = false;
			return;
		}

		target.freezeRotation = true;
			// We will be controlling the rotation of the target, so we tell the physics system to leave it be
		walking = false;
	}
	
	
	void Update ()
	// Handle rotation here to ensure smooth application.
	{
        if (isRemotePlayer) return;

		float rotationAmount;
		
		
			if(target.name.Contains("Builder"))
			{
				if (Input.GetKeyUp("r")){
				if(blockammo>0){
						PhotonNetwork.Instantiate("pBlock",transform.position+transform.forward,transform.rotation,0);
				
				blockammo--;
					}
				}
				if (Input.GetKeyUp("t")){
				if(plankammo>0){
						PhotonNetwork.Instantiate("pPlatform",transform.position+transform.forward,transform.rotation,0);
				
				plankammo--;
						}
					}
				
			}
			
			
		if(target.name.Contains("Mover"))
		{
				if(gravityGunState == GravityGunState.Free) 
				{
					    if(Input.GetButton("Fire1")) 
						{
					                RaycastHit hit;
									LayerMask layerMask = -1;
					                if(Physics.Raycast(transform.position, transform.forward-transform.up,out hit, 50.0f, layerMask)) 
									{
					                    if(hit.rigidbody) 
										{
					                        rigid = hit.rigidbody;
					                        rigid.isKinematic = true;
					                        gravityGunState = GravityGunState.Catch;
					                       
					                    }
					                }
					     }
				}
				else if(gravityGunState == GravityGunState.Catch) 
				{
					    rigid.transform.position = transform.position + transform.forward * holdDistance;
					    rigid.transform.rotation = transform.rotation;
					    if(!Input.GetButton("Fire1"))
					           gravityGunState = GravityGunState.Occupied;     
				}
				else if(gravityGunState == GravityGunState.Occupied) 
				{            
					    rigid.transform.position = transform.position + transform.forward * holdDistance;
						rigid.transform.rotation = transform.rotation;
					    if(Input.GetButton("Fire1"))
					           gravityGunState = GravityGunState.Charge;
				}
				else if(gravityGunState == GravityGunState.Charge && Screen.lockCursor == true) 
				{
						rigid.transform.position = transform.position + transform.forward * holdDistance;
						rigid.transform.rotation = transform.rotation;
					    if(!Input.GetButton("Fire1") && Screen.lockCursor == true)
					    {
							if(rigid.name.Contains("pPlatform"))
								rigid.isKinematic = true;
							else	
								rigid.isKinematic = false;
							gravityGunState = GravityGunState.Release;
					                   
					    }
				}
				else if(gravityGunState == GravityGunState.Release && Screen.lockCursor == true) 
				{
					            
					    gravityGunState = GravityGunState.Free;
				}
		
		}
		
		if (Input.GetMouseButton (1) && (!requireLock || controlLock || Screen.lockCursor))
		// If the right mouse button is held, rotation is locked to the mouse
		{
			if (controlLock)
			{
				Screen.lockCursor = true;
			}
			
			rotationAmount = Input.GetAxis ("Mouse X") * mouseTurnSpeed * Time.deltaTime;
		}
		else
		{
			if (controlLock)
			{
				Screen.lockCursor = false;
			}
			
			rotationAmount = Input.GetAxis ("Horizontal") * turnSpeed * Time.deltaTime;
		}
		
		target.transform.RotateAround (target.transform.up, rotationAmount);
		
		if (Input.GetKeyDown(KeyCode.Backslash) || Input.GetKeyDown(KeyCode.Plus))
		{
			walking = !walking;
		}
	}
	
	
	float SidestepAxisInput
	// If the right mouse button is held, the horizontal axis also turns into sidestep handling
	{
		get
		{
			if (Input.GetMouseButton (1))
			{
				float sidestep = -(Input.GetKey(KeyCode.Q)?1:0) + (Input.GetKey(KeyCode.E)?1:0);
                float horizontal = Input.GetAxis ("Horizontal");
				
				return Mathf.Abs (sidestep) > Mathf.Abs (horizontal) ? sidestep : horizontal;
			}
			else
			{
                float sidestep = -(Input.GetKey(KeyCode.Q) ? 1 : 0) + (Input.GetKey(KeyCode.E) ? 1 : 0);
                return sidestep;
			}
		}
	}
	
	
	bool isFourPointGrounded()
	{
		bool pt1, pt2, pt3, pt4;
		
	    Vector3 dist_forward = target.transform.forward/2;
		Vector3 dist_right = target.transform.right/2;
		
		pt1 = Physics.Raycast (                                                                                                                    
			
			(target.transform.position + dist_forward) + target.transform.up * -groundedCheckOffset,
			target.transform.up * -1,
			groundedDistance,
			groundLayers
		);

		pt2 = Physics.Raycast (                                                                                                                    
			
			(target.transform.position - dist_forward) + target.transform.up * -groundedCheckOffset,
			target.transform.up * -1,
			groundedDistance,
			groundLayers
		);
		pt3 = Physics.Raycast (                                                                                                                    
			
			(target.transform.position + dist_right) + target.transform.up * -groundedCheckOffset,
			target.transform.up * -1,
			groundedDistance,
			groundLayers
		);
		pt4 = Physics.Raycast (                                                                                                                    
			
			(target.transform.position - dist_right) + target.transform.up * -groundedCheckOffset,
			target.transform.up * -1,
			groundedDistance,
			groundLayers
		);
		
		if (pt1 == false && pt2 == false && pt3 == false && pt4 == false)		
			return false;
		else //any one of the points is touching the ground
			return true;
	}
	
	

	void FixedUpdate ()
	// Handle movement here since physics will only be calculated in fixed frames anyway
	{
		grounded = isFourPointGrounded ();

      	if (isRemotePlayer) return;
		
	
		if (grounded)
		{
			target.drag = groundDrag;
				// Apply drag when we're grounded
			
			if (Input.GetButtonDown ("Jump"))
			// Handle jumping
			{
				target.AddForce (
					jumpSpeed * target.transform.up +
						target.velocity.normalized * directionalJumpFactor,
					ForceMode.VelocityChange
				);
					// When jumping, we set the velocity upward with our jump speed
					// plus some application of directional movement
				
				if (onJump != null)
				{
					onJump ();
				}
			}
			else
			// Only allow movement controls if we did not just jump
			{
				Vector3 movement = Input.GetAxis ("Vertical") * target.transform.forward +
					SidestepAxisInput * target.transform.right;
				
				float appliedSpeed = walking ? speed / walkSpeedDownscale : speed;
					// Scale down applied speed if in walk mode
				/*
				if (Input.GetAxis ("Vertical") < 0.0f)
				// Scale down applied speed if walking backwards
				{
					appliedSpeed /= walkSpeedDownscale;
				}*/

				if (movement.magnitude > inputThreshold)
				// Only apply movement if we have sufficient input
				{
					target.AddForce (movement.normalized * appliedSpeed, ForceMode.VelocityChange);
				}
				else
				// If we are grounded and don't have significant input, just stop horizontal movement
				{
					target.velocity = new Vector3 (0.0f, target.velocity.y, 0.0f);
					return;
				}
			}
		}
		else
		{
			target.drag = 0.0f;
				// If we're airborne, we should have no drag
		}
	}
	
	
	void OnDrawGizmos ()
	// Use gizmos to gain information about the state of your setup
	{
		if (!showGizmos || target == null)
		{
			return;
		}
		
		Gizmos.color = grounded ? Color.blue : Color.red;
		Gizmos.DrawLine (target.transform.position + target.transform.up * -groundedCheckOffset,
			target.transform.position + target.transform.up * -(groundedCheckOffset + groundedDistance));
	}
}
