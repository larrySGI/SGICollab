using UnityEngine;
using System.Collections;
//using System.Text;
//using System.Net;
//using System.IO;
using System.Collections.Generic;

public class UserDatabase : MonoBehaviour {
	
	static string url = "http://sgicollab.herokuapp.com/users";
	public static string token;
	
	float lastTime;
	float intervalForUserCheck = 10;
	
    void Start() {
		lastTime = Time.time;
    }
	
	void Update(){
		if(Time.time - lastTime > intervalForUserCheck){
			StartCoroutine(verifyUser());
			lastTime = Time.time;
		}
	}
	
	public static IEnumerator signUp(string email, string username, string password){
		print("Signing up...");
		
		string urlconcat ="?user[name]=" + username + 
							"&user[email]=" + email + 
							"&user[password]=" + password + 
							"&user[password_confirmation]=" + password + 
							"&user[maxStageReached]=1";
	
		var r = new HTTP.Request ("POST", url + urlconcat);
		r.Send ();
		while (!r.isDone) {
				if (r.exception != null) {
					Debug.Log (r.exception.ToString ());
			}
		}
		
		if (r.exception != null) {
			Debug.Log (r.exception.ToString ());
		} else {
			Debug.Log(r.response.Text);	
			Hashtable json = (Hashtable)JsonSerializer.Decode(r.response.Bytes);
			if (json.ContainsKey ("auth_token")) {
			 	token = json["auth_token"].ToString();
				MainMenuVik.userTally = true;
			}else{
				MainMenuVik.userTally = false;
			}
		}
		
		yield return null;		
	}
	
	//User log in
	public static IEnumerator login(string username, string password){
		print("Logging in...");
				
		string urlconcat ="/sign_in" + 
							"?user[name]=" + username + 
							"&user[password]=" + password;
		
		var r = new HTTP.Request ("POST", url + urlconcat);
		r.Send ();
		while (!r.isDone) {
				if (r.exception != null) {
					Debug.Log (r.exception.ToString ());
			}
		}
		
		if (r.exception != null) {
			Debug.Log (r.exception.ToString ());
		} else {
			Debug.Log(r.response.Text);	
			
			Hashtable json = (Hashtable)JsonSerializer.Decode(r.response.Bytes);
			 if (json.ContainsKey ("auth_token")) {
			 	token = json["auth_token"].ToString();
			 	MainMenuVik.maxLevelData = (int)json["maxStageReached"];
				MainMenuVik.userTally = true;
			 }else{
				MainMenuVik.userTally = false;
			 }
		}
		
		yield return null;	
	}
	
	//Just retrieving some data
	public static IEnumerator getData(string username, string password, string data){
		print("Retrieving data...");
		
		var user = ParseClass.Authenticate(username, password);
		while(!user.isDone) yield return null;
		Debug.Log(user.Get<string>(data));		
	}
	
	IEnumerator verifyUser(){
		print("Verifying...");
				
		string urlconcat ="/signedin" +
							"?auth_token=" + token;
		
		var r = new HTTP.Request ("POST", url + urlconcat);
		r.Send ();
		while (!r.isDone) {
				if (r.exception != null) {
					Debug.Log (r.exception.ToString ());
			}
		}
		
		if (r.exception != null) {
			Debug.Log (r.exception.ToString ());
		} else {
			Debug.Log(r.response.Text);	
			
			if(r.response.Text == "not signed in")
				Application.Quit();
		}
		
		yield return null;
	}
}