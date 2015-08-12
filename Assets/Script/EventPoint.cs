using UnityEngine;
using System.Collections;

public class EventPoint : MonoBehaviour
{
	public enum EventRoles
	{ 
		Activater,
		Receiver
	}

	public enum EventType
	{ 
		Destroy,
		OnOffLight,
		OnOffAllElectric,
		Scare,
		Active
	}
	private AudioManager audioManager;
	public string EventName;
	public EventRoles EventRole;
	public EventType ReceiverEventType;

	// Destroy
	public bool isDestroyOnStart;
	public bool isDestroyActivater;
	// OnOffAllElectric && OnOffLight
	public bool isActiveObject;

	void Start ()
	{
		NotificationManager.Instance.AddListener (this, "Save");
		NotificationManager.Instance.AddListener (this, "Load");

		audioManager = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		//if (this.EventRole == EventRoles.Activater && this.ReceiverEventType == EventType.OnOffAllElectric) {
		//CommonVariable.Instance.isElectricOn = isTurnOn;
		//}
		if (this.isDestroyOnStart)
			this.gameObject.SetActive (false);
		else
		if (this.EventRole == EventRoles.Receiver && this.ReceiverEventType == EventType.OnOffAllElectric) {
			if (this.transform.GetChild (0).name.Contains ("Light")) {
				if (this.transform.GetChild (0).gameObject.activeSelf)
					isActiveObject = true;
				else
					isActiveObject = false;
			}
		}
		//
		EventManager.Instance.AddEvent (this, EventName);
	}

	public void Active ()
	{
		if (EventRole == EventRoles.Activater) {
			if (this.ReceiverEventType == EventType.OnOffAllElectric) {
				if (CommonVariable.Instance.isElectricOn) {
					CommonVariable.Instance.isElectricOn = false;
				} else {
					CommonVariable.Instance.isElectricOn = true;
				}
			}
		}
		StartCoroutine (waitForActiveRoom (0.1f));
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
		CommonVariable.Instance.currentEventName = this.EventName;
		EventManager.Instance.PostEvent (this, EventName);
		yield return new WaitForSeconds (second);
		_maincamera.gameObject.GetComponent<RoomManager> ().enabled = true;
		if (isDestroyActivater && EventRole == EventRoles.Activater) {
			Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
			Transform _bubbletalk = _canvas.Find ("TalkBubble");
			_bubbletalk.gameObject.SetActive (false);
			this.gameObject.SetActive (false);
		}
	}

	public void Receive ()
	{
		if (CommonVariable.Instance.currentEventName == this.EventName) {
			if (EventRole == EventRoles.Receiver) {
				if (this.EventRole == EventRoles.Receiver) {
					// for Destroy
					if (this.ReceiverEventType == EventType.Destroy) {
						if (this.gameObject != null && this.gameObject.activeSelf) {
							this.isDestroyOnStart = true;
							foreach (Transform child in this.transform) {
								child.gameObject.SetActive (false);
							}
							if (this.gameObject.GetComponent<BoxCollider2D> () != null)
								this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
							if (this.gameObject.GetComponent<MeshRenderer> () != null)
								this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
							else if (this.gameObject.GetComponent<SpriteRenderer> () != null)
								this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
						}

					} else
					// for On/Off light
					if (this.ReceiverEventType == EventType.OnOffLight) {
						if (CommonVariable.Instance.isElectricOn) {		
							if (this.transform.GetChild (0).gameObject.activeSelf) {
								this.transform.GetChild (0).gameObject.SetActive (false);
							} else {
								this.transform.GetChild (0).gameObject.SetActive (true);
							}
						}
					} else
					// for On/Off electric
					if (this.ReceiverEventType == EventType.OnOffAllElectric) {
						/*if (!CommonVariable.Instance.isElectricOn) {
							if (this.transform.GetChild (0).gameObject.activeSelf) {
								this.isActiveObject = true;
							} else {
								this.isActiveObject = false;
							}
						}*/
						if (CommonVariable.Instance.isElectricOn) {
							//if (this.isActiveObject)
								this.transform.GetChild (0).gameObject.SetActive (true);
						} else {
							this.transform.GetChild (0).gameObject.SetActive (false);
						}	
					} else
					// for Scare
					if (this.ReceiverEventType == EventType.Scare) {
						StartCoroutine (waitForScare (1f));
						audioManager.au_scream.Play ();
					}
					// for Active
					if (this.ReceiverEventType == EventType.Active) {
						this.isDestroyOnStart = true;
						foreach (Transform child in this.transform) {
							child.gameObject.SetActive (true);
						}
						if (this.gameObject.GetComponent<MeshRenderer> () != null)
							this.gameObject.GetComponent<MeshRenderer> ().enabled = true;
						if (this.gameObject.GetComponent<BoxCollider2D> () != null)
							this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
					}
				}
			}
		}
	}

	IEnumerator waitForScare (float second)
	{
		this.transform.GetChild (0).gameObject.SetActive (true);
		yield return new WaitForSeconds (second);
		this.transform.GetChild (0).gameObject.SetActive (false);
	}

	public void Save ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (isDestroyOnStart, this.gameObject.name + "EventPoint" + i + "?tag=isDestroyOnStart" + i);  
		//print ("save: " + isDestroyOnStart + "___" + this.gameObject.name);
		//ES2.Save (giveItem, this.gameObject.name + "EventPoint" + i + "?tag=giveItem" + i);
		//ES2.Save (currentDialogue.ToString (), this.gameObject.name + "EventPoint" + i + "?tag=currentDialogue" + i); 
	}
	
	public void Load ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "EventPoint" + i)) {
			isDestroyOnStart = ES2.Load<bool> (this.gameObject.name + "EventPoint" + i + "?tag=isDestroyOnStart" + i); 
			//giveItem = ES2.Load<string> (this.gameObject.name + "MapItem" + i + "?tag=giveItem" + i);
			//string _currentDialogue = ES2.Load<string> (this.gameObject.name + "MapItem" + i + "?tag=currentDialogue" + i);
			//if (_currentDialogue == DialogueType.DefaultDialogue.ToString())
			//this.currentDialogue = DialogueType.DefaultDialogue;
			//else if (_currentDialogue == DialogueType.SecondDialogue.ToString())
			//this.currentDialogue = DialogueType.SecondDialogue;
			if (isDestroyOnStart && this.EventRole == EventRoles.Receiver && this.ReceiverEventType == EventType.Destroy) {
				foreach (Transform child in this.transform) {
					child.gameObject.SetActive (false);
				}
				if (this.gameObject.GetComponent<BoxCollider2D> () != null)
					this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
				if (this.gameObject.GetComponent<MeshRenderer> () != null)
					this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
				if (this.gameObject.GetComponent<SpriteRenderer> () != null)
					this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
			if (isDestroyOnStart && this.EventRole == EventRoles.Receiver && this.ReceiverEventType == EventType.Active) {
				foreach (Transform child in this.transform) {
					child.gameObject.SetActive (true);
				}
				if (this.gameObject.GetComponent<BoxCollider2D> () != null)
					this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
				if (this.gameObject.GetComponent<MeshRenderer> () != null)
					this.gameObject.GetComponent<MeshRenderer> ().enabled = true;
				if (this.gameObject.GetComponent<SpriteRenderer> () != null)
					this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			} 
		}//else { 
		//print ("chưa có");
		//}
	}
}
