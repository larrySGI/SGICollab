using UnityEngine;
using System.Collections;
//using System.Text;
//using System.Net;
//using System.IO;
using System.Collections.Generic;

public class UserDatabase : MonoBehaviour {
	
	static string url = "http://sgicollab1.herokuapp.com/users";
	public static string token;
	
	float lastTime;
	float intervalForUserCheck = 300;
	
    void Start() {
		lastTime = Time.time;
    }
	
	void Update(){
		if(Time.time - lastTime > intervalForUserCheck){
			StartCoroutine(verifyUser());
			lastTime = Time.time;
		}
	}
	
	public static void signUp(string email, string username, string password){
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
			//if(json["success"] == false){};
			
			if (json.ContainsKey ("auth_token")) {
			 	token = json["auth_token"].ToString();
			}
		}	
	}
	
	//User log in
	public static void login(string username, string password){
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
			}
		}	
	}
	
	//Just retrieving some data
//	public static IEnumerator getData(string username, string password, string data){
//		print("Retrieving data...");
//		
//		var user = ParseClass.Authenticate(username, password);
//		while(!user.isDone) yield return null;
//		Debug.Log(user.Get<string>(data));		
//	}
	
	
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
			
			if(r.response.Text == "user not signed in")
			{
				//destroy the code object here! Otherwise when we go back to main we'll actually *duplicate* it, which is what we don't want because
				//it messes up the main menu.
				Destroy(GameObject.Find("Code"));
				Application.LoadLevel(0);
			}
		}
		
		yield return null;
	}
	
	
	public static string getGameID(){
		print("Getting game ID...");
				
		string level = GameManagerVik.nextLevel.ToString();
		
		string urlconcat ="http://sgicollab1.herokuapp.com/game" +
							"?auth_token=" + token +
							"&game[room_name]=" +  WWW.EscapeURL(PhotonNetwork.room.name) +
							"&game[level]=" + level;
		print(urlconcat);
		var r = new HTTP.Request ("POST", urlconcat);
		r.Send ();
		while (!r.isDone) {
				if (r.exception != null) {
					Debug.Log (r.exception.ToString ());
			}
		}
		
		if (r.exception != null) {
			Debug.Log (r.exception.ToString ());
			return null;
		} else {
			Debug.Log(r.response.Text);	
			
			//Must inform us if there is any errors here
//			if(r.response.Text == ""){
//			}
			
			return r.response.Text;
		}
		
	}
	
	public static void verifyGameID(string gameID){
		print("Setting game ID..." + gameID);
		
		string urlconcat ="http://sgicollab1.herokuapp.com/add_user_to_game" +
							"?game_id=" + gameID +
							"&auth_token=" + token;
		
		var r = new HTTP.Request ("GET", urlconcat);
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
			
			if(r.response.Text == "user not signed in")
				Application.LoadLevel(0);
		}
	}
}