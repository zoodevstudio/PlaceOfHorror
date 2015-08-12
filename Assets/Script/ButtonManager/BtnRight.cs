using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnRight : MonoBehaviour
{
	Image button;
	//public Sprite OtherSprite;
	private GameObject player;
	private float maxSpeed;
	private float defaultSpeed ;
	private bool doubleClicks = false;
	int mouseClicks = 0; 
	// Use this for initialization
	void Start ()
	{
		button = GetComponent<Image> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		maxSpeed = player.GetComponent<PlayerController> ().runSpeed;
		defaultSpeed = player.GetComponent<PlayerController> ().defaultMoveSpeed;
	}

	void OnTouchDown ()
	{
		CommonVariable.Instance.btn_Move = "RightButtonDown";
		//button.sprite = OtherSprite;
		button.color = Color.gray;
		mouseClicks++;
		StartCoroutine (LockClicks ());
		if (doubleClicks && mouseClicks == 2) {
			this.OnDoubleTouch ();
		} else
			player.GetComponent<PlayerController> ().moveSpeed = defaultSpeed;

	}
	
	void OnTouchUp ()
	{
		button.color = Color.white;
		CommonVariable.Instance.btn_Move = "RightButtonUp";
		//StartCoroutine(lockClicks());
		player.GetComponent<PlayerController> ().moveSpeed = defaultSpeed;

	}

	void OnTouchStay ()
	{
		CommonVariable.Instance.btn_Move = "RightButtonDrag";
		//player.transform.Translate(Vector3.right* speed * Time.deltaTime);
	}
	
	void OnTouchExit ()
	{
		button.color = Color.white;
		CommonVariable.Instance.btn_Move = "RightButtonUp";
		//StartCoroutine(lockClicks());
		player.GetComponent<PlayerController> ().moveSpeed = defaultSpeed;

	}

	void OnDoubleTouch ()
	{
		CommonVariable.Instance.btn_Move = "RightButtonDouble";
		mouseClicks = 0;
		//Debug.Log ("Double Clicked!");
		player.GetComponent<PlayerController> ().moveSpeed = maxSpeed;
	}

	IEnumerator LockClicks ()
	{ 
		doubleClicks = true;
		yield return new WaitForSeconds (0.3f); 
		doubleClicks = false;
		mouseClicks = 0;
	}
}
