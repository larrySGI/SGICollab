using UnityEngine;
using System.Collections;

public class EndingBoxScript : Photon.MonoBehaviour {
	private bool isBuilderAtEnd;
	private bool isMoverAtEnd;
	private bool isJumperAtEnd;
	private bool isViewerAtEnd;
	
	private int nextLevel;
	private bool alreadyLoading = false;
	
//	public Texture aTexture;
	private int lastFrameTime;
	private int thisFrameTime;
	private int delta;
	
	private string statusText = "";
	private bool started = false;
	
	//This boolean tracks whether a player has reached the end.
	[HideInInspector]	
	public bool PlayersHaveReachedEnd = false;
	public bool isWaitingForNextStage = false;
	
	private GameManagerVik currGameManager = null;
	//private ThirdPersonControllerNET currController = null;
	
	private int ReadyCount = 0;
	private int TargetReadyCount = 0;
	
	public int levelTimeInMinutes = 1;
	[HideInInspector]
	public  int timeLeft = -1;
	private int levelEndMode = 0; //default;
	
	public GUISkin endGameSkin;
	
	[RPC]
	void callReady()
	{	
		++ReadyCount;
//		Debug.Log(ReadyCount);
	}
	
	
	[RPC]
	void SyncTimer(int currTime)
	{
		//if I have more time left than someone, then someone else's time must be correct. 
		//if (currTime < timeLeft)
		Debug.Log("time recieved " + currTime);		
		
		if (timeLeft > currTime)
		{
			Debug.Log("Syncing Timer");
			timeLeft = currTime;
		}
		
//		started = true; //doesn't matter who calls it.
	}
	
	[RPC]
	void SyncOnJoin()
	{		
		//send my time left to everyone.
		Debug.Log("RPC SyncOnJoin called");
		Debug.Log(currGameManager.selectedClass);
	//	if (photonView.isMine)
		photonView.RPC("SyncTimer",PhotonTargets.OthersBuffered, timeLeft);		
	}
	
	
	
	void Awake()
	{
		//60 seconds in a minute, assume 30 fps.
		//timeLeft = levelTimeInMinutes * 60 * 30;	
		started = false;
	}
	
	
	// Use this for initialization
	void Start () {
		ReadyCount = 0;
		isWaitingForNextStage = PlayersHaveReachedEnd = false;
		
		lastFrameTime = thisFrameTime = (int)Time.time;
		delta = 0;
		isBuilderAtEnd = false; 
		isMoverAtEnd = false;
		isJumperAtEnd = false;
		isViewerAtEnd = false;
		
		statusText = "Press [Spacebar] to Go To Next Stage";
				
		//Now I'll handle it here. 
		GameManagerVik.setNextLevel(Application.loadedLevel);
		currGameManager = GameObject.Find("Code").GetComponent<GameManagerVik>();
		
		if (currGameManager.level_tester_mode)
			TargetReadyCount = 1;		
		else
			TargetReadyCount = 4;
		
		timeLeft = levelTimeInMinutes * 60;
//		Debug.Log("Running Start");
				
	}
	
	// Update is called once per frame
	void Update () 
	{
	//	if (currGameManager.level_tester_mode) return;
	
		if (!started) 
		{
			if (currGameManager.gameStarted)
			{
				timeLeft = levelTimeInMinutes * 60;
				started = true;
				
				Debug.Log(currGameManager.selectedClass +" Calling Sync on Join");
				photonView.RPC("SyncOnJoin",PhotonTargets.Others);	
			}
		
			return;
			
		}
		if (alreadyLoading) return;
		if (PlayersHaveReachedEnd && ReadyCount >=TargetReadyCount)
		{
			
			//Now done in a call to GameManagerVik
			/*
			nextLevel += 1; 			
							//last level check
							if (nextLevel > (Application.levelCount - 1)) 
								nextLevel = -1;
							GameManagerVik.nextLevel = nextLevel;
								Debug.Log("nextLevel updated = "+nextLevel);
			*/
			GameManagerVik.checkNextLevel(); //automatic
			nextLevel = GameManagerVik.nextLevel;

			
			alreadyLoading = true;
							
							
					
				//		Playtomic.Log.LevelAverageMetric("Time", 0, Time.timeSinceLevelLoad);
				
				
					
			if (nextLevel > -1)
			{
					Application.LoadLevel(nextLevel);
					//timeLeft = levelTimeInMinutes * 60;
			}
			else
			{
					PhotonNetwork.LeaveRoom();
			}
		}
		
		if (timeLeft <= 0)
		{
			PlayersHaveReachedEnd = true;	
		}
		
		if (Input.GetKeyUp(KeyCode.Space) && PlayersHaveReachedEnd)
		{
				isWaitingForNextStage = true;
				statusText = "waiting for next stage";

				photonView.RPC("callReady",PhotonTargets.AllBuffered);			
		}
	
	}
	
