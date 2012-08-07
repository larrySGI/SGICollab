using UnityEngine;
using System.Collections;

public class WebSocketExample : MonoBehaviour {

	IEnumerator Start () {
		yield return null;
		
		var ws = new HTTP.WebSocket();
		StartCoroutine(ws.Dispatcher());
		
		ws.Connect(" http://chernobyl.local:8888/");
		ws.OnTextMessageRecv += (e) => {
			Debug.Log("Reply came from server -> " + e);
		};
		ws.Send("Hello");
		
		ws.Send("Hello again!");
		
		ws.Send("Goodbye");
	}
	
	
}
