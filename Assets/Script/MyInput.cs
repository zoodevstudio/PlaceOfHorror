using UnityEngine;
using System.Collections;

public class MyInput : MonoBehaviour
{
	private int keyPress = 0;
	private bool doubleKeyPress = false;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Jump
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			CommonVariable.Instance.btn_Jump = "JumpButtonDown";
		} else
		if (Input.GetKeyUp (KeyCode.UpArrow)) {
			CommonVariable.Instance.btn_Jump = "JumpButtonUp";
		}

		// Action 
		if(Input.GetKeyDown (KeyCode.Space)){
			CommonVariable.Instance.btn_Action = "ActionButtonDown";
		}else
		if(Input.GetKeyUp (KeyCode.Space)){
			CommonVariable.Instance.btn_Action = "ActionButtonUp";
		}else if(Input.GetKey (KeyCode.Space)){
			CommonVariable.Instance.btn_Action = "ActionButtonDrag";
		}

		// Right
		if (Input.GetKey (KeyCode.RightArrow)) {
			CommonVariable.Instance.btn_Move = "RightButtonDrag";
		} else
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			CommonVariable.Instance.btn_Move = "RightButtonDown";
		} else
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			CommonVariable.Instance.btn_Move = "RightButtonUp";
		}

		// Left
		if (Input.GetKey (KeyCode.LeftArrow)) {
			CommonVariable.Instance.btn_Move = "LeftButtonDrag";
		} else
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			CommonVariable.Instance.btn_Move = "LeftButtonDown";
		} else
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			CommonVariable.Instance.btn_Move = "LeftButtonUp";
		}

		// Double
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			keyPress++;
			StartCoroutine (LockPress ());
			if (doubleKeyPress && keyPress == 2) {
				CommonVariable.Instance.btn_Move = "LeftButtonDouble";
			}
		} else 
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			keyPress++;
			StartCoroutine (LockPress ());
			if (doubleKeyPress && keyPress == 2) {
				CommonVariable.Instance.btn_Move = "RightButtonDouble";
			}
		}

		NotificationManager.Instance.PostNotification (this, "OnAction");
	}

	IEnumerator LockPress ()
	{ 
		doubleKeyPress = true;
		yield return new WaitForSeconds (0.3f); 
		doubleKeyPress = false;
		keyPress = 0;
	}


}