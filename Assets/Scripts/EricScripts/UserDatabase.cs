using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;

public class UserDatabase : MonoBehaviour {
		
    void Start() {	
    }
		
	public static IEnumerator signUp(string email, string username, string password){
		
		
		
		string url = "http://fierce-wind-6489.herokuapp.com/users?";
		string urlconcat ="&user[name]="+username+"&user[email]="+email+"&user[password]="+password+"&user[password_confirmation]="+password+"&user[maxStageReached]=1";
	
			var r = new HTTP.Request ("POST", url+urlconcat);
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
					MainMenuVik.userTally = true;
				}else{
				MainMenuVik.userTally = false;
				}
			}
		
		
		
			yield return null;
		
		
		
		
		
		
		
		
		
		
		
//		//Register a new user
//		var user = ParseClass.users.New();
//		user.Set("username", username);
//		user.Set("password", password);
//		user.Set("email", email);
//		user.Set("maxStageReached", 0);
//		user.Create();		
//		Debug.Log("Creating user...");
//		while(!user.isDone) yield return null;
//		//check for error
//		if(user.error != null) {
//			//A message is printed automatically. We can diagnose the issue by examing the HTTP code.
//			Debug.Log(user.code);
//			print("Username " + username + " already taken!");	
//			MainMenuVik.userTally = false;
//		}
//		else{
//			print("User created successfully!");
//			MainMenuVik.userTally = true;
//		}
	}
	
	//User log in
	public static IEnumerator login(string username, string password){
		print("Logging in...");
		
			string url = "http://fierce-wind-6489.herokuapp.com/users/sign_in?";
			string urlconcat ="user[name]="+username+"&user[password]="+password;
			
			var r = new HTTP.Request ("POST", url+urlconcat);
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
				 MainMenuVik.maxLevelData = (int)json["maxStageReached"];
					MainMenuVik.userTally = true;
				}else{
				MainMenuVik.userTally = false;
				}
			}
		
		
		
			yield return null;
		
		
		
		
		
		
		
		
		
		
		
//		var user = ParseClass.Authenticate(username, password);
//		while(!user.isDone) yield return null;
//		//check for error
//		if(user.error != null) {
//			Debug.Log("Either user does not exists, or wrong password!");
//			MainMenuVik.userTally = false;
//		}
//		else{
//			//Debug.Log("Max level reached = " + user.Get<int>("maxStageReached"));
//			MainMenuVik.maxLevelData = user.Get<int>("maxStageReached");
//			MainMenuVik.userTally = true;
//		}		
	}
	
	//Just retrieving some data
	public static IEnumerator getData(string username, string password, string data){
		print("Retrieving data...");
		
		var user = ParseClass.Authenticate(username, password);
		while(!user.isDone) yield return null;
		Debug.Log(user.Get<string>(data));		
	}
}