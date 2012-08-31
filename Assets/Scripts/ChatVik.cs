using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

/// <summary>
/// This simple chat example showcases the use of RPC targets and targetting certain players via RPCs.
/// </summary>
public class ChatVik : Photon.MonoBehaviour
{
	public GUISkin chatSkin;
    public static ChatVik SP;
    public List<string> messages = new List<string>();
	public List<Color> messageColor = new List<Color>();
	
    private int chatHeight = 160;
    private Vector2 scrollPos = Vector2.zero;
    private string chatInput = "";
    private float lastUnfocusTime = 0;
	float inputAreaY = -(Screen.height * 0.4f);
	
	private GameManagerVik manager;
	
	private string chatterClass = "";
	private bool joined = false;
	
	private string path; //path to save chat log to
	private string fileName; //file name for the chat log
	
	//public Texture2D chatBG;
	
    void Awake()
    {
       	GameObject SpawnManager = GameObject.Find("Code");
		manager = SpawnManager.GetComponent<GameManagerVik>();
		
		
		var dt = DateTime.Now;
		var timeStamp = String.Format("{0:hh-mm-ss-yyyy-MM-dd}", dt);
		
		//Access the initial chatlog to see if it is empty
		path = Application.dataPath;
		fileName = "/ChatLog " + timeStamp + ".txt";	
		path = Application.dataPath +fileName;
		
		SP = this;
    }
	
	void Start()
	{
	}
	
	public void Reset()
	{
		SP = this;
	}

    void OnGUI()
    {        
      //  Debug.Log(SP == null);
		GUI.skin = chatSkin;
		if (!manager.gameStarted) return;
	
		
		//Chat log area
		GUI.SetNextControlName("");		
	//	GUI.DrawTexture(new Rect(Screen.width * 0.05f, Screen.height * 0.65f, Screen.width * 0.4f , Screen.height * 0.3f), chatBG);
	  	GUI.Box(new Rect(Screen.width * 0.05f, Screen.height * 0.65f, Screen.width * 0.4f , Screen.height * 0.3f), "");
      		
        GUILayout.BeginArea(new Rect(Screen.width * 0.05f, Screen.height * 0.65f, Screen.width * 0.4f, Screen.height * 0.3f));
	        //Show scroll list of chat messages
			
			//note: do NOT scroll to chatheight - use scrollpos to mathf infinity instead to go to the very bottom. 
			scrollPos = new Vector2(0, Mathf.Infinity);
	        scrollPos = GUILayout.BeginScrollView(scrollPos); //no scrollbars!
		
	        for (int i = 0; i < messages.Count; i++)
	        {
	            GUI.color = messageColor[i];
				GUILayout.Label(messages[i]);
			
	        }
	        GUILayout.EndScrollView();
	        GUI.color = Color.white;		

   		GUILayout.EndArea();			
		
		
		//Chat input
		GUILayout.Space(inputAreaY);
        GUILayout.BeginHorizontal(); 
	        GUI.SetNextControlName("ChatField");
			GUILayout.Space(Screen.width * 0.5f - 100); //keep your min and max the same! 
	    	chatInput = GUILayout.TextField(chatInput, GUILayout.MinWidth(200), GUILayout.MaxWidth(200));
		
	        if (Event.current.type == EventType.KeyDown && Event.current.character == '\n')
			{
				//My way of hiding the GUI
				Input.ResetInputAxes();
				inputAreaY *= -1;
				
	         	if (GUI.GetNameOfFocusedControl() == "ChatField")
	            {                
	                SendChat(PhotonTargets.All);
	                lastUnfocusTime = Time.time;
	                GUI.FocusControl("");
	               	GUI.UnfocusWindow();
	            }
	            else
	            {
	                if (lastUnfocusTime < Time.time - 0.1f)
	                {
	                    GUI.FocusControl("ChatField");
					
	                }
	            }
	        }		
	        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public static void AddMessage(string text, string incomingchatterclass)
	{
        SP.messages.Add(text);
		
		if (incomingchatterclass == "Builder")
			SP.messageColor.Add(Color.white);
		
		if (incomingchatterclass == "Viewer")
			SP.messageColor.Add(Color.green);

		if (incomingchatterclass == "Jumper")
			SP.messageColor.Add(Color.cyan);

		if (incomingchatterclass == "Mover")
			SP.messageColor.Add(Color.yellow);

        if (SP.messages.Count > 5)
		{
            SP.messages.RemoveAt(0);
			SP.messageColor.RemoveAt(0);
		}
    }


    [RPC]
    void SendChatMessage(string text, string incomingchatter, PhotonMessageInfo info)
    {		
        string communication = "[" + info.sender + "]" + text;
		
		AddMessage(communication, incomingchatter);
				
		using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
		{
			AddToChatLog(fs, communication + Environment.NewLine);	
			fs.Close();
			fs.Dispose();
		}
    }
	

	public void AnnounceJoin()
	{
		string communication =  "<"+chatterClass +"> has joined the game.";
		if (photonView)
		photonView.RPC("SendChatMessage", PhotonTargets.All, communication, chatterClass);
	}
	
	public void AnnounceLeave()
	{
		string communication =  "<"+chatterClass +"> has left the game.";
		if (photonView)
		photonView.RPC("SendChatMessage", PhotonTargets.All, communication, chatterClass);
	
	}
	
	
    void SendChat(PhotonTargets target)
    {
        if (chatInput != "")
        {
			//Send to other players
			if (photonView)
            photonView.RPC("SendChatMessage", target, "<"+chatterClass+">: " +chatInput, chatterClass);
			
			//Send to online database
			StartCoroutine(saveChatToOnlineDatabase());
			
			//Empty input
            chatInput = "";
        }
    }
	
	IEnumerator saveChatToOnlineDatabase(){
		if(collabAnalytics.sendEnabled){
			string token = UserDatabase.token;
			string level = GameManagerVik.nextLevel.ToString();
			
			string url = "http://sgicollab.herokuapp.com/chat" +
							"?auth_token=" + token +
							"&chat[message]=" + chatInput +
							"&chat[level]=" + level +
							"&chat[userclass]=" + chatterClass;
			
			var r = new HTTP.Request ("POST", url);
			r.Send ();		
			Debug.Log("Chat saved online.");				
		}
		yield return null;
	}
	
    void SendChat(PhotonPlayer target)
    {
        if (chatInput != "")
        {
            chatInput = "[PM] " + chatInput;
            photonView.RPC("SendChatMessage", target, "<"+chatterClass+">: " +chatInput, chatterClass);
            chatInput = "";
        }
    }
	
    void OnLeftRoom()
    {
       	
		this.enabled = false;
    }

    void OnJoinedRoom()
    {
        this.enabled = true;
	   
		
    }
    void OnCreatedRoom()
    {
        this.enabled = true;
    }
	
	void Update()
	{
		if (!joined)
		{
			
			chatterClass = manager.selectedClass;
			if (chatterClass != "" && manager.gameStarted)
			{
				joined = true;
			 	AnnounceJoin();
			}		
		}
	}
		
	private static void AddToChatLog(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }
}

