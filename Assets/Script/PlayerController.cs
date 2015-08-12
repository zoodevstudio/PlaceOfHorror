using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	public enum PlayerState
	{
		Standing,
		Walking,
		Running,
		Jumping,
		Sleeping,
		Stand_PushAndPull,
		Walk_PushAndPull,
		MovingTo,
		Hiding,
		Trapped,
		Traping,
		OpenTheDoor,
		Reading,
		Freeze,
		Die,
		DieCrazy,
		Sleeping2
	}
	;

	private AudioManager audioManager;
	public float speedPushPull = 0.7f;
	public bool isWounded_Head, isWounded_Leg;
//	[HideInInspector]
	public int
		HeadPlusIndex, LegPlusIndex;
	public bool isAction;
	public bool disableToLeft;
	public bool disableToRight;
	public bool isHiding;
	public bool isSleep = false;
	public GameObject Trap;
	public GameObject MainCamera;
//	public GameObject PanelLock;
	//
	private string  btn_Move;
	private string btn_Jump;
	private string btn_Action;
	//private string btn_Lamp;
	private Animator anim;
	private float GravityScale;
	public float ScaleX;

	// Point Catch Player
	public GameObject PointCatchPlayer;

	// Lamp
	public GameObject BtnLamp;
	public GameObject lampObject;
	private GameObject currentDoor;
	//private Transform Transporter;
	[HideInInspector]
	public float
		moveSpeed;
	public float runSpeed = 5;
	public float defaultMoveSpeed = 2;
	public float jump = 600;
	private float ran;
	private bool ranBool = false;
	private bool doorIsReady = false;
	public GameObject Head;
	public GameObject Leg;
	//[HideInInspector]
	public PlayerState playerState;
