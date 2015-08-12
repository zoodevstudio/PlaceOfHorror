using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (CommonVariable.Instance); 
		StartCoroutine(DisplayScene(3));
	}

	IEnumerator DisplayScene (float _second)
	{
		yield return new WaitForSeconds (_second);
		Application.LoadLevel("Menu");
	}
}
