using UnityEngine;
using System.Collections;

public class triggerFallingScript : MonoBehaviour {
	
	// variables for time counter
	private float startTime;
	private float startFallingTime;
	private float enterTime;
	private float restSeconds;
	private int roundedRestSeconds;
	private float stayTime;
	private float displaySeconds;
	private bool startFalling = false;

	public int countDownSeconds = 5;
	
	// var for falling objects
	public GameObject target;
	private Vector3 startPos;
	private Vector3 endPos;
		
	// Use this for initialization
	void Start () {

		startTime = Time.time;
		startPos = target.transform.position;
		endPos = startPos - new Vector3(0,25,0);
	}
	
	void OnTriggerEnter () {

		enterTime = Time.time;
		//print ("triggerEnter at " + startTime);
	
	}
	
	// on Trigger script, start countdown
	void OnTriggerStay (){
		
		stayTime = Time.time - enterTime;
		
		//print ("triggerStay with " + stayTime);
		
		restSeconds = countDownSeconds - (stayTime);
		roundedRestSeconds = Mathf.CeilToInt(restSeconds);
		displaySeconds = roundedRestSeconds % 60;

		//print ("rest:" + displaySeconds);
		
		if (displaySeconds <= 0){
			startFalling = true;
			startFallingTime = Time.time;
		}

	}
	
	// on trigger exit, reset countdown timer
	void OnTriggerExit(){
	
		//print ("exit");
	
	}
	
	
	// Update is called once per frame
	void Update () {
		if (startFalling){
			target.transform.position = Vector3.Lerp(startPos,endPos, Time.time-startTime);	
			
			// after falling, set timer to return bridge to original position
			stayTime = Time.time - startFallingTime;
					
			restSeconds = countDownSeconds - (stayTime);
			roundedRestSeconds = Mathf.CeilToInt(restSeconds);
			displaySeconds = roundedRestSeconds % 60;
				
			if (displaySeconds <= 0){
				startFalling = false;
				target.transform.position = startPos;
			}
		}
	}
}
