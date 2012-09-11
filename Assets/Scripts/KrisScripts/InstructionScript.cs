using UnityEngine;
using System.Collections;

public class InstructionScript : Photon.MonoBehaviour {
	public string InstructionText;
	public GUISkin guiskin;
	private bool showInstruction=false;
	private float instructionScreenHeight;
	
	void OnTriggerEnter(Collider other) {
		if(other.transform.GetComponent<ThirdPersonNetworkVik>().photonView.isMine)//if user is current user
		{
			showInstruction=true;//show instruction
			
			instructionScreenHeight=0;
		}
	}
	void OnTriggerExit(Collider other) {
		if(other.transform.GetComponent<ThirdPersonNetworkVik>().photonView.isMine)//if user is current user
		{
			showInstruction=false;//hide instruction
		}		
    }
	
	void Awake()
    {
    }
	
    void Start()
    {
		
	}
	
	void Update ()
	{
	   
	}
	
	// Use this for initialization
	void OnGUI () {
		
		if(showInstruction)
		{
			GUI.skin = guiskin;
			if(instructionScreenHeight<Screen.height){
				instructionScreenHeight=instructionScreenHeight+(0.1f*Screen.height);
				
			}
			
			var tButtonRect = new Rect(0, 0, Screen.width, instructionScreenHeight);
			
 			 GUI.Button(tButtonRect, InstructionText);
		}else{
			GUI.skin = guiskin;
			if(instructionScreenHeight>0){
				instructionScreenHeight=instructionScreenHeight-(0.1f*Screen.height);
				
			}
			
			var tButtonRect = new Rect(0, 0, Screen.width, instructionScreenHeight);
			 GUI.Button(tButtonRect, InstructionText);
		}
		
	}
}

	

