using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
	//[HideInInspector]
	public float
		currentPhysical;
	[Header("Health")]
	public Image
		Health;
	public Image HealthMask ;
	private float currentHealth;
	[Header("Peckish")]
	public Image
		Peckish;
	public Image PeckishMask ;
	public float PeckishDescending = 100.0f;
	private float currentPeckish;
	[Header("Stamina")]
	public Image
		Stamina;
	public Image StaminaMask ;
	public float StaminaDescending = 100.0f;
	private float currentStamina;
	//

	[Header("Spirit")]
	public Image
		Spirit;
	public Image SpiritMask ;
	private float currentSpirit;
	//public float waitTimeSpirit = 3.0f;

	[Header("Lamp")]
	public GameObject
		LampObject;
	public Image Lamp;
	public Image LampMask ;
	public float OilDescending = 7.0f;
	private float currentOil;
	private bool start;
	private GameObject player;
	private bool walking;

	void Start ()
	{
		currentPhysical = 100;
		currentOil = 1;
		NotificationManager.Instance.AddListener (this, "Save");
		NotificationManager.Instance.AddListener (this, "Load");
	}

	public void Save ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (currentHealth, this.gameObject.name + "Status" + i + "?tag=currentHealth" + i);
		ES2.Save (currentStamina, this.gameObject.name + "Status" + i + "?tag=currentStamina" + i);
		ES2.Save (currentPeckish, this.gameObject.name + "Status" + i + "?tag=currentPeckish" + i);
		ES2.Save (currentSpirit, this.gameObject.name + "Status" + i + "?tag=currentSpirit" + i);
		ES2.Save (currentOil, this.gameObject.name + "Status" + i + "?tag=currentOil" + i);
	}
	
	public void Load ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		if (ES2.Exists (this.gameObject.name + "Status" + i)) {
			currentHealth = ES2.Load<float> (this.gameObject.name + "Status" + i + "?tag=currentHealth" + i);
			HealthMask.fillAmount = currentHealth;
			currentStamina = ES2.Load<float> (this.gameObject.name + "Status" + i + "?tag=currentStamina" + i);
			StaminaMask.fillAmount = currentStamina;
			currentPeckish = ES2.Load<float> (this.gameObject.name + "Status" + i + "?tag=currentPeckish" + i);
			PeckishMask.fillAmount = currentPeckish;
			currentSpirit = ES2.Load<float> (this.gameObject.name + "Status" + i + "?tag=currentSpirit" + i);
			SpiritMask.fillAmount = currentSpirit;
			currentOil = ES2.Load<float> (this.gameObject.name + "Status" + i + "?tag=currentOil" + i);
			LampMask.fillAmount = currentOil;
		} 
	}

