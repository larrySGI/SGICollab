using UnityEngine;
using System.Collections;
//using System.Net;
//using System.IO;

public enum menuState {splash, login, signup, profile};

public class MainMenuVik : Photon.MonoBehaviour
{	
    private menuState currentMenuState;
	public static bool userTally = false;
	public static int maxLevelData = 1;
	
	void Start()
	{
		
		
	}
	
    void Awake()
    {
		//this will keep the code object from being destroyed
		
 		// added after push
        //PhotonNetwork.logLevel = NetworkLogLevel.Full;
		Playtomic.Initialize(428042, "077e33f2d4704abd", "15d6d5a5dd864d97a38851a7541448");
		Playtomic.Log.View();
        //Connect to the main photon server. This is the only IP and port we ever need to set(!)
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v1.0"); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)

        //Load name from PlayerPrefs
        PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));

        //Set camera clipping for nicer "main menu" background
        //Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
		//StartCoroutine(Example());
		maxLevelData = (Application.levelCount - 1);
		currentMenuState = menuState.login;
//		GameObject codeObj = GameObject.Find("Code");
//		if (codeObj)
//		{		
//			if (codeObj.GetComponent<GameManagerVik>().level_tester_mode)
//			{
//				//bypass login
//				currentMenuState = menuState.profile;
//			}
//			else
//			{
//						currentMenuState = menuState.login;
//			}
//		}
//		else
//			currentMenuState = menuState.login;
    }

    private string roomName = "myRoom";
    private Vector2 scrollPos = Vector2.zero;
	private string emailInput = "";
	private string email2Input = "hotmail";
	private string email3Input = ""; //email1 + email2 combined xxx@hotmail.com
	private string nickInput = "";
	private string pass1Input = "";
	private string pass2Input = "";
	private int levelSelected = 1; //awake should give me level 1 from the beginning
	
	private bool initState = false;
	
	
    void OnGUI()
    {
//		if(currentMenuState == menuState.splash  || !initState){
//			//print ("stuck");
//			if(initState)
//				return;
//			print ("stuck2");
//			StartCoroutine(Example());
//			initState = true;
//			print ("initstate is true");
//			return;
//		}
		
        if (!PhotonNetwork.connected)
        {
            ShowConnectingGUI();
            return;   //Wait for a connection
        }

        if (PhotonNetwork.room != null)
            return; //Only when we're not in a Room
		
		switch (currentMenuState) {
			//case menuState.splash:
			//	StartCoroutine(ShowSplash());
			//	break;
			case menuState.login:
				//if(!initState){
					StartCoroutine(ShowLoginGUI());
				//	initState = true;
				//}
				break;
			case menuState.signup:
				//if(!initState){
					StartCoroutine(ShowSignupGUI());
				//	initState = true;
				//}
				break;
			case menuState.profile:
				ShowProfileGUI();
				break;
		}
    }
	IEnumerator Example() {
        print("Before = "+Time.time);
        yield return new WaitForSeconds(2);
        print("After = " + Time.time);
    }
	
	void changeStateTo(menuState nextState){
		//initState = false;
		currentMenuState = nextState;
	}

    void ShowConnectingGUI()
    {
        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

        GUILayout.Label("Connecting to Photon server.");
        GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");

        GUILayout.EndArea();
    }
	
	IEnumerator ShowSplash(){
		print("befiore");
		//	while(!initState){
		//yield return new WaitForSeconds(2);
		
			//print ("waaaad");
		//yield return StartCoroutine(letsWait(2.0f));		
		StartCoroutine("letsWait", 2.0f);		
		yield return new WaitForSeconds(2);
		StopCoroutine("letsWait");
		//	initState = true;
		//}
		print("after");
		changeStateTo(menuState.login);
		
	}
	
	IEnumerator letsWait (float seconds) {
        yield return new WaitForSeconds(seconds);
    }
	
	//IEnumerator because needs yield return to check for errors in login
	IEnumerator ShowLoginGUI(){
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		
			GUILayout.Label("Log In");
		
			//Player name input
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("Nickname:", GUILayout.Width(150));
		        PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
		        if (GUI.changed)
		            PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
	        GUILayout.EndHorizontal();
		
	        GUILayout.Space(15);
		
			//Player password input
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("Password:", GUILayout.Width(150));
		        pass1Input = GUILayout.TextField(pass1Input);
	        GUILayout.EndHorizontal();
		
	        GUILayout.Space(20);
		
			//Log in or create new account
	        GUILayout.BeginHorizontal();		
		        if (GUILayout.Button("Create New Account", GUILayout.Width(200)))
		        {
					//StartCoroutine(UserDatabase.getData("qq", "qq", "email")); 		tested getData() works, delete this testing line soon
					pass1Input = "";
					currentMenuState = menuState.signup;
		        }		
		        if (GUILayout.Button("GO", GUILayout.Width(200)))
		        {
					yield return StartCoroutine(UserDatabase.login(PhotonNetwork.playerName, pass1Input));
					if(userTally)
						currentMenuState = menuState.profile;
		        }
	        GUILayout.EndHorizontal();
        GUILayout.EndArea();
	}
	
	//IEnumerator because needs yield return to check for errors in creating account
	IEnumerator ShowSignupGUI(){
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		
			GUILayout.Label("Sign Up");
		
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("Email:", GUILayout.Width(150));
		        emailInput = GUILayout.TextField(emailInput);
		        GUILayout.Label("@", GUILayout.Width(20));
		        email2Input = GUILayout.TextField(email2Input);
		        GUILayout.Label(".com");
	        GUILayout.EndHorizontal();
		
	        GUILayout.Space(15);
		
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("Nickname:", GUILayout.Width(150));
		        nickInput = GUILayout.TextField(nickInput);
	        GUILayout.EndHorizontal();
		
	        GUILayout.Space(15);
		
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("Password:", GUILayout.Width(150));
		        pass1Input = GUILayout.TextField(pass1Input);
	        GUILayout.EndHorizontal();
		
	        GUILayout.Space(15);
		
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("Confirm Password:", GUILayout.Width(150));
		        pass2Input = GUILayout.TextField(pass2Input);
	        GUILayout.EndHorizontal();
		
	        GUILayout.Space(20);
		
	        GUILayout.BeginHorizontal();	
				//GUILayout.Width(120);		
		        if (GUILayout.Button("Create!", GUILayout.Width(200)))
		        {
					if(pass1Input != pass2Input){
						print("Passwords do not match!");
					}
					else{	
						email3Input = emailInput + "@"+ email2Input + ".com";
						yield return StartCoroutine(UserDatabase.signUp(email3Input, nickInput, pass1Input));
						if(userTally){
							PhotonNetwork.playerName = nickInput;
							currentMenuState = menuState.profile;
						}
					}
		        }	
		        if (GUILayout.Button("Cancel", GUILayout.Width(200))){
		      			pass1Input = "";
						pass2Input = "";
						currentMenuState = menuState.login;
				}
	        GUILayout.EndHorizontal();
        GUILayout.EndArea();
	}
	
	
	void ShowProfileGUI(){
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		
	        //Player name
	        GUILayout.Label("Welcome back, " + PhotonNetwork.playerName);
	        GUILayout.Label("Max level reached: " + maxLevelData);
	
	        GUILayout.Space(30);	
	
	        //Join room by title
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("JOIN ROOM:", GUILayout.Width(150));
		        roomName = GUILayout.TextField(roomName);
		        if (GUILayout.Button("GO"))
		        {
					Playtomic.Log.Play();
		            PhotonNetwork.JoinRoom(roomName);
		        }
	        GUILayout.EndHorizontal();
	
	        //Join random room
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("JOIN RANDOM ROOM:", GUILayout.Width(150));
		        if (PhotonNetwork.GetRoomList().Length == 0)
		        {
		            GUILayout.Label("..no games available...");
		        }
		        else
		        {
		            if (GUILayout.Button("GO"))
		            {
						Playtomic.Log.Play();
		                PhotonNetwork.JoinRandomRoom();
		            }
		        }
	        GUILayout.EndHorizontal();
		
	        //Create a room (fails if exist!)
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("CREATE ROOM:", GUILayout.Width(150));
		        roomName = GUILayout.TextField(roomName);
	        GUILayout.EndHorizontal();
		
	        GUILayout.BeginHorizontal();
				GUILayout.Space(150);
				if(GUILayout.Button("<<", GUILayout.Width(30))){
					levelSelected--;
					if(levelSelected < 1)
						levelSelected = 1;
				}
				GUILayout.Label("Level " + levelSelected.ToString(), GUILayout.Width(50));
				if(GUILayout.Button(">>", GUILayout.Width(30))){
					levelSelected++;
					if(levelSelected > maxLevelData)
						levelSelected = maxLevelData;
					//Debug.Log(Application.levelCount);
					if (levelSelected > (Application.levelCount))
						levelSelected = Application.levelCount;
				}
		
		        if (GUILayout.Button("GO"))
		        {
					
			
					GameManagerVik.setNextLevel(levelSelected); //+1 because 0 is set aside for menu, tutorial is 1, and levels from 2 onwards
					Playtomic.Log.Play();
					//set number of players to 4. - Larry
		            PhotonNetwork.CreateRoom(roomName, true, true, 4);
					//ChatVik.createdLevelIndex = levelSelected + 1;
					//photonView.RPC("syncServerLevel", PhotonTargets.All, levelSelected + 1);
		        }
	        GUILayout.EndHorizontal();
		
	        GUILayout.Space(30);
		
	        GUILayout.Label("ROOM LISTING:");
	        if (PhotonNetwork.GetRoomList().Length == 0)
	        {
	            GUILayout.Label("..no games available..");
	        }
	        else
	        {
	            //Room listing: simply call GetRoomList: no need to fetch/poll whatever!
	            scrollPos = GUILayout.BeginScrollView(scrollPos);
	            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
	            {
	                GUILayout.BeginHorizontal();
		                GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
		                if (GUILayout.Button("JOIN"))
		                {
							GameManagerVik.setNextLevel(levelSelected + 1); //+1 because 0 is set aside for menu, tutorial is 1, and levels from 2 onwards
							Playtomic.Log.Play();
		                    PhotonNetwork.JoinRoom(game.name);
		                }
	                GUILayout.EndHorizontal();
	            }
	            GUILayout.EndScrollView();
	        }

        GUILayout.EndArea();
	}
	
	
	void ShowOldGUI(){
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

	        GUILayout.Label("Main Menu");
	
	        //Player name
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("Player name:", GUILayout.Width(150));
		        GUILayout.TextField(PhotonNetwork.playerName);
		        if (GUI.changed)//Save name
		            PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
	        GUILayout.EndHorizontal();
	
	        GUILayout.Space(15);	
	
	        //Join room by title
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("JOIN ROOM:", GUILayout.Width(150));
		        roomName = GUILayout.TextField(roomName);
		        if (GUILayout.Button("GO"))
		        {
					Playtomic.Log.Play();
		            PhotonNetwork.JoinRoom(roomName);
		        }
	        GUILayout.EndHorizontal();
	
	        //Create a room (fails if exist!)
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("CREATE ROOM:", GUILayout.Width(150));
		        roomName = GUILayout.TextField(roomName);
		        if (GUILayout.Button("GO"))
		        {
					Playtomic.Log.Play();
					//set number of players to 4. - Larry
		            PhotonNetwork.CreateRoom(roomName, true, true, 4);
		        }
	        GUILayout.EndHorizontal();
	
	        //Join random room
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("JOIN RANDOM ROOM:", GUILayout.Width(150));
		        if (PhotonNetwork.GetRoomList().Length == 0)
		        {
		            GUILayout.Label("..no games available...");
		        }
		        else
		        {
		            if (GUILayout.Button("GO"))
		            {
						Playtomic.Log.Play();
		                PhotonNetwork.JoinRandomRoom();
		            }
		        }
	        GUILayout.EndHorizontal();
	
	        GUILayout.Space(30);
	        GUILayout.Label("ROOM LISTING:");
	        if (PhotonNetwork.GetRoomList().Length == 0)
	        {
	            GUILayout.Label("..no games available..");
	        }
	        else
	        {
	            //Room listing: simply call GetRoomList: no need to fetch/poll whatever!
	            scrollPos = GUILayout.BeginScrollView(scrollPos);
	            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
	            {
	                GUILayout.BeginHorizontal();
		                GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
		                if (GUILayout.Button("JOIN"))
		                {
							Playtomic.Log.Play();
		                    PhotonNetwork.JoinRoom(game.name);
		                }
	                GUILayout.EndHorizontal();
	            }
	            GUILayout.EndScrollView();
	        }

        GUILayout.EndArea();
	}
}
