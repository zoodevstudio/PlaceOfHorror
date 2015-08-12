using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NotificationManager : MonoBehaviour
{
		//-----------------------------------------------
		//-------singleton
		void Awake ()
		{
				CheckSingletonInstance ();
		}
		
		void CheckSingletonInstance ()
		{
				// check duplicate instance
				if (instance != null && instance.GetInstanceID () != GetInstanceID ()) {
						Destroy (gameObject);//delete duplicate
				} else {
						instance = this;
						//DontDestroyOnLoad(gameObject);// optiocal
				}
		}
		
		private static NotificationManager instance = null;

		public static NotificationManager Instance {
				get {
						if (instance == null)
								instance = new GameObject ("NotificationManager").AddComponent<NotificationManager> ();
						return instance;
				}
		}
		
		
		//------------------------------------------------
		//Internal reference to all listeners for notifications
		private Dictionary<string, List<Component>> m_listeners = new Dictionary<string, List<Component>> ();
		
		
		//Methods
		//------------------------------------------------
		//Function to add a listener for an notification to the listeners list
		public void AddListener (Component sender, string notificationType)
		{
				//Add listener to dictionary
				if (!m_listeners.ContainsKey (notificationType))
						m_listeners.Add (notificationType, new List<Component> ());
			
				//Add object to listener list for this notification
				m_listeners [notificationType].Add (sender);
		}
		//------------------------------------------------
		//Function to remove a listener for a notification
		public void RemoveListener (Component sender, string notificationType)
		{
				//If no key in dictionary exists, then exit
				if (!m_listeners.ContainsKey (notificationType))
						return;
				//Cycle through listeners and identify component, and then remove
				List<Component> listeners = m_listeners [notificationType];
				for (int i = listeners.Count - 1; i >= 0; i--) {
						if (listeners [i].GetInstanceID () == sender.GetInstanceID ())
								listeners.RemoveAt (i);
				}
		}
		//------------------------------------------------
		//Function to post a notification to a listener
		public void PostNotification (Component sender, string notificationType)
		{
				//If no key in dictionary exists, then exit
				if (!m_listeners.ContainsKey (notificationType)) {
						//Debug.Log("Doesn't exit");
						return;
				}
				//Else post notification to all matching listeners
				//IListener scriptPointer;
				foreach (Component listener in m_listeners[notificationType]) {
						//Listener.SendMessage(notificationType, Sender, SendMessageOptions.DontRequireReceiver);
						//scriptPointer = listener.GetComponent<IListener>();
						//scriptPointer = listener as IListener;
						//if (scriptPointer != null) 	scriptPointer.OnNotificationReceived(sender, notificationType);
						listener.SendMessage (notificationType, sender, SendMessageOptions.DontRequireReceiver);
				}
		}
		
		//------------------------------------------------
		//Function to remove redundant listeners - deleted and removed listeners
		public void RemoveRedundancies ()
		{
				//Create new dictionary
				Dictionary<string,List<Component>> tmpListeners = new Dictionary<string, List<Component>> ();
				//Cycle through all dictionary entries
				foreach (KeyValuePair<string, List<Component>> listeners in m_listeners) {
						//Cycle through all listener objects in list, remove null objects
						for (int i = listeners.Value.Count - 1; i >= 0; i --) {
								if (listeners.Value [i] == null)
										listeners.Value.RemoveAt (i);
						}
						//If items remain in list for this notification, then add this to tmp dictionary
						if (listeners.Value.Count > 0)
								tmpListeners.Add (listeners.Key, listeners.Value);
				}
				//Replace listeners object with new, optimized dictionary
				m_listeners = tmpListeners;
		}
		
}
	

