using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class ListString : MonoBehaviour {
	public List<string> ListStringDialogue = new List<string> ();

	[Header("Vietnamese")]
	public List<string>
		ListStringDialogueVIE = new List<string> ();
	[Header("English")]
	public List<string>
		ListStringDialogueENG = new List<string> ();
	void Start ()
	{
		if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
			ListStringDialogue = ListStringDialogueVIE;
		} else
		if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) {
			ListStringDialogue = ListStringDialogueENG;
		}
	}
}
