using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;

public class UserDatabase : MonoBehaviour {
		
    void Start() {	
    }
		
	public static IEnumerator signUp(string email, string username, string password){
		
		//Register a new user
		var user = ParseClass.users.New();
		user.Set("username", username);
		user.Set("password", password);
		user.Set("email", email);
		user.Set("maxStageReached", 0);
		user.Create();		
		Debug.Log("Creating user...");
		while(!user.isDone) yield return null;
		//check for error
		if(user.error != null) {
			//A message is printed automatically. We can diagnose the issue by examing the HTTP code.
			Debug.Log(user.code);
			print("Username " + username + " already taken!");	
			MainMenuVik.userTally = false;
		}
		else{
			print("User created successfully!");
			MainMenuVik.userTally = true;
		}
	}
	
	//User log in
	public static IEnumerator login(string username, string password){
		print("Logging in...");
		
		var user = ParseClass.Authenticate(username, password);
		while(!user.isDone) yield return null;
		//check for error
		if(user.error != null) {
			Debug.Log("Either user does not exists, or wrong password!");
			MainMenuVik.userTally = false;
		}
		else{
			//Debug.Log("Max level reached = " + user.Get<int>("maxStageReached"));
			MainMenuVik.maxLevelData = user.Get<int>("maxStageReached");
			MainMenuVik.userTally = true;
		}		
	}
	
	//Just retrieving some data
	public static IEnumerator getData(string username, string password, string data){
		print("Retrieving data...");
		
		var user = ParseClass.Authenticate(username, password);
		while(!user.isDone) yield return null;
		Debug.Log(user.Get<string>(data));		
	}
}