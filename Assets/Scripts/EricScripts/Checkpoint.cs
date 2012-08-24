using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
		
	public Vector3 originalPosition;
	
	public float checkpointMessageDuration = 5.0f;
	public GUISkin checkpointLabelSkin;
	
	private float checkpointMessageShowTime = 0.0f;
	private Rect checkpointLabelPos;
	private float rwidth = 100.0f;
	private float rheight = 50.0f;
	
	// Use this for initialization
	void Start () {
		originalPosition = this.transform.position;
		checkpointLabelPos = new Rect(Screen.width - rwidth/2, Screen.height - rheight /2, rwidth, rheight); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI ()
	{
		
		if (checkpointMessageShowTime > 0.0f)
		{
			//ToDO: Implement Fade Effect
			if (checkpointLabelSkin != null)
			{
				GUI.skin = checkpointLabelSkin;
				GUI.Label(checkpointLabelPos, "Checkpoint Reached!", "Instructions");
		
			}
			else
			{
				GUI.Label(checkpointLabelPos, "Checkpoint Reached!");
			
			}
			
			checkpointMessageShowTime -= Time.deltaTime;
		}
		
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Builder" ||
				other.tag == "Mover" ||
					other.tag == "Viewer" ||
						other.tag == "Jumper"){
			
			Debug.Log("Hit a checkpoint");
			checkpointMessageShowTime = checkpointMessageDuration * 30.0f; //assume 30fps;
			
			other.GetComponent<ThirdPersonControllerNET>().lastRespawn = this.transform.position;
		}
	}
}
