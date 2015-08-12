using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventManager : MonoBehaviour
{
	void Awake ()
	{
		CheckSingletonInstance ();
	}
	
	void CheckSingletonInstance ()
	{
		if (instance != null && instance.GetInstanceID () != GetInstanceID ()) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}
	
	private static EventManager instance = null;
	
	public static EventManager Instance {
		get {
			if (instance == null)
				instance = new GameObject ("EventManager").AddComponent<EventManager> ();
			return instance;
		}
	}

	private Dictionary<string, List<Component>> m_listeners = new Dictionary<string, List<Component>> ();

	public void AddEvent (Component sender, string notificationType)
	{
		//Add listener to dictionary
		if (!m_listeners.ContainsKey (notificationType))
			m_listeners.Add (notificationType, new List<Component> ());
		m_listeners [notificationType].Add (sender);
	}

	public void RemoveEvent (Component sender, string notificationType)
	{
		if (!m_listeners.ContainsKey (notificationType))
			return;
		List<Component> listeners = m_listeners [notificationType];
		for (int i = listeners.Count - 1; i >= 0; i--) {
			if (listeners [i].GetInstanceID () == sender.GetInstanceID ())
				listeners.RemoveAt (i);
		}
		//m_listeners [notificationType].Remove (sender);
	}

	public void PostEvent (Component sender, string notificationType)
	{
		if (!m_listeners.ContainsKey (notificationType)) {
			return;
		}
		foreach (Component listener in m_listeners[notificationType]) {
			listener.SendMessage ("Receive", sender, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void RemoveRedundancies ()
	{
		Dictionary<string,List<Component>> tmpListeners = new Dictionary<string, List<Component>> ();
		foreach (KeyValuePair<string, List<Component>> listeners in m_listeners) {
			for (int i = listeners.Value.Count - 1; i >= 0; i --) {
				if (listeners.Value [i] == null)
					listeners.Value.RemoveAt (i);
			}
			if (listeners.Value.Count > 0)
				tmpListeners.Add (listeners.Key, listeners.Value);
		}
		m_listeners = tmpListeners;
	}
	
}


