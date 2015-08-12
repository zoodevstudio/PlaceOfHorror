using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnInventory : MonoBehaviour {
	Image button;
	public GameObject PanelItem;
	public GameObject BtnPause,BtnAction,BtnRight,BtnLeft,BtnJump,BtnLamp;
	// Use this for initialization
	void Start () {
		button = GetComponent<Image>();
	}
	

	void OnTouchDown ()
	{
		CommonVariable.Instance.isPause = true;
		button.color = Color.gray;
		//CommonVariable.Instance.btn_Action = "InventoryButtonDown";
		PanelItem.GetComponent<ItemInventory2> ().Restart ();
		PanelItem.SetActive (true);
		BtnPause.SetActive(false);
		BtnLamp.SetActive(false);
		BtnAction.SetActive(false);
		BtnRight.SetActive(false);
		BtnLeft.SetActive(false);
		BtnJump.SetActive(false);
	}
	
	void OnTouchUp ()
	{
		button.color = Color.white;
		//CommonVariable.Instance.btn_Action = "InventoryButtonUp";
	}
	
	void OnTouchStay ()
	{
		//button.color = Color.white;
	}
	
	void OnTouchExit ()
	{
		button.color = Color.white;
		//CommonVariable.Instance.btn_Action = "InventoryButtonUp";
	}
}
