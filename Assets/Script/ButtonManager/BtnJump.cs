using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnJump : MonoBehaviour
{
	Image button;
	private GameObject player;
	private float jump;
	//private Rigidbody2D rb;

	// Use this for initialization
	void Start ()
	{
		button = GetComponent<Image>();
		player = GameObject.FindGameObjectWithTag ("Player");
		jump = player.GetComponent<PlayerController> ().jump;



	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTouchDown ()
	{
		button.color = Color.gray;
		CommonVariable.Instance.btn_Jump = "JumpButtonDown";
		//player.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * jump);

	}
	
	void OnTouchUp ()
	{
		button.color = Color.white;
		CommonVariable.Instance.btn_Jump = "JumpButtonUp";
	}
	
	void OnTouchStay ()
	{
		//button.color = Color.white;
	}
	
	void OnTouchExit ()
	{
		button.color = Color.white;
		CommonVariable.Instance.btn_Jump = "JumpButtonUp";
	}
}
