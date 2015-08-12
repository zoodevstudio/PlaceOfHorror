using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemPriority : MonoBehaviour
{

	[System.Serializable]
	public class HitObjects
	{
//		public HitObjects (Collider2D CollObject)
//		{
//			_Collider2D = CollObject;
//		}
		public HitObjects (Collision2D CollObject)
		{
			_Collision2D = CollObject;
		}

		public HitObjects (int value, Collider2D CollObject)
		{
			_Priority = value;
			_Collider2D = CollObject;
		}
		
		public int _Priority;
		public Collider2D _Collider2D;
		public Collision2D _Collision2D;
	}
	public List<HitObjects>  HitObjectsList = new List<HitObjects> ();

	// Use this for initialization
	void Start ()
	{
		NotificationManager.Instance.AddListener (this, "OnAction");
	}

	void OnAction ()
	{

	}
	// Update is called once per frame
	void Update ()
	{
		if (!this.gameObject.GetComponent<PlayerController> ().isAction) {
			SortList ();
		}
	}

	void RemoveDuplicate ()
	{
	}

	void SortList ()
	{
		HitObjectsList = HitObjectsList.OrderBy (x => x._Priority).ToList ();
//		foreach (HitObjects t in HitObjectsList)
//			Debug.Log (t._Priority);
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.name == "MouseTrap") {
			HitObjectsList.Add (new HitObjects (1, coll));
		} 
		if (coll.gameObject.name == "Trap") {
			HitObjectsList.Add (new HitObjects (2, coll));
		} 
		
		if (coll.gameObject.tag == "Desk") {
			HitObjectsList.Add (new HitObjects (3, coll));
		} 
		if (coll.gameObject.tag == "ReadableItem") {
			if (this.GetComponent<PlayerInteractive> ().isVisible
			    ||this.GetComponent<PlayerController> ().LampTurnedOn
			    && this.GetComponent<PlayerController> ().lampObject.transform.FindChild ("Area light Player").gameObject.GetComponent<Light> ().enabled
			    && this.GetComponent<PlayerController> ().lampObject.transform.FindChild ("Point light").gameObject.GetComponent<Light> ().enabled
			    ) {
				HitObjectsList.Add (new HitObjects (4, coll));
			}
		}
		if (coll.gameObject.tag == "Cabinet") {
			HitObjectsList.Add (new HitObjects (5, coll));
		} 
		if (coll.gameObject.tag == "Bed") {
			HitObjectsList.Add (new HitObjects (6, coll));
		}  
		if (coll.gameObject.tag == "Door") {
			HitObjectsList.Add (new HitObjects (7, coll));
		}
	}


	void OnTriggerExit2D (Collider2D coll)
	{
		if (!this.gameObject.GetComponent<PlayerController> ().isAction) {
			if (coll.gameObject.name == "MouseTrap") {
				RemoveAtList (1);
			}
			if (coll.gameObject.name == "Trap") {
				RemoveAtList (2);
			} 
			if (coll.gameObject.tag == "Desk") {
				RemoveAtList (3);
			} 
			if (coll.gameObject.tag == "ReadableItem") {
				RemoveAtList (4);
			} 
			if (coll.gameObject.tag == "Cabinet") {
				RemoveAtList (5);
			} 
			if (coll.gameObject.tag == "Bed") {
				RemoveAtList (6);
			}  
			if (coll.gameObject.tag == "Door") {
				for (int i=HitObjectsList.Count - 1; i > -1; i--) {
					if (HitObjectsList [i]._Priority == 7 && HitObjectsList [i]._Collider2D == coll) {
						HitObjectsList.RemoveAt (i);
					}
				}
			}
		} else {
			if (this.GetComponent<PlayerInteractive> ().CurrentObject != null)
			if (this.GetComponent<PlayerInteractive> ().CurrentObject.gameObject.tag != coll.gameObject.tag) {
				if (coll.gameObject.name == "MouseTrap") {
					RemoveAtList (1);
				}
				if (coll.gameObject.name == "TrapPlayer") {
					RemoveAtList (2);
				} 
				if (coll.gameObject.tag == "Desk") {
					RemoveAtList (3);
				} 
				if (coll.gameObject.tag == "ReadableItem" && this.GetComponent<PlayerInteractive> ().CurrentObject.activeSelf) {
					RemoveAtList (4);
				}
				if (coll.gameObject.tag == "Cabinet") {
					RemoveAtList (5);
				} 
				if (coll.gameObject.tag == "Bed") {
					RemoveAtList (6);
				}  
				if (coll.gameObject.tag == "Door") {
					for (int i=HitObjectsList.Count - 1; i > -1; i--) {
						if (HitObjectsList [i]._Priority == 7 && HitObjectsList [i]._Collider2D == coll) {
							HitObjectsList.RemoveAt (i);
						}
					}
				}
			}
		}
	}
	
	public void RemoveAtList (int _index)
	{
		for (int i=HitObjectsList.Count - 1; i > -1; i--) {
			if (HitObjectsList [i]._Priority == _index) {
				HitObjectsList.RemoveAt (i);
			}
		}
	}
}
