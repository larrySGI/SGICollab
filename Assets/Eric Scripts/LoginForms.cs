using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;

public class LoginForms : MonoBehaviour {
		
    IEnumerator Start() {	
//		WebHeaderCollection headers = new WebHeaderCollection();
//		headers.Add("X-Parse-Application-Id","S5QPwQwlKxpCtgxBoIgPYcSMkQtX1afM4j4B6DJd");
//		headers.Add("X-Parse-REST-API-Key","O7bzXG417ywXS2B5P0mB9jNjHwqAFgkiFUrzvK0W");
//		//headers.Add("X-Parse-Session-Token","pnktnjyb996sj4p156gjtp4im");
//		headers.Add("Content-Type","application/json");		
//		headers.Set("username", username);
//		headers.Set("password", password);
//		headers.Set("email", email);
//		print (headers.AllKeys);
		
//		UTF8Encoding utf8 = new UTF8Encoding();				
//		string jsonstring = "{\"username\":\"cooldude6\",\"password\":\"p_n7!-e8\",\"phone\":\"415-392-0202\"}";
//		WWW www = new WWW("https://api.parse.com/1/users",utf8.GetBytes(jsonstring),headers);
//		yield return www;
//		print(www.text);
//		HttpWebRequest myReq =
//		(HttpWebRequest)WebRequest.Create("http://www.google.com.sg/");
//		HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
//			string responsestring = response.ToString();	
//		print(response.StatusDescription);
	
//		WebRequest request = WebRequest.Create (" https://api.parse.com/1/users");
//        // If required by the server, set the credentials.
//        request.Credentials = CredentialCache.DefaultCredentials;
//        // Get the response.
//		request.Headers = headers;
//        HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
//        // Display the status.
//        print (response.StatusDescription);
//        // Get the stream containing content returned by the server.
//        Stream dataStream = response.GetResponseStream ();
//        // Open the stream using a StreamReader for easy access.
//        StreamReader reader = new StreamReader (dataStream);
//        // Read the content.
//        string responseFromServer = reader.ReadToEnd ();
//        // Display the content.
//        print (responseFromServer);
//        // Cleanup the streams and the response.
//        reader.Close ();
//        dataStream.Close ();
		
		return null;
    }
	
	public static IEnumerator signUp(string email, string username, string password){
		print("signing up...");
		
//	   	var url = "https://api.parse.com/1/users";
//		var request = new HTTP.Request("POST", url);
//		//set headers	
//		request.SetHeader("X-Parse-Application-Id", "S5QPwQwlKxpCtgxBoIgPYcSMkQtX1afM4j4B6DJd");
//		request.SetHeader("X-Parse-REST-API-Key", "O7bzXG417ywXS2B5P0mB9jNjHwqAFgkiFUrzvK0W");
//		request.SetHeader("Content-Type", "application/json");
//		request.SetHeader("username", username);
//		request.SetHeader("password", password);
//		//request.Text = "Hello from UniWeb!";
//		request.Send();
//		while(!request.isDone) yield return new WaitForEndOfFrame();
//		if(request.exception != null) 
//		    Debug.LogError(request.exception);
//		else {
//		    var response = request.response;
//		    //inspect response code
//		    Debug.Log(response.status);
//		    //inspect headers
//		    Debug.Log(response.GetHeader("Content-Type"));
//		    //Get the body as a byte array
//		    //Debug.Log(response.bytes);
//		    //Or as a string
//		    Debug.Log(response.Text);			
//		}
		
		
		//Register a new user
		var user = ParseClass.users.New();
		user.Set("username", username);
		user.Set("password", password);
		user.Set("email", email);
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
			print("No error");
			MainMenuVik.userTally = true;
		}
	}
	
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
			Debug.Log("Max level reached = " + user.Get<int>("maxStageReached"));
			MainMenuVik.userTally = true;
		}		
	}
	
}