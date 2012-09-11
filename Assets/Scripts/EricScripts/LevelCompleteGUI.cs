using UnityEngine;
using System.Collections;

public class LevelCompleteGUI : MonoBehaviour {
	//The custom skin
	public GUISkin mySkin;
	public Texture star, starNull;
		
	//Predetermined window size
	Rect windowRect;
	
	//Other variables
	public static bool showWindow;
	static bool showStars;
	public int oneStarBenchmark, twoStarBenchmark, threeStarBenchmark, fourStarBenchmark, fiveStarBenchmark;
	public int starsGrade;
	
	public static string statusText = "";
	
	
	// Use this for initialization
	void Awake () {
		windowRect = new Rect (Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.8f, Screen.height * 0.8f);		
		statusText = "Press [Spacebar] to proceed";
	}
	
	
	public void countStarsGrade(int timeLeft, int deaths, int gemsCollected){
		int grade = timeLeft - deaths * 30 + gemsCollected * 100;
		
		if(grade >= fiveStarBenchmark)		
			starsGrade = 5;
		else if(grade >= fourStarBenchmark)		
			starsGrade = 4;
		else if(grade >= threeStarBenchmark)		
			starsGrade = 3;
		else if(grade >= twoStarBenchmark)		
			starsGrade = 2;
		else if(grade >= oneStarBenchmark)		
			starsGrade = 1;
		else
			starsGrade = 0;
		
		Debug.Log("starsGrade = " + starsGrade);
		
		//Send score analytic
		if(PhotonNetwork.isMasterClient)
			collabAnalytics.sendScoreFactorData(EndingBoxScript.clearTime, EndingBoxScript.completeStatus, starsGrade);
		
		showStars = true;
	}
	
	
	void drawWindow(int windowID){
		GUI.DragWindow (new Rect (0,0,10000,10000));
		GUILayout.Space(30);
		
		GUILayout.Label("LEVEL COMPLETE!", "LegendaryText");	
		if(showStars){
			for(int x = 0; x < 5; x++){
				if(x < starsGrade)
					GUI.DrawTexture(new Rect(windowRect.x + windowRect.width * 0.075f + windowRect.width * 0.125f * x, windowRect.y + windowRect.height * 0.25f, 
											windowRect.width * 0.1f, windowRect.height * 0.125f), star);		
				else
					GUI.DrawTexture(new Rect(windowRect.x + windowRect.width * 0.075f + windowRect.width * 0.125f * x, windowRect.y + windowRect.height * 0.25f, 
											windowRect.width * 0.1f, windowRect.height * 0.125f), starNull);	
			}
		}
		
		GUILayout.Space(90);
		GUILayout.BeginArea(new Rect(windowRect.x + windowRect.width * 0.1f, windowRect.y + windowRect.height * 0.4f, 
										windowRect.width *  0.6f, windowRect.height * 0.4f));
		
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUILayout.Label("Clear Time", "PlainText");
		GUILayout.Label("Total Deaths", "PlainText");
		GUILayout.Label("Gems Found", "PlainText");
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical();
		Hashtable stats = EndingBoxScript.clearLevelScore;
		if(stats["Clear Time"].ToString() == "Failed")
			GUILayout.Label(stats["Clear Time"].ToString(), "CursedText");
		else
			GUILayout.Label(stats["Clear Time"].ToString(), "PlainText");
		GUILayout.Label(stats["Total Deaths"].ToString(), "PlainText");
		GUILayout.Label(GameManagerVik.gemsCollected + "/3", "PlainText");
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Label(statusText, "BoldOutlineText");
		GUILayout.EndArea();
	}
	
	
	void OnGUI(){
		GUI.skin = mySkin;
		
		if(showWindow)
			GUILayout.Window(0, windowRect, drawWindow, "");
	}
	
}
