using UnityEngine;
using System.Collections;

public class ChangePosition : MonoBehaviour
{
	private float xDefault;
	private float yDefault;
	private float zDefault;
	public float zChange;
	private bool currentZ;

	void Start ()
	{
		xDefault = this.gameObject.transform.position.x;
		yDefault = this.gameObject.transform.position.y;
		zDefault = this.gameObject.transform.position.z;
		currentZ = false;
	}

	public void Change ()
	{
		if (!currentZ) {
			currentZ = true;
			this.gameObject.transform.position = new Vector3 (xDefault, yDefault, zChange);
		} else {
			currentZ = false;
			this.gameObject.transform.position = new Vector3 (xDefault, yDefault, zDefault);
		}
	}

}
