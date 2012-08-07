using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace HTTP {
	public class SimpleWWW : MonoBehaviour {
		static SimpleWWW _instance = null;
		static public SimpleWWW Instance {
			get {
				if(_instance == null) {
					_instance = new GameObject("SimpleWWW", typeof(SimpleWWW)).GetComponent<SimpleWWW>();
					_instance.hideFlags = HideFlags.HideAndDontSave;
				}
				return _instance;
			}
		}
		
		public void Send(Request request, System.Action<HTTP.Request> requestDelegate) {
			StartCoroutine(_Send(request, requestDelegate));
		}
		
		public void Send(Request request, System.Action<HTTP.Response> responseDelegate) {
			StartCoroutine(_Send(request, responseDelegate));
		}
		
		IEnumerator _Send(Request request, System.Action<HTTP.Response> responseDelegate) {
			request.Send();
			while(!request.isDone)
				yield return new WaitForEndOfFrame();
			if(request.exception != null) {
				Debug.LogError(request.exception);	
			} else {
				responseDelegate(request.response);
			}
		}
		
		IEnumerator _Send(Request request, System.Action<HTTP.Request> requestDelegate) {
			request.Send();
			while(!request.isDone)
				yield return new WaitForEndOfFrame();
			requestDelegate(request);
		}
		
		
	}
}