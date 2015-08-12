using UnityEngine;
using System.Collections;

public class Exitgame : MonoBehaviour {
	//public GameObject[] PanelObj;
	//public Animator[] AniObj;
	//public PanelManager pnl;
	
	public GameObject MainmenuChild;
	
	// Use this for initialization
	void Start () {
		
	}
	public void SetActive(){
		if (!MainmenuChild.activeSelf)
			MainmenuChild.SetActive (true);
		else
			MainmenuChild.SetActive (false);
	}
	
	public void Exit(){
		Application.Quit ();
	}
	
	// Update is called once per frame
	void Update () {
		//test thu thoi. toi uu sau :))
		/*if (Input.GetKeyDown (KeyCode.Escape) && AniObj[0].gameObject.activeSelf == true ){ 
			pnl.OpenPanel(AniObj[3]);
		}
		if (Input.GetKeyDown (KeyCode.Escape) && AniObj[1].gameObject.activeSelf == true ){ 
			pnl.OpenPanel(AniObj[0]);

		}
		if (Input.GetKeyDown (KeyCode.Escape) && AniObj[2].gameObject.activeSelf == true ){ 
			pnl.OpenPanel(AniObj[0]);
		}
		if (Input.GetKeyDown (KeyCode.Escape) && AniObj[3].gameObject.activeSelf == true ){ 
			pnl.OpenPanel(AniObj[0]);
		}
		if (Input.GetKeyDown (KeyCode.Escape) && AniObj[4].gameObject.activeSelf == true ){ 	
			pnl.OpenPanel(AniObj[0]);
		}*/
	}
}
