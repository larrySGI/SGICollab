using UnityEngine;
using System.Collections;


/// <summary>
/// This script is attached to each player and ensures that each player's movements are updated across the network
/// </summary>



public class LiftUpdate : Photon.MonoBehaviour {
	
	public Texture viewerTexture;
	
	// Use this for initialization
	void Awake () 
	{
			
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		
		GameObject SpawnManager = GameObject.Find("Code");
		GameManagerVik MoverTest = SpawnManager.GetComponent<GameManagerVik>();
		
		if(MoverTest.selectedClass == "Viewer")
		{
	
			this.renderer.material.mainTexture = viewerTexture;
		}
		
	}
	/*
	void FixedUpdate()
	{
			//If the player has moved at all then fire an RPC to update across the network
		liftLastPosition = liftThisPosition;
		liftThisPosition = transform.position.y;
	}
	
	

	void OnTriggerStay(Collider other)
	{
		//Debug.Log("Stay Triggered");
		
		other.transform.position = new Vector3 (other.transform.position.x,
												other.transform.position.y + (liftThisPosition - liftLastPosition),
												other.transform.position.z);
	
	}
	
	*/
}
