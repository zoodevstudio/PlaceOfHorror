using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Story : MonoBehaviour
{
	[System.Serializable]
	public class Page
	{
		public List<Sprite> storypage;
		public List<string> storysub;
		public List<string> storysubENG;
	}
	public List<Page> storypageList = new List<Page> ();

}
