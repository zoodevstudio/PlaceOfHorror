using UnityEngine;
using System.Collections;

public class LightEffect : MonoBehaviour
{

	public bool _break;
	public bool _flicker;
	float randomNumber;
	AudioClip au_breakLamp;
	// Use this for initialization
	void Start ()
	{
		au_breakLamp = (AudioClip) Resources.Load("Sounds/ChuotThetcuaBo");
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (_flicker) {
			randomNumber = Random.Range (0f, 3f);
			if (randomNumber <= 2.9f) {
				this.GetComponent<Light> ().enabled = true;
			} else
				this.GetComponent<Light> ().enabled = false;
		}
	
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if(_break && coll.gameObject.tag == "Player"){
			this.GetComponent<Light> ().enabled = false;
			// Play sound
			AudioSource.PlayClipAtPoint(au_breakLamp, transform.position,0.05f);
		}
	}
}
