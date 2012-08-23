using UnityEngine;
using System.Collections;

public class PressurePlateAnimate : MonoBehaviour {
	
	public float plateDescendRate = 0.01f;
	
	private float curr_t = 0.0f;
	
	private bool plateDown = false;
	
	private float plateDown_coordinate = 0.0f;
	private float plateUp_coordinate = 0.0f;

	private Transform target = null;
	
	void Awake()
	{
		target = transform.FindChild("PressurePlate");	
		if (target != null)
		{
			plateUp_coordinate = target.position.y;
			plateDown_coordinate = plateUp_coordinate - target.renderer.bounds.size.y;
		}
	}
	
	//no OnTriggerEnter needed
	public void OnTriggerStay()
	{
		plateDown = true;
	}	
	
	public void OnTriggerExit()
	{		
		plateDown = false;
	}
	
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (target == null) return;
		
		if (!plateDown)
		{
			if (curr_t < 1.0f)
				curr_t += plateDescendRate;
	
			if (curr_t > 1.0f) curr_t = 1.0f;
			
		}
		else
		{
			if (curr_t > 0.0f) 
				curr_t -= plateDescendRate;
			
			if (curr_t < 0.0f) curr_t = 0.0f;
		}
		
		float plateYPos = Mathf.Lerp(plateDown_coordinate, plateUp_coordinate, curr_t);
		target.position = new Vector3(target.position.x, plateYPos, target.position.z);
		
	}
}
