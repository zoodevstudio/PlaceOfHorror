using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayerInteractive : MonoBehaviour
{

	private AudioManager audioManager;
	public GameObject MainCamera;
	public GameObject btnButton;
	public GameObject btnChatbox;
	public int Direction;
	// Kiểm tra Player đang chuyển phòng
	public bool isMovingRoom;
	public bool isAttractAttention;
	// Object hiển thị thông báo 
	public GameObject TalkBubble;

	// Object mà character đang đè lên
	public GameObject CurrentObject;
	public GameObject CurrentBox;
	// Vá lỗi Enter xảy ra trước Exit khi chuyển cửa
	private string CheckDoor;
	public bool isVisible;

	// Audio


	void Start ()
	{
		audioManager = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		isMovingRoom = false;
		NotificationManager.Instance.AddListener (this, "OnAction");
	}

	void OnAction ()
	{
		if (GetComponent<ItemPriority> ().HitObjectsList.Count > 0)
		if (GetComponent<ItemPriority> ().HitObjectsList [0]._Collider2D != null)
			CurrentObject = GetComponent<ItemPriority> ().HitObjectsList [0]._Collider2D.gameObject;
		else 
			CurrentObject = GetComponent<ItemPriority> ().HitObjectsList [0]._Collision2D.gameObject;
		else
			CurrentObject = null;
		if (CurrentObject != null) 
		if (CurrentObject.gameObject.activeSelf == false && CurrentObject.gameObject.tag == "ReadableItem") {
			this.GetComponent<ItemPriority> ().RemoveAtList (4);
//			for (int i=GetComponent<ItemPriority>().HitObjectsList.Count - 1; i > -1; i--) {
//				if (GetComponent<ItemPriority> ().HitObjectsList [i]._Priority == 3) {
//					GetComponent<ItemPriority> ().HitObjectsList.RemoveAt (i);
//				}
//			}
		}

		if (CurrentObject != null && CommonVariable.Instance.btn_Action == "ActionButtonDown" 
			&& this.GetComponent<PlayerController> ().playerState != PlayerController.PlayerState.Die) {
			// Remove btn_Action's Data
			CommonVariable.Instance.btn_Action = "";
			CommonVariable.Instance.btn_Move = "";
			
			// Chạy Event nếu trong object đó có chứa Activater
			if (CurrentObject.gameObject.GetComponent<EventPoint> () != null) {
				if (CurrentObject.gameObject.GetComponent<EventPoint> ().EventRole == EventPoint.EventRoles.Activater)
					CurrentObject.gameObject.GetComponent<EventPoint> ().Active ();
				if (CurrentObject.name == "ElectricControl Main") {
					audioManager.au_hard_drive_shut_down.Play ();
				} else
				if (CurrentObject.name.Contains ("ElectricControl ")) {
					audioManager.au_switch_on_off.Play ();
				} 


			}
			// Chạy đổi ảnh nếu có ImageChange trong object đó
			if (CurrentObject.gameObject.GetComponent<ChangeImage> () != null) {
				CurrentObject.gameObject.GetComponent<ChangeImage> ().Change ();
				//this.gameObject.GetComponent<ItemPriority> ().HitObjectsList.Clear ();
				//this.CurrentObject = null;
			}
			// Chạy string  nếu có ListString trong object đó 
			if (CurrentObject.gameObject.GetComponent<ListString> () != null) {
				GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
				GameObject chatbox = canvas.transform.FindChild ("Chatbox").gameObject;
				chatbox.GetComponent<BtnChatbox> ().addChatboxDialogue (CurrentObject.gameObject.GetComponent<ListString> ().ListStringDialogue);
				chatbox.GetComponent<BtnChatbox> ().ChatBoxActive ();
			}

			//Đoạn code chưa hoàn thiện
			// Apply Sleep when tap Action Button
			if (CurrentObject.tag == "Bed") {
				if (!this.GetComponent<PlayerController> ().isAction && !this.GetComponent<PlayerController> ().isWounded_Head) {
					this.GetComponent<PlayerController> ().isAction = true;
					this.GetComponent<Animator> ().CrossFade ("Walking", 0.1f);
					this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.MovingTo;
				} else {
					//this.GetComponent<Animator> ().CrossFade ("Standing", 0.1f);
					this.GetComponent<PlayerController> ().isAction = false;
					//this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Standing;
				}
			} else
			if (CurrentObject.tag == "Cabinet") {
				if (!this.GetComponent<PlayerController> ().isAction && !this.GetComponent<PlayerController> ().isWounded_Head) {
					this.GetComponent<PlayerController> ().isAction = true;
					this.GetComponent<Animator> ().CrossFade ("Walking", 0.1f);
					//CurrentObject.GetComponent<Animator> ().Play ("CabinetOpen");
					this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.MovingTo;
					StartCoroutine (HiddenPlayer (1.5f, "Default", 7));
				} else {
					if (this.GetComponent<PlayerController> ().isHiding) {
						this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Sleeping;
						this.GetComponent<PlayerController> ().EnablezZSleeping ();
//						audioManager.au_cabinet_open.Play ();
//						this.GetComponent<Animator> ().CrossFade ("Reverse_Hiding", 0.1f);
//						//OrderInLayer("Default",-1);
//						StartCoroutine (HiddenPlayer (0.7f, "Layer 2", -1));
//						CurrentObject.GetComponent<Animator> ().Play ("CabinetOpen");
//						this.GetComponent<PlayerController> ().isAction = false;
//						this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Standing;
					}
				}
			} else
			if (CurrentObject.tag == "Desk") {
				if (!this.GetComponent<PlayerController> ().isAction) {
					this.GetComponent<PlayerController> ().isAction = true;
					this.GetComponent<Animator> ().CrossFade ("Walking", 0.1f);
					//this.GetComponent<Animator> ().CrossFade ("Standing_PushAndPull", 0.1f);
					this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.MovingTo;
				
				} else {
					this.GetComponent<Animator> ().SetLayerWeight (3, 0);
					this.GetComponent<Animator> ().SetLayerWeight (2, 0);
					this.GetComponent<PlayerController> ().isAction = false;
					this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Standing;
					this.GetComponent<PlayerController> ().moveSpeed = this.GetComponent<PlayerController> ().defaultMoveSpeed;
				}
			} else

				// Đặt và nhặt bẫy AI đã có trong kho
			if (CurrentObject.name == "Trap") {
				GameObject _TrapPlayer = CurrentObject.transform.parent.gameObject;
				CurrentObject.gameObject.SetActive (false);
				_TrapPlayer.GetComponent<FollowPlayer> ().enabled = true;
				//this.GetComponent<ItemPriority> ().HitObjectsList.Clear ();
				this.GetComponent<ItemPriority> ().RemoveAtList (2);
				this.GetComponent<PlayerInventory2> ().AddItemKey ("FullTrap");
			} else
			if (CurrentObject.name == "MouseTrap") {
				// nếu bẫy đã bắt được chuột
				if (CurrentObject.GetComponent<MouseTrap> ().Rat != null) {
					CurrentObject.GetComponent<MouseTrap> ().isCapturedMouse = false;
					Destroy (CurrentObject.GetComponent<MouseTrap> ().Rat.gameObject);
					CurrentObject.SetActive (false);
					GameObject MouseTrap = GameObject.Find ("MouseTrap");
					MouseTrap.GetComponent<FollowPlayer> ().enabled = true;
					Debug.Log ("Đã nhặt bẫy + chuột");
					this.gameObject.GetComponent<PlayerInventory2> ().AddItemNecessity ("Rat");
					this.GetComponent<PlayerInventory2> ().AddItemKey ("MouseTrap");
					//CurrentObject.GetComponent<MouseTrap> ().Rat.GetComponent<Mouse>().destroyRat();
				} 
				// nếu bẫy không có chuột
				else {
					// them lai vao inventory
					CurrentObject.SetActive (false);
					GameObject MouseTrap = GameObject.Find ("MouseTrap");
					MouseTrap.GetComponent<FollowPlayer> ().enabled = true;
					Debug.Log ("them lai vao Inventory");
					this.GetComponent<PlayerInventory2> ().AddItemKey ("MouseTrap");
				}
				this.GetComponent<ItemPriority> ().RemoveAtList (1);
				//this.GetComponent<ItemPriority> ().HitObjectsList.Clear ();
				
			} else
			if (CurrentObject.tag == "ReadableItem") {
				if (CurrentObject.GetComponent<MapItem> () != null) {
					GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
					GameObject chatbox = canvas.transform.FindChild ("Chatbox").gameObject;
					//	<Check
					// if map item do not have give item then display current dialogue
					if (CurrentObject.GetComponent<MapItem> ().giveItem == "") {
						if (CurrentObject.GetComponent<MapItem> ().currentDialogue == MapItem.DialogueType.DefaultDialogue)
							chatbox.GetComponent<BtnChatbox> ().addChatboxDialogue (CurrentObject.GetComponent<MapItem> ().DefaultDialogue);
						else
							if (CurrentObject.GetComponent<MapItem> ().currentDialogue == MapItem.DialogueType.SecondDialogue)
							chatbox.GetComponent<BtnChatbox> ().addChatboxDialogue (CurrentObject.GetComponent<MapItem> ().SecondDialogue);
					} else {
						// if map item have give item but dont need take item
						// or
						// if map item have give item and inventory already have take item
						// this is what happen to character when she take the give item
						if ((CurrentObject.GetComponent<MapItem> ().takeItem == "" && CurrentObject.GetComponent<MapItem> ().takeItems.Count == 0) ||
							(CurrentObject.GetComponent<MapItem> ().takeItem != "" && CurrentObject.GetComponent<MapItem> ().takeItems.Count == 0 &&
							this.gameObject.GetComponent<PlayerInventory2> ().ItemNormal.Contains (CurrentObject.GetComponent<MapItem> ().takeItem)) ||
							(CurrentObject.GetComponent<MapItem> ().takeItem == "" && checkTakeItems (CurrentObject.GetComponent<MapItem> ().takeItems))) {
							// Display dialogue
							List<string> temp_list = new List<string> ();
							string temp_string = "";
							if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
								temp_string = "Bạn tìm thấy <color=red>" + CurrentObject.GetComponent<MapItem> ().GetItemDialogue [0] + "</color>";
							} else if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) {
								if (CurrentObject.GetComponent<MapItem> ().GetItemDialogue [0] [0] == 'U' ||
									CurrentObject.GetComponent<MapItem> ().GetItemDialogue [0] [0] == 'E' ||
									CurrentObject.GetComponent<MapItem> ().GetItemDialogue [0] [0] == 'O' ||
									CurrentObject.GetComponent<MapItem> ().GetItemDialogue [0] [0] == 'A' ||
									CurrentObject.GetComponent<MapItem> ().GetItemDialogue [0] [0] == 'I')
									temp_string = "You got an <color=red>" + CurrentObject.GetComponent<MapItem> ().GetItemDialogue [0] + "</color>";
								else
									temp_string = "You got a <color=red>" + CurrentObject.GetComponent<MapItem> ().GetItemDialogue [0] + "</color>";
							}
							temp_list.Add (temp_string);
							chatbox.GetComponent<BtnChatbox> ().addChatboxDialogue (temp_list);
							
							// Add give item into inventory
							switch (CurrentObject.GetComponent<MapItem> ().GiveItemType) {
							case MapItem.ItemType.Normal:
								this.gameObject.GetComponent<PlayerInventory2> ().AddItemNormal (CurrentObject.GetComponent<MapItem> ().giveItem);
								this.gameObject.GetComponent<PlayerInventory2> ().reStart ();
								break;
							case MapItem.ItemType.Key:
								this.gameObject.GetComponent<PlayerInventory2> ().AddItemKey (CurrentObject.GetComponent<MapItem> ().giveItem);
								break;
							//case MapItem.ItemType.Manufacture: //////////////////del
							//this.gameObject.GetComponent<PlayerInventory2> ().ItemManufacture.Add (CurrentObject.GetComponent<MapItem> ().giveItem);
							//break;
							case MapItem.ItemType.Necessity: 
								this.gameObject.GetComponent<PlayerInventory2> ().AddItemNecessity (CurrentObject.GetComponent<MapItem> ().giveItem);
								break;
							}
							// Delete give item from map item
							CurrentObject.GetComponent<MapItem> ().giveItem = "";
							CurrentObject.GetComponent<MapItem> ().currentDialogue = MapItem.DialogueType.SecondDialogue;

							// Check if remove take item from inventory
							if (CurrentObject.GetComponent<MapItem> ().isDestroyTakeItem) {
								this.gameObject.GetComponent<PlayerInventory2> ().ItemNormal.Remove (CurrentObject.GetComponent<MapItem> ().takeItem);
							}
							// Check if destroy current object
							if (CurrentObject.GetComponent<MapItem> ().isDestroyObject) {
								CurrentObject.GetComponent<MapItem> ().isDestroyOnStart = true;
								CurrentObject.GetComponent<MapItem> ().currentDialogue = MapItem.DialogueType.Destroyed;
								foreach (Transform child in CurrentObject.transform) {
									child.gameObject.SetActive (false);
								}
								//CurrentObject.gameObject.SetActive (false);
								CurrentObject.GetComponent<MapItem> ().DestroyMapItem ();
								GetComponent<ItemPriority> ().HitObjectsList.Clear ();
								this.CurrentObject = null;
								TalkBubble.SetActive (false);
							}
						} else {
							chatbox.GetComponent<BtnChatbox> ().addChatboxDialogue (CurrentObject.GetComponent<MapItem> ().DefaultDialogue);
						}
					}
					// Check>
					chatbox.GetComponent<BtnChatbox> ().ChatBoxActive ();
				}
			} else
				// Open the Door when tap Action Button
			if (CurrentObject.tag == "Door") {
				if (CurrentObject.gameObject.GetComponent<End> () != null)
					CurrentObject.gameObject.GetComponent<End> ().EndGame ();
				if (CurrentObject.gameObject.GetComponent<Door> () != null) {
					GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
					GameObject chatbox = canvas.transform.FindChild ("Chatbox").gameObject;
					Door door = CurrentObject.gameObject.GetComponent<Door> ();
					// If the door is broken
					if (door.isBroken) {
						List<string> temp_list = new List<string> ();
						if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese) {
							string temp_string1 = "Cánh cửa này đã bị hỏng rồi...";
							string temp_string2 = "...Có vẻ nó sẽ không bao giờ mở ra được nữa!";
							temp_list.Add (temp_string1);
							temp_list.Add (temp_string2);
						} else if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English) {
							string temp_string1 = "This door has been broken ...";
							string temp_string2 = "...  never Open!";
							temp_list.Add (temp_string1);
							temp_list.Add (temp_string2);
						}
						chatbox.GetComponent<BtnChatbox> ().addChatboxDialogue (temp_list);
						chatbox.GetComponent<BtnChatbox> ().ChatBoxActive ();
					} else {					
						// If the door is unlock
						if (!door.isLock && !door.isLockNumber) {
							if (CurrentObject.name.Contains ("Room_Door") || CurrentObject.name.Contains ("Side_Door"))
								audioManager.au_door_open.Play ();
							NotificationManager.Instance.PostNotification (this, "AfterAction");
							if (CurrentObject.GetComponent<Animator> () != null) {
								this.GetComponent<Animator> ().CrossFade ("OpenTheDoor", 0.1f);
							} else {
								this.GetComponent<Animator> ().CrossFade ("Walking", 0.1f);
							}


							//Wait for second 
							StartCoroutine (GotoNextDoorWithTime (0.5f));
							if (CurrentObject.GetComponent<Animator> () != null) {
								CurrentObject.GetComponent<Animator> ().CrossFade ("DoorOpen", 0.1f);
							}
							GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.OpenTheDoor;
						}
						// If the door is locked
						else if (door.isLock) {
							// Check the inventory
							// If had key
							if (!chatbox.activeSelf)
								audioManager.au_door_locking.Play ();
							if (this.gameObject.GetComponent<PlayerInventory2> ().ItemNormal.Contains (door.Key)) {
								List<string> temp_list = new List<string> ();
								string temp_string = "Bạn đã mở khóa cánh cửa này!";
								temp_list.Add (temp_string);
								chatbox.GetComponent<BtnChatbox> ().addChatboxDialogue (temp_list);
								chatbox.GetComponent<BtnChatbox> ().ChatBoxActive ();
								if (!chatbox.activeSelf || CommonVariable.Instance.btn_ChatBox == "ChatBoxButtonDown")
									door.isLock = false;
							}
							// If not had key
							else {
								if (!chatbox.activeSelf)
									audioManager.au_door_locking.Play ();

								List<string> temp_list = new List<string> ();
								string temp_string = "";
								if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.Vietnamese)
									temp_string = "Cánh cửa này đã bị khóa!";
								else if (CommonVariable.Instance.GameLanguage == CommonVariable.Language.English)
									temp_string = "The door has been locked!";
								temp_list.Add (temp_string);
								chatbox.GetComponent<BtnChatbox> ().addChatboxDialogue (temp_list);
								chatbox.GetComponent<BtnChatbox> ().ChatBoxActive ();
							}
						} else if (door.isLockNumber) {
							Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
							Transform _panelkey = _canvas.FindChild ("Panel 1");
							GameObject _panel = _panelkey.FindChild ("PanelKey").FindChild ("Panel").gameObject;
							_panelkey.gameObject.SetActive (true);
							_panel.gameObject.GetComponent<KeyNumber> ().currentDoorNumber = door.lockNumber;
							_panel.gameObject.GetComponent<KeyNumber> ().currentDoor = door;
						}

					}


				}
			}
		} else {
			if (CurrentObject == null && CommonVariable.Instance.btn_Action == "ActionButtonDown") {
				CommonVariable.Instance.btn_Action = "";
				GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
				GameObject chatbox = canvas.transform.FindChild ("Chatbox").gameObject;
				if (chatbox.activeSelf)
					chatbox.GetComponent<BtnChatbox> ().ChatBoxActive ();
			}
		}
	}

	private bool checkTakeItems (List<string> _takeitems)
	{
		if (_takeitems.Count == 0)
			return false;
		else {
			foreach (string _item in _takeitems) {
				Debug.Log (_item);
				if (!this.gameObject.GetComponent<PlayerInventory2> ().ItemNormal.Contains (_item))
					return false;
			}
		}
		return true;
	}

	void OnTriggerStay2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Light") {
			this.isVisible = true;
		} 
