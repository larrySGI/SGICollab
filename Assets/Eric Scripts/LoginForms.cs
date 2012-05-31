using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;

public class LoginForms : MonoBehaviour {
	
    //public static MarbleGameManager SP;
	
    //public string url = "http://www.google.com.sg";
    IEnumerator Start() {	
		WebHeaderCollection headers = new WebHeaderCollection();
		headers.Add("X-Parse-Application-Id","S5QPwQwlKxpCtgxBoIgPYcSMkQtX1afM4j4B6DJd");
		headers.Add("X-Parse-REST-API-Key","O7bzXG417ywXS2B5P0mB9jNjHwqAFgkiFUrzvK0W");
		headers.Add("X-Parse-Session-Token","pnktnjyb996sj4p156gjtp4im");
		headers.Add("Content-Type","application/json");
		print (headers.AllKeys);
//		UTF8Encoding utf8 = new UTF8Encoding();	
//			
//		string jsonstring = "{\"username\":\"cooldude6\",\"password\":\"p_n7!-e8\",\"phone\":\"415-392-0202\"}";
//		WWW www = new WWW("https://api.parse.com/1/users",utf8.GetBytes(jsonstring),headers);
//		yield return www;
//		print(www.text);
//		HttpWebRequest myReq =
//		(HttpWebRequest)WebRequest.Create("http://www.google.com.sg/");
//		HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
//			string responsestring = response.ToString();	
//		print(response.StatusDescription);
		
		
		
			WebRequest request = WebRequest.Create ("http://www.contoso.com/default.html");
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
			//request.Headers = headers;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
            // Display the status.
            print (response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream ();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader (dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd ();
            // Display the content.
            print (responseFromServer);
            // Cleanup the streams and the response.
            reader.Close ();
            dataStream.Close ();
            response.Close ();
		
		
		return null;
		
    }
}