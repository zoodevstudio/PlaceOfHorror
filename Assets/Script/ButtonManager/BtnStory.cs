using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnStory : MonoBehaviour
{
	public GameObject storytext;
	// Dialogue hiện tại
	private int currentStory = 0;
	private int currentPage = 0;
	// Object Text con của chatbox
	private GameObject ChatboxText;
	
	// Gán giá trị cho 2 biến dưới ngay khi khởi tạo object
	void Start ()
	{
		if (CommonVariable.Instance.loadi.Contains ("0"))
			callStoryPage (0);
	}

	void OnTouchDown ()
	{
		//<Lam> action: moved 

	}

	void OnTouchUp ()
	{
		StoryActive ();
	}
	
	void OnTouchExit ()
	{
	}

	public void callStoryPage (int _story)
	{
		storytext.SetActive (true);
		currentStory = _story;
		currentPage = 0;
		this.gameObject.GetComponent<BoxCollider> ().enabled = true;
		this.gameObject.GetComponent<Image> ().enabled = true;
		this.gameObject.GetComponent<Image> ().sprite = this.gameObject.GetComponent<Story> ().storypageList [currentStory].storypage [currentPage];
		storytext.GetComponent<Text>().text = this.gameObject.GetComponent<Story> ().storypageList [currentStory].storysub [currentPage];
	}

	public void StoryActive ()
	{		
		currentPage++;
		if (currentPage < this.gameObject.GetComponent<Story> ().storypageList [currentStory].storypage.Count) {
			Debug.Log (currentPage);
			this.gameObject.GetComponent<Image> ().sprite = this.gameObject.GetComponent<Story> ().storypageList [currentStory].storypage [currentPage];
			storytext.GetComponent<Text>().text = this.gameObject.GetComponent<Story> ().storypageList [currentStory].storysub [currentPage];
		} else {
			GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
			if (currentStory != 0) {
				GameObject YouWin = canvas.transform.FindChild ("YouWin").gameObject;
				YouWin.SetActive (true);
			}
			else
			{
				storytext.SetActive (false);
				this.gameObject.GetComponent<BoxCollider> ().enabled = false;
				this.gameObject.GetComponent<Image> ().enabled = false;
			}
			this.gameObject.GetComponent<BoxCollider> ().enabled = false;
			//this.gameObject.GetComponent<Image> ().enabled = false;
		}
	}
}