//	[HideInInspector]
	public bool
		outofphysical = false;

	void Update ()
	{

		if (this.gameObject.GetComponent<PlayerController> ().playerState == PlayerController.PlayerState.Running && !outofphysical) {
			if (currentPhysical > 0f)
				currentPhysical -= 0.25f;
			else {
				//Debug.Log ("Het Physical");
				outofphysical = true;
			}
		} else if (this.gameObject.GetComponent<PlayerController> ().playerState != PlayerController.PlayerState.Walking) {
			if (currentPhysical < 100f)
				currentPhysical += 0.3f;
			else {
				//Debug.Log ("Full Physical");
				outofphysical = false;
			}
		}

		if (!CommonVariable.Instance.isPause) {
			// Peckish
			changePeckishOverTime (PeckishDescending);

			// Stamina
			changeStaminaOverTime (StaminaDescending);

			// Lamp
			if (LampObject.activeSelf == true)
				changeOilOverTime (-OilDescending);
		}
	}

	public void changeHealth (float _health)
	{
		currentHealth += _health;
		HealthMask.fillAmount = currentHealth;
		if (currentHealth >= 1) {
			Debug.Log ("Chết vì mất máu");
			currentPeckish = 1;
			this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.DieCrazy;
		}
		if (currentHealth > 0.99f && currentHealth < 1) {
			this.GetComponent<Animator> ().CrossFade ("DieOfStatus", 0.1f);
			this.GetComponent<PlayerController> ().Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3);
		}
	}

	public void changePeckishOverTime (float _peckish)
	{
		currentPeckish += 1.0f / _peckish * 0.01f;
		PeckishMask.fillAmount = currentPeckish;
		if (currentPeckish > 1) {
			Debug.Log ("Chết vì đói");
			currentPeckish = 1;
			this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.DieCrazy;
		}
		if (currentPeckish > 0.99f && currentPeckish < 1) {
			this.GetComponent<Animator> ().CrossFade ("DieOfStatus", 0.1f);
			this.GetComponent<PlayerController> ().Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3);
		}
	}

	public void changePeckish (float _peckish)
	{
		currentPeckish = currentPeckish - _peckish;
		if (currentPeckish < 0)
			currentPeckish = 0;
	}

	public void changeStaminaOverTime (float _stamina)
	{
		currentStamina += 1.0f / _stamina * 0.01f;
		StaminaMask.fillAmount = currentStamina;
		if (currentStamina > 1) {
			Debug.Log ("Nằm xuống đất ngủ , " + currentStamina);
			currentStamina = 1;
			this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Sleeping2;

			//
		}
		if (currentStamina > 0.99f && currentStamina < 1) {
			if (!this.GetComponent<PlayerController> ().isAction) {
				if (!this.GetComponent<PlayerController> ().isHiding) {
					this.GetComponent<PlayerController> ().isAction = true;
					this.GetComponent<Animator> ().CrossFade ("PreSleepWhenStaminaOver", 0.1f);
				} else {
					this.GetComponent<PlayerController> ().isAction = true;
					this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Sleeping;
					//this.GetComponent<PlayerController> ().EnablezZSleeping ();
				}
			} else {
				if (this.GetComponent<PlayerInteractive> ().CurrentObject != null) {
					if (this.GetComponent<PlayerInteractive> ().CurrentObject.tag == "Bed") {
						this.GetComponent<PlayerController> ().EnablezZSleeping ();
						this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.MovingTo;
					} else if (this.GetComponent<PlayerInteractive> ().CurrentObject.tag == "Cabinet") {
						this.GetComponent<PlayerController> ().EnablezZSleeping ();
						this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Sleeping;
					} else if (this.GetComponent<PlayerInteractive> ().CurrentObject.tag == "Desk") {
						this.GetComponent<Animator> ().CrossFade ("PreSleepWhenStaminaOver", 0.1f);
						this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.Sleeping2;
					}
				}
			}
			//this.GetComponent<PlayerController>().Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3);
		}
	}

	public void changeSpirit (float _spirit)
	{
		currentSpirit += _spirit;
		SpiritMask.fillAmount = currentSpirit;
		if (currentSpirit > 1) {
			Debug.Log ("Chết vì mất tinh thần");
			currentSpirit = 1;
			this.GetComponent<PlayerController> ().playerState = PlayerController.PlayerState.DieCrazy;
		}
		if (currentSpirit > 0.99f && currentSpirit <= 1) {
			this.GetComponent<Animator> ().CrossFade ("DieOfStatus", 0.1f);
			this.GetComponent<PlayerController> ().Head.GetComponent<Uni2DSprite> ().spriteAnimation.Play (3);
		}
	}

	public void changeOil (float _oil)
	{
		currentOil = currentOil + _oil;
		LampMask.fillAmount = currentOil;
		if (currentOil > 1)
			currentOil = 1;
	}

	public void changeOilOverTime (float _oil)
	{
		//currentOil -= _oil * 0.01f;
		currentOil += 1.0f / _oil * 0.01f;
		LampMask.fillAmount = currentOil;
		if (currentOil < 0) {
			Debug.Log ("Hết dầu");
			currentOil = 0;
			this.GetComponent<PlayerController> ().lampObject.transform.FindChild ("Area light Player").gameObject.GetComponent<Light> ().enabled = false;
			this.GetComponent<PlayerController> ().lampObject.transform.FindChild ("Point light").gameObject.GetComponent<Light> ().enabled = false;
		}
	}
}
