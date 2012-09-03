using UnityEngine;
using System.Collections;

public class NecroGUI : MonoBehaviour {
	//The custom skin
	public GUISkin mySkin;
	
	//Boolean to display each window
	public static bool connectWindow;
	public static bool loginWindow;
	public static bool signupWindow;
	public static bool lobbyWindow;
	public static bool roleSelectWindow;
	public static bool pauseWindow;
	
	//Predetermined window sizes
	private Rect connectWindowRect;
	private Rect loginWindowRect;
	private Rect signupWindowRect;
	private Rect lobbyWindowRect;
	private Rect roleSelectWindowRect;
	private Rect pauseWindowRect;
	
	//Custom GUI
	private float leafOffset;
	private float frameOffset;
	private float skullOffset;
	
	private float RibbonOffsetX;
	private float FrameOffsetX;
	private float SkullOffsetX;
	private float RibbonOffsetY;
	private float FrameOffsetY;
	private float SkullOffsetY;
	
	private float WSwaxOffsetX;
	private float WSwaxOffsetY;
	private float WSribbonOffsetX;
	private float WSribbonOffsetY;
		
	private int spikeCount;	
	
	string[] allRoomsCapacity = new string[20];
	
	//Not used yet	
//	private Vector2 scrollPosition;
//	private float HroizSliderValue = 0.5f;
//	private float VertSliderValue = 0.5f;
//	private bool ToggleBTN = false;
	
	
	void Awake(){
		connectWindowRect = new Rect (Screen.width * 0.2f, Screen.height * 0.5f, Screen.width * 0.6f, Screen.height * 0.5f);
		loginWindowRect = new Rect (Screen.width * 0.2f, Screen.height * 0.4f, Screen.width * 0.6f, Screen.height * 0.6f);
		signupWindowRect = new Rect (Screen.width * 0.2f, Screen.height * 0.2f, Screen.width * 0.6f, Screen.height * 0.8f);
		lobbyWindowRect = new Rect (Screen.width * 0.2f, Screen.height * 0.2f, Screen.width * 0.6f, Screen.height * 0.8f);
		roleSelectWindowRect = new Rect (Screen.width * 0.2f, Screen.height * 0.4f, Screen.width * 0.6f, Screen.height * 0.6f);
		
		pauseWindowRect = new Rect (Screen.width * 0.25f, Screen.height * 0.4f, Screen.width * 0.5f, Screen.height * 0.5f);
//		print(Screen.width);
//		print(Screen.height);
	}
	
	
	void AddSpikes(float windowWidth)
	{
		spikeCount = (int)Mathf.Floor(windowWidth - 152)/22;
		GUILayout.BeginHorizontal();
		GUILayout.Label ("", "SpikeLeft");//-------------------------------- custom
		for (int i = 0; i < spikeCount; i++)
	        {
				GUILayout.Label ("", "SpikeMid");//-------------------------------- custom
	        }
		GUILayout.Label ("", "SpikeRight");//-------------------------------- custom
		GUILayout.EndHorizontal();
	}
	
	
	void FancyTop(float windowWidth)
	{
		leafOffset = (windowWidth/2)-64;
		frameOffset = (windowWidth/2)-27;
		skullOffset = (windowWidth/2)-20;
		GUI.Label(new Rect(leafOffset, 18, 0, 0), "", "GoldLeaf");//-------------------------------- custom	
		GUI.Label(new Rect(frameOffset, 3, 0, 0), "", "IconFrame");//-------------------------------- custom	
		GUI.Label(new Rect(skullOffset, 12, 0, 0), "", "Skull");//-------------------------------- custom	
	}
	
	
	void WaxSeal(float windowWidth, float windowHeight)
	{
		WSwaxOffsetX = windowWidth - 120;
		WSwaxOffsetY = windowHeight - 115;
		WSribbonOffsetX = windowWidth - 114;
		WSribbonOffsetY = windowHeight - 83;
		
		GUI.Label(new Rect(WSribbonOffsetX, WSribbonOffsetY, 0, 0), "", "RibbonBlue");//-------------------------------- custom	
		GUI.Label(new Rect(WSwaxOffsetX, WSwaxOffsetY, 0, 0), "", "WaxSeal");//-------------------------------- custom	
	}
	
	
	void DeathBadge(float xPos, float yPos)
	{
		RibbonOffsetX = xPos;
		FrameOffsetX = xPos+3;
		SkullOffsetX = xPos+10;
		RibbonOffsetY = yPos+22;
		FrameOffsetY = yPos;
		SkullOffsetY = yPos+9;
		
		GUI.Label(new Rect(RibbonOffsetX, RibbonOffsetY, 0, 0), "", "RibbonRed");//-------------------------------- custom	
		GUI.Label(new Rect(FrameOffsetX, FrameOffsetY, 0, 0), "", "IconFrame");//-------------------------------- custom	
		GUI.Label(new Rect(SkullOffsetX, SkullOffsetY, 0, 0), "", "Skull");//-------------------------------- custom	
	}
	
	
	void drawConnectWindow(int windowID){
		// use the spike function to add the spikes
		AddSpikes(connectWindowRect.width);
				
        GUILayout.Label("Connecting to Photon server...", "PlainText");
        GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.", "PlainText");
	}
	
	
	void drawLoginWindow(int windowID){				
		// use the spike function to add the spikes
		AddSpikes(loginWindowRect.width);
		
		GUILayout.BeginVertical();
		GUILayout.Label("Log In");
		GUILayout.Label ("", "Divider");
        GUILayout.Space(5);
	
        GUILayout.BeginHorizontal();
		//Player name input
    	GUILayout.Label("Username", GUILayout.Width(Screen.width * 0.2f));
        PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
        if (GUI.changed)
            PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
        GUILayout.EndHorizontal();	
        GUILayout.Space(15);
	
		//Player password input
        GUILayout.BeginHorizontal();
        GUILayout.Label("Password", GUILayout.Width(Screen.width * 0.2f));		
        MainMenuVik.pass1Input = GUILayout.PasswordField(MainMenuVik.pass1Input, "*"[0], 15);
        GUILayout.EndHorizontal();	
        GUILayout.Space(20);
	
		//Log in or create new account
        GUILayout.BeginHorizontal();		
        if (GUILayout.Button("Create New Account", GUILayout.Width(Screen.width * 0.22f))){			
			MainMenuVik.pass1Input = "";
			MainMenuVik.currentMenuState = menuState.signup;
        }		

        if ((Event.current.type == EventType.KeyDown && Event.current.character == '\n') 
				|| GUILayout.Button("GO", GUILayout.Width(Screen.width * 0.22f))){
			UserDatabase.login(PhotonNetwork.playerName, MainMenuVik.pass1Input);
			MainMenuVik.currentMenuState = menuState.profile;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
		
		GUI.DragWindow (new Rect (0,0,10000,10000));
	}
	
	
	void drawSignupWindow(int windowID){		
		// use the spike function to add the spikes
		AddSpikes(signupWindowRect.width);
				
		GUILayout.Label("Create Account");
		GUILayout.Label ("", "Divider");
	
        GUILayout.BeginHorizontal();
        GUILayout.Label("Email", GUILayout.Width(Screen.width * 0.2f));
        MainMenuVik.emailInput = GUILayout.TextField(MainMenuVik.emailInput, GUILayout.MinWidth(Screen.width * 0.05f), GUILayout.MaxWidth(Screen.width * 0.13f));
        GUILayout.Label("@", "PlainText", GUILayout.Width(Screen.width * 0.02f));
        MainMenuVik.email2Input = GUILayout.TextField(MainMenuVik.email2Input, GUILayout.MaxWidth(Screen.width * 0.1f));
        GUILayout.Label(".com", "PlainText");
        GUILayout.EndHorizontal();	
        GUILayout.Space(15);
	
        GUILayout.BeginHorizontal();
        GUILayout.Label("Nickname", GUILayout.Width(Screen.width * 0.2f));
        MainMenuVik.nickInput = GUILayout.TextField(MainMenuVik.nickInput);
        GUILayout.EndHorizontal();	
        GUILayout.Space(15);
	
        GUILayout.BeginHorizontal();
        GUILayout.Label("Password", GUILayout.Width(Screen.width * 0.2f));
        MainMenuVik.pass1Input = GUILayout.PasswordField(MainMenuVik.pass1Input, "*"[0], 15);
        GUILayout.EndHorizontal();	
        GUILayout.Space(15);
	
        GUILayout.BeginHorizontal();
	        GUILayout.Label("Confirm Password", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.1f));
	        MainMenuVik.pass2Input = GUILayout.PasswordField(MainMenuVik.pass2Input, "*"[0], 15);
        GUILayout.EndHorizontal();	
        GUILayout.Space(20);
	
        GUILayout.BeginHorizontal();		
        if (GUILayout.Button("Create!", GUILayout.Width(Screen.width * 0.25f)))
        {
			if(MainMenuVik.pass1Input != MainMenuVik.pass2Input){
				print("Passwords do not match!");
			}
			else{	
				MainMenuVik.email3Input = MainMenuVik.emailInput + "@"+ MainMenuVik.email2Input + ".com";
				UserDatabase.signUp(MainMenuVik.email3Input, MainMenuVik.nickInput, MainMenuVik.pass1Input);
				PhotonNetwork.playerName = MainMenuVik.nickInput;
				MainMenuVik.levelSelected = MainMenuVik.maxLevelData;
				MainMenuVik.currentMenuState = menuState.profile;
			}
        }	
        if (GUILayout.Button("Cancel", GUILayout.Width(Screen.width * 0.25f))){
      			MainMenuVik.pass1Input = "";
				MainMenuVik.pass2Input = "";
				MainMenuVik.currentMenuState = menuState.login;
		}
        GUILayout.EndHorizontal();
		
		GUI.DragWindow (new Rect (0,0,10000,10000));
	}
	
	
	void drawLobbyWindow(int windowID){
		// use the spike function to add the spikes
		AddSpikes(lobbyWindowRect.width);
					
		//Player name
        GUILayout.BeginHorizontal();
//        GUILayout.Space(Screen.width * 0.1f);        
        GUILayout.Label("Welcome back, " + PhotonNetwork.playerName, GUILayout.Height(Screen.height * 0.1f));
		GUILayout.EndHorizontal();
		
		//Player's latest stage
//        GUILayout.Space(Screen.width * 0.1f);
        GUILayout.Label("Max level reached: " + MainMenuVik.maxLevelData, "ShortLabel");
        //GUILayout.Space(Screen.width * 0.1f);		
        GUILayout.BeginHorizontal();
		GUILayout.Label("Select a level");	
		if(GUILayout.Button("<<")){
			MainMenuVik.levelSelected--;
			if(MainMenuVik.levelSelected < 1)
				MainMenuVik.levelSelected = 1;
		}		
		GUILayout.Label("Level " + MainMenuVik.levelSelected.ToString(), "LegendaryText");
		if(GUILayout.Button(">>")){
			MainMenuVik.levelSelected++;
			if(MainMenuVik.levelSelected > MainMenuVik.maxLevelData)
				MainMenuVik.levelSelected = MainMenuVik.maxLevelData;
		}
		GUILayout.EndHorizontal();		
        GUILayout.Space(10);
		GUILayout.Label("", "Divider");
		GameManagerVik.setNextLevel(MainMenuVik.levelSelected);
	
        //Rooms list
	    GUILayout.Label("ROOMS LIST", GUILayout.Height(Screen.height * 0.1f));
		MainMenuVik.scrollPos = GUILayout.BeginScrollView(MainMenuVik.scrollPos);
		string[] roomNames = updateAllRoomsNames();
		
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		for(int x = 0;x < 10; x++){
			if(GUILayout.Button(roomNames[x] + allRoomsCapacity[x], GUILayout.Width(Screen.width * 0.2f))){
				attemptEnterRoom(roomNames[x]);
			}
		}
		GUILayout.EndVertical();		
		GUILayout.BeginVertical();	
		for(int x = 10;x < 20; x++){
			if(GUILayout.Button(roomNames[x] + allRoomsCapacity[x], GUILayout.Width(Screen.width * 0.2f))){
				attemptEnterRoom(roomNames[x]);
			}
		}	
		GUILayout.EndVertical();	
		GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
		
		GUI.DragWindow (new Rect (0,0,10000,10000));
	}
	
	
	void attemptEnterRoom(string roomName){
		Playtomic.Log.Play();
        foreach (RoomInfo game in PhotonNetwork.GetRoomList())
        {
			if(game.name == roomName){
				if(game.playerCount < game.maxPlayers){
	            	PhotonNetwork.JoinRoom(roomName);
					MainMenuVik.currentMenuState = menuState.roleSelect;
					Debug.Log("JOINED = " + game.name);
				}
				else{
					//tell user its full maybe
				}
				return;
			}
		}	
	    PhotonNetwork.CreateRoom(roomName, true, true, 4);
		Debug.Log("CREATED = " + roomName);
		MainMenuVik.currentMenuState = menuState.roleSelect;
	}
	
	
	string[] updateAllRoomsNames(){
		string[] allRoomsNames = new string[20];
		for(int x = 0; x < 20; x++){
			allRoomsNames[x] = "Room " + (x + 1);
			allRoomsCapacity[x] = "  0/4";
	        foreach (RoomInfo game in PhotonNetwork.GetRoomList())
	        {
				if(game.name != allRoomsNames[x])
					continue;
				
				allRoomsCapacity[x] = "  " + game.playerCount + "/" + game.maxPlayers;
				break;
			}
		}
		return allRoomsNames;
	}
	
	
	void drawRoleSelectWindow(int windowID){
		// use the spike function to add the spikes
		AddSpikes(lobbyWindowRect.width);		
		
		GUILayout.Label("Choose a role");
		GUILayout.Label("", "Divider");
		GUILayout.Space(15);
		
		if(!GameObject.FindWithTag("Builder")){
			if (GUILayout.Button("Builder")){
				GetComponent<GameManagerVik>().selectedClass = GameManagerVik.builderPrefabName;
       		 	GetComponent<GameManagerVik>().StartGame(GameManagerVik.builderPrefabName);
        	}
		}
		if(!GameObject.FindWithTag("Viewer")){
			if (GUILayout.Button("Viewer")){
				GetComponent<GameManagerVik>().selectedClass = GameManagerVik.viewerPrefabName;
 				GetComponent<GameManagerVik>().StartGame(GameManagerVik.viewerPrefabName);
    	    }
		}
		if(!GameObject.FindWithTag("Mover")){
			if (GUILayout.Button("Mover")){
				GetComponent<GameManagerVik>().selectedClass = GameManagerVik.moverPrefabName;
 				GetComponent<GameManagerVik>().StartGame(GameManagerVik.moverPrefabName);
			}
		}
		if(!GameObject.FindWithTag("Jumper")){
			if (GUILayout.Button("Jumper")){
				GetComponent<GameManagerVik>().selectedClass = GameManagerVik.jumperPrefabName;
      		    GetComponent<GameManagerVik>().StartGame(GameManagerVik.jumperPrefabName);
        	}
		}
		if (GUILayout.Button("QUIT")){
    	    PhotonNetwork.LeaveRoom();
			GetComponent<GameManagerVik>().selectedClass = "";
			MainMenuVik.currentMenuState = menuState.profile;
	    }
		
		GUI.DragWindow (new Rect (0,0,10000,10000));
	}
	
	
	void drawPauseWindow(int windowID){
		// use the spike function to add the spikes
		AddSpikes(signupWindowRect.width);
		
		GUILayout.Label("Menu");
		GUILayout.Label("", "Divider");
		GUILayout.Space(15);
				
        if (GUILayout.Button("Retry")){
			Debug.Log("RETRY");
			GetComponent<GameManagerVik>().retry();
			pauseWindow = false;
		}
        if (GUILayout.Button("QUIT")){
			Debug.Log("QUIT");
			GetComponent<GameManagerVik>().quitGame();
			pauseWindow = false;
		}
		
		GUI.DragWindow (new Rect (0,0,10000,10000));		
	}
	
	
	void OnGUI(){
		GUI.skin = mySkin;
		
		if (connectWindow)
			connectWindowRect = GUI.Window (0, connectWindowRect, drawConnectWindow, "");
		
		if (loginWindow)
			loginWindowRect = GUI.Window (1, loginWindowRect, drawLoginWindow, "");
		
		if (signupWindow)
			signupWindowRect = GUI.Window (2, signupWindowRect, drawSignupWindow, "");
		
		if (lobbyWindow)
			lobbyWindowRect = GUI.Window (3, lobbyWindowRect, drawLobbyWindow, "");
		
		if (roleSelectWindow)
			roleSelectWindowRect = GUI.Window (4, roleSelectWindowRect, drawRoleSelectWindow, "");
		
		if (pauseWindow)
			pauseWindowRect = GUI.Window (5, pauseWindowRect, drawPauseWindow, "");
		
//			//now adjust to the group. (0,0) is the topleft corner of the group.
//			GUI.BeginGroup (new Rect (0,0,100,100));
//			// End the group we started above. This is very important to remember!
//			GUI.EndGroup ();
	}
}
