using UnityEngine;
using System.Collections;

public class ReadableItem : MonoBehaviour
{
	public bool isPoisoned;
	public bool isCallEnemy;
	private AudioManager audioManager;
	// Use this for initialization
	void Start ()
	{
		NotificationManager.Instance.AddListener (this, "Load");
		NotificationManager.Instance.AddListener (this, "Save");
		audioManager = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
	}

	public void Save ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (isPoisoned, this.gameObject.name + "ReadableItem" + i + "?tag=isPoisoned" + i);
	}
	
	public void Load ()
	{
		//chạy load -> gán lại cho biến thôi
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "ReadableItem" + i)) {
			isPoisoned = ES2.Load<bool> (this.gameObject.name + "ReadableItem" + i + "?tag=isPoisoned" + i);
			if (isPoisoned)
				this.gameObject.GetComponent<ListString> ().ListStringDialogueVIE [0] = "Miếng thịt đã được tẩm độc";
		} else {
			//chưa có lưu gì. load dữ liệu mặc định
			print ("chưa có");
		}
	}
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && isCallEnemy) {

//			if (coll.gameObject.transform.localScale.x > 0) {
//				Debug.Log("cai");
//				this.transform.localScale = new Vector3 (1, this.transform.localScale.y, this.transform.localScale.z);
//			} else {
//				Debug.Log("nao");
//				this.transform.localScale = new Vector3 (-1, this.transform.localScale.y, this.transform.localScale.z);
//			}
			this.GetComponent<BoxCollider2D> ().enabled = false;
			if (this.GetComponent<Animator> () != null)
				this.GetComponent<Animator> ().enabled = true;
		}
	}

	public void PlaySound ()
	{
		audioManager.au_glass_broken.Play ();
	}

	public void CallEnemy ()
	{
		this.gameObject.SetActive (false);
		GameObject AI = GameObject.FindGameObjectWithTag ("AI");
		if (!AI.transform.FindChild ("EnemySight").gameObject.GetComponent<EnemySight> ().Chasing)
			AI.GetComponent<EnemyInteractive> ().EnterRoom ();
	}
}
