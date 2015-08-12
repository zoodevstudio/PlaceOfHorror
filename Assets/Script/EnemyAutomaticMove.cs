using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class EnemyAutomaticMove : MonoBehaviour
{

	// <Lâm> gọi gameObject cha
	public GameObject AI;
	public GameObject currentDoor;
	// Các phòng vừa vào
	public List<string> alreadyRoom = new List<string> ();
	//Tốc độ di chuyển của AI
	public float MoveSpeed;
	//Hướng di chuyển của AI 1 == right &&  -1 == left
	private int Direction;
	public bool AandB;
	public bool CandD;

	//Kiểm tra xem có phải AI vừa sử dụng SideDoor để chuyển dãy hay không true == chưa  &&  fale == rồi
	public bool isChangingRoom = false;
	public bool alreadyChangeRoom = false;
	
	// Kiểm tra xem có phải AI vừa đi từ phòng ra hành lang hay không
	public bool isFromRoomToHall = false;
	private Animator anim;

	void Start ()
	{
		AI.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Walking;
		anim = GetComponent<Animator> ();
		anim.CrossFade ("Walking", 0.1f);
		alreadyRoom = new List<string> ();
		isChangingRoom = false;
		alreadyChangeRoom = false;
		isFromRoomToHall = false;

		//<Viên>
		Direction = 1;

		//<Lâm> 
		//StartCoroutine (whatWayToGo (0.2f));
	}

	void Update ()
	{
		AI.GetComponent<EnemyInteractive> ().isMovingRoom = false;
		//MoveSpeed = AI.GetComponent<EnemyController>().moveSpeed;
		currentDoor = this.GetComponent<EnemyBoxCollider2D> ().currentDoor;

		// <Viên> Old code
		//Vector3 _move = new Vector3 (MoveSpeed * Direction, 0, 0);
		//transform.position += _move * Time.deltaTime;
		
		// <Lâm> Edit new code
		Vector3 _move = new Vector3 (AI.GetComponent<EnemyController>().moveSpeed * Direction, 0, 0);
		AI.transform.position += _move * Time.deltaTime;

		if (Direction == 1)
			AI.transform.localScale = new Vector3 (-1, this.transform.localScale.y, this.transform.localScale.z);
		else 
			AI.transform.localScale = new Vector3 (1, this.transform.localScale.y, this.transform.localScale.z);
	}

	// Danh sách phòng vừa vào tối đa 3
	private void alreadyRoomCheck (GameObject _roomdoor)
	{
		GameObject _room = _roomdoor.transform.parent.parent.gameObject;
		if (!_room.name.Contains ("_0")) {
			if (alreadyRoom.Count < 3)
				alreadyRoom.Add (_room.name);
			else {
				string temp1 = alreadyRoom [1];
				string temp2 = alreadyRoom [2];
				alreadyRoom = new List<string> ();
				alreadyRoom.Add (temp1);
				alreadyRoom.Add (temp2);
				alreadyRoom.Add (_room.name);
			}
		}
	}

	// Đổi hướng di chuyển của AI
	public void ChangeDirection ()
	{
		this.Direction = this.Direction * (-1);

	}
	// Đưa ra quyết định xem có vào room này hay không true == vào  &&  false == không vào
	private bool isEnterThisRoom ()
	{
		float _random = Random.Range (0f, 10.0f);
		if (0f <= _random && _random < 7.0f)
			return true;
		else
			return false;
	}
	// Đưa ra quyết định xem sẽ đi hướng nào khi vừa từ phòng ra khỏi hành lang
	public IEnumerator whatWayToGo (float second)
	{
		yield return new WaitForSeconds (second);
		float _random = Random.Range (0f, 10.0f);
		if (0f <= _random && _random < 5.0f)
			Direction = 1;
		else
			Direction = -1;
		isChangingRoom = false;
	}

	// AI tương tác với cửa Door
	public void ChangeRoom (GameObject _roomdoor)
	{
		if (!alreadyRoom.Contains (_roomdoor.gameObject.GetComponent<Door> ().NextPosition.transform.parent.parent.gameObject.name)) {
			alreadyRoomCheck (_roomdoor.gameObject.GetComponent<Door> ().NextPosition.gameObject);
			// AI tương tác với cửa nối 2 dãy
			if (_roomdoor.name.Contains ("Side_Door_") && _roomdoor.gameObject.GetComponent<Door> ().NextPosition.gameObject.name.Contains ("Side_Door_")) {
				if (((AandB && !_roomdoor.gameObject.GetComponent<Door> ().NextPosition.transform.parent.parent.gameObject.name.Contains ("_C")) &&
					(AandB && !_roomdoor.gameObject.GetComponent<Door> ().NextPosition.transform.parent.parent.gameObject.name.Contains ("_D"))) ||
					((CandD && !_roomdoor.gameObject.GetComponent<Door> ().NextPosition.transform.parent.parent.gameObject.name.Contains ("_A")) &&
					(CandD && !_roomdoor.gameObject.GetComponent<Door> ().NextPosition.transform.parent.parent.gameObject.name.Contains ("_B")))
				    ) {
					if (_roomdoor.name.Contains ("Side_Door_2"))
						Direction = 1;
					else if (_roomdoor.name.Contains ("Side_Door_1"))
						Direction = -1;
					MoveToRoom (_roomdoor);
				}
			} else
		// AI tương tác với cửa nối từ room ra hành lang
		if (_roomdoor.name.Contains ("Side_Door_") && _roomdoor.gameObject.GetComponent<Door> ().NextPosition.gameObject.name.Contains ("Room_Door_")) {
				isFromRoomToHall = true;
				MoveToRoom (_roomdoor);
			} else {
				// AI tương tác với cửa nối từ hàng lang vào room
				if (!_roomdoor.name.Contains ("Side_Door_")) {
					isFromRoomToHall = false;
					if (isEnterThisRoom ()) {
						if (_roomdoor.name.Contains ("Room_Door_"))
							Direction = 1;
						MoveToRoom (_roomdoor);
					}
				}
			}
		} else {
			Debug.Log ("Phong nay vua di roi");
		}
	}

	// Chuyển AI từ room này sang room khác
	private void MoveToRoom (GameObject _roomdoor)
	{
		float enemy_z = this.transform.position.z;
		// Get next position
		Vector3 NextPosition = _roomdoor.GetComponent<Door> ().NextPosition.position;
		NextPosition.Set (NextPosition.x, NextPosition.y + 1.3f, enemy_z);
		
		// Tranform character to next position
		AI.transform.position = NextPosition;
		this.EnemyAfterAction ();
	}

	//<Lâm> Chuyển AI từ room này sang room khác
//	IEnumerator MoveToRoom (GameObject _roomdoor)
//	{
//		float enemy_z = this.transform.position.z;
//
//		yield return new WaitForSeconds (1f);
//		AI.GetComponent<EnemyInteractive>().isMovingRoom =true;
//		// Get next position
//		Vector3 NextPosition = _roomdoor.GetComponent<Door> ().NextPosition.position;
//		NextPosition.Set (NextPosition.x, NextPosition.y, enemy_z);
//		MoveSpeed = 0;
//		// Tranform character to next position
//		AI.transform.position = NextPosition;
//		yield return new WaitForSeconds (1f);
//
//		MoveSpeed = AI.GetComponent<EnemyController>().moveSpeed;
//		AI.GetComponent<EnemyInteractive>().isMovingRoom =false;
//		this.EnemyAfterAction ();
//	}

	// Chức năng giống AfterAction của Character
	private void EnemyAfterAction ()
	{
		CurrentRoom Enemy_CurrentRoom = this.gameObject.GetComponent<CurrentRoom> ();
		if (Enemy_CurrentRoom.prevRoom != null) {
			StartCoroutine (Enemy_CurrentRoom.DelayFalseRoom (0.7f, Enemy_CurrentRoom.prevRoom));
		}
		Enemy_CurrentRoom.prevRoom = Enemy_CurrentRoom.currentRoom;
		Enemy_CurrentRoom.currentRoom = Enemy_CurrentRoom.nextRoom;
		if (isFromRoomToHall)
			StartCoroutine (whatWayToGo (0.2f));
	}
	
}
