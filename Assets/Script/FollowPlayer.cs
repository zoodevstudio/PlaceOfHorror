using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
	public GameObject player;
	public Vector3 TargetPos;

	/* Start is: 
		X:1.05 
		Y:2.53
	*/
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3 (player.transform.position.x + TargetPos.x, player.transform.position.y + TargetPos.y, this.transform.position.z);
	}
}
