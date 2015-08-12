using UnityEngine;
using System.Collections;

public class MouseTrap : MonoBehaviour {
	public bool isCapturedMouse;
	public GameObject Rat;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.name == "Rat(Clone)") {
			if (!isCapturedMouse) {
				Rat = coll.gameObject;
				this.GetComponent<Uni2DSprite> ().spriteAnimation.Play (0);
				coll.gameObject.GetComponent<Mouse>().MouseType = Mouse.MouseOrMousehole.MouseCaptured;
			}
			isCapturedMouse = true;
		}
	}

//	void OnTriggerStay2D (Collider2D coll)
//	{
//		if (coll.gameObject.tag == "Player") {
//			if(CommonVariable.Instance.btn_Action == ""){
//
//				Destroy(Rat);
//			}
//		}
//	}
}