//	[HideInInspector]
	public bool
		isRight, LampTurnedOn;
	public GameObject point;


	// Use this for initialization
	void Start ()
	{
		//Debug.Log ("Start:" + LegPlusIndex + ", " + HeadPlusIndex);
		NotificationManager.Instance.AddListener (this, "Load");
		NotificationManager.Instance.AddListener (this, "Save");
		CommonVariable.Instance.btn_Move = "";
		CommonVariable.Instance.btn_Jump = "";
		CommonVariable.Instance.btn_Action = "";
		CommonVariable.Instance.isPause = false;
		audioManager = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		StartCoroutine (RandomActionEyes ());
		GravityScale = this.GetComponent<Rigidbody2D> ().gravityScale;
		moveSpeed = defaultMoveSpeed;
		OnStateChange (PlayerState.Standing);
		anim = GetComponent<Animator> ();
		ScaleX = this.transform.localScale.x;
		Head = GameObject.Find ("Sprite_GonzalesChika_Head_1_2");
		Leg = GameObject.Find ("Sprite_GonzalesChika_Leg_Fore_Left 1");
		NotificationManager.Instance.AddListener (this, "OnAction");
		CheckWounded ();
	}

	public void CheckWounded ()
	{
		if (isWounded_Head)
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (4);
		else
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0 + HeadPlusIndex);
		
		if (isWounded_Leg)
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (1);
		else
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0 + LegPlusIndex);
	}

	public void Save ()
	{ 
		string i = CommonVariable.Instance.loadi;
		ES2.Save (HeadPlusIndex, this.gameObject.name + "PlayerController" + i + "?tag=HeadPlusIndex" + i);
		ES2.Save (LegPlusIndex, this.gameObject.name + "PlayerController" + i + "?tag=LegPlusIndex" + i);
		ES2.Save (isWounded_Head, this.gameObject.name + "PlayerController" + i + "?tag=isWounded_Head" + i);
		ES2.Save (isWounded_Leg, this.gameObject.name + "PlayerController" + i + "?tag=isWounded_Leg" + i);
	}
	
	public void Load ()
	{
		//chạy load -> gán lại cho biến thôi
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "PlayerController" + i)) {
			HeadPlusIndex = ES2.Load<int> (this.gameObject.name + "PlayerController" + i + "?tag=HeadPlusIndex" + i);
			Debug.Log ("LOAD" + HeadPlusIndex);
			LegPlusIndex = ES2.Load<int> (this.gameObject.name + "PlayerController" + i + "?tag=LegPlusIndex" + i);
			isWounded_Head = ES2.Load<bool> (this.gameObject.name + "PlayerController" + i + "?tag=isWounded_Head" + i);
			isWounded_Leg = ES2.Load<bool> (this.gameObject.name + "PlayerController" + i + "?tag=isWounded_Leg" + i);
			CheckWounded ();
		} else {
			//chưa có lưu gì. load dữ liệu mặc định
			print ("chưa có");
		}
	}

	// Update is called once per frame
	void Update ()
	{

		//Debug.Log(""+ gameObject.GetComponentsInChildren<Transform>().GetLength(10) );

		//keo,day
		if (Input.GetKeyDown (KeyCode.A)) {
			anim.CrossFade ("Pushing", 0.1f);
			OnStateChange (PlayerState.Stand_PushAndPull);
		} else 
		if (Input.GetKeyDown (KeyCode.S)) {
			anim.CrossFade ("Pulling", 0.1f);
		}
//		else
//		if(Input.GetKeyDown(KeyCode.H)){
//			anim.CrossFade("Hiding",0.1f);
//		}

		//  Tắt, bật đèn
		if (Input.GetKeyDown (KeyCode.F)) {
			if (lampObject.activeSelf) {
				anim.SetLayerWeight (1, 0);
				lampObject.SetActive (false);
				
			} else {
				anim.CrossFade ("Lamp", 0.1f);
				anim.SetLayerWeight (1, 1);
				lampObject.SetActive (true);
			}
		}
	}

	public void OnStateChange (PlayerState state)
	{
		switch (state) {
		case PlayerState.Standing:
			playerState = PlayerState.Standing;
			break;
		case PlayerState.Walking:
			playerState = PlayerState.Walking;
			break;
		case PlayerState.Jumping:
			playerState = PlayerState.Jumping;
			break;
		case PlayerState.Sleeping:
			playerState = PlayerState.Sleeping;
			break;
		case PlayerState.Stand_PushAndPull:
			playerState = PlayerState.Stand_PushAndPull;
			break;
		case PlayerState.Walk_PushAndPull:
			playerState = PlayerState.Walk_PushAndPull;
			break;
		case PlayerState.Hiding:
			playerState = PlayerState.Hiding;
			break;
		case PlayerState.Running:
			playerState = PlayerState.Running;
			break;
		case PlayerState.MovingTo:
			playerState = PlayerState.MovingTo;
			break;
		case PlayerState.Trapped:
			playerState = PlayerState.Trapped;
			break;
		case PlayerState.Traping:
			playerState = PlayerState.Traping;
			break;
		case PlayerState.OpenTheDoor:
			playerState = PlayerState.OpenTheDoor;
			break;
		case PlayerState.Reading:
			playerState = PlayerState.Reading;
			break;
		case PlayerState.Freeze:
			playerState = PlayerState.Freeze;
			break;
		case PlayerState.Die:
			playerState = PlayerState.Die;
			break;
		case PlayerState.DieCrazy:
			playerState = PlayerState.DieCrazy;
			break;
		case PlayerState.Sleeping2:
			playerState = PlayerState.Sleeping2;
			break;
		default:
			break;
		}
	}

	void OnAction ()
	{
		if (isWounded_Head) {
			Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
			GameObject RedPanel = _canvas.Find ("RedPanel").gameObject;
			RedPanel.SetActive (true);
		}
		if (CommonVariable.Instance.isPause) {
			
			audioManager.au_running.Stop ();
			audioManager.au_footstep.Stop ();
		}
//		Debug.Log (CommonVariable.Instance.btn_Move + "");
		btn_Move = CommonVariable.Instance.btn_Move;
		btn_Jump = CommonVariable.Instance.btn_Jump;
		btn_Action = CommonVariable.Instance.btn_Action;
		//btn_Lamp = CommonVariable.Instance.btn_Lamp;
		switch (playerState) {
		case PlayerState.Standing:
			OnStandingState ();
			break;
		case PlayerState.Walking:
			OnWalkingState ();
			break;
		case PlayerState.Jumping:
			OnJumpingState ();
			break;
		case PlayerState.Running:
			OnRunningState ();
			break;
		case PlayerState.Sleeping:
			OnSleepingState ();
			break;
		case PlayerState.Stand_PushAndPull:
			OnStand_PushAndPullState ();
			break;
		case PlayerState.Walk_PushAndPull:
			OnWalk_PushAndPullState ();
			break;
		case PlayerState.Hiding:
			OnHidingState ();
			break;
		case PlayerState.MovingTo:
			OnMovingToState ();
			break;
		case PlayerState.OpenTheDoor:
			OnOpenTheDoorState ();
			break;
		case PlayerState.Trapped:
			OnTrappedState ();
			break;
		case PlayerState.Traping:
			OnTrapingState ();
			break;
		case PlayerState.Reading:
			OnReadingState ();
			break;
		case PlayerState.Freeze:
			OnFreezeState ();
			break;
		case PlayerState.Die:
			OnDieState ();
			break;
		case PlayerState.DieCrazy:
			OnDieCrazyState ();
			break;
		case PlayerState.Sleeping2:
			OnSleeping2State ();
			break;
		default:
			break;
		}
	}

	void OnStandingState ()
	{
		this.gameObject.GetComponent<Status> ().StaminaDescending = 140;
		isSleep = false;
		//OnOffLamp();
		DisablezZSleeping ();
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();
		if (!CommonVariable.Instance.isPause) {
			//moveSpeed = defaultMoveSpeed;
			if (ranBool)
				StartCoroutine (RandomActionEyes ());
			if (btn_Jump == "JumpButtonDown") {
				OnStateChange (PlayerState.Jumping);
				anim.CrossFade ("Jumping", 0.1f);
				GetComponent<Rigidbody2D> ().AddForce (Vector3.up * jump);

			}
		// Di chuyển
		else if (btn_Move == "LeftButtonDrag" && !disableToLeft || btn_Move == "RightButtonDrag" && !disableToRight) {
				anim.CrossFade ("Walking", 0.1f);
				OnStateChange (PlayerState.Walking);
				CheckWounded ();
				audioManager.au_footstep.Play ();
			}

			if (btn_Move == "LeftButtonDouble" && !disableToLeft || btn_Move == "RightButtonDouble" && !disableToRight) {
				if (!this.GetComponent<Status> ().outofphysical) {
					OnStateChange (PlayerState.Running);
					audioManager.au_running.Play ();
				}
			}

			if (Input.GetKeyDown (KeyCode.M)) {
				//anim.CrossFade("Sleeping",0.3f);
				anim.CrossFade ("Walking", 0.1f);
				//if(Vector3.Distance(gameObject.transform.position, bed.position)<1)
				//OnStateChange (PlayerState.MovingTo);
				//Head.GetComponent<Uni2DSprite>().spriteAnimation.Play(3);
			}
		}
	}

	void OnWalkingState ()
	{

		audioManager.au_running.Stop ();
		if (!CommonVariable.Instance.isPause) {
			if (moveSpeed == runSpeed) {
				anim.CrossFade ("Running", 0.1f);
				OnStateChange (PlayerState.Running);
			} else
				OnStateChange (PlayerState.Walking);

			// Di chuyển sang phải + Nhảy , Trái + Nhảy
			if (btn_Jump == "JumpButtonDown" && btn_Move == "LeftButtonDrag" && !disableToLeft
				|| btn_Jump == "JumpButtonDown" && btn_Move == "LeftButtonDown" && !disableToLeft) {	
				//GetComponent<Rigidbody2D>().AddForce (Vector3.left * Jump_Move);
				anim.CrossFade ("Jumping", 0.1f);
				//transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
				transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);
			} else if (btn_Jump == "JumpButtonDown" && btn_Move == "RightButtonDrag" && !disableToRight
				|| btn_Jump == "JumpButtonDown" && btn_Move == "RightButtonDown" && !disableToRight) {
				//GetComponent<Rigidbody2D>().AddForce (Vector3.right * Jump_Move);
				anim.CrossFade ("Jumping", 0.1f);
				transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
				transform.localScale = new Vector3 (-ScaleX, this.transform.localScale.y, 1);
			} else

		// Left
		if (btn_Move == "LeftButtonDrag" && !disableToLeft) {
				transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
				anim.Play ("Walking");
				//anim.CrossFade ("Walking", 0.1f);
				transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);
			} else if (btn_Move == "LeftButtonUp") {
				anim.CrossFade ("Standing", 0.1f);
				OnStateChange (PlayerState.Standing);

				// Right
			} else if (btn_Move == "RightButtonDrag" && !disableToRight) {
				transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
				anim.Play ("Walking");
				//anim.CrossFade ("Walking",0.1f);
				transform.localScale = new Vector3 (-ScaleX, this.transform.localScale.y, 1);
			} else if (btn_Move == "RightButtonUp") {
				anim.CrossFade ("Standing", 0.1f);
				OnStateChange (PlayerState.Standing);
			}
			if (btn_Jump == "JumpButtonDown") {
				OnStateChange (PlayerState.Jumping);
				//transform.localScale = new Vector3 (this.transform.localScale.x, this.transform.localScale.y, 1);
				GetComponent<Rigidbody2D> ().AddForce (Vector3.up * jump);
				//anim.CrossFade ("Flying", 0.1f);
			}
		}
	}

	void OnJumpingState ()
	{
		audioManager.au_footstep.Stop ();
		audioManager.au_running.Stop ();
		if (!CommonVariable.Instance.isPause) {
			if (isWounded_Head)
				Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (4);
			else
				Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (1 + HeadPlusIndex);
			
			if (isWounded_Leg)
				Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (1);
			else
				Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0 + LegPlusIndex);
			if (btn_Move == "LeftButtonDrag" && !disableToLeft) {		
				transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
				transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);
			} else if (btn_Move == "RightButtonDrag" && !disableToRight) {	

				transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
				transform.localScale = new Vector3 (-ScaleX, this.transform.localScale.y, 1);
			}
		}
	}

	void OnRunningState ()
	{
//		au_footstep.Stop ();
		if (isWounded_Head)
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (4);
		else
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (1 + HeadPlusIndex);
		
		if (isWounded_Leg)
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (1);
		else
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0 + LegPlusIndex);

		if (this.GetComponent<Status> ().outofphysical) 
			moveSpeed = defaultMoveSpeed;

		if (!CommonVariable.Instance.isPause) {
			if (btn_Jump == "JumpButtonDown" && btn_Move == "LeftButtonDrag" && !disableToLeft
				|| btn_Jump == "JumpButtonDown" && btn_Move == "LeftButtonDown") {	
				//GetComponent<Rigidbody2D>().AddForce (Vector3.left * Jump_Move);
				anim.CrossFade ("Jumping_Run", 0.1f);
				transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
				transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);
			} else if (btn_Jump == "JumpButtonDown" && btn_Move == "RightButtonDrag" && !disableToRight
				|| btn_Jump == "JumpButtonDown" && btn_Move == "RightButtonDown") {
				//GetComponent<Rigidbody2D>().AddForce (Vector3.right * Jump_Move);
				anim.CrossFade ("Jumping_Run", 0.1f);
				transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
				transform.localScale = new Vector3 (-ScaleX, this.transform.localScale.y, 1);
			} else

		// Left
		if (btn_Move == "LeftButtonDrag" && !disableToLeft) {
				if (!this.GetComponent<Status> ().outofphysical) { 
					anim.Play ("Running");
					moveSpeed = runSpeed;
//					audioManager.au_footstep.Stop ();
//					audioManager.au_running.Play ();
				} else {
//					audioManager.au_footstep.Play ();
//					audioManager.au_running.Stop ();
					anim.Play ("Walking");
					moveSpeed = defaultMoveSpeed;
				}
				transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
				//anim.CrossFade ("Walking", 0.1f);
				transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);

			} else if (btn_Move == "LeftButtonUp") {
				CheckWounded ();
				anim.CrossFade ("Standing", 0.1f);
				OnStateChange (PlayerState.Standing);
				moveSpeed = defaultMoveSpeed;
			} else 
		
		// Right
		if (btn_Move == "RightButtonDrag" && !disableToRight) {
				if (!this.GetComponent<Status> ().outofphysical) { 
					anim.Play ("Running");
					moveSpeed = runSpeed;
//					audioManager.au_running.Play ();
//					audioManager.au_footstep.Stop ();
				} else {
//					audioManager.au_footstep.Play ();
//					audioManager.au_running.Stop ();
					anim.Play ("Walking");
					moveSpeed = defaultMoveSpeed;
				}
				transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
				transform.localScale = new Vector3 (-ScaleX, this.transform.localScale.y, 1);
			} else if (btn_Move == "RightButtonUp") {
				CheckWounded ();
				anim.CrossFade ("Standing", 0.1f);
				OnStateChange (PlayerState.Standing);
				moveSpeed = defaultMoveSpeed;
			}
			if (btn_Jump == "JumpButtonDown") {
				OnStateChange (PlayerState.Jumping);
				//transform.localScale = new Vector3 (this.transform.localScale.x, this.transform.localScale.y, 1);
				GetComponent<Rigidbody2D> ().AddForce (Vector3.up * jump);
				//anim.CrossFade ("Flying", 0.1f);
			}
		}
		
	
	}

	void OnStand_PushAndPullState ()
	{
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();
		if (!CommonVariable.Instance.isPause) {
			moveSpeed = speedPushPull;

			if (btn_Move == "LeftButtonDrag") {
				audioManager.au_footstep.Play ();
				if (isRight) {
					anim.CrossFade ("Pushing", 0.1f);
					anim.SetLayerWeight (2, 1);
				} else { 
					anim.CrossFade ("Pulling", 0.1f);
					anim.SetLayerWeight (3, 0);
				}
				OnStateChange (PlayerState.Walk_PushAndPull);


			} else if (btn_Move == "RightButtonDrag") {
				audioManager.au_footstep.Play ();
				if (isRight) {
					anim.CrossFade ("Pulling", 0.1f);
					anim.SetLayerWeight (3, 1);
				} else {
					anim.CrossFade ("Pushing", 0.1f);
					anim.SetLayerWeight (2, 0);
				}
				OnStateChange (PlayerState.Walk_PushAndPull);


			}

			if (btn_Move == "RightButtonDouble" || btn_Move == "LeftButtonDouble") {
				audioManager.au_footstep.Play ();
				if (!isRight) {
					if (btn_Move == "RightButtonDouble")
						anim.CrossFade ("Pulling", 0.1f);
					else 
					if (btn_Move == "LeftButtonDouble")
						anim.CrossFade ("Pushing", 0.1f);
				} else {
					if (btn_Move == "RightButtonDouble")
						anim.CrossFade ("Pushing", 0.1f);
					else 
					if (btn_Move == "LeftButtonDouble")
						anim.CrossFade ("Pulling", 0.1f);
				}
			}
			// press Action button
			if (btn_Action == "ActionButtonDown") {
				OnStateChange (PlayerState.Standing);
				anim.CrossFade ("Standing", 0.1f);
				anim.SetLayerWeight (3, 0);
				anim.SetLayerWeight (2, 0);
				//isAction = false;
			}
		}
	}

	void OnWalk_PushAndPullState ()
	{
		if (!CommonVariable.Instance.isPause) {
			moveSpeed = speedPushPull;
			// Left
			if (btn_Move == "LeftButtonDrag" && !disableToLeft) {
				transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
				this.GetComponent<PlayerInteractive> ().CurrentObject.transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
			} else 
		if (btn_Move == "LeftButtonUp") {
				anim.CrossFade ("Standing_PushAndPull", 0.1f);
				OnStateChange (PlayerState.Stand_PushAndPull);
				anim.SetLayerWeight (3, 0);
				anim.SetLayerWeight (2, 0);
			} else 
			// Right
		if (btn_Move == "RightButtonDrag" && !disableToRight) {
				transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
				this.GetComponent<PlayerInteractive> ().CurrentObject.transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
			} else if (btn_Move == "RightButtonUp") {
				anim.CrossFade ("Standing_PushAndPull", 0.1f);
				OnStateChange (PlayerState.Stand_PushAndPull);
				anim.SetLayerWeight (3, 0);
				anim.SetLayerWeight (2, 0);
			}
		}
	}

	void OnMovingToState ()
	{
		if (CommonVariable.Instance.isPause)
			moveSpeed = 0;
		else
			moveSpeed = defaultMoveSpeed;
		CommonVariable.Instance.btn_Action = "";
		CommonVariable.Instance.btn_Move = "";
		if (this.GetComponent<PlayerInteractive> ().CurrentObject != null && this.GetComponent<PlayerInteractive> ().CurrentObject.transform.childCount > 0) {
			if (this.GetComponent<PlayerInteractive> ().CurrentObject.transform.FindChild ("Center_Point") != null) {
				point = this.GetComponent<PlayerInteractive> ().CurrentObject.transform.FindChild ("Center_Point").gameObject;
				this.transform.position = Vector3.MoveTowards (this.transform.position, point.transform.position, moveSpeed * Time.deltaTime);
				//Debug.Log(""+(this.transform.position.x - point.transform.position.x));
//			if (this.GetComponent<PlayerInteractive> ().CurrentObject.tag != "Blocking") {
				if (this.transform.position.x - this.GetComponent<PlayerInteractive> ().CurrentObject.transform.position.x < 0) {
					transform.localScale = new Vector3 (-ScaleX, this.transform.localScale.y, 1);
				} else 
			if (this.transform.position.x - this.GetComponent<PlayerInteractive> ().CurrentObject.transform.position.x > 0) {
					transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);
				}
//			}
				if (this.transform.position.x - point.transform.position.x == 0) {

					if (this.GetComponent<PlayerInteractive> ().CurrentObject.tag == "Bed") {
						this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);				
						this.transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);
						anim.CrossFade ("Prepare_Sleeping", 0.3f);
						//OnStateChange (PlayerState.Sleeping);
						//Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3);
					} else 
				if (this.GetComponent<PlayerInteractive> ().CurrentObject.tag == "Cabinet") {

						audioManager.au_cabinet_open.Play ();
						this.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<Animator> ().Play ("CabinetOpen");
						anim.CrossFade ("Hiding", 0.3f);
						this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);				
						this.transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);
						//OnStateChange (PlayerState.Hiding);
					} 
