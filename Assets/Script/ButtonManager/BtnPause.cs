using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnPause : MonoBehaviour {
	Image button;
	public GameObject BtnInventory,BtnAction,BtnRight,BtnLeft,BtnJump,BtnLamp;
	public GameObject PanelPause;
	// Use this for initialization
	void Start () {
		button = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTouchDown ()
	{
		CommonVariable.Instance.isPause = true;
		BtnInventory.SetActive(false);
		BtnLamp.SetActive(false);
		BtnAction.SetActive(false);
		BtnRight.SetActive(false);
		BtnLeft.SetActive(false);
		BtnJump.SetActive(false);
		button.color = Color.gray;
		//CommonVariable.Instance.btn_Jump = "PauseButtonDown";
		PanelPause.SetActive (true);
	}
	
	void OnTouchUp ()
	{
		button.color = Color.white;
		//CommonVariable.Instance.btn_Jump = "PauseButtonUp";
	}
	
	void OnTouchStay ()
	{
		//button.color = Color.white;
	}
	
	void OnTouchExit ()
	{
		button.color = Color.white;
		//CommonVariable.Instance.btn_Jump = "PauseButtonUp";
	}
}
