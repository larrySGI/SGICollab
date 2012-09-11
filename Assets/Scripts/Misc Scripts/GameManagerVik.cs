using UnityEngine;
using System.Collections;

public class GameManagerVik : Photon.MonoBehaviour
{	
    public static string playerPrefabName = "Charprefab";
	public static string builderPrefabName = "Builder";
	public static string jumperPrefabName = "Jumper";
	public static string moverPrefabName = "Mover";
	public static string viewerPrefabName = "Viewer";
    public string selectedClass;
		
	public bool gameStarted = false;	
	public bool level_tester_mode = false;
	public bool sendAnalytics = true;
	public static bool builderConnected, moverConnected, viewerConnected, jumperConnected;
	
	public int playerCount = 0;
	public static int maxStageReached = 5;
	
	public Texture aTexture;	
		
	public static int nextLevel = 0;	
	public static string gameID;
	bool syncedGameID;
	
	public static double startTime;
	public static int deathCount, gemsCollected;
	
	public GUISkin inGameSkin;
	
	
	void Awake(){
		DontDestroyOnLoad(this);
	}
	
	void OnJoinedRoom()
    {
		Debug.Log("on joined room");
		photonView.RPC("checkAccountValidity", PhotonTargets.AllBuffered);	
		
		if(!PhotonNetwork.isMasterClient){
			photonView.RPC("retrieveLevelFromMaster", PhotonTargets.MasterClient);			
		}else{
			gameID = GetComponent<UserDatabase>().getGameID();
			if(gameID == UserDatabase.failedResponse)
				PhotonNetwork.LeaveRoom();
		}
		
		
    }
	
    
    IEnumerator OnLeftRoom()
    {
        //Easy way to reset the level: Otherwise we'd manually reset the camera
		
        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while(PhotonNetwork.room!=null || PhotonNetwork.connected==false)
            yield return 0;

        if (level_tester_mode)
			Application.LoadLevel(Application.loadedLevel);
		else
		{		
		//destroy the code object here! Otherwise when we go back to main we'll actually *duplicate* it, which is what we don't want because
		//it messes up the main menu.
			Destroy(GameObject.Find("Code"));
		
		//kick the user back to the MainMenu. (Might wanna put something in that level)
			Application.LoadLevel(0);
		}
    }
		
	[RPC]
	void retrieveLevelFromMaster( PhotonMessageInfo info){
		photonView.RPC("syncLevelLocally",info.sender, nextLevel);	
	}
	[RPC]
	void checkAccountValidity(PhotonMessageInfo info){
		Debug.Log("checking validity...");
		StartCoroutine(UserDatabase.verifyUser()) ;
		
	}
	[RPC]
	void syncLevelLocally(int levelIndex, PhotonMessageInfo info){
		nextLevel = levelIndex;
		print("Synced level = " + nextLevel);		
	}
	
	[RPC]
	void syncGameIDLocally(string serverID, PhotonMessageInfo info){	
		gameID = serverID;
		
		if(gameID == "Failed" || gameID == "" || gameID == null){
			Debug.Log("game id = "+gameID);
			PhotonNetwork.LeaveRoom();
			
		}else{
			if(!PhotonNetwork.isMasterClient)
			UserDatabase.verifyGameID(gameID);
			
			print("Synced gameID = " + gameID);	
		}
	}
	
	
	void OnLevelWasLoaded(int level)  
	{		
		//Set tracked variables to default
		deathCount = 0;
		startTime = PhotonNetwork.time;
		gemsCollected = 0;
	}
		
    public void StartGame(string prefabName)
    {    		
		MainMenuVik.currentMenuState = menuState.none;
		Camera.main.farClipPlane = 1000; //Main menu set this to 0.4 for a nicer BG    

        //prepare instantiation data for the viking: Randomly disable the axe and/or shield
		bool[] enabledRenderers = new bool[2];
        enabledRenderers[0] = false;//Axe
        enabledRenderers[1] = false; //Shield
        
        object[] objs = new object[1]; // Put our bool data in an object array, to send
        objs[0] = enabledRenderers;
		
        // Spawn our local player
		if(prefabName=="Mover")
			PhotonNetwork.Instantiate(prefabName, transform.position+transform.right*5,transform.rotation, 0, objs);
		
		if(prefabName=="Jumper")
			PhotonNetwork.Instantiate(prefabName, transform.position+transform.right*2, transform.rotation, 0, objs);
		
		if(prefabName=="Builder")
			PhotonNetwork.Instantiate(prefabName, transform.position+transform.right*3, transform.rotation, 0, objs);
			
		if(prefabName=="Viewer")
			PhotonNetwork.Instantiate(prefabName, transform.position+transform.right*4, transform.rotation, 0, objs);	
		
		
		gameStarted = true;

		if (level_tester_mode) 
			Time.timeScale = 1;
		else		
			Time.timeScale = 0;
	}
	
