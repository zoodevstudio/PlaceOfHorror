using UnityEngine;
using System.Collections;

public class SaveTransform : MonoBehaviour
{

	private Transform objectTransform;

	void Start ()
	{
		objectTransform = this.gameObject.transform;
		NotificationManager.Instance.AddListener (this, "Load");
		NotificationManager.Instance.AddListener (this, "Save");
		//dùng để xóa dữ liệu, để test khi làm thôi
		NotificationManager.Instance.AddListener (this, "Del");
	}
	
	public void Save ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (objectTransform, this.gameObject.name + "SaveTransform" + i + "?tag=objectTransform" + i);
	}
	
	public void Load ()
	{
		//chạy load -> gán lại cho biến thôi
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "SaveTransform" + i)) {
			ES2.Load<Transform> (this.gameObject.name + "SaveTransform" + i + "?tag=objectTransform" + i, objectTransform);
		} else {
			//chưa có lưu gì. load dữ liệu mặc định
			print ("chưa có");
		}
	}
}
