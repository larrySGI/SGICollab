using UnityEngine;
using System.Collections;

public class DoorScript : Photon.MonoBehaviour {
	
	//public float height_x = 3.2f;
	public float height_y = 3.2f;
	//public float height_z = 3.2f;
	
	//public bool travel_x = false;
	//public bool travel_y = false;
	//public bool travel_z = false;
	
	public float speed = 0.3f;
	
	private Vector3 downPos;
	private Vector3 upPos;
	
	public bool startingDoorState = false; //closed;
	private bool currDoorState = false;
	private bool height_y_negative = false;

	public void ResetDoor()
	{
		currDoorState = startingDoorState;
	}
	
	// Use this for initialization
	void Start () 
	{
		downPos = this.transform.position;
		upPos = new Vector3(downPos.x, downPos.y + height_y, downPos.z);
		
		if (height_y < 0)
			height_y_negative = true;
		
		currDoorState = startingDoorState;
	}
	
	bool isDoorInPosition(Vector3 pos, Vector3 targetPos)
	{
		return (pos == targetPos);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		if (currDoorState) //up
		{
			if (!isDoorInPosition(this.transform.position, upPos))
			{
				if (height_y_negative)
				{
					this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - speed, this.transform.position.z);
					if (this.transform.position.y < upPos.y)
						this.transform.position = upPos;
				}
				else
				{
					this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + speed, this.transform.position.z);
					if (this.transform.position.y > upPos.y)
						this.transform.position = upPos;
				
				}
			
			}
		}	
		else //down
		{
			if (!isDoorInPosition(this.transform.position, downPos))
			{
				if (height_y_negative)
				{
					this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + speed, this.transform.position.z);
					if (this.transform.position.y > downPos.y)
						this.transform.position = downPos;
				}
				else
				{
					this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - speed, this.transform.position.z);
					if (this.transform.position.y < downPos.y)
						this.transform.position = downPos;
				
				}
			
			}
		
		}
	}
	
	public void TriggerDoor()
	{
		currDoorState = !currDoorState;		
	}
}
