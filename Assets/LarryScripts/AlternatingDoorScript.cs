using UnityEngine;
using System.Collections;

public class AlternatingDoorScript : Photon.MonoBehaviour {

public Texture viewerTexture;

//I assume both doors move at the same speed, or the logic won't work. 
public float speed = 0.3f;

public GameObject door1;
public GameObject door2;
public float doorOpenHeight = 3.2f;
	
public int startmode;	//where 0 is the default, 1 swaps the up and down, 2 means both doors down, 3 means both doors up.
	
private Vector3 door1Down;
private Vector3 door1Up;
private Vector3 door2Down;
private Vector3 door2Up;
	
private Vector3 door1CurrentPos;
private Vector3 door2CurrentPos;
		
private float lastFrameTime;
private float thisFrameTime;
private float photonDelta;

private bool started = false;
private bool doorState = false; //true for "open", false for "close"
	
//Larry: Network code attempt, disabled for now
	
	
[RPC]
void resetDoors()
{	
		
 
	//Temporarily here
	switch (startmode)
	{
		case 1:
			door1.transform.position = door1Down;
			door2.transform.position = door2Up;
			break;
		case 2:
			door1.transform.position = door1Up;
			door2.transform.position = door2Down;

			break;
		case 3:
			door1.transform.position = door1Down;
			door2.transform.position = door2Down;

			break;
		default:
			door1.transform.position = door1Up;
			door2.transform.position = door2Up;
			break;
	}
		
	doorState = false;	
}
	
void resetDoorTime(PhotonTargets ptarget)
{
	photonView.RPC("resetDoors",ptarget);		
}


// Use this for initialization
void Start () {
	//startMove = false;
	door1Down = door1Up = door1.transform.position;
	door2Down = door2Up = door2.transform.position;
		
	door1Up.y += doorOpenHeight;
	door2Up.y += doorOpenHeight;
		
	thisFrameTime = (float)PhotonNetwork.time;
		
	doorState = false;
}	
/*
bool isDoorInPosition(vector3 currPos, vector3 targetPos)
{
	return (currPos == targetPos);
}*/
		
		
void UpdateDoor1()
{
	if (startmode == 0 || startmode == 2)
	{
		if (doorState) //"close"
		{
		//	if (isDoorInPosition(door1.transform.position, door1Down)) return;	
			
			
		}
		else //"open"
		{
		//	if (isDoorInPosition(door1.transform.position, door1Up)) return;	
						
		}
		
		
	}
	else
	{
		if (doorState) //"close"
		{
		//		if (isDoorInPosition(door1.transform.position, door1Up)) return;	

			
		}
		else //"open"
		{
		//		if (isDoorInPosition(door1.transform.position, door1Down)) return;	
					
		}

		
	}		
	
}
	
void UpdateDoor2()
{
	if (startmode == 0 || startmode == 2)
	{
		if (doorState) //"close"
		{
		}
		else //"open"
		{
						
		}
		
	}
	else
	{
		if (doorState) //"close"
		{
		}
		else //"open"
		{
						
		}
		
	}		

	
}	
	
//These must be updated every frame. Put only those things you can't escape from updating every frame here.
void FixedUpdate()
{
	lastFrameTime = thisFrameTime;
	thisFrameTime = (float)PhotonNetwork.time;
		
	photonDelta = thisFrameTime - lastFrameTime;

	GameObject SpawnManager = GameObject.Find("Code");
	GameManagerVik MoverTest = SpawnManager.GetComponent<GameManagerVik>();
	
	if(MoverTest.selectedClass == "Viewer")
	{
	
		this.renderer.material.mainTexture = viewerTexture;
		
		if (door1 != null) 
			door1.renderer.material.mainTexture = viewerTexture;
		if (door2 != null)
			door2.renderer.material.mainTexture = viewerTexture;
			
	}
		
		
	if (MoverTest.gameStarted && !started)
	{		
		resetDoorTime(PhotonTargets.OthersBuffered);
		started = true;		
	}	
		
	UpdateDoor1();
	UpdateDoor2();		
}		
	
/*
//Larry: This method is more useful than TriggerEnter and TriggerExit. It only runs if there's an object on top of the trigger, so will handle situations such as "player quits game while on top of button" or
//"object is deleted while on top of button". 
void OnTriggerStay()
{
		
	localDoorTime += photonDelta;
			
	float math = Mathf.Sin(localLiftTime*speed+timingOffset);
	//Debug.Log(math);
	float offset = (1.0f + math)* height / 2.0f;
	
	FinalPos = originPos + new Vector3(0.0f, offset, 0.0f);
			
	if(target.transform.position != FinalPos)
	{
			target.transform.position = Vector3.Lerp(target.transform.position, FinalPos, Time.deltaTime * 2);
	}
		
}*/		

	
//The door will open/shut upon touch, doesn't matter if the player is on the button or not. Therefore TriggerEntry is the best method, doors will not have
//to reset this way.
void OnTriggerEntry()
{
	doorState = !doorState;		
}
	
}