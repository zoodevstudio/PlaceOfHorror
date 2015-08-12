using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContinueLoad : MonoBehaviour
{
	
	public Button[] btnLoad;
	
	// Use this for initialization
	void Start ()
	{
		//NotificationManager.Instance.AddListener (this, "hih");
		for (int i=0; i<btnLoad.Length; i++) {
			int temp = i;
			loadpl (temp, btnLoad [temp]);
			btnLoad [i].onClick.AddListener (() => {
				clickload (temp);
			});
		}
	}

	void loadpl (int temp, Button btnload)
	{
		string namesave = CommonVariable.Instance.name_save;
		if (!ES2.Exists (namesave + (temp + 1))) { 

		} else {  
			int tg = ES2.Load<int> (namesave + (temp + 1) + "?tag=PlayTime" + (temp + 1)); 
			GameObject playtime = btnload.transform.FindChild ("Text").gameObject;
			Text txtpt = playtime.GetComponent<Text> ();
			txtpt.text = "Load " + (temp + 1) + ": " + tg.ToString (); 
		}			
	}

	void clickload (int temp)
	{
		string namesave = CommonVariable.Instance.name_save;
		if (!ES2.Exists (namesave + (temp + 1))) {
			//nếu chưa toofnt ại thì cứ đưa nó về 0
			CommonVariable.Instance.loadi = "0";
			CommonVariable.Instance.PlayTime = 0;
			CommonVariable.Instance.TempPlayTime = 0;
			//ES2.Save ((temp + 1), namesave+(temp+1) + "?tag=loadi" + (temp + 1));
		} else {
			print ("đã lưu lần trước");
			int tg = ES2.Load<int> (namesave + (temp + 1) + "?tag=PlayTime" + (temp + 1));
			CommonVariable.Instance.PlayTime = tg;
			CommonVariable.Instance.TempPlayTime = (float)tg;
			CommonVariable.Instance.loadi = (temp + 1).ToString ();    
		}			 
		FadeScene.Instance.TransitionToScene (FadeScene.SceneName.Scene);
		
	}
	
	
	
	/*********************************Thay đổi**********************/
	/*
	private int loadi=0;	
	public int lanload = 1; //lần load sửa ngoài game để test
	public Transform parent;
	public GameObject prefab;
	
	public void LoadSave(){
		string name_save = CommonVariable.Instance.name_save;
		if (ES2.Exists (name_save))
			loadi = ES2.Load<int> (name_save + "?tag=loadi");
		if (loadi>0) {
			for(int i=1; i<=loadi;i++){
				GameObject objLoad = Instantiate (prefab) as GameObject;
				objLoad.transform.SetParent(parent);
				objLoad.transform.localScale = Vector3.one;
				
				GameObject nameLoad = objLoad.gameObject.transform.FindChild ("Text").gameObject;
				nameLoad.GetComponent<Text>().text = "Load "+i;
				
				Button tempButton = objLoad.GetComponent<Button>();
				int tempInt = i;			
				tempButton.onClick.AddListener(() => ButtonDetaiItem(tempInt));
			}

			//đến đây là load ra số lần đã save. -> truyền sang bên kia với tham số là lần loadi
			//NotificationManager.Instance.PostNotification (this, "Load"); 


			print(ES2.Load<string> ("objdemotest?tag=demo"+lanload));

		} else
			print ("Chưa có lần lwuu nào");
	}
	void ButtonDetaiItem(int temp){
		FadeScene.Instance.TransitionToScene(FadeScene.SceneName.Scene);
		CommonVariable.Instance.loadi = temp.ToString(); 
		print (temp);
	}
	public void DesGameobject(){
		foreach(Transform child in parent.transform){
			Destroy(child.gameObject);
		}
	}
*/
	public void Delallsave ()
	{
		ES2.Delete ("GameSave" + 0);
		ES2.Delete ("GameSave" + 1);
		ES2.Delete ("GameSave" + 2);
		ES2.Delete ("GameSave" + 3);
		ES2.Delete ("GameSave" + 4);
		ES2.Delete ("GameSave" + 5);
		ES2.Delete ("ItemList0");
		ES2.Delete ("ItemList1");
		ES2.Delete ("ItemList2");
		ES2.Delete ("ItemList3");
		ES2.Delete ("ItemList4");
		ES2.Delete ("ItemList5"); 
		ES2.Delete ("objdemotest");
		PlayerPrefs.DeleteAll ();
		Application.LoadLevel ("Menu");
	}
	
}
