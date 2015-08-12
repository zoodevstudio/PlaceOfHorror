using UnityEngine;
using System.Collections;

public class SmoothCameraFollow : MonoBehaviour
{
	
	public float dampTime = 0.15f;
	private float _dampTime;
	public GameObject target;
	private GameObject Player;
	private GameObject AI;
	private Vector3 velocity = Vector3.zero;
	[Range(0,1)]
	public float
		targetY;
	public float defaultTargetY;
	//public bool removeTargetY;
	public bool isCharacter;
	public bool isEnemy;
	
	void Start ()
	{
		Player = GameObject.FindGameObjectWithTag ("Player");
		AI = GameObject.FindGameObjectWithTag ("AI");
		//removeTargetY = true;
		_dampTime = dampTime;
	}
	
	void Update ()
	{
		if (target && target.gameObject.name == Player.gameObject.name) {
			if (Player.GetComponent<PlayerInteractive> ().isMovingRoom)
				dampTime = 0;
			else
				dampTime = _dampTime;
		} else
		if (target && target.gameObject.name == AI.gameObject.name) {
			if (AI.GetComponent<EnemyInteractive> ().isMovingRoom)
				dampTime = 0;
			else
				dampTime = _dampTime;
		}
		
		if (target) {
			Vector3 point = Camera.main.WorldToViewportPoint (target.transform.position);
			Vector3 delta = target.transform.position - Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, point.z)); //(new Vector3(0.5. 0.5. point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);
			
			// Code thêm vào để tùy chỉnh chiều cao Camera so với Player,AI
			if (target.gameObject.name == Player.gameObject.name) {
				transform.position = new Vector3 (this.transform.position.x, defaultTargetY + targetY, this.transform.position.z);
			} else if (target.gameObject.name == AI.gameObject.name) {
				transform.position = new Vector3 (this.transform.position.x, target.transform.position.y + targetY, this.transform.position.z);
			}
		} else {
			isCharacter = false;
			isEnemy = false;
		}
		
	}
}
