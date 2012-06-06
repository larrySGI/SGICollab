using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class UniParse : MonoBehaviour
{
	string baseURL = "https://api.parse.com/1";
	//This must be set with your parse.com API key.
	string RESTAPIKey = "O7bzXG417ywXS2B5P0mB9jNjHwqAFgkiFUrzvK0W"; 
	//This must be set with your parse.com applicationId.
	string applicationId = "S5QPwQwlKxpCtgxBoIgPYcSMkQtX1afM4j4B6DJd";
	
	

	public static UniParse Instance {
		get {
			if (_instance == null) {
				_instance = new GameObject ("UniParse", typeof(UniParse)).GetComponent<UniParse> ();
			}
			return _instance;
		}
	}
	
	static UniParse _instance;
	[HideInInspector]
	public string sessionToken = null;

	public HTTP.Request Request (string method, string path) {
		return Request(method, path, null);
	}
	
	public HTTP.Request Request (string method, string path, object payLoad)
	{
		var url = baseURL + path;
		var r = new HTTP.Request (method, url);
		r.AddHeader ("X-Parse-Application-Id", applicationId);
		r.AddHeader ("X-Parse-REST-API-Key", RESTAPIKey);
		if(sessionToken != null) r.AddHeader ("X-Parse-Session-Token", sessionToken);
		r.AddHeader ("Content-Type", "application/json");
		if(payLoad != null) r.Text = JSON.JsonEncode (payLoad);
		return r;
	}	
}