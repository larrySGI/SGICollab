using UnityEngine;
using System.Collections;

public class EndingBoxScript : Photon.MonoBehaviour {
	private bool isBuilderAtEnd;
	private bool isMoverAtEnd;
	private bool isJumperAtEnd;
	private bool isViewerAtEnd;
	
	private int nextLevel;
	private bool alreadyLoading = false;
	
	public Texture aTexture;
	
	
	private string statusText = "";
	
	//This boolean tracks whether a player has reached the end.
	[HideInInspector]	
	public bool PlayersHaveReachedEnd = false;
	public bool isWaitingForNextStage = false;
	
	private GameManagerVik currGameManager = null;
	private ThirdPersonControllerNET currController = null;
	
	private int ReadyCount = 0;
	
	[RPC]
	void callReady()
	{	
		++ReadyCount;
//		Debug.Log(ReadyCount);
	}
	
	
	// Use this for initialization
	void Start () {
		ReadyCount = 0;
		isWaitingForNextStage = PlayersHaveReachedEnd = false;
		
		isBuilderAtEnd = false;
		isMoverAtEnd = false;
		isJumperAtEnd = false;
		isViewerAtEnd = false;
		
		nextLevel = GameManagerVik.nextLevel;
		currGameManager = GameObject.Find("Code").GetComponent<GameManagerVik>();
		currController = GameObject.Find("Code").GetComponent<ThirdPersonControllerNET>();

		//last level check
		if (nextLevel > (Application.levelCount - 1)) 
			nextLevel = -1;
		
		//Debug.Log("nextLevel at start = "+nextLevel);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (currGameManager.level_tester_mode) return;
		if (alreadyLoading) return;
		if (PlayersHaveReachedEnd && ReadyCount >=4)
		{
			
					nextLevel += 1; 			
							//last level check
							if (nextLevel > (Application.levelCount - 1)) 
								nextLevel = -1;
							GameManagerVik.nextLevel = nextLevel;
								Debug.Log("nextLevel updated = "+nextLevel);
					
							alreadyLoading = true;
							
							
					
				//		Playtomic.Log.LevelAverageMetric("Time", 0, Time.timeSinceLevelLoad);
				
				
					
							if (nextLevel > -1)
								Application.LoadLevel(nextLevel);
							else
							{
								PhotonNetwork.LeaveRoom();
							}
		}
	
	
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
			GUI.DrawTexture(new Rect (Screen.width *0.125f, Screen.height *0.125f, Screen.width * 0.75f, Screen.height * 0.75f), aTexture, ScaleMode.StretchToFill);
		
			//Stats here. Note: you might want to stop stat collecting for a given stage when a player first reaches the end point.	
			GUILayout.BeginArea(new Rect(Screen.width * 0.5f - 100, Screen.height * 0.65f, 200, Screen.height * 0.25f));			
	        	GUILayout.Label("Clear Time: " + GameManagerVik.startTime);			
	        	GUILayout.Label("Deaths: " + GameManagerVik.deathCount);			
	        	GUILayout.Label("Total Objects Built: " + GameManagerVik.objectsBuilt);	
			GUILayout.EndArea();
			
			
			if (nextLevel ==  -1)
			{
				if (GUI.Button(new Rect (Screen.width *0.4f, Screen.height *0.8f, Screen.width * 0.25f, Screen.height * 0.1f), "Complete!"))
				{
					PhotonNetwork.LeaveRoom();
				}			
			}
			else
			{
				if (!isWaitingForNextStage)
				{

				if (GUI.Button(new Rect (Screen.width *0.4f, Screen.height *0.8f, Screen.width * 0.25f, Screen.height * 0.1f), "Go To Next Stage"))
				{					
					if (currGameManager.level_tester_mode)
					{
						nextLevel += 1; 			
						//last level check
						if (nextLevel > (Application.levelCount - 1)) 
							nextLevel = -1;
						//s
						GameManagerVik.nextLevel = nextLevel;
			
						ThirdPersonControllerNET.blockammo = ThirdPersonControllerNET.blocksToStart;
						ThirdPersonControllerNET.plankammo = ThirdPersonControllerNET.planksToStart;
						
						if (nextLevel > -1)
							Application.LoadLevel(nextLevel);
						else
						{
							PhotonNetwork.LeaveRoom();
						}
					}
					else
					{
						isWaitingForNextStage = true;
						statusText = "waiting for next stage";

						photonView.RPC("callReady",PhotonTargets.AllBuffered);	
						

						}
					}
				}
			}
			
			
			GUI.Label(	new Rect (Screen.width *0.125f, Screen.height *0.9f, Screen.width * 0.75f, Screen.height * 0.1f), statusText);
				
		}	
	
	}

}
