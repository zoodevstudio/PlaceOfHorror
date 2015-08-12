using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour
{
	private AudioManager audioManager;
	public EnemyAutomaticMove enemyAutomaticMove;
	public GameObject AI;
	public GameObject Enemy;
	public GameObject Player;
	public LayerMask LayerMaskHit;
	[HideInInspector]
	public Vector2
		PosAI, PosPlayer;
	[HideInInspector]
	RaycastHit2D
		hit;
	public float sightWidth = 15.5f;
	public bool Chasing;
	public bool SeePlayer;
	// Use this for initialization
	void Start ()
	{
		audioManager = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		NotificationManager.Instance.AddListener (this, "OnAction");
	}
	
	// Update is called once per frame
	void Update ()
	{

		//Debug.Log("" +  (Player.transform.position.x - AI.transform.position.x));
		if (Input.GetKey (KeyCode.Keypad1)) {
			AI.transform.localScale = new Vector3 (1, AI.transform.localScale.y, AI.transform.localScale.z);
			AI.transform.Translate (Vector3.left * 8 * Time.deltaTime);
		} else
		if (Input.GetKey (KeyCode.Keypad3)) {
			AI.transform.localScale = new Vector3 (-1, AI.transform.localScale.y, AI.transform.localScale.z);
			AI.transform.Translate (Vector3.right * 8 * Time.deltaTime);
		}
		
		// Vector2 mouseposition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
//		PosAI = new Vector2 (AI.transform.position.x, AI.transform.position.y - 0.5f);
		
		// Chọn đối tượng có có Layer là "LayerMaskHit"
//		RaycastHit2D hit = Physics2D.Raycast (PosAI, -Vector2.right, sightWidth, LayerMaskHit);
//		Debug.DrawLine (PosAI, PosPlayer, Color.white);

	}

	void OnAction ()
	{
	}

	void LookTowardsPlayer ()
	{
		if (Player.transform.position.x - AI.transform.position.x < 0)
			AI.transform.localScale = new Vector3 (1, AI.transform.localScale.y, AI.transform.localScale.z);
		else
			AI.transform.localScale = new Vector3 (-1, AI.transform.localScale.y, AI.transform.localScale.z);
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			if (AI.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Die
			    && !AI.GetComponent<EnemyController> ().isTraped
			    && !AI.GetComponent<EnemyController> ().isPoisoning
			    ) {
				//SeePlayer = true;
				AI.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Standing;
				if(!audioManager.au_roar.isPlaying)
					audioManager.au_roar.Play ();
				if (!Chasing) {
					Enemy.GetComponent<Animator> ().CrossFade ("Standing", 0.1f);
				}
				if (!Player.GetComponent<PlayerController> ().isHiding) {
					if (!AI.GetComponent<EnemyInteractive> ().enabled)
						AI.GetComponent<EnemyInteractive> ().enabled = true;
					if (enemyAutomaticMove.enabled)
						enemyAutomaticMove.enabled = false;
					LookTowardsPlayer ();
					StartCoroutine (waitforsecond ());
					//Enemy.GetComponent<EnemyController>().enemyState = EnemyController.EnemyState.MoveTo;
					//TouchSound();
					AI.GetComponent<EnemyInteractive> ().Sensing ();
				}
			}
		}
	}

	void OnTriggerStay2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			if (AI.GetComponent<EnemyController> ().enemyState != EnemyController.EnemyState.Die
			    ) {
				SeePlayer = true;
				Player.GetComponent<PlayerController>().PointCatchPlayer = Enemy.GetComponent<EnemyBoxCollider2D> ().PointCatchPlayer;
			}
		}
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			SeePlayer = false;
		}
	}

	IEnumerator waitforsecond ()
	{
		yield return new WaitForSeconds (1);
		Chasing = true;
		if (!AI.GetComponent<EnemyInteractive> ().enabled)
			AI.GetComponent<EnemyInteractive> ().enabled = true;
		if (!AI.GetComponent<EnemyInteractive> ().isActtacking)
			Enemy.GetComponent<Animator> ().CrossFade ("Running", 0.1f);
		if (AI.GetComponent<EnemyInteractive> ().isActtacking)
			AI.GetComponent<EnemyController> ().enemyState = EnemyController.EnemyState.Attacking;
	}
}