	void FixedUpdate()
	{
		if (!started) return;
		if (isWaitingForNextStage || PlayersHaveReachedEnd) return;
		
		
		if (currGameManager.isPaused() || !currGameManager.gameStarted) return;
		
		lastFrameTime = thisFrameTime;
		thisFrameTime = (int)Time.time;		
		delta = thisFrameTime - lastFrameTime;
	//		Debug.Log(thisFrameTime +" " + lastFrameTime + " " +photonDelta + " " + timeLeft);
		timeLeft -= delta;			
	//	Debug.Log(timeLeft);
	}

	
	 void OnTriggerEnter(Collider other) 
	{			
		if (currGameManager.level_tester_mode)
		{
			PlayersHaveReachedEnd = true;
		}
		
		else
		{
 	   	 if(other.attachedRigidbody.name.Contains("Builder"))
			{
				isBuilderAtEnd =true;	
				int objbuilt = GameManagerVik.objectsBuilt;
				photonView.RPC("updateObjectsBuilt", PhotonTargets.AllBuffered, objbuilt);
			}
		
			if(other.attachedRigidbody.name.Contains("Jumper"))
			{
				isJumperAtEnd =true;
			}	
	
			if(other.attachedRigidbody.name.Contains("Viewer"))
			{
				isViewerAtEnd =true;		
			}
		
			if(other.attachedRigidbody.name.Contains("Mover"))
			{
				isMoverAtEnd =true;
			}
	
			if (isMoverAtEnd && isViewerAtEnd && isJumperAtEnd && isBuilderAtEnd){
				PlayersHaveReachedEnd = true;
				GameManagerVik.startTime = (int)((float)PhotonNetwork.time - GameManagerVik.startTime);
				
				//Send time analytic
				if(other.transform.GetComponent<ThirdPersonNetworkVik>().photonView.isMine)
					collabAnalytics.sendClearTime((int)GameManagerVik.startTime);
				
				//Tally total deaths
				if(!PhotonNetwork.isMasterClient)
					photonView.RPC("tallyTotalDeaths", PhotonTargets.MasterClient, GameManagerVik.deathCount);
			}
		}
	}
		
	[RPC]
	void updateObjectsBuilt(int objBuilt){
		GameManagerVik.objectsBuilt = objBuilt;			
	}
		
	[RPC]
	void tallyTotalDeaths(int deathCount){
		GameManagerVik.deathCount += deathCount;	
		photonView.RPC("updateTotalDeaths", PhotonTargets.OthersBuffered, GameManagerVik.deathCount);	
	}
	
	[RPC]
	void updateTotalDeaths(int totalDeaths){
		GameManagerVik.deathCount = totalDeaths;		
	}
	
	void OnTriggerExit(Collider other) 
	{	
       	//we do not disable localPlayerAtEnd here. 
		if(other.attachedRigidbody.name.Contains("Builder"))
			isBuilderAtEnd =false;
	   	if(other.attachedRigidbody.name.Contains("Jumper"))
			isJumperAtEnd =false;
		if(other.attachedRigidbody.name.Contains("Viewer"))
			isViewerAtEnd =false;
		if(other.attachedRigidbody.name.Contains("Mover"))
			isMoverAtEnd =false;
    }
	
	void OnGUI()
	{
		if (PlayersHaveReachedEnd)
		{	
			GUI.skin = endGameSkin;		
	//		GUI.DrawTexture(new Rect (Screen.width *0.125f, Screen.height *0.125f, Screen.width * 0.75f, Screen.height * 0.75f), aTexture, ScaleMode.StretchToFill);
		
			//Stats here. Note: you might want to stop stat collecting for a given stage when a player first reaches the end point.	
			GUILayout.BeginArea(new Rect(Screen.width * 0.5f - 100, Screen.height * 0.65f, 200, Screen.height * 0.2f));			
	        	GUILayout.Label("Clear Time: " + GameManagerVik.startTime);			
	        	GUILayout.Label("Deaths: " + GameManagerVik.deathCount);			
	        	GUILayout.Label("Total Objects Built: " + GameManagerVik.objectsBuilt);	
			GUILayout.EndArea();
			
			GUI.Label(	new Rect (Screen.width *0.5f - 150, Screen.height *0.8f, 300, Screen.height * 0.1f), statusText);
				
		}	
	
	}

}
