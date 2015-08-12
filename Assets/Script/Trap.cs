using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{
	private AudioManager audioManager;
	private GameObject player;
	// Use this for initialization
	void Start ()
	{
		audioManager = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.GetComponent<Status> ().changeHealth (0.25f);
			Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
			_canvas.Find ("RedPanel").gameObject.SetActive (true);
			audioManager._start = true;
		}
		if (coll.gameObject.tag == "Ground") {
			StartCoroutine (waitDestroyTrap (0.5f));
		}
	}

	IEnumerator waitDestroyTrap (float second)
	{
		yield return new WaitForSeconds (second);
		this.gameObject.SetActive (false);
	}
}
