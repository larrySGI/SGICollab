using UnityEngine;
using System.Collections;

public class triggerCsScript : Photon.MonoBehaviour {
	public float height = 3.2f;
	public string liftNameAnalytics;
	
	public bool move_x = false;
	public bool move_y = true;
	public bool move_z = false;
	
	public float height_x = 3.2f;	
	public float height_y = 3.2f;
	public float height_z = 3.2f;
	
	private float offset_x = 0.0f;
	private float offset_y = 0.0f;
	private float offset_z = 0.0f;
	
	public Texture viewerTexture;
	private Texture originalTexture;
	private Texture originalTargetTexture;
	public static bool revealColours = false;
		
	public float speed = 0.2f;
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
	
		
	[RPC]
	void updateLiftTime (float liftTime)
	{	
			
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
		localLiftTime = -5.0f;	
		thisFrameTime = (float)PhotonNetwork.time;
		
		originalTexture = this.renderer.material.mainTexture;
		originalTargetTexture = target.renderer.material.mainTexture;
			
	}	
	
	//These must be updated every frame. Put only those things you can't escape from updating every frame here.
	void FixedUpdate()
	{
		lastFrameTime = thisFrameTime;
		thisFrameTime = (float)PhotonNetwork.time;
			
		photonDelta = thisFrameTime - lastFrameTime;
			
		GameObject SpawnManager = GameObject.Find("Code");
		GameManagerVik MoverTest = SpawnManager.GetComponent<GameManagerVik>();
		
		if(MoverTest.selectedClass == "Viewer"){
			if(revealColours == true)
			{				
				this.renderer.material.mainTexture = viewerTexture;
				if(target)
					target.renderer.material.mainTexture = viewerTexture;
			}
			else{
				this.renderer.material.mainTexture = originalTexture;
				if(target)
					target.renderer.material.mainTexture = originalTargetTexture;
			}
		}
			
			
		if (MoverTest.gameStarted && !started)
		{		
			localLiftTime = -5.0f;
			sendLiftTime(PhotonTargets.Others);
			started = true;		
		}	
	}		
		
	void OnTriggerEnter(){
			Playtomic.Log.LevelCounterMetric(liftNameAnalytics, 0);
			print("sent analytics for lift switch pressed");
	}
		
	//Larry: This method is more useful than TriggerEnter and TriggerExit. It only runs if there's an object on top of the trigger, so will handle situations such as "player quits game while on top of button" or
	//"object is deleted while on top of button". 
		
	void OnTriggerStay()
	{
			
		localLiftTime += photonDelta;
			
		float math = Mathf.Sin(localLiftTime*speed+timingOffset);
		//Debug.Log(math);
		if (move_x) 	
			 offset_x = (1.0f + math)* height_x / 2.0f;
		else
			 offset_x = 0.0f;
		
			
		if (move_y) 	
			 offset_y = (1.0f + math)* height_y / 2.0f;
		else
			 offset_y = 0.0f;
			
		if (move_z) 	
			 offset_z = (1.0f + math)* height_z / 2.0f;
		else
			 offset_z = 0.0f;
		
		
			
		FinalPos = originPos + new Vector3(offset_x, offset_y, offset_z);
				
		if(target.transform.position != FinalPos)
		{
				target.transform.position = Vector3.Lerp(target.transform.position, FinalPos, Time.deltaTime);
		}
			
	}		

	public void toggleRevealColours(){
		if(revealColours)
			revealColours = false;
		else
			revealColours = true;
	}
}