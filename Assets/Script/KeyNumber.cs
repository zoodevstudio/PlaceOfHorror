using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyNumber : MonoBehaviour
{
	public int i1 = 0;
	public int i2 = 0;
	public int i3 = 0;
	public Button[] btnKey;
	public Door currentDoor;
	public int currentDoorNumber;
	// Use this for initialization
	void Start ()
	{
		btnKey [0].onClick.AddListener (() => {
			Increment (btnKey [0], i1);
		});
		btnKey [1].onClick.AddListener (() => {
			Increment (btnKey [1], i2);
		});
		btnKey [2].onClick.AddListener (() => {
			Increment (btnKey [2], i3);
		});
		btnKey [3].onClick.AddListener (() => {
			StartCoroutine (CheckLockNumber (0.1f));
		});
	}

	IEnumerator CheckLockNumber (float second)
	{
		if (((i1 * 100) + (i2 * 10) + i3) == currentDoorNumber) {
			GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
			GameObject chatbox = canvas.transform.FindChild ("Chatbox").gameObject;
			List<string> temp_list = new List<string> ();
			string temp_string = "Bạn đã mở khóa cánh cửa này!";
			temp_list.Add (temp_string);
			chatbox.GetComponent<BtnChatbox> ().addChatboxDialogue (temp_list);
			chatbox.GetComponent<BtnChatbox> ().ChatBoxActive ();
			currentDoor.isLockNumber = false;
		}
		yield return new WaitForSeconds (second);
		// Reset tất cả giá trị
		currentDoorNumber = 0;
		currentDoor = null;
		btnKey [0].transform.FindChild ("Text").gameObject.GetComponent<Text> ().text = "0";
		btnKey [1].transform.FindChild ("Text").gameObject.GetComponent<Text> ().text = "0";
		btnKey [2].transform.FindChild ("Text").gameObject.GetComponent<Text> ().text = "0";
		i1 = i2 = i3 = 0;
		Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
		Transform _panelkey = _canvas.FindChild ("Panel 1");
		_panelkey.gameObject.SetActive (false);
	}

	void Increment (Button btnnum, int i)
	{
		GameObject obj = btnnum.transform.FindChild ("Text").gameObject;
		Text txtbox = obj.GetComponent<Text> ();
		if (i < 9) {
			i++; 
			txtbox.text = i.ToString ();
		} else {
			i = 0;
			txtbox.text = i.ToString ();
		} 
		switch (btnnum.name) {
		case "key1":
			i1 = i;
			break;
		case "key2":
			i2 = i;
			break;
		case "key3":
			i3 = i;
			break;
		}
	}

}
