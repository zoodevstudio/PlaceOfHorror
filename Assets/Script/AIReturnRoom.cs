using UnityEngine;
using System.Collections;

public class AIReturnRoom : MonoBehaviour
{
	private bool enable;
	public bool AI_1;
	public bool AI_2;

	// Use this for initialization
	void Start ()
	{
		enable = true;
		if (enable)
			StartCoroutine (GoingDineRoom ());
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	IEnumerator GoingDineRoom ()
	{
		enable = false;
		yield return new WaitForSeconds (300);
		if (this.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Die
			&& this.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.PrankPlayer
			&& this.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Attacking
			&& !this.GetComponent<EnemyController> ().isTraped
			&& !this.GetComponent<EnemyController> ().isPoisoning) {
			if (!this.GetComponent<EnemyInteractive> ().enemySight.Chasing && !this.GetComponent<EnemyInteractive> ().isActtacking) {

				if (AI_1) {
					// Phòng bị lỗi, không kiếm đc cái ghế : vì nó setactive = False
					GameObject Chair = GameObject.FindGameObjectWithTag ("DineRoom");
					GameObject Room = Chair.transform.parent.parent.gameObject;
					Debug.Log ("den h an: " + Chair.name + ", " + Chair.transform.parent.parent.name);
					Vector3 NextPosition = Chair.transform.parent.parent.FindChild ("Door").transform.FindChild ("Side_Door_1").position;
					NextPosition.x += 5;
					this.transform.position = NextPosition;
					this.GetComponent<EnemyController> ().enemy.GetComponent<CurrentRoom> ().currentRoom = Room;
				}
				if (AI_2) {
					GameObject Others = GameObject.FindGameObjectWithTag ("PointReturnAI_2");
					GameObject Room = Others.transform.parent.gameObject;
					Vector3 NextPosition = Others.transform.position;
					//NextPosition.x += 5;
					this.transform.position = NextPosition;
					this.GetComponent<EnemyController> ().enemy.GetComponent<CurrentRoom> ().currentRoom = Room;
				}
			}

		}
		enable = true;
	}
}
