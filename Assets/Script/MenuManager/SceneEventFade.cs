using UnityEngine;
using System.Collections;

public class SceneEventFade : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void CloseScene(){
		FadeScene.Instance.LoadScene ();
	}
	public void DestroyAnimationOpen(){
		Destroy (gameObject);
	}
}
