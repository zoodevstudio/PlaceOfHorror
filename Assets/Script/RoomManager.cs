using UnityEngine;
using System.Collections;

// Mục đích của Class là khi bắt đầu game thì khởi tạo phòng cho character và enemy
public class RoomManager : MonoBehaviour
{
	private GameObject Character;
	private GameObject[] Enemy;
	private string currentroom_Save;

	void Start ()
	{
		Character = GameObject.FindGameObjectWithTag ("Player");
		Enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		NotificationManager.Instance.AddListener (this, "Load");
	}

	void Update ()
	{
		if (Character.GetComponent<CurrentRoom> ().firstRoom != null && checkEnemyRoomAvailable ()) {
			foreach (GameObject box in GameObject.FindGameObjectsWithTag("Box")) {
				if (checkCharacterRoomName (box.name) || checkEnemyRoomName (box.name) ||
					checkCharacterCurrentRoomName (box.name) || checkEnemyCurrentRoomName (box.name) ||
					checkCharacterNextRoomName (box.name) || checkEnemyNextRoomName (box.name) ||
					checkCharacterPrevRoomName (box.name) || checkEnemyPrevRoomName (box.name) || currentroom_Save == box.name
					|| box.name == "Box_B1_5_Enemy1_Kill"
				    || box.name == "Box_D3_5") {
					box.gameObject.SetActive (true);
				} else
					box.gameObject.SetActive (false);
			}
			// Sau khi thực hiện class này sẽ tự dừng lại
			this.gameObject.GetComponent<RoomManager> ().enabled = false;
		}
	}

	public void Load ()
	{ 
		currentroom_Save = "Box_A2_2_FirstRoom";  
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists ("CurrentRoom_Player" + i)) {
			currentroom_Save = ES2.Load<string> ("CurrentRoom_Player" + i + "?tag=currentRoom_name" + i); 
			print ("load: " + currentroom_Save);
		} else { 
			print ("Chưa save room");
		}
	}


	// Kiểm tra xem dữ liệu của Enemy đã load xong chưa
	private bool checkEnemyRoomAvailable ()
	{
		foreach (GameObject _enemy in Enemy) {
			if (_enemy.GetComponent<CurrentRoom> ().firstRoom == null)
				return false;
		}
		return true;
	}

	// Kiểm tra tên phòng có trùng với phòng enemy đang đứng không
	private bool checkEnemyRoomName (string _name)
	{
		foreach (GameObject _enemy in Enemy) {
			if (_enemy.GetComponent<CurrentRoom> ().firstRoom.name == _name)
				return true;
		}
		return false;
	}

	private bool checkEnemyCurrentRoomName (string _name)
	{
		foreach (GameObject _enemy in Enemy) {
			if (_enemy.GetComponent<CurrentRoom> ().currentRoom != null) {
				if (_enemy.GetComponent<CurrentRoom> ().currentRoom.name == _name)
					return true;
			}
		}
		return false;
	}

	private bool checkEnemyNextRoomName (string _name)
	{
		foreach (GameObject _enemy in Enemy) {
			if (_enemy.GetComponent<CurrentRoom> ().nextRoom != null) {
				if (_enemy.GetComponent<CurrentRoom> ().nextRoom.name == _name)
					return true;
			}
		}
		return false;
	}

	private bool checkEnemyPrevRoomName (string _name)
	{
		foreach (GameObject _enemy in Enemy) {
			if (_enemy.GetComponent<CurrentRoom> ().prevRoom != null) {
				if (_enemy.GetComponent<CurrentRoom> ().prevRoom.name == _name)
					return true;
			}
		}
		return false;
	}

	// Kiểm tra tên phòng có trùng với phòng Character đang đứng không
	private bool checkCharacterRoomName (string _name)
	{
		if (Character.GetComponent<CurrentRoom> ().firstRoom.name == _name)
			return true;
		return false;
	}

	private bool checkCharacterCurrentRoomName (string _name)
	{
		if (Character.GetComponent<CurrentRoom> ().currentRoom != null) {
			if (Character.GetComponent<CurrentRoom> ().currentRoom.name == _name)
				return true;
		}
		return false;
	}

	private bool checkCharacterNextRoomName (string _name)
	{
		if (Character.GetComponent<CurrentRoom> ().nextRoom != null) {
			if (Character.GetComponent<CurrentRoom> ().nextRoom.name == _name)
				return true;
		}
		return false;
	}

	private bool checkCharacterPrevRoomName (string _name)
	{
		if (Character.GetComponent<CurrentRoom> ().prevRoom != null) {
			if (Character.GetComponent<CurrentRoom> ().prevRoom.name == _name)
				return true;
		}
		return false;
	}
}
