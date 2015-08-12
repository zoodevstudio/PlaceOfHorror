using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FillImage : MonoBehaviour {
	public Image cooldown;
	public bool coolingDown;
	public float waitTime = 3.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (coolingDown == true)
		{
			//Reduce fill amount over 30 seconds
			cooldown.fillAmount -= 1.0f/waitTime * Time.deltaTime;
		}
	}
}
