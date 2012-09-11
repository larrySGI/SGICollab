using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour {
	public bool mute;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnLevelWasLoaded(){
		if(!mute){
			if(audio.isPlaying)
				audio.Stop();
			audio.Play();
		}
	}
}
