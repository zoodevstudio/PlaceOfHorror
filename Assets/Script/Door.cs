
using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	// Position where character will move on
	public Transform NextPosition;
	// true = lock  &&   false = unlock
	public bool isLock;
	// true = player and enemy cannot go through && false = can
	public bool isBroken;
	// true = enemy cannot go through && false = can
	public bool isBlockEnemy;

	// require item to unlock the door
	public string Key;

	//dùng cho khóa số
	public bool isLockNumber;
	public int lockNumber;

	void Start ()
	{
		NotificationManager.Instance.AddListener (this, "Load");
		NotificationManager.Instance.AddListener (this, "Save"); 
		//dùng để xóa dữ liệu, để test khi làm thôi
		NotificationManager.Instance.AddListener (this, "Del");
	}

	public void Save ()
	{
		//Debug.Log ("HAD SAVED"); 
		string tengameobject = this.transform.parent.parent.gameObject.name+"_"+this.gameObject.name;
		string i = CommonVariable.Instance.loadi; 

		ES2.Save (isLock, tengameobject+"_Door"+i+"?tag=isLock"+i);
		ES2.Save (isBroken, tengameobject+"_Door"+i+"?tag=isBroken"+i);
		ES2.Save (isBlockEnemy, tengameobject+"_Door"+i+"?tag=isBlockEnemy"+i);
		ES2.Save (Key, tengameobject+"_Door"+i+"?tag=Key"+i); //string
		ES2.Save (isLockNumber, tengameobject+"_Door"+i+"?tag=isLockNumber"+i);
		ES2.Save (lockNumber, tengameobject+"_Door"+i+"?tag=lockNumber"+i); //int
		ES2.Save (NextPosition, tengameobject+"_Door"+i+"?tag=NextPosition"+i);
	}

	public void Load ()
	{
		//Debug.Log ("HAD LOADED"); 
		string tengameobject = this.transform.parent.parent.gameObject.name+"_"+this.gameObject.name;
		string i = CommonVariable.Instance.loadi; 

		if(ES2.Exists (tengameobject+"_Door"+i)){
			ES2.Load<Transform>(tengameobject + "_Door"+i+"?tag=NextPosition"+i, NextPosition); 
			isLock = ES2.Load<bool> (tengameobject + "_Door"+i+"?tag=isLock"+i);
			isBroken = ES2.Load<bool> (tengameobject + "_Door"+i+"?tag=isBroken"+i);
			isBlockEnemy = ES2.Load<bool> (tengameobject + "_Door"+i+"?tag=isBlockEnemy"+i);
			isLockNumber = ES2.Load<bool> (tengameobject + "_Door"+i+"?tag=isLockNumber"+i);
			Key = ES2.Load<string> (tengameobject + "_Door"+i+"?tag=Key"+i);
			lockNumber = ES2.Load<int> (tengameobject + "_Door"+i+"?tag=lockNumber"+i);

		}//else {
			//chưa có lưu gì. load dữ liệu mặc định
		//	print ("chưa có");
		//}
	}
	public void Del(){
		string i = CommonVariable.Instance.loadi; 
		string name_save = CommonVariable.Instance.name_save;
		string tengameobject = this.transform.parent.parent.gameObject.name+"_"+this.gameObject.name;
		if(ES2.Exists (tengameobject+"_Door"+i)){
			ES2.Delete(tengameobject+"_Door"+i); 
			print ("Đã xóa: "+tengameobject+"_Door"+i);
		}
	}
}
