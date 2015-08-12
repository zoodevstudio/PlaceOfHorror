using UnityEngine;
using System.Collections;

public class EnemyBoxCollider2D : MonoBehaviour
{
	public GameObject currentDoor;
	public GameObject AI;
	public GameObject player;
	public GameObject enemySight;
	public GameObject AI_Corpse;
	public GameObject PointCatchPlayer;
	// Use this for initialization
	void Start ()
	{
		AI_Corpse.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
//	void OnCollisionEnter2D (Collision2D coll)
//	{
//		if (!player.GetComponent<PlayerController> ().isHiding)
//		if (coll.gameObject.tag == "Player") {
//			if (this.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Die) {
//				AI.GetComponent<EnemyInteractive>().isActtacking = true;		
//				enemy.GetComponent<Animator> ().CrossFade ("Attack", 0.1f);
//				this.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Attacking;
//				enemySight.GetComponent<EnemySight> ().Chasing = false;
//				player.GetComponent<Animator> ().CrossFade ("Die", 0.1f);
//				//player.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Die;
//			}
//		}
//	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "TrapPlayer") {
//			CurrentObject = coll.gameObject;
			if (AI.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Die) {
				coll.gameObject.GetComponent<Animator> ().CrossFade ("TrapClose", 0.1f);
				coll.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
				this.GetComponent<Animator> ().CrossFade ("Trapped", 0.1f);
				AI.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Die;
				enemySight.GetComponent<EnemySight> ().Chasing = false;
				enemySight.GetComponent<CircleCollider2D>().enabled = false;
				this.GetComponent<EnemyAutomaticMove> ().enabled = false;
				AI.GetComponent<EnemyInteractive> ().enabled = false;
				AI.GetComponent<EnemyController> ().isPrank = true;
				Debug.Log ("remove inventory");
				Debug.Log ("AI Die");
				AI.GetComponent<EnemyController> ().isTraped = true;
				AI_Corpse.SetActive (true);
				AI.GetComponent<Rigidbody2D> ().gravityScale = 0;
				this.GetComponent<CircleCollider2D> ().isTrigger = true;
			}
		}
		if (coll.gameObject.tag == "Door") {
			if (!coll.gameObject.GetComponent<Door> ().isBroken || !coll.gameObject.GetComponent<Door> ().isBlockEnemy)
				currentDoor = coll.gameObject;
		}
		if (!player.GetComponent<PlayerController> ().isHiding)
		if (coll.gameObject.tag == "Player") {
			if (AI.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Die 
			    && AI.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.PrankPlayer
			    && !AI.GetComponent<EnemyController> ().isTraped
			    && !AI.GetComponent<EnemyController> ().isPoisoning) {
				AI.GetComponent<EnemyInteractive> ().isActtacking = true;		
				this.GetComponent<Animator> ().CrossFade ("Attack", 0.1f);
				AI.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Attacking;
				enemySight.GetComponent<EnemySight> ().Chasing = false;
				player.GetComponent<Animator> ().CrossFade ("Die", 0.1f);
				player.GetComponent<Rigidbody2D> ().gravityScale = 0;
				//player.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Freeze;
				foreach (Collider2D cool in player.GetComponents<Collider2D> ()) {
					cool.enabled = false;
				}
			} else {
				if (!AI.GetComponent<EnemyController> ().isPrank)
					AI.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.PrankPlayer;
			}
		}
		if (coll.gameObject.tag == "ReadableItem") {
			if (coll.gameObject.GetComponent<ReadableItem> () != null && coll.gameObject.GetComponent<ReadableItem> ().isPoisoned) {
				if (!enemySight.GetComponent<EnemySight> ().Chasing) {
					AI.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Die;
					Debug.Log ("AI Die");
					AI.GetComponent<EnemyController> ().isPoisoning = true;
					AI_Corpse.SetActive (true);
					this.GetComponent<Animator> ().CrossFade ("Die", 0.2f);
					AI.GetComponent<Rigidbody2D> ().gravityScale = 0;
					this.GetComponent<CircleCollider2D> ().isTrigger = true;
				}
			}
		}
		if (coll.gameObject.name.Contains ("Point_Enemy") || coll.gameObject.tag == "Blocking") {
			enemySight.GetComponent<EnemySight> ().Chasing = false;
			enemySight.GetComponent<EnemySight> ().SeePlayer = false;
			StartCoroutine (ResetEnemySight ());
			//this.gameObject.GetComponent<EnemyAutomaticMove> ().ChangeDirection ();
			this.GetComponent<Animator> ().CrossFade ("Walking", 0.1f);
			if (!this.GetComponent<EnemyAutomaticMove> ().enabled)
				this.GetComponent<EnemyAutomaticMove> ().enabled = true;
			if (AI.GetComponent<EnemyInteractive> ().enabled)
				AI.GetComponent<EnemyInteractive> ().enabled = false;
		}
	}
	
	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Door") {
			currentDoor = null;
		}
	}

	IEnumerator ResetEnemySight ()
	{
		enemySight.GetComponent<CircleCollider2D> ().enabled = false;
		yield return new WaitForSeconds (3);
		enemySight.GetComponent<CircleCollider2D> ().enabled = true;
	}
}
