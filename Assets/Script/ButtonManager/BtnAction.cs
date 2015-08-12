using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnAction : MonoBehaviour {

	Image button;

	// Use this for initialization
	void Start () {
		button = GetComponent<Image>();
	}

	void OnTouchDown ()
	{
		button.color = Color.gray;
		CommonVariable.Instance.btn_Action = "ActionButtonDown";
	}
	
	void OnTouchUp ()
	{
		button.color = Color.white;
		CommonVariable.Instance.btn_Action = "ActionButtonUp";
	}
	
	void OnTouchStay ()
	{
		//button.color = Color.white;
	}
	
	void OnTouchExit ()
	{
		button.color = Color.white;
		CommonVariable.Instance.btn_Action = "ActionButtonUp";
	}
}
