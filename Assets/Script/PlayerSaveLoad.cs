using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSaveLoad : MonoBehaviour
{
	public Transform Player;
	// Use this for initialization
	void Start ()
	{ 
		NotificationManager.Instance.AddListener (this, "Load");
		NotificationManager.Instance.AddListener (this, "Save");
		
		//dùng để xóa dữ liệu, để test khi làm thôi
		NotificationManager.Instance.AddListener (this, "Del");
	}

	public void Save ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (Player, this.gameObject.name + "PlayerSaveLoad" + i + "?tag=PlayerTransform" + i);
	}

	public void Load ()
	{
		//chạy load -> gán lại cho biến thôi
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "PlayerSaveLoad" + i)) {
			ES2.Load<Transform> (this.gameObject.name + "PlayerSaveLoad" + i + "?tag=PlayerTransform" + i, Player);
		} else {
			//chưa có lưu gì. load dữ liệu mặc định
			print ("chưa có vi trí player");
		}
	}

	public void Del ()
	{
		string i = CommonVariable.Instance.loadi; 
		string name_save = CommonVariable.Instance.name_save;
		if (ES2.Exists (this.gameObject.name + "PlayerSaveLoad" + i)) {
			ES2.Delete (this.gameObject.name + "PlayerSaveLoad" + i);
			//xoa playtime
			ES2.Delete (name_save + (i));
			print ("Đã xóa" + this.gameObject.name + "PlayerSaveLoad" + i);
		}
	}

}
