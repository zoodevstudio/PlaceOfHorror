using UnityEngine;
using System.Collections;

public class EnemyInteractive : MonoBehaviour
{
	public GameObject currentDoor;
	//public GameObject boxCollider2D;
	public GameObject player;
	public GameObject enemy;
	public EnemySight enemySight;
	public EnemyAutomaticMove enemyAutomaticMove;
	public GameObject _currentDoor;
	public GameObject FirstDoor; 		//first last door player
	public GameObject SecondDoor;		//second last door player
	public GameObject CurrentObject;
	public bool Wounding, _Sensing;
	public bool isMovingRoom;
	Door door;
	public GameObject nextRoom;
	public bool isActtacking;

	// Use this for initialization
	void Start ()
	{
		this.GetComponent<EnemyInteractive> ().enabled = false;
		isMovingRoom = false;
		NotificationManager.Instance.AddListener (this, "OnAction");
		this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Running;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.Keypad0)) {
			StartCoroutine (GotoDoorWithTime (0.5f));
		}
	}

	void OnAction ()
	{
		//if(this.GetComponent<EnemyController> ().enemyState == EnemyController.EnemyState.Attacking){
		if (isActtacking) {
			if (player.GetComponent<PlayerController> ().playerState != PlayerController.PlayerState.Die
				&& player.GetComponent<PlayerController> ().playerState != PlayerController.PlayerState.Freeze) {
				player.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Freeze;
				//isActtacking = false;
				enemy.GetComponent<Animator> ().CrossFade ("Attack", 0.1f);
				enemySight.GetComponent<EnemySight> ().Chasing = false;
				player.GetComponent<Animator> ().CrossFade ("Die", 0.1f);
			}
		}
		//Debug.Log ("" + Chasing () + "_" + Sensing ());
		//Debug.Log(Mathf.Abs(this.transform.position.x - player.transform.position.x)+"");
		if (FirstDoor != null) {
			door = FirstDoor.gameObject.GetComponent<Door> ();
			// Gán nextRoom = gameObject chứa vị trí Transform NextPosition ( )
			nextRoom = door.gameObject.GetComponent<Door> ().NextPosition.transform.parent.parent.gameObject;
		}

		// Lấy cửa hiện tại của Player khi ấn nút Action
		if (player.GetComponent<PlayerInteractive> ().CurrentObject != null 
			&& player.GetComponent<PlayerInteractive> ().CurrentObject.tag == "Door"
			&& CommonVariable.Instance.btn_Action == "ActionButtonDown") {

			if (FirstDoor == null) {
				if (Sensing ()) {
					FirstDoor = player.GetComponent<PlayerInteractive> ().CurrentObject;

				} else
					FirstDoor = null;
			} else {
				if (!Sensing ()) {
					if (!player.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<Door> ().isLock && !player.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<Door> ().isBroken) {
						SecondDoor = player.GetComponent<PlayerInteractive> ().CurrentObject;
					} else { 
//						if(player.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<Door> ().isLock || player.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<Door> ().isBroken)
						Debug.Log ("k lay dc gia tri cua");

					}
//					else{
//						Debug.Log("k lay dc gia tri cua");
//					}
				} else {
					SecondDoor = null;
					FirstDoor = player.GetComponent<PlayerInteractive> ().CurrentObject;
				}
			}


		}

		// Lấy cửa hiện tại mà AI đang đứng
		_currentDoor = enemy.GetComponent<EnemyBoxCollider2D> ().currentDoor;
//		Wounding = enemySight.GetComponent<EnemySight> ().Wounding;
		EnemyChasingPlayer ();

//		if (player.GetComponent<PlayerInteractive> ().isAttractAttention) {
//			EnterRoom ();
//			print ("toi roi");
//			player.GetComponent<PlayerInteractive> ().isAttractAttention =false;
//		}
	}

	void EnemyChasingPlayer ()
	{
		// Nếu AI đang khác phòng với Player
		if (!Sensing ()) {
			// Nếu Ai đang đuổi theo Player
			if (Chasing ()) {
				
				if (FirstDoor != null) {
					if (FirstDoor.transform.position.x - this.transform.position.x < 0)
						this.transform.localScale = new Vector3 (1, this.transform.localScale.y, this.transform.localScale.z);
					else
						this.transform.localScale = new Vector3 (-1, this.transform.localScale.y, this.transform.localScale.z);
					this.transform.position = Vector3.MoveTowards (this.transform.position, FirstDoor.transform.position, this.GetComponent<EnemyController> ().runSpeed * Time.deltaTime);
					if (_currentDoor != null) {
						// Ai đi vào cửa mà nó nhìn thấy Player đã vào lần cuối 
						if (_currentDoor.name == FirstDoor.name) {
							enemy.GetComponent<Animator> ().CrossFade ("OpenTheDoor", 0.1f);
							if (!_currentDoor.GetComponent<Door> ().isBlockEnemy || !_currentDoor.GetComponent<Door> ().isBroken) {
								FirstDoor = null;
								StartCoroutine (GotoDoorWithTime (0.6f));
							} else {
								if (enemyAutomaticMove.enabled == false)
									enemyAutomaticMove.enabled = true;
								if (this.GetComponent<EnemyInteractive> ().enabled)
									this.GetComponent<EnemyInteractive> ().enabled = false;
							}
						}
					} else {
						//enemy.GetComponent<Animator> ().CrossFade ("Running", 0.1f);
					}
				}
				//enemy.GetComponent<Animator> ().CrossFade ("Running", 0.1f);
				
			} else {
				//this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Standing;
				//enemy.GetComponent<Animator> ().CrossFade ("Standing", 0.1f);
				if (this.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Die) {
					if (enemyAutomaticMove.enabled == false)
						enemyAutomaticMove.enabled = true;
					if (this.GetComponent<EnemyInteractive> ().enabled)
						this.GetComponent<EnemyInteractive> ().enabled = false;
				}
			}
		}
		
		// Nếu Ai đang cùng phòng với Player
		else {
			// Nếu Ai đang đuổi theo Player
			if (Chasing ()) {
				if (player.transform.position.x - this.transform.position.x < 0)
					this.transform.localScale = new Vector3 (1, this.transform.localScale.y, this.transform.localScale.z);
				else
					this.transform.localScale = new Vector3 (-1, this.transform.localScale.y, this.transform.localScale.z);
				this.transform.position = Vector3.MoveTowards (this.transform.position, player.transform.position, this.GetComponent<EnemyController> ().moveSpeed * Time.deltaTime);
				if (player.GetComponent<PlayerController> ().isHiding) {
					if (!enemySight.SeePlayer) {
						enemySight.GetComponent<EnemySight> ().Chasing = false;
						if (this.GetComponent<EnemyInteractive> ().enabled)
							this.GetComponent<EnemyInteractive> ().enabled = false;
						if (enemyAutomaticMove.enabled == false)
							enemyAutomaticMove.enabled = true;
					} else {
						//this.transform.position = Vector3.MoveTowards (this.transform.position, player.transform.position, this.GetComponent<EnemyController> ().runSpeed * Time.deltaTime);
						if (Mathf.Abs (this.transform.position.x - player.transform.position.x) < 3) {
							//this.GetComponent<EnemyController> ().runSpeed = 0;
							this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Standing;
							player.GetComponent<Animator> ().CrossFade ("Reverse_Hiding", 0.1f);
							StartCoroutine (player.GetComponent<PlayerInteractive> ().HiddenPlayer (0.7f, "Layer 2", -1));
							player.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<Animator> ().Play ("CabinetOpen");
							player.GetComponent<PlayerController> ().isAction = false;
							player.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Freeze;
							enemySight.GetComponent<EnemySight> ().Chasing = false;
							player.GetComponent<PlayerController> ().OnShowingState ();
						}
						
						//this.transform.position = Vector3.MoveTowards (this.transform.position, player.transform.position, this.GetComponent<EnemyController> ().moveSpeed * Time.deltaTime);
					}
					
				} else {
					//					if (enemySight.SeePlayer) {
					//						if (Mathf.Abs (this.transform.position.x - player.transform.position.x) < 3) {
					//							//this.GetComponent<EnemyController> ().runSpeed = 0;
					//							this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Standing;
					//							player.GetComponent<Animator> ().CrossFade ("Reverse_Hiding", 0.1f);
					//							StartCoroutine (player.GetComponent<PlayerInteractive> ().HiddenPlayer (0f, "Layer 2", -1));
					//							player.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<Animator> ().Play ("CabinetOpen");
					//							//player.GetComponent<PlayerController> ().isAction = false;
					//							player.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Freeze;
					//							//enemySight.GetComponent<EnemySight> ().Chasing = false;
					//							player.GetComponent<PlayerController> ().OnShowingState ();
					//							Debug.Log ("qua");
					//						}
					//
					//					} else {
					//Debug.Log ("qua day roi");
					if (!this.GetComponent<EnemyInteractive> ().enabled)
						this.GetComponent<EnemyInteractive> ().enabled = true;
					if (enemyAutomaticMove.enabled == true)
						enemyAutomaticMove.enabled = false;
					//					if (enemySight.SeePlayer) {
					this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Running;
					//this.transform.position = Vector3.MoveTowards (this.transform.position, player.transform.position, this.GetComponent<EnemyController> ().runSpeed * Time.deltaTime);
					//					}
				}
			} else {
				//this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Standing;
				
				if (isActtacking) {
					enemyAutomaticMove.enabled = false;
					//this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Standing;
				} else if (!enemySight.GetComponent<CircleCollider2D> ().enabled) {
					enemyAutomaticMove.enabled = true;
					this.GetComponent<EnemyInteractive> ().enabled = false;
					Debug.Log ("s");
				} else {
					if (this.GetComponent<EnemyInteractive> ().enabled)
						this.GetComponent<EnemyInteractive> ().enabled = false;
					if (!enemyAutomaticMove.enabled)
						enemyAutomaticMove.enabled = true;
					//enemy.GetComponent<Animator> ().CrossFade ("Walking", 0.1f);
				}
			}
		}
	}

	// Kiểm tra Player với AI có ở cùng hành lang không 
	public bool Sensing ()
	{
		if (enemy.GetComponent<CurrentRoom> ().currentRoom == player.GetComponent<CurrentRoom> ().currentRoom) {
			return true;
		} else {
			return false;
		}

	}

	// Kiểm tra AI đã mất dấu player hay chưa ( mất dấu player khi player ở vị trí cách giữa AI 1 phòng )
	public bool Chasing ()
	{
		if (enemySight.GetComponent<EnemySight> ().Chasing)
			return true;
		else
			return false;
		if (nextRoom.name == player.GetComponent<CurrentRoom> ().currentRoom.name)
			return true;
		else {
			enemySight.GetComponent<EnemySight> ().Chasing = false;
			return false;
		}
	}

	//AI lập tức đến một phòng
	public void EnterRoom ()
	{
		if (this.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Die) {
			if (enemyAutomaticMove.AandB) {
				if (player.GetComponent<CurrentRoom> ().currentRoom.name.Contains ("Box_A") 
					|| player.GetComponent<CurrentRoom> ().currentRoom.name.Contains ("Box_B"))
					Debug.Log ("AOrB");
				Transform transformDoor;
				if (player.GetComponent<CurrentRoom> ().currentRoom.transform.FindChild ("Door").FindChild ("Side_Door_1") != null) {
					transformDoor = player.GetComponent<CurrentRoom> ().currentRoom.transform.FindChild ("Door").FindChild ("Side_Door_1");
				} else {
					transformDoor = player.GetComponent<CurrentRoom> ().currentRoom.transform.FindChild ("Door").FindChild ("Room_Door_1");
				}
				Vector3 NextPosition = transformDoor.position;
				NextPosition.Set (NextPosition.x, NextPosition.y + 2f, this.transform.position.z);
				this.transform.position = NextPosition;
				
				player.GetComponent<PlayerInteractive> ().isAttractAttention = false;
				enemySight.GetComponent<EnemySight> ().Chasing = true;
				enemyAutomaticMove.enabled = false;
			} else {
				if (player.GetComponent<CurrentRoom> ().currentRoom.name.Contains ("Box_C") 
					|| player.GetComponent<CurrentRoom> ().currentRoom.name.Contains ("Box_D"))
					Debug.Log ("DOrC");
				Transform transformDoor;
				if (player.GetComponent<CurrentRoom> ().currentRoom.transform.FindChild ("Door").FindChild ("Side_Door_1") != null) {
					transformDoor = player.GetComponent<CurrentRoom> ().currentRoom.transform.FindChild ("Door").FindChild ("Side_Door_1");
				} else {
					transformDoor = player.GetComponent<CurrentRoom> ().currentRoom.transform.FindChild ("Door").FindChild ("Room_Door_1");
				}
				Vector3 NextPosition = transformDoor.position;
				NextPosition.Set (NextPosition.x, NextPosition.y + 2f, this.transform.position.z);
				this.transform.position = NextPosition;
				
				player.GetComponent<PlayerInteractive> ().isAttractAttention = false;
				enemySight.GetComponent<EnemySight> ().Chasing = true;
				enemyAutomaticMove.enabled = false;
			}

		}
	}

	//AI kiểm tra một phòng
	public void Fossick ()
	{

	}
	
	public void SetTrap ()
	{
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
//		if (coll.gameObject.tag == "Door") {
//			if (!coll.gameObject.GetComponent<Door> ().isBroken || !coll.gameObject.GetComponent<Door> ().isBlockEnemy)
//				currentDoor = coll.gameObject;
//		}
	}
	
	void OnTriggerExit2D (Collider2D coll)
	{
//		if (coll.gameObject.tag == "Door") {
//			currentDoor = null;
//		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{

		if (!player.GetComponent<PlayerController> ().isHiding)
		if (coll.gameObject.tag == "Player") {
			if (this.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Die
			    && !this.GetComponent<EnemyController> ().isTraped
			    && !this.GetComponent<EnemyController> ().isPoisoning) {
				isActtacking = true;
				enemy.GetComponent<Animator> ().CrossFade ("Attack", 0.1f);
				this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Attacking;
				enemySight.GetComponent<EnemySight> ().Chasing = false;
				player.GetComponent<Animator> ().CrossFade ("Die", 0.1f);
				player.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Freeze;
			}
		}
	}

	IEnumerator GotoDoorWithTime (float second)
	{
		Door door = _currentDoor.gameObject.GetComponent<Door> ();

		yield return new WaitForSeconds (second);
		isMovingRoom = true;
		float enemy_z = this.transform.position.z;
		enemySight.GetComponent<CircleCollider2D> ().enabled = false;
		// Get next position
		Vector3 NextPosition = door.NextPosition.position;
		NextPosition.Set (NextPosition.x, NextPosition.y + 1.3f, enemy_z);

		// Tranform character to next position
		this.transform.position = NextPosition;


		isMovingRoom = false;

		if (SecondDoor != null) {
			if (SecondDoor != FirstDoor) {
				enemySight.GetComponent<EnemySight> ().Chasing = false;
				SecondDoor = null;
				if (enemyAutomaticMove.enabled == false)
					enemyAutomaticMove.enabled = true;
				if (this.GetComponent<EnemyInteractive> ().enabled)
					this.GetComponent<EnemyInteractive> ().enabled = false;
			}
		}

		yield return new WaitForSeconds (0.3f);
		//Play animation
		if (!isActtacking) {
			

			if (Chasing ()) {
				this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Running;
				enemy.GetComponent<Animator> ().CrossFade ("Running", 0.1f);
			} else {
				this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Walking;
				enemy.GetComponent<Animator> ().CrossFade ("Walking", 0.1f);
			}
			if (!Sensing ()) {
				if (enemyAutomaticMove.enabled == false) {
					enemyAutomaticMove.enabled = true;
					enemy.GetComponent<Animator> ().CrossFade ("Walking", 0.1f);
				}
				if (this.GetComponent<EnemyInteractive> ().enabled)
					this.GetComponent<EnemyInteractive> ().enabled = false;
			}
		}
		enemySight.GetComponent<CircleCollider2D> ().enabled = true;
		//this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Standing;
	}
}
