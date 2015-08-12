using UnityEngine;
using System.Collections;

public class SavePlayerTrap : MonoBehaviour
{

	public GameObject trapcon;
	// Use this for initialization
	void Start ()
	{
		NotificationManager.Instance.AddListener (this, "Load");
		NotificationManager.Instance.AddListener (this, "Save");
	}
	
	public void Save ()
	{ 
		bool isactive = trapcon.activeSelf;
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (isactive, this.gameObject.name + "SavePlayerTrap" + i + "?tag=isactive" + i);
	}
	
	public void Load ()
	{
		//chạy load -> gán lại cho biến thôi
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "SavePlayerTrap" + i)) {
			bool isactive = ES2.Load<bool> (this.gameObject.name + "SavePlayerTrap" + i + "?tag=isactive" + i);
			if (isactive) {
				trapcon.gameObject.SetActive (true);
				this.gameObject.GetComponent<FollowPlayer> ().enabled = false;
			} else {
				trapcon.gameObject.SetActive (false);
				this.gameObject.GetComponent<FollowPlayer> ().enabled = true;
			}
		} else {
			//chưa có lưu gì. load dữ liệu mặc định
			print ("chưa có");
		}
	}
}
