using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class MapItem : MonoBehaviour
{ 
	public bool isDestroyOnStart;
	// true = remove take item from character inventory && false = do not
	public bool isDestroyTakeItem;
	// character need take item already in inventory to take give item
	// if takeitem == "" character can get giveitem free
	public string takeItem;
	public List<string> takeItems;
	// Add give item intop character inventory
	public string giveItem;
	// type of give item
	public enum ItemType
	{
		Normal,
		Key,
		Manufacture ,
		Necessity
	}
	public ItemType GiveItemType;

	// true = destroy this object from the game after take give item && false = do not
	public bool isDestroyObject;

	// current status of this item
	public enum DialogueType
	{
		DefaultDialogue,
		SecondDialogue,
		Destroyed
	}
	public DialogueType currentDialogue;
	public List<string> DefaultDialogue = new List<string> ();
	public List<string> GetItemDialogue = new List<string> ();
	public List<string> SecondDialogue = new List<string> ();
	[Header("Vietnamese")]
	public List<string>
		DefaultDialogueVIE = new List<string> ();
	public List<string> GetItemDialogueVIE = new List<string> ();
	public List<string> SecondDialogueVIE = new List<string> ();
	[Header("English")]
	public List<string>
		DefaultDialogueENG = new List<string> ();
	public List<string> GetItemDialogueENG = new List<string> ();
	public List<string> SecondDialogueENG = new List<string> ();

	void Start ()
	{
		if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
			DefaultDialogue = DefaultDialogueVIE;
			GetItemDialogue = GetItemDialogueVIE;
			SecondDialogue = SecondDialogueVIE;
		} else
		if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) {
			DefaultDialogue = DefaultDialogueENG;
			GetItemDialogue = GetItemDialogueENG;
			SecondDialogue = SecondDialogueENG;
		}		

		NotificationManager.Instance.AddListener (this, "Save");
		NotificationManager.Instance.AddListener (this, "Load");
	}

	public void DestroyMapItem ()
	{
		foreach (Transform child in this.gameObject.transform) {
			child.gameObject.SetActive (false);
		}
		if (this.gameObject.GetComponent<Uni2DSprite> () != null) {
			this.gameObject.GetComponent<Uni2DSprite> ().enabled = false;
			this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
		} else {
			this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		}
		this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
	}

	public void Save ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (isDestroyOnStart, this.gameObject.name + "MapItem" + i + "?tag=isDestroyOnStart" + i);  
		//print ("save: " + isDestroyOnStart + "___" + this.gameObject.name);
		ES2.Save (giveItem, this.gameObject.name + "MapItem" + i + "?tag=giveItem" + i);
		ES2.Save (currentDialogue.ToString (), this.gameObject.name + "MapItem" + i + "?tag=currentDialogue" + i); 
	}

	public void Load ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "MapItem" + i)) {
			isDestroyOnStart = ES2.Load<bool> (this.gameObject.name + "MapItem" + i + "?tag=isDestroyOnStart" + i); 
			giveItem = ES2.Load<string> (this.gameObject.name + "MapItem" + i + "?tag=giveItem" + i);
			string _currentDialogue = ES2.Load<string> (this.gameObject.name + "MapItem" + i + "?tag=currentDialogue" + i);
			if (_currentDialogue == DialogueType.DefaultDialogue.ToString ())
				this.currentDialogue = DialogueType.DefaultDialogue;
			else if (_currentDialogue == DialogueType.SecondDialogue.ToString ())
				this.currentDialogue = DialogueType.SecondDialogue;
			if (isDestroyOnStart) {
				this.DestroyMapItem ();
			} 
		}//else { 
		//print ("chưa có");
		//}
	}

}