//				else
//				if (this.GetComponent<PlayerInteractive> ().CurrentObject.tag == "Blocking") {
//					OnStateChange (PlayerState.Reading);
//					anim.CrossFade ("Standing", 0.3f);
//					for (int i=GetComponent<ItemPriority>().HitObjectsList.Count - 1; i > -1; i--) {
//						if (GetComponent<ItemPriority> ().HitObjectsList [i]._Collision2D.gameObject.tag == "Blocking") {
//							GetComponent<ItemPriority> ().HitObjectsList.RemoveAt (i);
//						}
//					}
//				}
				}
			} else if (this.GetComponent<PlayerInteractive> ().CurrentObject.transform.FindChild ("Left_Point") != null ||
				this.GetComponent<PlayerInteractive> ().CurrentObject.transform.FindChild ("Right_Point") != null) {
				//if(this.GetComponent<PlayerInteractive>().CurrentObject.transform.FindChild("Left_Point") != null){
				if (this.transform.position.x - this.GetComponent<PlayerInteractive> ().CurrentObject.transform.position.x < 0) {
					transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);
					point = this.GetComponent<PlayerInteractive> ().CurrentObject.transform.FindChild ("Left_Point").gameObject;
					isRight = false;

				} else if (this.transform.position.x - this.GetComponent<PlayerInteractive> ().CurrentObject.transform.position.x >= 0) {
					transform.localScale = new Vector3 (-ScaleX, this.transform.localScale.y, 1);
					point = this.GetComponent<PlayerInteractive> ().CurrentObject.transform.FindChild ("Right_Point").gameObject;
					isRight = true;
				}
				if (point != null) {
					this.transform.position = Vector3.MoveTowards (this.transform.position, point.transform.position, moveSpeed * Time.deltaTime);
				}
				if (this.transform.position.x - point.transform.position.x == 0) {
					this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);				
					//this.transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);
					if (isRight)
						this.transform.localScale = new Vector3 (ScaleX, this.transform.localScale.y, 1);
					else
						this.transform.localScale = new Vector3 (-ScaleX, this.transform.localScale.y, 1);
					anim.CrossFade ("Standing_PushAndPull", 0.1f);
					OnStateChange (PlayerState.Stand_PushAndPull);
					//Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3);
				}
			}
		} else {
			//OnStateChange (PlayerState.Reading);
//			StartCoroutine( StopCharacterAutoWalking(1));
			MoveDirection (this.GetComponent<PlayerInteractive> ().Direction);
		}

	}

	public void OnTrappedState ()
	{
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();
	}

	void OnTrapingState ()
	{
		Trap = Instantiate (Trap) as GameObject;
		Trap.transform.position = new Vector3 (this.transform.position.x - 1, this.transform.position.y);
		Trap.tag = "TrapPlayer";
	}

	void OnSleepingState ()
	{
		if (!isSleep) {
			isSleep = true;
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3 + HeadPlusIndex);
			if (!isHiding)
				this.gameObject.GetComponent<Status> ().StaminaDescending = -25;
			else
				this.gameObject.GetComponent<Status> ().StaminaDescending = -45;
		}
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();

		if (isWounded_Head)
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (4);
		else
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3 + HeadPlusIndex);
		
		if (isWounded_Leg)
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (1);
		else
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0 + LegPlusIndex);

		if (CommonVariable.Instance.btn_Action == "ActionButtonDown" && !isHiding) {
			DisablezZSleeping ();
			isSleep = false;
			this.gameObject.GetComponent<Status> ().StaminaDescending = 140;
			CheckWounded ();
			anim.CrossFade ("WakingUp", 0.1f);
			OnStateChange (PlayerState.Standing);
		}
		if (isHiding) {
			if (CommonVariable.Instance.btn_Move == "LeftButtonDrag" || CommonVariable.Instance.btn_Move == "RightButtonDrag") {
				OnShowingState ();
				isSleep = false;
				DisablezZSleeping ();
				audioManager.au_cabinet_open.Play ();
				this.GetComponent<Animator> ().CrossFade ("Reverse_Hiding", 0.1f);
				StartCoroutine (this.GetComponent<PlayerInteractive> ().HiddenPlayer (0.7f, "Layer 2", -1));
				this.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<Animator> ().Play ("CabinetOpen");
				this.GetComponent<PlayerController> ().isAction = false;
				CheckWounded ();
			}
		}
	}

	void OnSleeping2State ()
	{
		isAction = true;
		if (!isSleep) {
			isSleep = true;
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3 + HeadPlusIndex);
			this.gameObject.GetComponent<Status> ().StaminaDescending = -21;
		}
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();
		if (isWounded_Head)
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (4);
		else
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3 + HeadPlusIndex);
		
		if (isWounded_Leg)
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (1);
		else
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0 + LegPlusIndex);

		if (CommonVariable.Instance.btn_Action == "ActionButtonDown") {
			isAction =false;
			DisablezZSleeping ();
			isSleep = false;
			this.gameObject.GetComponent<Status> ().StaminaDescending = 140;
			CheckWounded ();
			anim.CrossFade ("WakingUpStaminaOver", 0.1f);
			OnStateChange (PlayerState.Standing);
		}
	}

	public void EnablezZSleeping ()
	{
		Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
		_canvas.Find ("SleepingzZ").gameObject.SetActive (true);
	}

	public void DisablezZSleeping ()
	{
		Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
		_canvas.Find ("SleepingzZ").gameObject.SetActive (false);
	}

	void OnOpenTheDoorState ()
	{
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();
		CommonVariable.Instance.btn_Action = "";
		CommonVariable.Instance.btn_Move = "";

	}

	// On Reading State
	void OnReadingState ()
	{
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();
		//CommonVariable.Instance.btn_Move = "";
		if (CommonVariable.Instance.btn_Action == "ActionButtonDown"
			|| CommonVariable.Instance.btn_Action == "ActionButtonUp"
			|| CommonVariable.Instance.btn_Move == "RightButtonUp"
			|| CommonVariable.Instance.btn_Move == "LeftButtonUp"
			|| CommonVariable.Instance.btn_Move == "JumpButtonUp") {
			OnStateChange (PlayerState.Standing);
		}
		anim.CrossFade ("Standing", 0.1f);
		CheckWounded ();
	}

	public void MoveDirection (int _direction)
	{
		CommonVariable.Instance.btn_Move = "";
		//this.transform.position = Vector3.MoveTowards (this.transform.position,this.GetComponent<PlayerInteractive> ().posMove , moveSpeed * Time.deltaTime);
		this.transform.Translate (Vector2.right * moveSpeed * _direction * Time.deltaTime);

	}

	void OnFreezeState ()
	{
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();

		CommonVariable.Instance.btn_Action = "";
		CommonVariable.Instance.btn_Move = "";
		DisablezZSleeping ();
	}

	void OnHidingState ()
	{
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();

		isHiding = true;
		// Check Lamp
		anim.SetLayerWeight (1, 0);
		lampObject.SetActive (false);

		foreach (Collider2D cool in GetComponents<Collider2D> ()) {
			cool.enabled = false;
		}
		this.GetComponent<Rigidbody2D> ().gravityScale = 0;
//		if (CommonVariable.Instance.btn_Action == "ActionButtonDown") {
//			OnShowingState ();
//		}

		if (CommonVariable.Instance.btn_Move == "LeftButtonDrag" || CommonVariable.Instance.btn_Move == "RightButtonDrag") {
			OnShowingState ();
			audioManager.au_cabinet_open.Play ();
			this.GetComponent<Animator> ().CrossFade ("Reverse_Hiding", 0.1f);
			//OrderInLayer("Default",-1);
			StartCoroutine (this.GetComponent<PlayerInteractive> ().HiddenPlayer (0.7f, "Layer 2", -1));
			this.GetComponent<PlayerInteractive> ().CurrentObject.GetComponent<Animator> ().Play ("CabinetOpen");
			this.GetComponent<PlayerController> ().isAction = false;
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0 + HeadPlusIndex);
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0 + LegPlusIndex);
			//this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Standing;
		}

	}

	public void OnShowingState ()
	{
		if (BtnLamp.activeSelf == true && LampTurnedOn) {
			anim.SetLayerWeight (1, 1);
			lampObject.SetActive (true);
		}
		isHiding = false;
		this.GetComponent<Rigidbody2D> ().gravityScale = GravityScale;
		foreach (Collider2D cool in GetComponents<Collider2D> ()) {
			cool.enabled = true;
		}
	}
	// On Die State
	void OnDieState ()
	{
		CommonVariable.Instance.btn_Action = "";
		CommonVariable.Instance.btn_Move = "";
//		Debug.CommonVariable.Instance.btn_Action = "";Log ("GameOver");
		StartCoroutine (this.GetComponent<PlayerInteractive> ().HiddenPlayer (0f, "Layer 2", -1));
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();

		this.GetComponent<CircleCollider2D> ().isTrigger = true;
		MainCamera.GetComponent<SmoothCameraFollow> ().target = null;
		this.transform.position = new Vector3 (PointCatchPlayer.transform.position.x, PointCatchPlayer.transform.position.y - 0.47f, this.transform.position.z);
		//this.transform.position = Vector3.MoveTowards (this.transform.position, PointCatchPlayer.transform.position, Time.deltaTime);
		if (isWounded_Head)
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (4);
		else
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (1 + HeadPlusIndex);
		
		if (isWounded_Leg)
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (1);
		else
			Leg.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0 + LegPlusIndex);
		//this.GetComponent<Rigidbody2D> ().gravityScale = 0;
	}

	void OnDieCrazyState ()
	{
		CommonVariable.Instance.btn_Action = "";
		Debug.Log ("GameOver");
		Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
		//_canvas.Find ("RedPanel").gameObject.SetActive (true);
		GameOver ();
		_canvas.Find ("Button").gameObject.SetActive (false);
		_canvas.Find ("Status").gameObject.SetActive (false);
		audioManager.au_running.Stop ();
		audioManager.au_footstep.Stop ();
		//this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
		MainCamera.GetComponent<SmoothCameraFollow> ().target = null;
		this.GetComponent<Rigidbody2D> ().gravityScale = 0;

	}

	public void GameOver ()
	{
		Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
		GameObject GameOver = _canvas.Find ("GameOver").gameObject;
		GameOver.SetActive (true);
	}

	public void BloodScreen ()
	{
		Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
		GameObject BloodScreen = _canvas.Find ("BloodScreen").gameObject;
		BloodScreen.SetActive (true);
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Trap") {
			Debug.Log ("hit trap, trừ máu ở đây");
			//CurrentObject = coll.gameObject;
			this.GetComponent<Status> ().changeHealth (0.5f); 
			coll.gameObject.GetComponent<Animator> ().CrossFade ("TrapClose", 0.1f);
			this.GetComponent<Animator> ().CrossFade ("Trapped", 0.1f);
			this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Trapped;
			coll.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			int i;
			Debug.Log(coll.gameObject.name + ", " + coll.gameObject.transform.parent.name + ", ");
			if(coll.gameObject.transform.parent.name.Contains("AI_1")){
				GameObject Ai =GameObject.Find("AI_1");
				Ai.GetComponent<AISaveLoad> ().FollowPlayer_3 = false;
				Ai.GetComponent<AISaveLoad> ().BoxCollider2D_3 = false;
			}
			if(coll.gameObject.transform.parent.name.Contains("AI_2")){
				GameObject Ai =GameObject.Find("AI_2");
				Ai.GetComponent<AISaveLoad> ().FollowPlayer_3 = false;
				Ai.GetComponent<AISaveLoad> ().BoxCollider2D_3 = false;
			}
			isWounded_Leg = true;
			LegPlusIndex = 2;
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{

		if (coll.gameObject.tag == "Ground" && playerState == PlayerState.Jumping) {
			anim.CrossFade ("Standing", 0.2f);
			OnStateChange (PlayerState.Standing);
			CheckWounded ();


		} else
		if (coll.gameObject.tag == "Deadend" || coll.gameObject.tag == "Desk") {
			if (!isAction) {
				if (this.transform.localScale.x == -ScaleX) {
					disableToRight = true;
					disableToLeft = false;
				} else {
					disableToLeft = true;
					disableToRight = false;
				}
			}
		} 
//		else {
//			if (isRight) {
//				disableToRight = true;
//				disableToLeft = false;
//			} else {
//				disableToLeft = true;
//				disableToRight = false;
//			}
//		}
		if (coll.gameObject.tag == "Deadend") {
			if (isAction) {
				if (isRight) {
					disableToRight = true;
					disableToLeft = false;
				} else {
					disableToLeft = true;
					disableToRight = false;
				}
			}
		}
		if (coll.gameObject.name == "IronBall") {
			//anim.CrossFade ("Standing", 0.2f);
			//OnStateChange (PlayerState.Standing);
			Debug.Log ("dính chưởng_nếu dùng thuốc thì, isWounded = false && HeadPlusIndex = 5");
			//isWounded = true;
			isWounded_Head = true;
			//HeadPlusIndex = 5;
			//Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (4);
		}
	}

	void OnTriggerStay2D (Collider2D coll)
	{
//		if (coll.gameObject.tag == "AI") {
//			PointCatchPlayer = coll.gameObject.transform.FindChild ("Enemy").gameObject.GetComponent<EnemyBoxCollider2D> ().PointCatchPlayer;
//		}

		if (coll.gameObject.tag == "Desk") {
			if (!isAction) {
//				if (this.transform.localScale.x == -ScaleX) {
//					disableToRight = true;
//					disableToLeft = false;
//				} else {
//					disableToLeft = true;
//					disableToRight = false;
//				}
				if (this.transform.localScale.x == -ScaleX) {
					disableToLeft = false;

				} else
					disableToRight = false;
			}
		} 
	}

	void OnCollisionExit2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Deadend" || coll.gameObject.tag == "Desk") {
			disableToLeft = false;
			disableToRight = false;
		}
	}

