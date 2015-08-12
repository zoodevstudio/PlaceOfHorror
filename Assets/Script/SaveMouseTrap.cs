using UnityEngine;
using System.Collections;

public class SaveMouseTrap : MonoBehaviour
{
	public GameObject MousePrefab;
	public GameObject mousetrapCon;
	// Use this for initialization
	void Start ()
	{
		NotificationManager.Instance.AddListener (this, "Load");
		NotificationManager.Instance.AddListener (this, "Save");
	}
	
	public void Save ()
	{ 
		bool isactive = mousetrapCon.activeSelf;
		bool iscapture = mousetrapCon.GetComponent<MouseTrap> ().isCapturedMouse;
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (isactive, this.gameObject.name + "SaveMouseTrap" + i + "?tag=isactive" + i);
		ES2.Save (iscapture, this.gameObject.name + "SaveMouseTrap" + i + "?tag=iscapture" + i);
	}
	
	public void Load ()
	{
		//chạy load -> gán lại cho biến thôi
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "SaveMouseTrap" + i)) {
			bool isactive = ES2.Load<bool> (this.gameObject.name + "SaveMouseTrap" + i + "?tag=isactive" + i);
			if (isactive) {
				mousetrapCon.gameObject.SetActive (true);
				this.gameObject.GetComponent<FollowPlayer> ().enabled = false;
			} else {
				mousetrapCon.gameObject.SetActive (false);
				this.gameObject.GetComponent<FollowPlayer> ().enabled = true;
			}

			bool iscapture = ES2.Load<bool> (this.gameObject.name + "SaveMouseTrap" + i + "?tag=iscapture" + i);
			if (iscapture) {
				//mousetrapCon.GetComponent<MouseTrap> ().isCapturedMouse = true;
				mousetrapCon.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0);
				Instantiate (MousePrefab);
			}
		} else {
			//chưa có lưu gì. load dữ liệu mặc định
			print ("chưa có");
		}
	}
}
