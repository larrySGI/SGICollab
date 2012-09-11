using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnLevelWasLoaded(){		
		if(audio.isPlaying)
			audio.Stop();
		audio.Play();
	}
}
