using UnityEngine;
using System.Collections;

public class myCamera : MonoBehaviour {
    public Transform target;
	public float relativeHeight = 20.0f;
    public float zDistance = 1.0f;
    public float dampSpeed = 2;
	
	// Use this for initialization
	void Start () {
		//transform.Rotate(Vector3.down * 90);
		//transform.rotation = new Quaternion(90, 0, 0, 0);
	}
	
    void Update () {
        Vector3 newPos = target.position + new Vector3(0, relativeHeight, -zDistance);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime*dampSpeed);
	}
}
