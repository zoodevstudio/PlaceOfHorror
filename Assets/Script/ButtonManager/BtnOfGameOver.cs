using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnOfGameOver : MonoBehaviour {
	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTouchDown ()
	{
		text.color = Color.red;
		FadeScene.Instance.TransitionToScene(FadeScene.SceneName.Menu);
	}
	
	void OnTouchUp ()
	{
		text.color = Color.white; 
	}
	
	public void OnTouchStay ()
	{ 

	}
	
	void OnTouchExit ()
	{
		text.color = Color.white;		
	}

}
