using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnLamp : MonoBehaviour
{
	Image button;
	private GameObject player;
//	float number;
	// Use this for initialization
	void Start ()
	{
		button = GetComponent<Image> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void OnTouchDown ()
	{
		player.GetComponent<PlayerController> ().LampTurnedOn =! player.GetComponent<PlayerController> ().LampTurnedOn;
		button.color = Color.gray;
		CommonVariable.Instance.btn_Lamp = "LampButtonDown";

		if (player.GetComponent<PlayerController> ().lampObject.activeSelf) {
			player.GetComponent<Animator> ().SetLayerWeight (1, 0);
			player.GetComponent<PlayerController> ().lampObject.SetActive (false);
			//player.GetComponent<PlayerInteractive>().isVisible =false;
		} else {
			player.GetComponent<Animator> ().SetLayerWeight (1, 1);
			player.GetComponent<PlayerController> ().lampObject.SetActive (true);
			Debug.Log("" + player.GetComponent<PlayerController> ().LampTurnedOn);
//			if(player.GetComponent<PlayerController> ().lampObject.transform.FindChild ("Area light Player").gameObject.GetComponent<Light> ().enabled
//			   && player.GetComponent<PlayerController> ().lampObject.transform.FindChild ("Point light").gameObject.GetComponent<Light> ().enabled
//			   && player.GetComponent<PlayerInteractive>().isVisible
//			   ){
//				player.GetComponent<PlayerInteractive>().isVisible =true;
//			}
//			else{
//				player.GetComponent<PlayerInteractive>().isVisible =false;
//			}
		}
	}
	
	void OnTouchUp ()
	{
		button.color = Color.white;
		CommonVariable.Instance.btn_Lamp = "LampButtonUp";
	}
	
	void OnTouchStay ()
	{
		//button.color = Color.white;
		//CommonVariable.Instance.btn_Lamp = "LampButtonDrag";
	}
	
	void OnTouchExit ()
	{
		button.color = Color.white;
		CommonVariable.Instance.btn_Lamp = "LampButtonUp";
	}
}
