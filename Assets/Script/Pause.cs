using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

	public Button closePause, backmenu, btnsaveall, btnpostnotifdel, btnsettings;
	public GameObject panelPause;
	public GameObject BtnInventory,BtnAction,BtnRight,BtnLeft,BtnJump,BtnLamp;
	public Button[] btnSavenum;
	public GameObject PanelSave;
	public GameObject PanelSaving;
	public GameObject PanelNotif;
	public GameObject PanelSettings;

	public Text txtNotif;

	private int loadi=0;
	private int tempsave;

	// Use this for initialization
	void Start () {
		closePause.onClick.AddListener (()=>{
			CommonVariable.Instance.isPause = false;
			panelPause.SetActive(false);
			BtnLamp.SetActive(true);
			BtnInventory.SetActive(true);
			BtnAction.SetActive(true);
			BtnRight.SetActive(true);
			BtnLeft.SetActive(true);
			BtnJump.SetActive(true);

			PanelSave.SetActive(false);
			PanelNotif.SetActive(false);
			PanelSettings.SetActive(false);
			txtNotif.text = "";
		});
		backmenu.onClick.AddListener (()=>{
			FadeScene.Instance.TransitionToScene(FadeScene.SceneName.Menu);
		});
		btnsaveall.onClick.AddListener (()=>{  
			PanelNotif.SetActive(true);			
			PanelSettings.SetActive(false);

			GameObject _ai1 = GameObject.Find ("AI_1");
			GameObject _ai2 = GameObject.Find ("AI_2");
			GameObject _ai1_con = _ai1.transform.Find ("EnemySight").gameObject;
			GameObject _ai2_con = _ai2.transform.Find ("EnemySight").gameObject;
			GameObject _player = GameObject.FindGameObjectWithTag ("Player");
			if (_ai2_con.GetComponent<EnemySight> ().Chasing == true || _ai1_con.GetComponent<EnemySight> ().Chasing == true
			    || _player.GetComponent<PlayerController>().isWounded_Head == true
			    || _player.GetComponent<PlayerController>().isWounded_Leg == true
			    || _player.GetComponent<PlayerController> ().playerState == PlayerController.PlayerState.Sleeping
			    || _player.GetComponent<PlayerController> ().playerState == PlayerController.PlayerState.Sleeping2
			    ){
				txtNotif.text = CheckLanguage("Can't save now","Hiện tại không lưu được"); 
			}else {
				PanelNotif.SetActive(false);
				PanelSave.SetActive(true);
			} 

			
		});
		btnsettings.onClick.AddListener (()=>{
			PanelSave.SetActive(false);
			PanelNotif.SetActive(false);
			PanelSettings.SetActive(true);
		});
		btnpostnotifdel.onClick.AddListener (()=>{
			NotificationManager.Instance.PostNotification (this, "Del");
		});
		for(int i=0;i<btnSavenum.Length;i++){
			int temp=i;
			btnSavenum[i].onClick.AddListener(()=>{
				PanelSaving.SetActive(true);
				clicksave(temp);
			});
		}
	}
	void clicksave(int temp){
		StartCoroutine (waitForActiveRoom (0.1f));
		tempsave = temp;
		/*string name_save = CommonVariable.Instance.name_save;
		CommonVariable.Instance.loadi = (temp+1).ToString();
		
		/**************Demo save biến *****
		ES2.Save ("lần "+(temp+1), "objdemotest?tag=demo"+(temp+1));
		/************************************
		
		if (!ES2.Exists (name_save+(temp+1))) {
			ES2.Save (name_save+(temp + 1), name_save+(temp+1) + "?tag=" + (temp + 1));
		}
		ES2.Save (CommonVariable.Instance.PlayTime, name_save+(temp+1) + "?tag=PlayTime" + (temp + 1));
		//xoa' playtime ở palyersaveload

		NotificationManager.Instance.PostNotification (this, "Save");   
		print("lưu lần: "+(temp+1));*/
	}

	IEnumerator waitForActiveRoom (float second)
	{
		GameObject[] boxparents = GameObject.FindGameObjectsWithTag ("BoxParent");
		foreach (GameObject boxs in boxparents) {
			Transform _boxs = boxs.transform;
			foreach (Transform box in _boxs) {
				box.gameObject.SetActive (true);
			}
		}
		yield return new WaitForSeconds (second);
		StartCoroutine (waitForSendingEvent (0.3f));
	}
	
	IEnumerator waitForSendingEvent (float second)
	{
		GameObject _maincamera = GameObject.FindGameObjectWithTag ("MainCamera");
		string name_save = CommonVariable.Instance.name_save;
		CommonVariable.Instance.loadi = (tempsave+1).ToString();
		ES2.Save ("lần "+(tempsave+1), "objdemotest?tag=demo"+(tempsave+1));
		if (!ES2.Exists (name_save+(tempsave+1))) {
			ES2.Save (name_save+(tempsave + 1), name_save+(tempsave+1) + "?tag=" + (tempsave + 1));
		}
		ES2.Save (CommonVariable.Instance.PlayTime, name_save+(tempsave+1) + "?tag=PlayTime" + (tempsave + 1));
		NotificationManager.Instance.PostNotification (this, "Save");   
		print("lưu lần: "+(tempsave+1));

		PanelSaving.SetActive(false);

		yield return new WaitForSeconds (second);

		_maincamera.gameObject.GetComponent<RoomManager> ().enabled = true;
	}
	string CheckLanguage (string en, string vi)
	{
		if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
			return vi;
		} else if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) 
			return en;
		return "";
	}

	/**************************** thay đổi***********************************************/
	/*public void SaveAll(){
		string name_save = CommonVariable.Instance.name_save;
		if (ES2.Exists (name_save))
			loadi = ES2.Load<int> (name_save + "?tag=loadi");
		loadi++;
		CommonVariable.Instance.loadi = loadi.ToString();


		ES2.Save ("lần "+loadi, "objdemotest?tag=demo"+loadi);

		NotificationManager.Instance.PostNotification (this, "Save");  
		ES2.Save (loadi, name_save + "?tag=loadi");
		print("lưu lần: "+loadi);
	}*/
}
