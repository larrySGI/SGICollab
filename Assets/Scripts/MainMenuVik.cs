using UnityEngine;
using System.Collections;
//using System.Net;
//using System.IO;

public enum menuState {none, splash, login, signup, profile, roleSelect};

public class MainMenuVik : Photon.MonoBehaviour
{	
    public static menuState currentMenuState;
    private menuState lastMenuState;
	private bool endFrame = false;
//	public static bool userTally = false;
//	public static int maxLevelData = 5;
		
    public static string roomName = "Room 01";
    public static Vector2 scrollPos = Vector2.zero;
	public static string emailInput = "";
	public static string email2Input = "hotmail";
	public static string email3Input = ""; //email1 + email2 combined xxx@hotmail.com
	public static string nickInput = "";
	public static string pass1Input = "";
	public static string pass2Input = "";
	public static int levelSelected = 1; //awake should give me level 1 from the beginning
	
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
		
		currentMenuState = menuState.login;
    }
	
	void Start()
	{
	}
	
    void OnGUI()
    {
		
        if (!PhotonNetwork.connected)
        {
			NecroGUI.connectWindow = true;
            return;   //Wait for a connection
        }
		else
			NecroGUI.connectWindow = false;
				
//        if (PhotonNetwork.room != null)
//            return; //Only when we're not in a Room
		
		//Stop redrawing GUI if state is going to change
		if(lastMenuState != currentMenuState){
			endFrame = false;
			StartCoroutine(wait());
			lastMenuState = currentMenuState;
			Debug.Log("State changing");
		}
		if(!endFrame || currentMenuState == menuState.none)
			return;
		
		//Draw different GUI depending on state
		switch (currentMenuState) {
			case menuState.login:
				NecroGUI.loginWindow = true;
				break;
			case menuState.signup:
				NecroGUI.signupWindow = true;
				break;
			case menuState.profile:
				NecroGUI.lobbyWindow = true;
				break;
			case menuState.roleSelect:
				NecroGUI.roleSelectWindow = true;
				break;
		}		
    }
	
	IEnumerator wait(){
		yield return new WaitForEndOfFrame();
		NecroGUI.connectWindow = false;
		NecroGUI.loginWindow = false;
		NecroGUI.signupWindow = false;
		NecroGUI.lobbyWindow = false;
		NecroGUI.roleSelectWindow = false;
		NecroGUI.pauseWindow = false;
		endFrame = true;
		Debug.Log("all falsed");
	}
}
