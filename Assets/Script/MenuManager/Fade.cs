using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (FadeScene.Instance);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
