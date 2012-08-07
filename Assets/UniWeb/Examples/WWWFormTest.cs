using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WWWFormTest : MonoBehaviour
{
	
	string url = "http://www.differentmethods.com/";

	IEnumerator Start ()
	{
		var w = new WWWForm();
		
		
		w.AddField("hello", "world");
		w.AddBinaryData("file", new byte[] { 65,65,65,65 });
		var r = new HTTP.Request (url, w);
		r.Send();
		
		while (true) {
			if(r.exception != null) { 	//some error occured.
				Debug.Log(r.exception.ToString());
				break;	
			}
			if(r.state != HTTP.RequestState.Waiting) { //there might be some chunks available
				
			}
			yield return new WaitForEndOfFrame ();
		}
		Debug.Log("Stream has finished");
		
		
	}
	
	
}
