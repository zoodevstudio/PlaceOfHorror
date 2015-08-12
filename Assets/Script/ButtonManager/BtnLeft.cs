using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnLeft : MonoBehaviour {
	Image button;
	private GameObject player;
	private float maxSpeed;
	private float defaultSpeed;
	private bool doubleClicks = false;
	int mouseClicks = 0;
	// Use this for initialization
	void Start ()
	{
		button = GetComponent<Image>();
		player = GameObject.FindGameObjectWithTag("Player");
		maxSpeed = player.GetComponent<PlayerController>().runSpeed;
		defaultSpeed = player.GetComponent<PlayerController>().defaultMoveSpeed;
	}
	
	void OnTouchDown ()
	{
		button.color = Color.gray;
		CommonVariable.Instance.btn_Move = "LeftButtonDown";
		mouseClicks++;
		StartCoroutine(LockClicks());
		if(doubleClicks && mouseClicks==2){
			this.OnDoubleTouch();
		}else player.GetComponent<PlayerController>().moveSpeed = defaultSpeed;

	}
	
	void OnTouchUp ()
	{
		button.color = Color.white;
		CommonVariable.Instance.btn_Move = "LeftButtonUp";
		//StartCoroutine(lockClicks());
		player.GetComponent<PlayerController>().moveSpeed = defaultSpeed;

	}

	public void OnTouchStay ()
	{
		CommonVariable.Instance.btn_Move = "LeftButtonDrag";
		//player.transform.Translate(Vector3.left* speed * Time.deltaTime);
	}
	
	void OnTouchExit ()
	{
		button.color = Color.white;
		CommonVariable.Instance.btn_Move = "LeftButtonUp";
		//StartCoroutine(lockClicks());
		player.GetComponent<PlayerController>().moveSpeed = defaultSpeed;

	}
	void OnDoubleTouch()
	{
		CommonVariable.Instance.btn_Move = "LeftButtonDouble";
		mouseClicks=0;
		//Debug.Log("Double Clicked!");
		player.GetComponent<PlayerController>().moveSpeed = maxSpeed;
	}

	IEnumerator LockClicks() { 
		doubleClicks = true;
		yield return new WaitForSeconds(0.3f); 
		doubleClicks = false;
		mouseClicks=0;
	}
}
