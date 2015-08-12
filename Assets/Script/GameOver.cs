using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
	public Color a;
	public Color b;
	// Use this for initialization
	void Start () {
		Transform _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
		GameObject Status =  _canvas.Find("Status").gameObject;
		GameObject Button =  _canvas.Find("Button").gameObject;
		Status.SetActive(false);
		Button.SetActive(false);
		//RedPanel.SetActive(true);
		//RedPanel.GetComponent<Image>().color = Color.black;
		//RedPanel.GetComponent<Image>().color = new Color(0f,0f,0f,0.7f);
		//RedPanel.GetComponent<Image>().color = Color.Lerp(a,b,Time.time);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
