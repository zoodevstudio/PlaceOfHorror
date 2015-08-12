using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class ItemInventory2 : MonoBehaviour
{

	public RectTransform contentPanel;
	GameObject PlayInven;
	public GameObject BtnPause, BtnAction, BtnRight, BtnLeft, BtnJump, BtnLamp;
	public GameObject panelInventory;
	public Text txtDetailItem;
	private int currentButton;
	public Button[] btnClickColor;

	// Use this for initialization
	void Start ()
	{
	
	}

	public void Restart ()
	{ //run form BtnInventory.cs
		currentButton = -1;
		txtDetailItem.text = ""; //reset
		PlayInven = GameObject.FindGameObjectWithTag ("Player").gameObject;
		desGameobject (contentPanel); //destroy old gameobject
		btnNormalItem (); //run btnNormal first

	}

	public void ChangeColorBtn (Button btnChange, Button def1, Button def2)
	{
		btnChange.image.color = new Color32 (180, 130, 130, 255);
		def1.image.color = new Color32 (255, 255, 255, 255);
		def2.image.color = new Color32 (255, 255, 255, 255);
	}

	public void btnNormalItem ()
	{
		//click btnNormalItem
		ChangeColorBtn (btnClickColor [0], btnClickColor [1], btnClickColor [2]);	
		txtDetailItem.text = ""; //reset
		if (currentButton != 0) {
			desGameobject (contentPanel);
			LoadItemNormal (PlayInven.GetComponent<PlayerInventory2> ().ItemNormal);
		}
		currentButton = 0; 
	}

	/*public void btnManufactureItem ()
	{
		//click btnManufactureItem
		txtDetailItem.text = ""; //reset
		if (currentButton != 1) {
			desGameobject (contentPanel);
			LoadManufacture (PlayInven.GetComponent<PlayerInventory2> ().ItemManufacture);
		}
		currentButton = 1; 
	}*/

	public void btnKeyItem ()
	{
		//click btnKeyItem
		ChangeColorBtn (btnClickColor [1], btnClickColor [0], btnClickColor [2]);
		txtDetailItem.text = ""; //reset
		if (currentButton != 2) {
			desGameobject (contentPanel);
			LoadItemKey (PlayInven.GetComponent<PlayerInventory2> ().ItemKey);
		}
		currentButton = 2; 
	}

	public void btnNecessityItem ()
	{
		//click btnNecessityItem
		ChangeColorBtn (btnClickColor [2], btnClickColor [0], btnClickColor [1]);
		txtDetailItem.text = ""; //reset
		if (currentButton != 3) {
			desGameobject (contentPanel);
			LoadItemNecessity (PlayInven.GetComponent<PlayerInventory2> ().ItemNecessity);
		}
		currentButton = 3; 
	}

	public void ClosePanel ()
	{
		//close panel inventory
		CommonVariable.Instance.isPause = false;
		panelInventory.SetActive (false);
		BtnPause.SetActive (true);
		BtnAction.SetActive (true);
		BtnRight.SetActive (true);
		BtnLeft.SetActive (true);
		BtnJump.SetActive (true);
		BtnLamp.SetActive (true);
	}

	//destroy gameobject 
	void desGameobject (RectTransform parent)
	{
		foreach (Transform child in parent) {
			Destroy (child.gameObject);
		}
	}

	void ButtonDetaiItem (int temp, string contentitem)
	{
		txtDetailItem.text = contentitem;
	}

	/********************ITemNormal************************/
	void LoadItemNormal (List<string> item)
	{
		for (int i=0; i<item.Count; i++) {
			GameObject PrefabItemNormal = Instantiate (Resources.Load ("Item/Normal/" + item [i])) as GameObject;
			PrefabItemNormal.transform.SetParent (contentPanel);
			PrefabItemNormal.transform.localScale = Vector3.one;

			string contentitem = "";
			string nameitem = "";
			if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
				nameitem = PrefabItemNormal.GetComponent<ListString> ().ListStringDialogueVIE [0];
				contentitem = PrefabItemNormal.GetComponent<ListString> ().ListStringDialogueVIE [1];
			} else if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) {
				nameitem = PrefabItemNormal.GetComponent<ListString> ().ListStringDialogueENG [0];
				contentitem = PrefabItemNormal.GetComponent<ListString> ().ListStringDialogueENG [1];
			}
			PrefabItemNormal.gameObject.transform.FindChild ("NameItem").gameObject.GetComponent<Text> ().text = nameitem;
			
			Button btnItem = PrefabItemNormal.GetComponent<Button> ();
			int tempInt = i;			
			btnItem.onClick.AddListener (() => ButtonDetaiItem (tempInt, contentitem));

		}//set button dang click la button item. có số thứ tự là 0;
	}

	/********************ITemNormal************************/

	/********************Manufacture************************/
	/*void LoadManufacture (List<string> item)
	{
		for (int i=0; i<item.Count; i++) {
			string[] val = item [i].Split ('_');
			GameObject itv = Instantiate (Resources.Load ("Item/Manufacture/" + val [0])) as GameObject;
			itv.transform.SetParent (contentPanel);
			itv.transform.localScale = Vector3.one;

			GameObject nameitem = itv.gameObject.transform.FindChild ("NameItem").gameObject;
			Text txtnameitem = nameitem.GetComponent<Text> ();
			string contentitem = "";
			if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
				txtnameitem.text = itv.GetComponent<ListString> ().ListStringDialogueVIE [0];
				contentitem = itv.GetComponent<ListString> ().ListStringDialogueVIE [1];
			} else if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) {
				txtnameitem.text = itv.GetComponent<ListString> ().ListStringDialogueENG [0];
				contentitem = itv.GetComponent<ListString> ().ListStringDialogueENG [1];
			}				
			Button tempButton = itv.GetComponent<Button> ();
			int tempInt = i;			
			tempButton.onClick.AddListener (() => ButtonDetaiItem (tempInt, contentitem));
			
			if (val [1] == "lock") {
				//nút use để nâng cấp đồ
				GameObject ObjUse = itv.gameObject.transform.FindChild ("use").gameObject;
				ObjUse.SetActive (true);
				Button buttonUse = ObjUse.GetComponent<Button> ();
				int temp = i;
				buttonUse.onClick.AddListener (() => {
					lendo (val [0], ObjUse);
				});
			} else {
				GameObject ObjUse = itv.gameObject.transform.FindChild ("kq").gameObject;
				ObjUse.SetActive (true);				
			}
			
		}
	}

	void lendo (string nameitem, GameObject buttonuse)
	{
		//load điều kiện để lên cái item này nameitem -> nó cũng là tên 1 list luôn
		List<string> dieukien = ES2.LoadList<string> (nameitem + ".txt");
		bool sts = true;
		for (int j = 0; j < dieukien.Count; j++) {
			if (sts) {
				for (int i = 0; i< PlayInven.GetComponent<PlayerInventory2> ().ItemNormal.Count; i++) {
					if (dieukien [j] == PlayInven.GetComponent<PlayerInventory2> ().ItemNormal [i]) {
						sts = true;
						break;
					} else {
						sts = false;
					}
				}
			} else {
				break;
			}
		}
		if (sts) {
			for (int i=0; i<PlayInven.GetComponent<PlayerInventory2> ().ItemManufacture.Count; i++) {
				if (PlayInven.GetComponent<PlayerInventory2> ().ItemManufacture [i].Contains (nameitem + "_lock"))
					PlayInven.GetComponent<PlayerInventory2> ().ItemManufacture [i] = nameitem + "_unlock";
			}
			
			for (int j = 0; j < dieukien.Count; j++) {
				PlayInven.GetComponent<PlayerInventory2> ().ItemNormal.Remove (dieukien [j]); //xóa các item để lên huyết kiếm
			}	
			PlayInven.GetComponent<PlayerInventory2> ().ItemNormal.Add (nameitem);  
 
			txtDetailItem.text = CheckLanguage ("Upgraded " + nameitem, "Nâng cấp " + nameitem + " thành công ");

			buttonuse.SetActive (false); //disable button use đi
		} else {
			txtDetailItem.text = CheckLanguage ("Not enough item to upgrade", "Chưa đủ item để nâng cấp vật phẩm này");

		}
	}*/
	/********************Manufacture************************/

	/********************ItemKey************************/

	void LoadItemKey (List<string> item)
	{
		for (int i=0; i<item.Count; i++) {
			GameObject PrefabKeyItem = Instantiate (Resources.Load ("Item/KeyItem/" + item [i])) as GameObject;
			PrefabKeyItem.transform.SetParent (contentPanel);
			PrefabKeyItem.transform.localScale = Vector3.one;

			string contentitem = "";
			string nameitem = "";
			if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
				nameitem = PrefabKeyItem.GetComponent<ListString> ().ListStringDialogueVIE [0];
				contentitem = PrefabKeyItem.GetComponent<ListString> ().ListStringDialogueVIE [1];
			} else if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) {
				nameitem = PrefabKeyItem.GetComponent<ListString> ().ListStringDialogueENG [0];
				contentitem = PrefabKeyItem.GetComponent<ListString> ().ListStringDialogueENG [1];
			}		
			PrefabKeyItem.gameObject.transform.FindChild ("NameItem").gameObject.GetComponent<Text> ().text = nameitem;

			Button btnItem = PrefabKeyItem.GetComponent<Button> ();
			int tempInt = i;			
			btnItem.onClick.AddListener (() => ButtonDetaiItem (tempInt, contentitem));

			Button buttonUse = PrefabKeyItem.gameObject.transform.FindChild ("btnUse").gameObject.GetComponent<Button> ();
			buttonUse.onClick.AddListener (() => {
				string dulieutruyen = PrefabKeyItem.GetComponent<ListString> ().ListStringDialogueVIE [2];
				print ("click: " + dulieutruyen);
				if (dulieutruyen == "poison") {
					GameObject _player = GameObject.FindGameObjectWithTag ("Player");
					if (_player.GetComponent<PlayerInteractive> ().CurrentObject != null) {
						if (_player.GetComponent<PlayerInteractive> ().CurrentObject.name.Contains ("Meat")) {
							_player.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<ReadableItem> ().isPoisoned = true;
							_player.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<ReadableItem> ().gameObject.GetComponent<ListString> ().ListStringDialogueVIE [0] = "Miếng thịt đã được tẩm độc";
							//_player.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<Uni2DSprite> ().VertexColor = new Color (0.2f, 0.2f, 0.2f);
						}
					} else {
						txtDetailItem.text = CheckLanguage ("You can not use this item here!", "Bạn không thể sử dụng vật phẩm này ở đây!");
					}
				}
				if (dulieutruyen == "mousetrap") {
					//print ("đã đặt bẫy" + dulieutruyen);

					GameObject _MouseTrap = GameObject.Find ("MouseTrap");
					_MouseTrap.transform.FindChild ("MouseTrap").gameObject.SetActive (true);
					_MouseTrap.transform.FindChild ("MouseTrap").gameObject.GetComponent<Uni2DSprite> ().spriteAnimation.Play (1);
					_MouseTrap.GetComponent<FollowPlayer> ().enabled = false;
				}
				if (dulieutruyen == "trap") {
					GameObject _TrapPlayer = GameObject.Find ("TrapPlayer");
					_TrapPlayer.GetComponent<FollowPlayer> ().enabled = false;

					GameObject _Trap = _TrapPlayer.transform.FindChild ("Trap").gameObject;

					_Trap.SetActive (true); 
				}
				if (dulieutruyen != "poison") {
					item.Remove (item [tempInt]);  
					buttonUse.gameObject.SetActive (false);
				}
			});
		}
	}
	/********************ItemKey************************/

	/********************Necessity************************/
	//listring: 
	//0-> nameitem; 1-> description item; 2: giá trị của item đó (0.01 health); 3: phân loại item đó (oil, health)
	void LoadItemNecessity (List<string> item)
	{
		for (int i=0; i<item.Count; i++) {
			string[] val = item [i].Split ('_');
			GameObject PrefabItemNecessity = Instantiate (Resources.Load ("Item/Necessity/" + val [0])) as GameObject;
			PrefabItemNecessity.transform.SetParent (contentPanel);
			PrefabItemNecessity.transform.localScale = Vector3.one;

			string contentitem = "";
			string nameitem = "";
			if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
				nameitem = PrefabItemNecessity.GetComponent<ListString> ().ListStringDialogueVIE [0];
				contentitem = PrefabItemNecessity.GetComponent<ListString> ().ListStringDialogueVIE [1];
			} else if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) {
				nameitem = PrefabItemNecessity.GetComponent<ListString> ().ListStringDialogueENG [0];
				contentitem = PrefabItemNecessity.GetComponent<ListString> ().ListStringDialogueENG [1];
			}		
			PrefabItemNecessity.gameObject.transform.FindChild ("NameItem").gameObject.GetComponent<Text> ().text = nameitem;
			
			Button btnItem = PrefabItemNecessity.GetComponent<Button> ();
			int tempInt = i;			
			btnItem.onClick.AddListener (() => ButtonDetaiItem (tempInt, contentitem));

			GameObject amount = PrefabItemNecessity.gameObject.transform.FindChild ("txtsl").gameObject;
			Text txtsl = amount.GetComponent<Text> ();
			txtsl.text = val [1]; 		

			Button tempButton = PrefabItemNecessity.GetComponent<Button> ();			
			tempButton.onClick.AddListener (() => ButtonDetaiItem (0, contentitem));
			
			//GameObject giatri = PrefabItemNecessity.gameObject.transform.FindChild("float").gameObject; //giá trị	
			float giatri = float.Parse (PrefabItemNecessity.GetComponent<ListString> ().ListStringDialogueENG [2]);
			string type = PrefabItemNecessity.GetComponent<ListString> ().ListStringDialogueENG [3];
			string nameDisplay = "";
			if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) {
				nameDisplay = PrefabItemNecessity.GetComponent<ListString> ().ListStringDialogueENG [0];
			} else if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
				nameDisplay = PrefabItemNecessity.GetComponent<ListString> ().ListStringDialogueVIE [0];
			}			
			GameObject btnuse = PrefabItemNecessity.gameObject.transform.FindChild ("use").gameObject;			
			Button buse = btnuse.GetComponent<Button> ();
			buse.onClick.AddListener (() => btnNecessity (0, val [0], txtsl, giatri, type, nameDisplay));			
		}
	}

	void btnNecessity (int buttonNo, string nameItem, Text txtsl, float giatri, string type, string nameDisplay)
	{
		int sl = getAmount (PlayInven.GetComponent<PlayerInventory2> ().ItemNecessity, nameItem); //lấy số lượng
		//Debug.Log ("sô lượng "+namevatpham + " hiện có: " +sl);
		if (sl == 0) {
			//Debug.Log("chua có vật phẩm này");
			txtDetailItem.text = CheckLanguage ("You don't have any left", "Không đủ dùng");
		} else {
			for (int i=0; i<PlayInven.GetComponent<PlayerInventory2> ().ItemNecessity.Count; i++) {
				if (PlayInven.GetComponent<PlayerInventory2> ().ItemNecessity [i].Contains (nameItem + "_" + sl)) {
					PlayInven.GetComponent<PlayerInventory2> ().ItemNecessity [i] = nameItem + "_" + (sl - 1); 

					txtDetailItem.text = CheckLanguage ("You had used " + nameDisplay, "Bạn đã sử dụng " + nameDisplay);

					txtsl.text = (sl - 1).ToString ();  
					GameObject _player = GameObject.FindGameObjectWithTag ("Player");
					switch (type) {
					case "oil":
						PlayInven.GetComponent<Status> ().changeOil (0.75f);
						PlayInven.GetComponent<PlayerController> ().lampObject.transform.FindChild ("Area light Player").gameObject.GetComponent<Light> ().enabled = true;
						PlayInven.GetComponent<PlayerController> ().lampObject.transform.FindChild ("Point light").gameObject.GetComponent<Light> ().enabled = true;
						break;
					case "pekish":
						PlayInven.GetComponent<Status> ().changePeckish (giatri);
						break;
					case "mouse":
						PlayInven.GetComponent<Status> ().changePeckish (giatri);
						PlayInven.GetComponent<Status> ().changeSpirit (0.1f);
						break;
					case "health":
						_player.gameObject.GetComponent<Status> ().changeHealth (-0.60f);
						break;
					case "bandage": 
						Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
						GameObject _red = _canvas.Find ("RedPanel").gameObject;
						_red.SetActive (false);
						//GameObject _player = GameObject.FindGameObjectWithTag ("Player");
						if (_player.gameObject.GetComponent<PlayerController> ().isWounded_Head) {
							_player.gameObject.GetComponent<PlayerController> ().isWounded_Head = false;
							_player.gameObject.GetComponent<PlayerController> ().HeadPlusIndex = 5;
						}
						if (_player.gameObject.GetComponent<PlayerController> ().isWounded_Leg) {
							_player.gameObject.GetComponent<PlayerController> ().isWounded_Leg = false;
							_player.gameObject.GetComponent<PlayerController> ().LegPlusIndex = 2;
						}
						break;
					} 
					break;
				}				
			}
		}
	}

	int getAmount (List<string> l, string namevatpham)
	{
		int sl = 0; 
		for (int i=0; i<l.Count; i++) {
			string[] val = l [i].Split ('_');
			if (val [0] == namevatpham) {
				sl = int.Parse (val [1]);
			}			
		}
		return sl;
	}
	/********************Necessity************************/

	string CheckLanguage (string en, string vi)
	{
		if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
			return vi;
		} else if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) 
			return en;
		return "";
	}
}
