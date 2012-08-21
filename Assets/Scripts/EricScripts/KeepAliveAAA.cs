using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeepAliveAAA : MonoBehaviour
{

	string url = "http://hollow-cloud-6476.herokuapp.com/users";
	public int count = 1	;
	string urlconcat;
		
	void Start ()
	{
		urlconcat = "?user[name]=eric&user[email]=eric@test.com&user[password]=password&user[password_confirmation]=password";
		var requests = new List<HTTP.Request> ();
		while (count > 0) {
			count --;
			var r = new HTTP.Request ("POST", url+urlconcat);
			r.Send ();
			requests.Add (r);
		}
		
		while (true) {
			var done = true;
			foreach (var r in requests) {
				done = done & r.isDone;
				if (r.exception != null) {
					Debug.Log (r.exception.ToString ());
				}
			}
			
			if (done)
				break;
		}
		
		foreach (var r in requests) {
			if (r.exception != null) {
				Debug.Log (r.exception.ToString ());
			} else {
				Debug.Log(r.response.Text);	
			}
		}
		Debug.Log("Finished.");
		
	}
	
	
	
}