	void Update()
	{
		if (level_tester_mode) return;
		
		if(GameObject.FindWithTag("Builder"))
			builderConnected = true;
		else
			builderConnected = false;
		
		if(GameObject.FindWithTag("Mover"))
			moverConnected = true;
		else
			moverConnected = false;
		
		if(GameObject.FindWithTag("Viewer"))
			viewerConnected = true;
		else
			viewerConnected = false;
		
		if(GameObject.FindWithTag("Jumper"))
			jumperConnected = true;
		else
			jumperConnected = false;
		
		
		if (!builderConnected || !moverConnected || 
				!viewerConnected || !jumperConnected)
			
			timeControl();
		
		else{			
			//Sync game ID
			if(!syncedGameID){
				if(PhotonNetwork.isMasterClient){
					
					photonView.RPC("syncGameIDLocally", PhotonTargets.AllBuffered, gameID);	
					
				}
				syncedGameID = true;				
			}
						
			//Begin load level			
			if(Application.loadedLevel == 0){
				if(nextLevel > 0)
				{
					print("loading server level = " +nextLevel);
					Application.LoadLevel(nextLevel);
				    
					startTime = (float)PhotonNetwork.time;
				}
			}
			else
				Time.timeScale = 1;
		}
	}
	
	void timeControl(){
		Time.timeScale = 0;
		if(MainMenuVik.currentMenuState != menuState.none )
			Time.timeScale = 1; //premature return on scale 0. 
	}
	
	
	public bool isPaused()
	{
		return (Time.timeScale == 0);	
	}
	
    void OnGUI()
    {
		if (PhotonNetwork.room == null)  return;
		if (selectedClass != "")
		{		
			//Need to add the Level Tester Mode on. Apparently if we don't have a Room because we're in Level Tester Mode, OnGUI only works once!
			GUI.skin = inGameSkin;
			
			
			if(Time.timeScale==0 && !level_tester_mode)
			{
				GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height), aTexture, ScaleMode.StretchToFill);
				if(!builderConnected)
					GUILayout.Label("Builder not connected!");
				if(!moverConnected)
					GUILayout.Label("Mover not connected!");
				if(!viewerConnected)
					GUILayout.Label("Viewer not connected!");
				if(!jumperConnected)
					GUILayout.Label("Jumper not connected!");
				
				return;
			}
			
			GUILayout.BeginVertical();
			GUILayout.Space(Screen.height * 0.05f);
			
			GUILayout.BeginHorizontal();
				
				GUILayout.Space(Screen.width * 0.05f);
				GUILayout.BeginVertical();
			
					if (GameObject.Find("EndingBoundBox") != null)
					{
	
						int timeleft = GameObject.Find("EndingBoundBox").GetComponent<EndingBoxScript>().timeLeft;
						int minutes = timeleft/60;
						int seconds = timeleft - (minutes * 60);
				
				
						GUILayout.Label("Time remaining : " + minutes +":" +seconds );
					}
				
					GUILayout.Label("Current Room :" + NecroGUI.joinedRoomName);
					GUILayout.Label("Stage Number :" + Application.loadedLevel);
					GUILayout.Label("Gems Found :" + gemsCollected + "/3");			
					GUILayout.BeginHorizontal();
				
						GUILayout.Label("You are now a " + selectedClass);
						GUILayout.Space(Screen.width * 0.5f);
				
						if(selectedClass == "Builder")
						{
						       
						    GUILayout.BeginVertical();
								GUILayout.Label("Block Ammo: " + ThirdPersonControllerNET.blockammo);	
						   		GUILayout.Label("Plank Ammo: " + ThirdPersonControllerNET.plankammo);
				    
					      	GUILayout.EndVertical();
				
				   
						}
			
						if(selectedClass == "Viewer")
						{
							string camIndex = ThirdPersonCameraNET.currCameraIndex.ToString();
							if(camIndex == "0")
								camIndex = "Main";
				       		 GUILayout.Label("Camera: " + camIndex);			

						}

				GUILayout.EndHorizontal();	
			GUILayout.EndVertical();			
			GUILayout.EndVertical();
			
		}
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }
    void OnFailedToConnectToPhoton()
    {
        Debug.LogWarning("OnFailedToConnectToPhoton");
    }
	
	public static void setNextLevel(int level){
	//	if (GameObject.Find("Code").GetComponent<GameManagerVik>().level_tester_mode) nextLevel = Application.loadedLevel;
	//	else
			nextLevel = level;				
	}  
	
	public static void checkNextLevel()
	{		
		nextLevel += 1; 			
							//last level check
		if (nextLevel > (Application.levelCount - 1)) 
					nextLevel = -1;
	//	Debug.Log("nextLevel updated = "+nextLevel);
	}
	
	public static int getNextLevel(){
		return nextLevel;
	}
	
	
	public void retry(){
		GameObject.FindWithTag(selectedClass).GetComponent<ThirdPersonControllerNET>().menuOn = false;
		GameObject.FindWithTag(selectedClass).GetComponent<ThirdPersonControllerNET>().Retry();
		Screen.lockCursor = true;
	}
	
	
	public void quitGame(){
		Screen.lockCursor = false;
		ChatVik.SP.AnnounceLeave();	
		PhotonNetwork.LeaveRoom();
		selectedClass = "";
		gameStarted = false;
	}
}