//			else {
//			this.isVisible = false;
//		}
	}

	// Khi va chạm với Blocking sẽ dừng Character lại -> hiển thị thông báo -> Charater tự đi ngược lại 1 đoạn
	void OnCollisionEnter2D (Collision2D coll)
	{

		if (coll.gameObject.tag == "Blocking") {
			//CurrentObject = coll.gameObject;
			GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
			GameObject chatbox = canvas.transform.FindChild ("Chatbox").gameObject;
			chatbox.GetComponent<BtnChatbox> ().addChatboxDialogue (coll.gameObject.GetComponent<ListString> ().ListStringDialogue);
			chatbox.GetComponent<BtnChatbox> ().ChatBoxActive ();
			this.GetComponent<Animator> ().CrossFade ("Walking", 0.1f);
			StartCoroutine (StopCharacterAutoWalking (0.5f));
			//this.GetComponent<PlayerController> ().MoveDirection(Direction);
			this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.MovingTo;
			if (this.transform.localScale.x == this.GetComponent<PlayerController> ().ScaleX) {
				Direction = 1;
				//this.transform.position = Vector3.MoveTowards (this.transform.position, new Vector3(this.transform.position.x + 2.5f,this.transform.position.y,this.transform.position.z), this.GetComponent<PlayerController>().moveSpeed * Time.deltaTime);
				this.transform.localScale = new Vector3 (-this.GetComponent<PlayerController> ().ScaleX, this.transform.localScale.y, 1);
			} else {
				Direction = -1;
				this.transform.localScale = new Vector3 (this.GetComponent<PlayerController> ().ScaleX, this.transform.localScale.y, 1);
			}
		}


	}
	
	IEnumerator StopCharacterAutoWalking (float second)
	{
		yield return new WaitForSeconds (second);
		this.GetComponent<Animator> ().CrossFade ("Standing", 0.3f);
		this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Reading;
	}

	//Vá lỗi đèn điện
	private int active = 0;

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.name == "Pit") {
			GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.DieCrazy;
		}
		if (coll.gameObject.tag == "Scare") {
			this.gameObject.GetComponent<Animator> ().SetLayerWeight (1, 0);
			this.gameObject.GetComponent<PlayerController> ().lampObject.SetActive (false);
			// Chạy Event nếu trong object đó có chứa Activater
			if (coll.gameObject.GetComponent<EventPoint> () != null) {
				if (coll.gameObject.GetComponent<EventPoint> ().EventRole == EventPoint.EventRoles.Activater) {
					if (coll.gameObject.GetComponent<EventPoint> ().ReceiverEventType == EventPoint.EventType.OnOffAllElectric) {
						while (active<1) {
							active++;
							coll.gameObject.GetComponent<EventPoint> ().Active ();
						}
					} else {
						coll.gameObject.GetComponent<EventPoint> ().Active ();
					}
				}
			}
		}
		if (coll.gameObject.tag == "Box") {
			CurrentBox = coll.gameObject;
			// 5.145f là tọa độ y cộng thêm để camera chiếu vào Player
			MainCamera.GetComponent<SmoothCameraFollow> ().defaultTargetY = CurrentBox.transform.position.y + 5.145f;
		}
		//
		if (!this.gameObject.GetComponent<PlayerController> ().isAction) {
//			if (CurrentObject != null && coll.tag != "Door") {
//				if (CurrentObject.gameObject.GetComponent<BoxCollider2D> ().OverlapPoint (CurrentObject.transform.position))
//					return;
//			} else {
			if (coll.gameObject.tag == "ReadableItem") {
				if (isVisible
					|| this.GetComponent<PlayerController> ().LampTurnedOn
					&& this.GetComponent<PlayerController> ().lampObject.transform.FindChild ("Area light Player").gameObject.GetComponent<Light> ().enabled
					&& this.GetComponent<PlayerController> ().lampObject.transform.FindChild ("Point light").gameObject.GetComponent<Light> ().enabled
				   ) {
					TalkBubble.SetActive (true);
				}
				//CurrentObject = coll.gameObject;
			} else
				if (coll.gameObject.tag == "Bed") {
				//CurrentObject = coll.gameObject;
			} else
				if (coll.gameObject.tag == "Cabinet") {
				//CurrentObject = coll.gameObject;
			} else
				if (coll.gameObject.tag == "Desk") {
				//CurrentObject = coll.gameObject;
			} else
				if (coll.gameObject.tag == "Door") {
				//CurrentObject = coll.gameObject;
				//CheckDoor = coll.transform.parent.parent.name;
				//Debug.Log (CheckDoor);
			}
//			// nếu Player làm rơi, vỡ ly ,..... , gây sự chú ý với AI
//			else if (coll.gameObject.name.Contains ("Glass")) {
//				GameObject AI = GameObject.FindGameObjectWithTag ("AI");
//				if (coll.gameObject.GetComponent<ReadableItem> ().isCallEnemy )
//					isAttractAttention = true;
//			}
		}

	}

	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Light") {
			this.isVisible = false;
		} 
		if (coll.gameObject.tag == "ReadableItem") {
			TalkBubble.SetActive (false);
			//CurrentObject = null;
		}
		if (!this.gameObject.GetComponent<PlayerController> ().isAction) {
			// Clear current object when exit door's collider
			if (coll.gameObject.tag == "Bed") {
				//CurrentObject = null;
			} else if (coll.gameObject.tag == "Cabinet") {
				//CurrentObject = null;
			} else
			if (coll.gameObject.tag == "Door" && coll.transform.parent.parent.name == CheckDoor) {
				//CurrentObject = null;
			}
		}
	}

	// Change Orderlayer of Player, Cabinet
	// sortingLayerName of Player, sortingOrder of Cabinet
	public void OrderInLayer (string sortingLayerName, int sortingOrder)
	{
		//Player Sprite
		GameObject[] Player = GameObject.FindGameObjectsWithTag ("Sprite_GonzalesChika");
		foreach (GameObject gameObj in Player) {
			if (gameObj.tag == "Sprite_GonzalesChika") {
				gameObj.GetComponent<Uni2DSprite> ().SortingLayerName = sortingLayerName;
			}
		}
		// Cabinet
		if (this.CurrentObject != null) {
			Transform[] transforms = this.CurrentObject.GetComponentsInChildren<Transform> ();
			foreach (Transform child in transforms)
				if (child.gameObject.name.Contains ("Cabinet_")) {
					child.GetComponent<SpriteRenderer> ().sortingOrder = sortingOrder;
				}
		}
	}

	public IEnumerator HiddenPlayer (float second, string sortingLayerName, int sortingOrder)
	{
		yield return new WaitForSeconds (second);
		OrderInLayer (sortingLayerName, sortingOrder);
	}

	IEnumerator GotoNextDoorWithTime (float second)
	{
		Door door = CurrentObject.gameObject.GetComponent<Door> ();
		GameObject _canvas = GameObject.FindGameObjectWithTag ("Canvas");
		GameObject _fader = _canvas.transform.FindChild ("Screen Fader").gameObject;

		_fader.GetComponent<Animator> ().Play ("FadeWhenChangeRoom");

		// Player đang chuyển phòng
		yield return new WaitForSeconds (second);
		isMovingRoom = true;
		float character_z = this.transform.position.z;
		
		// Get next position
		Vector3 NextPosition = door.NextPosition.position;
		NextPosition.Set (NextPosition.x, NextPosition.y - 0.5f, character_z);

		// Tranform character to next position
		this.transform.position = NextPosition;

		//Play animation door
		if (CurrentObject.GetComponent<Animator> () != null) {
			door.GetComponent<Animator> ().Play ("Door");
		}
		//Play animation Player.Standing
		this.GetComponent<Animator> ().Play ("Standing");

		yield return new WaitForSeconds (0.4f);
		isMovingRoom = false;
		GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Standing;
	}

}
