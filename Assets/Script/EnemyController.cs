using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
	public Transform Trap;
	public enum EnemyState
	{
		Standing,
		Walking,
		Running,
		MoveTo,
		OpenTheDoor,
		Trapped,
		Traping,
		Stooping,
		Attacking,
		Die,
		PrankPlayer,
	}
	;
	private int _time;
	public bool isTrapping, isTraped,isPoisoning;
	public EnemyAutomaticMove enemyAutomaticMove;
	public GameObject enemySight;
	public GameObject player;
	public GameObject enemy;
	public float moveSpeed;
	public float runSpeed = 2;
	public float defaultMoveSpeed = 2;
	public EnemyState enemyState;
	public CurrentRoom currentRoom;
	public int TrapCount;
//	public bool Chasing,Wounding,Sensing;

	public bool isPrank;
	public GameObject LastDoorPlayer;
	public GameObject _currentDoor;

	// Use this for initialization
	void Start ()
	{
		//TrapCount = Trap.transform.childCount;
		isTrapping = false;
		moveSpeed = defaultMoveSpeed;
		NotificationManager.Instance.AddListener (this, "OnAction");
		NotificationManager.Instance.AddListener (this, "Load");
		NotificationManager.Instance.AddListener (this, "Save");
	}

	public void Save ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (TrapCount, this.gameObject.name + "EnemyController" + i + "?tag=TrapCount" + i);
		ES2.Save (Trap, this.gameObject.name + "EnemyController" + i + "?tag=TrapTransform" + i);
		ES2.Save (isTraped, this.gameObject.name + "EnemyController" + i + "?tag=isTraped" + i);
		ES2.Save (isPoisoning, this.gameObject.name + "EnemyController" + i + "?tag=isPoisoning" + i);
		ES2.Save (isPrank, this.gameObject.name + "EnemyController" + i + "?tag=isPrank" + i);

	}
	
	public void Load ()
	{
		//chạy load -> gán lại cho biến thôi
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "PlayerSaveLoad" + i)) {
			TrapCount = ES2.Load<int> (this.gameObject.name + "EnemyController" + i + "?tag=TrapCount" + i);
			ES2.Load<Transform> (this.gameObject.name + "EnemyController" + i + "?tag=TrapTransform" + i, Trap);
			isTraped = ES2.Load<bool> (this.gameObject.name + "EnemyController" + i + "?tag=isTraped" + i);
			isPoisoning = ES2.Load<bool> (this.gameObject.name + "EnemyController" + i + "?tag=isPoisoning" + i);
			isPrank = ES2.Load<bool> (this.gameObject.name + "EnemyController" + i + "?tag=isPrank" + i);
			if(isTraped){
				OnStateChange(EnemyState.Die);
				enemy.GetComponent<Animator>().Play ("Die2");
			}
			if(isPoisoning){
				OnStateChange(EnemyState.Die);
				enemy.GetComponent<Animator>().Play ("Die");
			}

		} else {
			//chưa có lưu gì. load dữ liệu mặc định
			print ("chưa có vi trí player");
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnStateChange (EnemyState state)
	{

		switch (state) {
		case EnemyState.Standing:
			enemyState = EnemyState.Standing;
			break;
		case EnemyState.Walking:
			enemyState = EnemyState.Walking;
			break;
		case EnemyState.Running:
			enemyState = EnemyState.Running;
			break;
		case EnemyState.MoveTo:
			enemyState = EnemyState.MoveTo;
			break;
		case EnemyState.OpenTheDoor:
			enemyState = EnemyState.OpenTheDoor;
			break;
		case EnemyState.Trapped:
			enemyState = EnemyState.Trapped;
			break;
		case EnemyState.Traping:
			enemyState = EnemyState.Traping;
			break;
		case EnemyState.Stooping:
			enemyState = EnemyState.Stooping;
			break;
		case EnemyState.Attacking:
			enemyState = EnemyState.Attacking;
			break;
		case EnemyState.Die:
			enemyState = EnemyState.Die;
			break;
		case EnemyState.PrankPlayer:
			enemyState = EnemyState.PrankPlayer;
			break;
		default :
			break;
		}

	}

	void OnAction ()
	{
//		Chasing = collider2DCircle.GetComponent<EnemySight>().Chasing;
//		Wounding = collider2DCircle.GetComponent<EnemySight>().Wounding;
//		Sensing = collider2DCircle.GetComponent<EnemySight>().Sensing;

		switch (enemyState) {
		case EnemyState.Standing:
			OnStandingState ();
			break;
		case EnemyState.Walking:
			OnWalkingState ();
			break;
		case EnemyState.Running:
			OnRunningState ();
			break;
		case EnemyState.MoveTo:
			OnMoveToState ();
			break;
		case EnemyState.OpenTheDoor:
			OnOpenTheDoorState ();
			break;
		case EnemyState.Trapped:
			OnTrappedState ();
			break;
		case EnemyState.Traping:
			OnTrapingState ();
			break;
		case EnemyState.Stooping:
			OnStoopingState ();
			break;
		case EnemyState.Attacking:
			OnAttackingState ();
			break;
		case EnemyState.Die:
			OnDieState ();
			break;
		case EnemyState.PrankPlayer:
			OnPrankPlayerState ();
			break;
		default :
			break;
		}
	}

	public void OnStandingState ()
	{
		moveSpeed = 0;
	}

	void OnWalkingState ()
	{
		if (CommonVariable.Instance.isPause)
			moveSpeed = 0;
		else
			moveSpeed = defaultMoveSpeed;
		// 300s , AI sẽ đặt bẫy 1 lần
		if (CommonVariable.Instance.PlayTime == (_time + 180)) {
			isTrapping = true;

			_time = CommonVariable.Instance.PlayTime;
			Debug.Log(_time + ": tie");
		} else if (CommonVariable.Instance.PlayTime > (_time + 200)) {
			_time = CommonVariable.Instance.PlayTime;
		}
		if (isTrapping) {
			isTrapping = false;
			if (TrapCount <= 0) {
				Debug.Log ("AI: khong con bay de dat ");
				TrapCount = 0;
			} else {

				//Debug.Log(TrapIndex + ", " + TrapCount);
				GameObject _Trap = Trap.gameObject.transform.FindChild ("Trap_" + TrapCount).gameObject;
				foreach (Transform child in _Trap.transform) {
					child.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
				}
				if (TrapCount == 3) {
					this.GetComponent<AISaveLoad> ().FollowPlayer_3 = false;
					this.GetComponent<AISaveLoad> ().BoxCollider2D_3 = true;
				} else if (TrapCount == 2) {
					this.GetComponent<AISaveLoad> ().FollowPlayer_2 = false;
					this.GetComponent<AISaveLoad> ().BoxCollider2D_2 = true;
				} else
				if (TrapCount == 1) {
					this.GetComponent<AISaveLoad> ().FollowPlayer_1 = false;
					this.GetComponent<AISaveLoad> ().BoxCollider2D_1 = true;
				}

				_Trap.GetComponent<FollowPlayer> ().enabled = false;
				_Trap.GetComponent<BoxCollider2D> ().enabled = true;
				TrapCount--;
			}
			
		}
	}

	void OnRunningState ()
	{
		if (CommonVariable.Instance.isPause)
			moveSpeed = 0;
		else
			moveSpeed = runSpeed;
	}

	void OnMoveToState ()
	{

	}

	void OnOpenTheDoorState ()
	{

	}

	void OnTrapingState ()
	{

		//OnStateChange(EnemyState.Walking);
//		Trap = Instantiate (Trap) as GameObject;
//		Trap.transform.position = new Vector3 (this.transform.position.x - 1, this.transform.position.y);
		//		Trap.tag = "TrapEnemy";
	}

	void EnemyTrap ()
	{

	}

	void OnTrappedState ()
	{
		if (CommonVariable.Instance.isPause)
			moveSpeed = 0;
		else
			moveSpeed = 0;
		this.GetComponent<EnemyInteractive> ().enabled = false;
		enemyAutomaticMove.enabled = false;
		enemySight.GetComponent<EnemySight> ().Chasing = false;
	}

	void OnStoopingState ()
	{
	}

	void OnAttackingState ()
	{
		enemySight.GetComponent<EnemySight> ().Chasing = false;
		enemyAutomaticMove.enabled = false;
		this.GetComponent<EnemyInteractive> ().enabled = false;
	}

	void OnDieState ()
	{
		moveSpeed = 0;
		//isPrank = false;
		//enemy.GetComponent<Animator> ().CrossFade ("Die", 0.2f);
		enemyAutomaticMove.enabled = false;
		this.GetComponent<EnemyInteractive> ().enabled = false;
		enemySight.GetComponent<EnemySight>().enabled = false;
		this.GetComponent<Rigidbody2D> ().gravityScale = 0;
		enemy.GetComponent<CircleCollider2D> ().isTrigger = true;
	}

	void OnPrankPlayerState ()
	{
		moveSpeed = 0;
		isPrank = true;
		enemy.GetComponent<Animator> ().CrossFade ("Prank", 0.1f);
		StartCoroutine (setMoveSpeed (1));
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
	}

	void OnTriggerExit2D (Collider2D coll)
	{

	}

	IEnumerator setMoveSpeed (float time)
	{
		yield return new WaitForSeconds (time);
		//isPrank = false;
		OnStateChange (EnemyState.Die);
	}

}
