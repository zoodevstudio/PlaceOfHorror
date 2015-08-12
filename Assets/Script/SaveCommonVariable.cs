using UnityEngine;
using System.Collections;

public class SaveCommonVariable : MonoBehaviour
{

	GameObject electric;

	void Start ()
	{
		NotificationManager.Instance.AddListener (this, "Save");
		NotificationManager.Instance.AddListener (this, "Load");
	}
	
	public void Save ()
	{ 
		string i = CommonVariable.Instance.loadi; 
		ES2.Save (CommonVariable.Instance.isElectricOn, this.gameObject.name + "SaveCommonVariable" + i + "?tag=isElectricOn" + i);
	}
	
	public void Load ()
	{ 
		string i = CommonVariable.Instance.loadi;
		if (!i.Contains ("0")) {
			//Debug.Log (ES2.Load<bool> (this.gameObject.name + "SaveCommonVariable" + i + "?tag=isElectricOn" + i));
			CommonVariable.Instance.isElectricOn = ES2.Load<bool> (this.gameObject.name + "SaveCommonVariable" + i + "?tag=isElectricOn" + i);
			/*if (CommonVariable.Instance.isElectricOn)
			CommonVariable.Instance.isElectricOn = false;
			else
			CommonVariable.Instance.isElectricOn = true;*/
			Transform player = GameObject.FindGameObjectWithTag ("Player").transform;
			electric = player.Find ("Electric").gameObject;
			StartCoroutine (waitLoad (0f));
		}
	}

	IEnumerator waitLoad (float second)
	{
		yield return new WaitForSeconds (second);
		electric.GetComponent<EventPoint> ().Active ();
	}
}