//	void OnTriggerEnter2D (Collider2D coll)
//	{
//		if (coll.gameObject.tag == "Deadend" || 
////		     coll.gameObject.GetComponent<BoxCollider2D> () !=null && 
//		    coll.gameObject.tag =="Desk"
////		     coll.gameObject.GetComponent<BoxCollider2D> () !=null && coll.gameObject.GetComponent<BoxCollider2D> ().isTrigger ==false && playerState == PlayerState.Running ||
////		     coll.gameObject.GetComponent<BoxCollider2D> () !=null && coll.gameObject.GetComponent<BoxCollider2D> ().isTrigger ==false && playerState == PlayerState.Jumping
//		    // coll.gameObject.GetComponent<BoxCollider2D> ()
//		    ) {
//			if (isAction) {
//				if (isRight){
//					disableToRight = true;
//					disableToLeft = false;
//				}
//				else{
//					disableToLeft = true;
//					disableToRight = false;
//				}
//			} else {
//				if (this.transform.localScale.x == -ScaleX) {
//					disableToRight = true;
//					disableToLeft = false;
//				} else {
//					disableToLeft = true;
//					disableToRight = false;
//				}
//			}
//		}
//
//	}
//
//	void OnTriggerExit2D (Collider2D coll)
//	{
//		if( coll.gameObject.tag =="Desk" || coll.gameObject.tag == "Deadend"){
//			disableToLeft = false;
//			disableToRight = false;
//		}
//	}

	public void HeadAnmimation (int headIndex)
	{
		Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (headIndex + HeadPlusIndex);
	}

	IEnumerator RandomActionEyes ()
	{
		ranBool = false;
		ran = Random.Range (3, 10);
	
		yield return new WaitForSeconds (ran);
		//		Debug.Log (ran + "");

		if (isWounded_Head)
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (4);
		else if (isSleep) {
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3 + HeadPlusIndex);
		} else
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (2 + HeadPlusIndex);

		yield return new WaitForSeconds (2);

		if (isWounded_Head)
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (4);
		else if (isSleep) {
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3 + HeadPlusIndex);
		} else	
			Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0 + HeadPlusIndex);
		ranBool = true;
	}

}
