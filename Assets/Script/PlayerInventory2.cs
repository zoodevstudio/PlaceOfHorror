using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInventory2 : MonoBehaviour
{

	public List<string> ItemNormal = new List<string> ();
	public List<string> ItemKey = new List<string> ();
	//	public List<string> ItemManufacture = new List<string> (); //close manufac
	public List<string> ItemNecessity = new List<string> (); 
	//public int[] Keys = new int[3];

	public GameObject panelKey;

	// Use this for initialization
	void Start ()
	{
		NotificationManager.Instance.AddListener (this, "Save");
		NotificationManager.Instance.AddListener (this, "Load");

		//<<
		Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
		Transform _button = _canvas.FindChild ("Button");
		Transform _lamp = _button.FindChild ("LampEnergy");
		if (!ItemNormal.Contains ("Lamp"))
			_lamp.gameObject.SetActive (false);
		else
			_lamp.gameObject.SetActive (true);
		//>>

	}

	public void reStart ()
	{
		Start ();
	}

	public void Save ()
	{
		string i = CommonVariable.Instance.loadi; 
		//ES2.Save("ItemList"+i, "ItemList"+i+"?tag="+i);
		ES2.Save (ItemNormal, "ItemNormal.txt?tag=" + i);
		ES2.Save (ItemKey, "ItemKey.txt?tag=" + i);
		//ES2.Save (ItemManufacture, "ItemManufacture.txt?tag=" + i); //close manufac
		ES2.Save (ItemNecessity, "ItemNecessity.txt?tag=" + i);
	}
	//khởi tạo, new game-> là load 0 nhé
	public void Load ()
	{
		//print ("thực hiện load dl");
		string i = CommonVariable.Instance.loadi; 
		//if(ES2.Exists ("ItemList"+i)){ //k cần check nữa vì i sang là chắc chắn phải có, k có thì nó là 0 mà k thì dc khởi tạo rồi
		ItemNormal = ES2.LoadList<string> ("ItemNormal.txt?tag=" + i);
		ItemKey = ES2.LoadList<string> ("ItemKey.txt?tag=" + i);
		//ItemManufacture = ES2.LoadList<string> ("ItemManufacture.txt?tag=" + i);
		ItemNecessity = ES2.LoadList<string> ("ItemNecessity.txt?tag=" + i);
		if (ItemNormal.Contains ("Lamp")) {
			Transform _button = GameObject.Find ("Button").transform;
			GameObject _lamp = _button.Find ("LampEnergy").gameObject;
			_lamp.SetActive (true);
		}
		//}
		//else k có gì thì gán giá trị mạc định
	}

	//nameitem: "kiembf"
	public void AddItemNormal (string nameitem)
	{
		//có rồi thì k add nữa
		if (!ContainsChild (ItemNormal, nameitem))
			ItemNormal.Add (nameitem);  
	}

	//close manufac
	/*public void AddItemManufacture(string nameitem){
		if (!ContainsChild (ItemManufacture, nameitem))  
			ItemManufacture.Add (nameitem+"_lock"); //phai khai bao item nay o menu 
	}*/ 

	//namekey: "den"
	public void AddItemKey (string namekey)
	{
		//có rồi thì k add nữa
		if (!ContainsChild (ItemKey, namekey))
			ItemKey.Add (namekey);  
	}

	//namenece: "banhbao" 
	public void AddItemNecessity (string namenece)
	{ 
		if (ContainsChild (ItemNecessity, namenece)) {		 
			for (int i=0; i<ItemNecessity.Count; i++) {
				string[] val = ItemNecessity [i].Split ('_'); 
				if (val [0] == namenece) {
					int sl = int.Parse (val [1]) + 1;
					ItemNecessity [i] = val [0] + "_" + sl;   //có dạng : "banhbao_n"
					break;
				}
			}
		} else { 
			ItemNecessity.Add (namenece + "_1"); //có dạng : "banhbao_1"
		}			
	}

	public bool ContainsChild (List<string> a, string b)
	{
		foreach (string c in a) {
			if (c.Contains (b))
				return true;
		}
		return false;
	}
}
