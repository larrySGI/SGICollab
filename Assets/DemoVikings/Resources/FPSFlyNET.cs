using UnityEngine;
using System.Collections;
[RequireComponent (typeof (CharacterController))]
public class FPSFlyNET : MonoBehaviour {
	private GameManagerVik MoverTest;
	private Vector3 moveDirection = Vector3.zero;
	private float speed = 6.0F;
	// Use this for initialization
	void Start () {
	GameObject SpawnManager = GameObject.Find("Code");
	MoverTest = SpawnManager.GetComponent<GameManagerVik>();
	if(MoverTest.selectedClass == "Spectator"){
	Destroy(GameObject.Find("Main Camera"));
	this.camera.enabled = true;
	DontDestroyOnLoad(this);	
		}
	}
	
	// Update is called once per frame
	void Update () {
	CharacterController controller = this.GetComponent<CharacterController>();
	if(Input.GetKey("w")) 
		controller.Move(transform.forward*speed* Time.deltaTime);
	
	if(Input.GetKey("s")) 
		controller.Move(-transform.forward*speed* Time.deltaTime);
	}
}
