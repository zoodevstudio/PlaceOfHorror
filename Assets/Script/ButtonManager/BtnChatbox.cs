using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class BtnChatbox : MonoBehaviour
{
	public GameObject btnAction;
	public GameObject btnLeft;
	public GameObject btnRight;
	public GameObject btnButton;
	public GameObject player;
	// Nhận List Dialogue từ các object khác để hiển thị
	[HideInInspector]
	public List<string>
		ChatboxDialogue = new List<string> ();
	// Dialogue hiện tại
	private int currentDialogue;
	// Object Text con của chatbox
	private GameObject ChatboxText;

	// Gán giá trị cho 2 biến dưới ngay khi khởi tạo object
	void Awake ()
	{
		ChatboxText = this.transform.FindChild ("Text").gameObject;
		currentDialogue = 0;
	}
	void OnTouchDown ()
	{
		//<Lam> action: moved 
		ChatBoxActive ();
		CommonVariable.Instance.btn_ChatBox = "ChatBoxButtonDown";
	}
	void OnTouchUp ()
	{
		//ChatBoxActive ();
	}
	
	void OnTouchExit ()
	{
	}

	public void addChatboxDialogue (List<string> _dialogue)
	{
		if (currentDialogue == 0)
			ChatboxDialogue = _dialogue;
	}

	public void ChatBoxActive ()
	{
		//btnButton.SetActive(false);
		if (ChatboxDialogue.Count != currentDialogue) {
			// Chạy các string liên tiếp trong list
			this.gameObject.SetActive (true);
			ChatboxText.GetComponent<Text> ().text = ChatboxDialogue [currentDialogue];
			currentDialogue++;
			btnButton.SetActive(false);
			btnAction.GetComponent<Image>().color = Color.white;
			btnRight.GetComponent<Image>().color = Color.white;
			btnLeft.GetComponent<Image>().color = Color.white;
			player.GetComponent<PlayerController>().playerState = PlayerController.PlayerState.Reading;
		} else {

			// Ẩn chatbox và reset các giá trị
			this.gameObject.SetActive (false);
			ChatboxDialogue = new List<string> ();
			currentDialogue = 0;
			btnButton.gameObject.SetActive(true);
			player.GetComponent<PlayerController>().playerState = PlayerController.PlayerState.Standing;
			player.GetComponent<Animator>().CrossFade("Standing",0.1f);
		}


	}
}
