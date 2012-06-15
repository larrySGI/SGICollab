using UnityEngine;
using System.Collections;

public class NewLevelScript : MonoBehaviour {
	public GameObject BuilderSpawnPoint;
	public GameObject JumperSpawnPoint;
	public GameObject MoverSpawnPoint;
	public GameObject ViewerSpawnPoint;
	
	private GameObject builderObj;
	private GameObject moverObj;
	private GameObject jumperObj;
	private GameObject viewerObj;
	
	private GameManagerVik manager;
	
	// Use this for initialization
	void Start () 
	{
		
		GameObject SpawnManager = GameObject.Find("Code");
		
		//move the code to the spawn manager's position
		SpawnManager.transform.position = transform.position;
		SpawnManager.transform.rotation = transform.rotation;
		
		manager = SpawnManager.GetComponent<GameManagerVik>();

		if(builderObj = GameObject.FindGameObjectWithTag("Builder"))
		{
			builderObj.transform.position = BuilderSpawnPoint.transform.position;
			ThirdPersonCameraNET cam = builderObj.GetComponent<ThirdPersonCameraNET>();
			cam.LoadCameras();	
	
		}	
		if(moverObj = GameObject.FindGameObjectWithTag("Mover"))
		{	
			moverObj.transform.position = MoverSpawnPoint.transform.position;
			ThirdPersonCameraNET cam = moverObj.GetComponent<ThirdPersonCameraNET>();
			cam.LoadCameras();	
	
		}	
		if(jumperObj = GameObject.FindGameObjectWithTag("Jumper"))
		{
			jumperObj.transform.position = JumperSpawnPoint.transform.position;
			ThirdPersonCameraNET cam = jumperObj.GetComponent<ThirdPersonCameraNET>();
			cam.LoadCameras();	
	
		}
		if(viewerObj = GameObject.FindGameObjectWithTag("Viewer"))
		{
				
			viewerObj.transform.position = ViewerSpawnPoint.transform.position;		
			ThirdPersonCameraNET cam = viewerObj.GetComponent<ThirdPersonCameraNET>();
			cam.LoadCameras();	
	
		}
		
		
					
			
		
	}
	
	//spawns on command, useful for getting people to spawn correctly midway through a game. 
	public void SpawnTargetChar(string charname)
	{
		
		if (charname == "Builder")
		{
		
			if(GameObject.FindGameObjectWithTag("Builder"))
			{
				GameObject.FindGameObjectWithTag("Builder").transform.position = BuilderSpawnPoint.transform.position;
				return;
			}
		}
		
		if (charname == "Mover")
		{
			if(GameObject.FindGameObjectWithTag("Mover"))
			{	
				GameObject.FindGameObjectWithTag("Mover").transform.position = MoverSpawnPoint.transform.position;
				return;
			}
		}
		
		if (charname == "Jumper")
		{
			if(GameObject.FindGameObjectWithTag("Jumper"))
			{
				GameObject.FindGameObjectWithTag("Jumper").transform.position = JumperSpawnPoint.transform.position;
				return;
			}
		}
		
		if (charname == "Viewer")
		{
			if(GameObject.FindGameObjectWithTag("Viewer"))
			{
				
				//only the viewer needs a specialized setup - which MUST change every level. /Larry
				GameObject viewer = GameObject.FindGameObjectWithTag("Viewer");
			    viewer.transform.position = ViewerSpawnPoint.transform.position;
				
				ThirdPersonCameraNET cam = viewer.GetComponent<ThirdPersonCameraNET>();
				cam.LoadCameras();
				return;
			}	
		}
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}