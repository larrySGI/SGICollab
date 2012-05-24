using UnityEngine;
using System.Collections;

public class EndingBoxScript : MonoBehaviour {
	private bool isBuilderAtEnd;
	private bool isMoverAtEnd;
	private bool isJumperAtEnd;
	private bool isViewerAtEnd;
	
	// Use this for initialization
	void Start () {
		isBuilderAtEnd = false;
		isMoverAtEnd = false;
		isJumperAtEnd = false;
		isViewerAtEnd = false;
	}
	
	// Update is called once per frame
	void Update () {
	if(isBuilderAtEnd && isMoverAtEnd && isJumperAtEnd && isViewerAtEnd)
		{
		print("game ends");	
		}
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

}
