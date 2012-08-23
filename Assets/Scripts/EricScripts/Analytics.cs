using UnityEngine;
using System.Collections;

public class Analytics : MonoBehaviour {
	
	string url = "";
	string token, level;
	
	// Use this for initialization
	void Start () {
		token = UserDatabase.token;
		level = GameManagerVik.nextLevel.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void sendAnalytics(){
		StartCoroutine(saveToOnlineDatabase());
	}
		
	IEnumerator saveToOnlineDatabase(){
		var r = new HTTP.Request ("POST", url);
		r.Send ();		
		Debug.Log("Analytics sent online.");
		
		yield return null;	
	}
}
