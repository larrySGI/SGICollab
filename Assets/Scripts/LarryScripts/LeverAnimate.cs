using UnityEngine;
using System.Collections;

public class LeverAnimate : MonoBehaviour {

	public float maxRotate = 45.0f;
	public float minRotate = 0.0f;
	public float rotateRate =1.0f;
	
	private float curr_t = 1.0f;

	private Transform target = null;
	
	private bool switchTriggered = false;
	private bool shouldHold = false;
	
	void Awake()
	{
		target = transform.FindChild("Lever");
		//targetRotate = minRotate;
		
		
	}
	
	void Start()
	{
		DoorTriggerScript dtsRef = transform.GetComponent<DoorTriggerScript>();	
		if (dtsRef.triggerMode == DoorTriggerScript.TriggerMode.Hold_To_Open)
			shouldHold = true;
	}
	
	public void OnTriggerEnter(Collider other)
	{
		switchTriggered = true;
	}
	
	public void OnTriggerExit()
	{
		switchTriggered = false;	
	}
	
	void FixedUpdate () 
	{
		if (target == null) return;
		
		float angle = Mathf.Lerp(minRotate, maxRotate, curr_t);
		target.eulerAngles = new Vector3(0, 0, angle);
		
		
		if (switchTriggered)
		{
			curr_t += rotateRate;
			if (curr_t >= 1.0f)
			{
				curr_t = 1.0f;
				if (!shouldHold)
					switchTriggered = false;
			}
		}
		else
		{
			if (curr_t > 0.0f) 
				curr_t -= rotateRate;
			
			if (curr_t < 0.0f) curr_t = 0.0f;
		}
	}
}
