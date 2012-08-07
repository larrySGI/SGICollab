using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FetchImage : MonoBehaviour
{
	List<string> msgs = new List<string>();
	//string url = "http://www.differentmethods.com/wp-content/uploads/2011/05/react.jpg";

	IEnumerator Start ()
	{
		var urls = new string[] {
		"http://www.differentmethods.com/wp-content/uploads/2011/05/uniweb.jpg",
		"http://www.differentmethods.com/wp-content/uploads/2011/05/react.jpg",
		"http://forum.differentmethods.com/uploads/PAK736H77DVR.png",
		"http://www.differentmethods.com/wp-content/uploads/2011/05/piemenus.jpg",
		"http://www.differentmethods.com/wp-content/uploads/2011/05/spacebox1.jpg",
		};
		
		//Only needed in WebPlayer
		//Security.PrefetchSocketPolicy("www.differentmethods.com", 843);
		
		Application.RegisterLogCallback(Logger);
		
		var requests = new List<HTTP.Request>();
		foreach(var url in urls) {
			var r = new HTTP.Request ("GET", url, true);
			r.Send();
			requests.Add(r);
		}
		
		while(true) {
			yield return null;
			var done = true;
			foreach(var r in requests) {
				done = done & r.isDone;
			}
			if(done) break;
		}
		
		foreach(var r in requests) {
			if (r.exception != null) {
				Debug.LogError (r.exception);
			} else {
				var tex = new Texture2D (512, 512);
				tex.LoadImage (r.response.Bytes);
				renderer.material.SetTexture ("_MainTex", tex);
				yield return new WaitForSeconds(1);
			}
		}
		
	}
	
	void Logger(string condition, string msg, LogType type) {
		msgs.Add(condition);
	}
	
	void OnGUI() {
		GUILayout.BeginVertical();
		foreach(var i in msgs) 
			GUILayout.Label(i);
		GUILayout.EndVertical();
	}
}
