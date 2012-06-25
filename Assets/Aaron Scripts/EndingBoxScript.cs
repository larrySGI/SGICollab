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
	
	// Use this for initialization
	void Start () {
		isBuilderAtEnd = false;
		isMoverAtEnd = false;
		isJumperAtEnd = false;
		isViewerAtEnd = false;
		
		nextLevel = GameManagerVik.nextLevel;
		
		//last level check
		if (nextLevel > (Application.levelCount - 1)) 
			nextLevel = -1;
		
		//Debug.Log("nextLevel at start = "+nextLevel);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	
	 void OnTriggerEnter(Collider other) {
       	if(other.attachedRigidbody.name.Contains("Builder"))
		isBuilderAtEnd =true;
	   	if(other.attachedRigidbody.name.Contains("Jumper"))
		isJumperAtEnd =true;
		if(other.attachedRigidbody.name.Contains("Viewer"))
		isViewerAtEnd =true;
		if(other.attachedRigidbody.name.Contains("Mover"))
		isMoverAtEnd =true;
		
	}
	void OnTriggerExit(Collider other) {
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
		if(isBuilderAtEnd && isMoverAtEnd && isJumperAtEnd && isViewerAtEnd && !alreadyLoading)
		{
			nextLevel += 1; 			
			//last level check
			if (nextLevel > (Application.levelCount - 1)) 
				nextLevel = -1;
			GameManagerVik.nextLevel = nextLevel;
			Debug.Log("nextLevel updated = "+nextLevel);
			
			alreadyLoading = true;
			
			ThirdPersonControllerNET.blockammo = 1;
			ThirdPersonControllerNET.plankammo = 5;
			
			Playtomic.Log.LevelAverageMetric("Time", 0, Time.timeSinceLevelLoad);
			GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height), aTexture, ScaleMode.StretchToFill);
			
			if (nextLevel > -1)
				Application.LoadLevel(nextLevel);
			else
			{
				PhotonNetwork.LeaveRoom();
			}
		}
	
	}

}
