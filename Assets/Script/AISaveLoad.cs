using UnityEngine;
using System.Collections;

public class AISaveLoad : MonoBehaviour
{
	public Transform AI;
	public bool FollowPlayer_1;
	public bool FollowPlayer_2;
	public bool FollowPlayer_3;
	public bool	BoxCollider2D_1;
	public bool	BoxCollider2D_2;
	public bool	BoxCollider2D_3;
	public Transform Transform_1;
	public Transform Transform_2;
	public Transform Transform_3;
	// Use this for initialization
	void Start ()
	{
		//Debug.Log (AI.name + "");
		NotificationManager.Instance.AddListener (this, "Load");
		NotificationManager.Instance.AddListener (this, "Save");
		
		//dùng để xóa dữ liệu, để test khi làm thôi
		NotificationManager.Instance.AddListener (this, "Del");
	}

	public void Save ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (AI, this.gameObject.name + "AISaveLoad" + i + "?tag=AITransform" + i);

		ES2.Save (FollowPlayer_1, this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "FollowPlayer_1" + i);
		ES2.Save (FollowPlayer_2, this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "FollowPlayer_2" + i);
		ES2.Save (FollowPlayer_3, this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "FollowPlayer_3" + i);

		ES2.Save (BoxCollider2D_1, this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "BoxCollider2D_1" + i);
		ES2.Save (BoxCollider2D_2, this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "BoxCollider2D_2" + i);
		ES2.Save (BoxCollider2D_3, this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "BoxCollider2D_3" + i);

		ES2.Save (Transform_1, this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "Transform_1" + i);
		ES2.Save (Transform_2, this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "Transform_2" + i);
		ES2.Save (Transform_3, this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "Transform_3" + i);
	}
	
	public void Load ()
	{
		//chạy load -> gán lại cho biến thôi
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "AISaveLoad" + i)) {
			ES2.Load<Transform> (this.gameObject.name + "AISaveLoad" + i + "?tag=AITransform" + i, AI);

			FollowPlayer_1 = ES2.Load<bool> (this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "FollowPlayer_1" + i);
			FollowPlayer_2 = ES2.Load<bool> (this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "FollowPlayer_2" + i);
			FollowPlayer_3 = ES2.Load<bool> (this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "FollowPlayer_3" + i);

			BoxCollider2D_1 = ES2.Load<bool> (this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "BoxCollider2D_1" + i);
			BoxCollider2D_2 = ES2.Load<bool> (this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "BoxCollider2D_2" + i);
			BoxCollider2D_3 = ES2.Load<bool> (this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "BoxCollider2D_3" + i);

			Transform_1 = ES2.Load<Transform> (this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "Transform_1" + i);
			Transform_2 = ES2.Load<Transform> (this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "Transform_2" + i);
			Transform_3 = ES2.Load<Transform> (this.gameObject.name + "AISaveLoad" + i + "?tag=" + AI.name + "Transform_3" + i);

		} else {
			//chưa có lưu gì. load dữ liệu mặc định
			print ("chưa có vi tri AI");
		}

		AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_1").gameObject.GetComponent<FollowPlayer> ().enabled = FollowPlayer_1;
		AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_1").gameObject.GetComponent<BoxCollider2D> ().enabled = BoxCollider2D_1;
	
		AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_2").gameObject.GetComponent<FollowPlayer> ().enabled = FollowPlayer_2;
		AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_2").gameObject.GetComponent<BoxCollider2D> ().enabled = BoxCollider2D_2;

		AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_3").gameObject.GetComponent<FollowPlayer> ().enabled = FollowPlayer_3;
		AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_3").gameObject.GetComponent<BoxCollider2D> ().enabled = BoxCollider2D_3;

		if (AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_1").gameObject.GetComponent<BoxCollider2D> ().enabled == true) {
			AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_1").gameObject.GetComponent<DisableTrapSprite> ().EnableSprite ();
			AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_1").gameObject.transform.position = Transform_1.position;
		}
		if (AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_2").gameObject.GetComponent<BoxCollider2D> ().enabled == true) {
			AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_2").gameObject.GetComponent<DisableTrapSprite> ().EnableSprite ();
			AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_2").gameObject.transform.position = Transform_2.position;
		}
		if (AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_3").gameObject.GetComponent<BoxCollider2D> ().enabled == true) {
			AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_3").gameObject.GetComponent<DisableTrapSprite> ().EnableSprite ();
			AI.GetComponent<EnemyController> ().Trap.transform.FindChild ("Trap_3").gameObject.transform.position = Transform_3.position;
		}

	}
	
	public void Del ()
	{
		string i = CommonVariable.Instance.loadi; 
		string name_save = CommonVariable.Instance.name_save;
		if (ES2.Exists (this.gameObject.name + "AISaveLoad" + i)) {
			ES2.Delete (this.gameObject.name + "AISaveLoad" + i);
			//xoa playtime
			ES2.Delete (name_save + (i));
			print ("Đã xóa" + this.gameObject.name + "AISaveLoad" + i);
		}
	}
}
