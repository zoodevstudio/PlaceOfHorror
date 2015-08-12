using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Music_Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (ES2.Exists ("settings")) {
			bool music = ES2.Load<bool> ("settings?tag=music"); 
			float vol = ES2.Load<float>("settings?tag=vol");
			AudioSource m = this.gameObject.GetComponent<AudioSource>();
			m.mute = music;
			m.volume = vol;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
