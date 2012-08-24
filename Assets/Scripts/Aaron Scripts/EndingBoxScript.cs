using UnityEngine;
using System.Collections;

public class EndingBoxScript : MonoBehaviour {
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
	public bool localPlayerHasReachedEnd = false;
	
	private GameManagerVik currGameManager = null;
	private ThirdPersonControllerNET currController = null;
	
	// Use this for initialization
	void Start () {
		
		
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
	void Update () {
	}
	
	
	 void OnTriggerEnter(Collider other) {
			
			string currClass = "";
			if (currGameManager)
				currClass = currGameManager.selectedClass;	
		
		
  	 	    if(other.attachedRigidbody.name.Contains("Builder"))
			{
				isBuilderAtEnd =true;
				if (currClass == "Builder")
				{
					localPlayerHasReachedEnd = true;
					currController.activateMenu();
				}
			}
			
			if(other.attachedRigidbody.name.Contains("Jumper"))
			{
				isJumperAtEnd =true;
				if (currClass == "Jumper")
				{
					localPlayerHasReachedEnd = true;
					currController.activateMenu();
				}
			}	
		
			if(other.attachedRigidbody.name.Contains("Viewer"))
			{
			
				isViewerAtEnd =true;
				if (currClass == "Viewer")
				{
					localPlayerHasReachedEnd = true;
					currController.activateMenu();
				}
	
			}
		
			if(other.attachedRigidbody.name.Contains("Mover"))
			{
				isMoverAtEnd =true;
				if (currClass == "Mover")
				{
					localPlayerHasReachedEnd = true;
					currController.activateMenu();
				}
	
			}
		
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
		if (localPlayerHasReachedEnd)
		{	
			GUI.DrawTexture(new Rect (Screen.width *0.125f, Screen.height *0.125f, Screen.width * 0.75f, Screen.height * 0.75f), aTexture, ScaleMode.StretchToFill);
		
			//Stats here. Note: you might want to stop stat collecting for a given stage when a player first reaches the end point.	
			
			
			if (nextLevel > -1)
			{
				if (GUI.Button(new Rect (Screen.width *0.25f, Screen.height *0.8f, Screen.width * 0.25f, Screen.height * 0.1f), "Complete!"))
				{
					PhotonNetwork.LeaveRoom();
				}
			
			}
			else
			{
			
		
				
				if (GUI.Button(new Rect (Screen.width *0.25f, Screen.height *0.8f, Screen.width * 0.25f, Screen.height * 0.1f), "Go To Next Stage"))
				{
					if (currGameManager.level_tester_mode)
					{
						nextLevel += 1; 			
						//last level check
						if (nextLevel > (Application.levelCount - 1)) 
							nextLevel = -1;
			
						GameManagerVik.nextLevel = nextLevel;
			
						alreadyLoading = true;
						ThirdPersonControllerNET.blockammo = 1;
						ThirdPersonControllerNET.plankammo = 5;
					
					}
					else
					{
						if(isBuilderAtEnd && isMoverAtEnd && isJumperAtEnd && isViewerAtEnd && !alreadyLoading)
						{
							nextLevel += 1; 			
							//last level check
							if (nextLevel > (Application.levelCount - 1)) 
								nextLevel = -1;
							GameManagerVik.nextLevel = nextLevel;
						//		Debug.Log("nextLevel updated = "+nextLevel);
					
							alreadyLoading = true;
							
							ThirdPersonControllerNET.blockammo = 1; 
							ThirdPersonControllerNET.plankammo = 5;
					
				//		Playtomic.Log.LevelAverageMetric("Time", 0, Time.timeSinceLevelLoad);
				
				
					
							if (nextLevel > -1)
								Application.LoadLevel(nextLevel);
							else
							{
								PhotonNetwork.LeaveRoom();
							}
					
						}
						else
						{
							statusText = "You must gather your party before venturing forth.";
						}
					}
				}
			}
			
			if (GUI.Button(new Rect (Screen.width *0.75f, Screen.height *0.8f, Screen.width * 0.25f, Screen.height * 0.1f), "Forgot something..."))
			{
				//shuts off the menu so player can move again. Player must step outside and reenter the exit zone. 
				localPlayerHasReachedEnd = false;
				currController.deactivateMenu();
			}
			
			GUI.Label(	new Rect (Screen.width *0.125f, Screen.height *0.9f, Screen.width * 0.75f, Screen.height * 0.1f), statusText);
				
		}	
	
	}

}
