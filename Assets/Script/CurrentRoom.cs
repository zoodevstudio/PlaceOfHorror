using UnityEngine;
using System.Collections;

public class CurrentRoom : MonoBehaviour
{
	// Vị trí phòng so với Object chứa code
	public GameObject prevRoom;
	public GameObject currentRoom;
	public GameObject nextRoom;

	// Dùng để load khi mới vào game
	private bool isFirst;
	public GameObject firstRoom;

	// Phân biệt Character và Enemy
	private GameObject Character;
	private GameObject[] Enemy;
	public bool isCharacter;
	public bool isEnemy;

	void Start ()
	{
		Character = GameObject.FindGameObjectWithTag ("Player");
		Enemy = GameObject.FindGameObjectsWithTag ("Enemy");

		isFirst = true;
		NotificationManager.Instance.AddListener (this, "AfterAction"); 

		NotificationManager.Instance.AddListener (this, "Save");
	}

	public void Save(){
		//use save currentroom.name for Player
		if(this.gameObject.name=="Player"){
			string i = CommonVariable.Instance.loadi; 
			ES2.Save(currentRoom.name, "CurrentRoom_Player"+i+"?tag=currentRoom_name"+i); 
			print("save: "+currentRoom.name);
		}
		
	}
	// Chỉ dùng cho Character kích hoạt sau khi thực hiện mở cửa ở interactive
	void AfterAction ()
	{
		if (isCharacter) {
			if (prevRoom != null) {
				StartCoroutine (DelayFalseRoom (0.7f, prevRoom));
			}
			prevRoom = currentRoom;
			currentRoom = nextRoom;
		}
	}

	// Kiểm tra xem phòng mà Object chứa code vừa đi qua có chứa Character/Enemy kg
	private bool CheckCurrentRoom (string _room)
	{
		bool _check = false;
		if (Character.gameObject.GetComponent<CurrentRoom> ().currentRoom != null) {
			if (Character.gameObject.GetComponent<CurrentRoom> ().currentRoom.name == _room)
				_check = true;
		}
		foreach (GameObject _enemy in Enemy) {
			if (_enemy.gameObject.GetComponent<CurrentRoom> ().currentRoom != null) {
				if (_enemy.gameObject.GetComponent<CurrentRoom> ().currentRoom.name == _room)
					_check = true;
			}
		}
		return _check;
	}

	private bool CheckNextRoom (string _room)
	{
		bool _check = false;
		if (Character.gameObject.GetComponent<CurrentRoom> ().nextRoom != null) {
			if (Character.gameObject.GetComponent<CurrentRoom> ().nextRoom.name == _room)
				_check = true;
		}
		foreach (GameObject _enemy in Enemy) {
			if (_enemy.gameObject.GetComponent<CurrentRoom> ().nextRoom != null) {
				if (_enemy.gameObject.GetComponent<CurrentRoom> ().nextRoom.name == _room)
					_check = true;
			}
		}
		return _check;
	}

	private bool CheckPrevRoom (string _room)
	{
		bool _check = false;
		if (Character.gameObject.GetComponent<CurrentRoom> ().prevRoom != null) {
			if (Character.gameObject.GetComponent<CurrentRoom> ().prevRoom.name == _room)
				_check = true;
		}
		foreach (GameObject _enemy in Enemy) {
			if (_enemy.gameObject.GetComponent<CurrentRoom> ().prevRoom != null) {
				if (_enemy.gameObject.GetComponent<CurrentRoom> ().prevRoom.name == _room)
					_check = true;
			}
		}
		return _check;
	}

	public IEnumerator DelayFalseRoom (float second, GameObject _room)
	{
		yield return new WaitForSeconds (second);
		if (!CheckCurrentRoom (_room.name) && !CheckNextRoom (_room.name) && !CheckPrevRoom (_room.name))
			_room.SetActive (false);
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		// Khi mới bước vào 1 phòng
		if (coll.tag == "Box") {
			//Chỉ dùng 1 lần khi khởi động
			if (isFirst) {
				currentRoom = coll.gameObject;
				firstRoom = coll.gameObject;
				isFirst = false;
			}
		}

		// Khi đi ngang 1 cánh cửa
		if (coll.tag == "Door" || coll.tag == "Stair") {
			currentRoom = coll.transform.parent.parent.gameObject;
			if (currentRoom.name == coll.transform.parent.parent.gameObject.name) {
				if (nextRoom != null) {
					StartCoroutine (DelayFalseRoom (0.7f, nextRoom));
				}
			}

			nextRoom = coll.gameObject.GetComponent<Door> ().NextPosition.transform.parent.parent.gameObject;
			nextRoom.SetActive (true);


			// Đoạn chạy riêng cho AI
			if (isEnemy) {
//				if (this.gameObject.GetComponent<EnemyAutomaticMove> () != null) {
				if (this.gameObject.GetComponent<EnemyAutomaticMove> ().enabled
				    && this.GetComponent<EnemyBoxCollider2D>().AI.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Die) {
					if (!this.gameObject.GetComponent<EnemyAutomaticMove> ().isChangingRoom && !this.gameObject.GetComponent<EnemyAutomaticMove> ().alreadyChangeRoom) {
						this.gameObject.GetComponent<EnemyAutomaticMove> ().isChangingRoom = true;
						this.gameObject.GetComponent<EnemyAutomaticMove> ().alreadyChangeRoom = true;
						this.gameObject.GetComponent<EnemyAutomaticMove> ().ChangeRoom (coll.gameObject);
					} else
					if (this.gameObject.GetComponent<EnemyAutomaticMove> ().isChangingRoom && this.gameObject.GetComponent<EnemyAutomaticMove> ().alreadyChangeRoom) {
						this.gameObject.GetComponent<EnemyAutomaticMove> ().alreadyChangeRoom = false;
					} else {
						this.gameObject.GetComponent<EnemyAutomaticMove> ().alreadyChangeRoom = false;
						this.gameObject.GetComponent<EnemyAutomaticMove> ().isChangingRoom = false;
					}
				}
			}
		}
		// Khi gặp vật cản AI sẽ đổi hướng
		if (coll.gameObject.tag == "Deadend"|| coll.gameObject.name.Contains("Point_Enemy") || coll.gameObject.tag == "Blocking") {
			if (isEnemy) {
				if (this.gameObject.GetComponent<EnemyAutomaticMove> () != null) {
					this.gameObject.GetComponent<EnemyAutomaticMove> ().isChangingRoom = false;
					this.gameObject.GetComponent<EnemyAutomaticMove> ().ChangeDirection ();
				}
			}
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		// Khi gặp vật cản AI sẽ đổi hướng
		if (coll.gameObject.tag == "Deadend"|| coll.gameObject.name.Contains("Point_Enemy") || coll.gameObject.tag == "Desk" || coll.gameObject.tag == "Blocking") {
			if (isEnemy) {
				if (this.gameObject.GetComponent<EnemyAutomaticMove> () != null) {
					this.gameObject.GetComponent<EnemyAutomaticMove> ().isChangingRoom = false;
					this.gameObject.GetComponent<EnemyAutomaticMove> ().ChangeDirection ();
				}
			}
		}
	}

}
