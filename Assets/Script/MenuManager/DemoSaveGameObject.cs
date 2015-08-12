using UnityEngine;
using System.Collections;

public class DemoSaveGameObject : MonoBehaviour {
	public bool t1bool;
	public string t1string;
	// Use this for initialization
	void Start () {
		//mở ra để chạy
		NotificationManager.Instance.AddListener (this, "Load");
		NotificationManager.Instance.AddListener (this, "Save");
		
		//dùng để xóa dữ liệu, để test khi làm thôi
		NotificationManager.Instance.AddListener (this, "Del");
	}
	
	/*************chú y****************/
	//khi test: xóa dữ liệu thì xóa bên nút postnotif del ở scene player
	//-> rồi qua scene menu xóa. 
	//scene menu là nó xóa những cái tên của những lần lưu đó. (check có thì nó mới đi vào lấy dữ liệu)
	//scene player nó lưu dữ liệu cảu biến. nó phụ thuộc vào tên của gameobject với tên script nên chỉ thực hiện khi 
	//postnotification xóa bên này. (nên nó ưu tiên xóa trước);
	
	public void Save(){
		//khi thực hiện postnotification Save thì chạy hàm này
		//Es2.Save(biến, tên_lưu)
		//biến: dữ liệu của biến cần lưu
		//tên_lưu: tên để lưu biến đó để sau này gọi ra. //dạng như vậy: this.gameObject.name + "tenscript"+i+"?tag=t1bool"+i
		
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (t1bool, this.gameObject.name + "tenscript"+i+"?tag=t1bool"+i);
		ES2.Save (t1string, this.gameObject.name + "tenscript"+i+"?tag=t1string"+i); 
	}
	public void Load(){
		//chạy load -> gán lại cho biến thôi
		string i = CommonVariable.Instance.loadi; 
		if(ES2.Exists (this.gameObject.name+"tenscript"+i)){
			t1bool = ES2.Load<bool> (this.gameObject.name + "tenscript"+i+"?tag=t1bool"+i);
			t1string = ES2.Load<string> (this.gameObject.name + "tenscript"+i+"?tag=t1string"+i); 
		}else {
			//chưa có lưu gì. load dữ liệu mặc định
			print ("chưa có");
		}
	}

	public void Del(){
		string i = CommonVariable.Instance.loadi; 
		if(ES2.Exists (this.gameObject.name+"tenscript"+i)){
			ES2.Delete(this.gameObject.name+"tenscript"+i);
			print ("Đã xóa"+this.gameObject.name+"tenscript"+i);
		}
	}

}
