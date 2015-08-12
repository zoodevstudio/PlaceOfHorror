using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour
{
	public enum MouseOrMousehole
	{
		Mouse,
		MouseHole,
		MouseCaptured
	}
	public MouseOrMousehole MouseType;
	public GameObject MousePrefab;
	public float MoveSpeed;
	public float Direction;
	private bool isCallMouse;

	void Start ()
	{
		isCallMouse = false;
		if (MouseType == MouseOrMousehole.MouseHole) {
			MousePrefab.gameObject.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
			MousePrefab.gameObject.GetComponent<Mouse> ().Direction = this.Direction;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (MouseType == MouseOrMousehole.Mouse) {
			Vector3 _move = new Vector3 (MoveSpeed * Direction, 0, 0);
			this.gameObject.transform.position += _move * Time.deltaTime;
		} else 
			//<Lam>
		if (MouseType == MouseOrMousehole.MouseCaptured) {
			// Empty
		} else {
			//
			//isCallMouse = false;
			if (CommonVariable.Instance.PlayTime % 60 == 0 && !isCallMouse) {
				isCallMouse = true;
				GameObject rat = GameObject.Find ("Rat(Clone)");
				if (rat == null)
					Instantiate (MousePrefab);
			} else if (CommonVariable.Instance.PlayTime % 70 == 0) {
				isCallMouse = false;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (MouseType == MouseOrMousehole.Mouse) {
			//if (coll.tag == "Player")
			//Destroy (this.gameObject);
			if (coll.tag == "Deadend")
				Destroy (this.gameObject);
		}
	}
}
