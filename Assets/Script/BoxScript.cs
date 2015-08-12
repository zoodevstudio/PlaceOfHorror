using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class BoxScript : MonoBehaviour
{

	private GameObject
		player;
	[HideInInspector]
	public Rigidbody2D
		rb;
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		rb.mass = 1000000;
		rb.angularDrag = 0;
		rb.fixedAngle = true;
		rb.gravityScale = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Deadend") {
			if (player.GetComponent<PlayerController> ().isAction) {
				if (!player.GetComponent<PlayerController> ().isRight) {
					player.GetComponent<PlayerController> ().disableToRight = true;
					player.GetComponent<PlayerController> ().disableToLeft = false;
				} else {
					player.GetComponent<PlayerController> ().disableToLeft = true;
					player.GetComponent<PlayerController> ().disableToRight = false;
				}
			}
//			else{
////				if (!player.GetComponent<PlayerController> ().isRight) {
////					player.GetComponent<PlayerController> ().disableToRight = true;
////					player.GetComponent<PlayerController> ().disableToLeft = false;
////				}
////				else {
////					player.GetComponent<PlayerController> ().disableToLeft = true;
////					player.GetComponent<PlayerController> ().disableToRight = false;
////				}
//				player.GetComponent<PlayerController> ().disableToLeft = false;
//				player.GetComponent<PlayerController> ().disableToRight = false;
//			}
		}
	}
	void OnCollisionExit2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Deadend" || coll.gameObject.tag == "Player") {
			player.GetComponent<PlayerController> ().disableToLeft = false;
			player.GetComponent<PlayerController> ().disableToRight = false;
		}
	}

}
