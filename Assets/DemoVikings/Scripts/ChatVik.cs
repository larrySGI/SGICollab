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

    public static ChatVik SP;
    public List<string> messages = new List<string>();

    private int chatHeight = (int)140;
    private Vector2 scrollPos = Vector2.zero;
    private string chatInput = "";
    private float lastUnfocusTime = 0;
	
	private GameManagerVik manager;
	
	private string chatterClass = "";
	private bool joined = false;
	
	private string path; //path to save chat log to
	private string fileName; //file name for the chat log

	void Start()
	{
	
	
	}
	
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
	
	public void Reset()
	{
		SP = this;
	}

    void OnGUI()
    {        
      //  Debug.Log(SP == null);
		
		if (!manager.gameStarted) return;
		
		GUI.SetNextControlName("");

        GUILayout.BeginArea(new Rect(0, Screen.height - chatHeight, Screen.width, chatHeight));
        
        //Show scroll list of chat messages
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUI.color = Color.red;
        for (int i = messages.Count - 1; i >= 0; i--)
        {
            GUILayout.Label(messages[i]);
        }
        GUILayout.EndScrollView();
        GUI.color = Color.white;

        //Chat input
        GUILayout.BeginHorizontal(); 
        GUI.SetNextControlName("ChatField");
    	chatInput = GUILayout.TextField(chatInput, GUILayout.MinWidth(200));
       
        if (Event.current.type == EventType.keyDown && Event.current.character == '\n')
		{
         	Debug.Log("This should work");
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

        //if (GUILayout.Button("SEND", GUILayout.Height(17)))
         //   SendChat(PhotonTargets.All);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

   		GUILayout.EndArea();
		

		
		
    }

    public static void AddMessage(string text)
    {
        SP.messages.Add(text);
        if (SP.messages.Count > 15)
            SP.messages.RemoveAt(0);
    }


    [RPC]
    void SendChatMessage(string text, PhotonMessageInfo info)
    {
        string communication = "[" + info.sender + "] " + text;
		
		AddMessage(communication);
				
		using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
		{
			AddToChatLog(fs, communication + Environment.NewLine);	
			fs.Close();
			fs.Dispose();
		}
    }
	

	public void AnnounceJoin()
	{
		string communication =  "("+chatterClass +") has joined the game.";
		if (photonView)
		photonView.RPC("SendChatMessage", PhotonTargets.All, communication);
	}
	
	public void AnnounceLeave()
	{
		string communication =  "("+chatterClass +") has left the game.";
		if (photonView)
		photonView.RPC("SendChatMessage", PhotonTargets.All, communication);
	
	}
	
	
    void SendChat(PhotonTargets target)
    {
        if (chatInput != "")
        {
            photonView.RPC("SendChatMessage", target, "("+chatterClass+") " +chatInput);
            chatInput = "";
        }
    }

    void SendChat(PhotonPlayer target)
    {
        if (chatInput != "")
        {
            chatInput = "[PM] " + chatInput;
            photonView.RPC("SendChatMessage", target, "("+chatterClass+") " +chatInput);
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
				Debug.Log(chatterClass);
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
