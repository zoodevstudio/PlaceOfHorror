using UnityEngine;
using System.Collections;

public class DisableTrapSprite : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void DisableSprite(){
		foreach (Transform child in this.gameObject.transform) {
			child.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}
	public void EnableSprite(){
		foreach (Transform child in this.gameObject.transform) {
			child.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		}
	}
}
