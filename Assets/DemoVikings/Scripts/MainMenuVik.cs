using UnityEngine;
using System.Collections;

public enum menuState {login, signup, profile};

public class MainMenuVik : Photon.MonoBehaviour
{	
    private menuState currentMenuState;
	
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
        Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
		
		currentMenuState = menuState.login;
    }

    private string roomName = "myRoom";
    private Vector2 scrollPos = Vector2.zero;
	
	
    void OnGUI()
    {
        if (!PhotonNetwork.connected)
        {
            ShowConnectingGUI();
            return;   //Wait for a connection
        }

        if (PhotonNetwork.room != null)
            return; //Only when we're not in a Room
		
		switch (currentMenuState){
			case menuState.login:
				ShowLoginGUI();
				break;
			case menuState.signup:
				ShowSignupGUI();
				break;
			case menuState.profile:
				ShowOldGUI();
				break;
		}
    }


    void ShowConnectingGUI()
    {
        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

        GUILayout.Label("Connecting to Photon server.");
        GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");

        GUILayout.EndArea();
    }
	
	
	void ShowLoginGUI(){
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		
			GUILayout.Label("Log In");
		
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("Username:", GUILayout.Width(150));
		        PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
		        if (GUI.changed)
		            PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
	        GUILayout.EndHorizontal();
		
	        GUILayout.Space(15);
		
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("Password:", GUILayout.Width(150));
		        GUILayout.TextField("");
	        GUILayout.EndHorizontal();
		
	        GUILayout.Space(20);
		
	        GUILayout.BeginHorizontal();		
		        if (GUILayout.Button("Create New Account", GUILayout.Width(200)))
		        {
					currentMenuState = menuState.signup;
		        }		
		        if (GUILayout.Button("GO", GUILayout.Width(200)))
		        {
					currentMenuState = menuState.profile;
		        }
	        GUILayout.EndHorizontal();
        GUILayout.EndArea();
	}
	
	
	void ShowSignupGUI(){
	}
	
	
	void ShowProfileGUI(){
		Playtomic.Log.Play();
        PhotonNetwork.JoinRoom(roomName);
	}
	
	
	void ShowOldGUI(){
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

	        GUILayout.Label("Main Menu");
	
	        //Player name
	        GUILayout.BeginHorizontal();
		        GUILayout.Label("Player name:", GUILayout.Width(150));
		        PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
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
		            PhotonNetwork.CreateRoom(roomName, true, true, 10);
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
