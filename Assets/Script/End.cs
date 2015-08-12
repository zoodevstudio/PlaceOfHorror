using UnityEngine;
using System.Collections;

public class End : MonoBehaviour
{

	public void EndGame ()
	{
		if (!this.gameObject.GetComponent<Door> ().isLock && !this.gameObject.GetComponent<Door> ().isLockNumber) {
			GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
			GameObject story = canvas.transform.FindChild ("Story").gameObject;
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			GameObject Button = canvas.transform.FindChild ("Button").gameObject;
			Button.SetActive (false);
			if (player.GetComponent<PlayerInventory2> ().ItemNormal.Contains ("Map") && player.GetComponent<PlayerInventory2> ().ItemNormal.Contains ("Compass")) {
				story.gameObject.GetComponent<BtnStory> ().callStoryPage (2);
			} else {
				story.gameObject.GetComponent<BtnStory> ().callStoryPage (1);
			}
		}
	}

}
