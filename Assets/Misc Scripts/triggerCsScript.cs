using UnityEngine;
using System.Collections;

public class triggerCsScript : Photon.MonoBehaviour {
public float height = 3.2f;
public Texture viewerTexture;
private float speed = 0.5f;
private float timingOffset = 0.0f;
//private bool startMove;
public GameObject target;

private Vector3 originPos;	
private Vector3 FinalPos;
	
private float lastFrameTime;
private float thisFrameTime;
private float photonDelta;

private float localLiftTime;	
	
private bool started = false;
	

/*
// Update is called once per frame
void Update () 
{

	
	lastFrameTime = thisFrameTime;
	thisFrameTime = (float)PhotonNetwork.time;
		
	float photonDelta = thisFrameTime - lastFrameTime;
	
		
	if (startMove) 
	{
		localLiftTime += photonDelta;
		
		if (!started)
		{
		
			localLiftTime = 0.0f;
			started = true;		
		}				
		float math = Mathf.Sin(localLiftTime*speed+timingOffset);
		float offset = (1.0f + math )* height / 2.0f;
	  	FinalPos = originPos + new Vector3(0.0f, offset, 0.0f);
			
		if(target.transform.position != FinalPos)
		{
			target.transform.position = Vector3.Lerp(target.transform.position, FinalPos, Time.deltaTime * 2);
		}
		
	}		
	
	GameObject SpawnManager = GameObject.Find("Code");
	GameManagerVik MoverTest = SpawnManager.GetComponent<GameManagerVik>();
	if(MoverTest.selectedClass == "Viewer")
	{
		this.renderer.material.mainTexture = viewerTexture;
	}
	
	else
	{
		
			if (FinalPos.y > originPos.y)
		{
			localLiftTime += photonDelta;
			float math = Mathf.Sin(localLiftTime*speed+timingOffset);
			float offset = (1.0f + math )* height / 2.0f;
	  	
		  	FinalPos = originPos - new Vector3(0.0f, offset, 0.0f);
		}
		if (FinalPos.y < originPos.y)
		{
			FinalPos = originPos;
		}
	}

	
	
}

void OnTriggerEnter() {
	startMove = true;
}

void OnTriggerExit() {
	startMove = false;
}*/

//Larry: Network code attempt, disabled for now
	
	
[RPC]
void updateLiftTime (float liftTime)
{	
	Debug.Log("updating lift");
		
    localLiftTime = liftTime;
	
	//Temporarily here
	target.transform.position = originPos;
}
	
void sendLiftTime(PhotonTargets ptarget)
{
	photonView.RPC("updateLiftTime", ptarget, localLiftTime);		
}


// Use this for initialization
void Start () {
	//startMove = false;
	originPos = target.transform.position;
	localLiftTime = 0.0f;	
	thisFrameTime = (float)PhotonNetwork.time;
	

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
	}
		
		
	if (MoverTest.gameStarted)
	{		
		localLiftTime = 0.0f;
		sendLiftTime(PhotonTargets.OthersBuffered);
		started = true;		
	}	
}		
	

//Larry: This method is more useful than TriggerEnter and TriggerExit. It only runs if there's an object on top of the trigger, so will handle situations such as "player quits game while on top of button" or
//"object is deleted while on top of button". 
void OnTriggerStay()
{
		
	localLiftTime += photonDelta;
		
				
	float math = Mathf.Sin(localLiftTime*speed+timingOffset);
	float offset = (1.0f + math )* height / 2.0f;
	FinalPos = originPos + new Vector3(0.0f, offset, 0.0f);
			
	if(target.transform.position != FinalPos)
	{
			target.transform.position = Vector3.Lerp(target.transform.position, FinalPos, Time.deltaTime * 2);
	}
		
}		

	

	
}