using UnityEngine;
using System.Collections;

public class InstructionScript : MonoBehaviour {
	bool tutorialHidden=false;
	bool level1Hidden=false;
	bool level2Hidden=false;
	
	
	// Use this for initialization
	void OnGUI () {
		if(Application.loadedLevelName == "Tutorial" && !tutorialHidden)
		if (GUI.Button (new Rect (0,0,Screen.width,Screen.height), "Instructions for Tutorial")) {
			tutorialHidden = true;
		}
		if(Application.loadedLevelName == "Level1" && !level1Hidden)
		if (GUI.Button (new Rect (0,0,Screen.width,Screen.height), "Instructions for level 1")) {
			level1Hidden = true;
		}
		if(Application.loadedLevelName == "Level2" && !level2Hidden)
		if (GUI.Button (new Rect (0,0,Screen.width,Screen.height), "Instructions for level 2")) {
			level2Hidden = true;
		}
		
	}
}
