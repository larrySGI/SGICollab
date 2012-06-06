using UnityEngine;
using System.Collections;

public class NewLevelScript : MonoBehaviour {
	public GameObject BuilderSpawnPoint;
	public GameObject JumperSpawnPoint;
	public GameObject MoverSpawnPoint;
	public GameObject ViewerSpawnPoint;
	
	
	
	// Use this for initialization
	void Start () {
		if(GameObject.FindGameObjectWithTag("Builder"))
		{
			GameObject.FindGameObjectWithTag("Builder").transform.position = BuilderSpawnPoint.transform.position;
		}	
		if(GameObject.FindGameObjectWithTag("Mover"))
		{	
				GameObject.FindGameObjectWithTag("Mover").transform.position = MoverSpawnPoint.transform.position;
		}	
		if(GameObject.FindGameObjectWithTag("Jumper"))
		{
				GameObject.FindGameObjectWithTag("Jumper").transform.position = JumperSpawnPoint.transform.position;
		}
		if(GameObject.FindGameObjectWithTag("Viewer"))
		{
				
				//only the viewer needs a specialized setup - which MUST change every level. /Larry
				GameObject viewer = GameObject.FindGameObjectWithTag("Viewer");
			    viewer.transform.position = ViewerSpawnPoint.transform.position;
				
				ThirdPersonCameraNET cam = viewer.GetComponent<ThirdPersonCameraNET>();
				cam.LoadCameras();
			
		}
			
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
