using UnityEngine;
using System.Collections;

public class GameManagerVik : Photon.MonoBehaviour
{
	
	//added by larry
	//added by hh
    // this is a object name (must be in any Resources folder) of the prefab to spawn as player avatar.
    // read the documentation for info how to spawn dynamically loaded game objects at runtime (not using Resources folders)
	private bool menuOn = false;
	
	
    public string playerPrefabName = "Charprefab";
	public string builderPrefabName = "Builder";
	public string jumperPrefabName = "Jumper";
	public string moverPrefabName = "Mover";
	public string viewerPrefabName = "Viewer";
    public string selectedClass;
	
	public bool gameStarted = false;
	
	public bool level_tester_mode = false;
	
	public int playerCount = 0;
	
	public Texture aTexture;	
	
	public int serverLevel = -1;	
	public static int nextLevel = 1;
	
	
	void Awake(){

		if (!level_tester_mode)
			DontDestroyOnLoad(this);
	}
	void OnJoinedRoom()
    {
		//StartGame(this.jumperPrefabName);
		//selectedClass = this.jumperPrefabName;
        //StartGame(this.moverPrefabName);
		//selectedClass = this.moverPrefabName;
		//StartGame(this.builderPrefabName);
		//selectedClass = this.builderPrefabName;
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
	void greetServer(int levelIndex, PhotonMessageInfo info){
		print("Greeting server");
		if(serverLevel < 0){
			serverLevel = levelIndex;
			print("Set server level"); 
		}
		else if(serverLevel != levelIndex){
			photonView.RPC("syncToServerLevel", PhotonTargets.Others, serverLevel);
			print("Level index adjusted");
		}	
		else{
			print("Level index correct");
			print("Level index = " + levelIndex);		
		}
	}
	
	[RPC]
	void syncToServerLevel(int levelIndex, PhotonMessageInfo info){
		serverLevel = levelIndex;
		print("Synced server level");
		
		print("Synced server = " + serverLevel);
	}
	
    void StartGame(string prefabName)
    {        
		print("BEFORE Greeted server");
		print("BEFORE server = " + serverLevel);
		print("BEFORE nextlevel = " + nextLevel);
		//Sync to server's level
		
		photonView.RPC("greetServer", PhotonTargets.MasterClient, nextLevel);
		
		print("AFTER Greeted server");
		print("AFTER server = " + serverLevel);
//			while(nextLevel != serverLevel){
//				print("level not synced yet");
//			}
		
		Camera.main.farClipPlane = 1000; //Main menu set this to 0.4 for a nicer BG    

        //prepare instantiation data for the viking: Randomly disable the axe and/or shield
       
		
		bool[] enabledRenderers = new bool[2];
        enabledRenderers[0] = false;//Axe
        enabledRenderers[1] = false; //Shield
        
        object[] objs = new object[1]; // Put our bool data in an object array, to send
        objs[0] = enabledRenderers;
		// print ("starting game");
        // Spawn our local player
		if(prefabName=="Mover")
		{
			PhotonNetwork.Instantiate(prefabName, transform.position+transform.right,transform.rotation, 0, objs);
		}
		if(prefabName=="Jumper")
		{
        	PhotonNetwork.Instantiate(prefabName, transform.position+transform.right*2, transform.rotation, 0, objs);
		
		}
		if(prefabName=="Builder")
		{
			PhotonNetwork.Instantiate(prefabName, transform.position+transform.right*3, transform.rotation, 0, objs);
		
		}
		if(prefabName=="Viewer")
		{
        	PhotonNetwork.Instantiate(prefabName, transform.position+transform.right*4, transform.rotation, 0, objs);
		
		}
		//spawn network synced objects
		//PhotonNetwork.Instantiate("checkPointTriggerLift", transform.position+transform.right*10, transform.rotation, 0);
		//PhotonNetwork.Instantiate("liftPrefab", transform.position+transform.right*15, transform.rotation, 0);
		gameStarted = true;

		if (level_tester_mode) 
			Time.timeScale = 1;
		else		
			Time.timeScale = 0;
	}
	
	void Update()
	{
		if (level_tester_mode) return;
		
		
		
		if (GameObject.FindWithTag("Viewer") && 
			GameObject.FindWithTag("Mover") &&
			GameObject.FindWithTag("Builder") &&
			GameObject.FindWithTag("Jumper"))

		{
			Time.timeScale = 1;
		}
		else
		{
			Time.timeScale = 0;
			return; //premature return on scale 0. 
		}
		
		
		
		//replace with main menu logic kthx
		if(Application.loadedLevel == 0)
		{
			if(serverLevel > 0){
				print("loading server level = " +serverLevel);
				Application.LoadLevel(serverLevel);			
			}
		}
		
		
		if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.Escape))
		{
			menuOn = !menuOn;
		}
	}



    void OnGUI()
    {
		if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room
		
		if (selectedClass == "")
		{
			
			GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
			if(!GameObject.FindWithTag("Builder")){
			if (GUILayout.Button("Join as Builder"))
     	 	{
       		 	StartGame(this.builderPrefabName);
				selectedClass = this.builderPrefabName;
        	}
			}
			if(!GameObject.FindWithTag("Viewer")){
			if (GUILayout.Button("Join as Viewer"))
 	       {
 				StartGame(this.viewerPrefabName);
				selectedClass = this.viewerPrefabName;
    	    }
			}
			if(!GameObject.FindWithTag("Mover")){
			if (GUILayout.Button("Join as Mover"))
  		    {
 				StartGame(this.moverPrefabName);
				selectedClass = this.moverPrefabName;
			}
			}
			if(!GameObject.FindWithTag("Jumper")){
			if (GUILayout.Button("Join as Jumper"))
	   	    {
      		    StartGame(this.jumperPrefabName);
				selectedClass = this.jumperPrefabName;
        	}
			}
			 if (GUILayout.Button("Leave& QUIT"))
	        {
        	    PhotonNetwork.LeaveRoom();
				selectedClass = "";
		    }

			GUILayout.EndArea();
		}
		else //already have a player type
		{
			//Need to add the Level Tester Mode on. Apparently if we don't have a Room because we're in Level Tester Mode, OnGUI only works once!
			if(Time.timeScale==0 && !level_tester_mode)
			{
				GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height), aTexture, ScaleMode.StretchToFill);
			}
			if (GUILayout.Button("Leave& QUIT"))
	       	{
					ChatVik.SP.AnnounceLeave();
				
					PhotonNetwork.LeaveRoom();
					selectedClass = "";
					gameStarted = false;			
        	}
	
			GUILayout.Label("You are now a " + selectedClass);
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
		nextLevel = level;
	}
  
}
