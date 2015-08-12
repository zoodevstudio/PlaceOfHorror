using UnityEngine;
using System.Collections;

public class FadeScene : MonoBehaviour
{
	Animator anim;
	private static FadeScene instance;
	public  enum SceneName
	{
		none,
		Menu,
		Scene
	}

	private enum TransitionEvent
	{
		None = 0,
		onCloseTransition = 1,
		onLoadScene = 2,
		onOpenTransition = 3
	}
	private TransitionEvent transitionState = TransitionEvent.None;
	private bool onTransition = false;
	private SceneName m_CurrentLevel = SceneName.none;
	private SceneName m_NextLevel = SceneName.none;

	public static FadeScene Instance {
		get {
			if (instance == null) {
				instance = new GameObject ("FadeScene").AddComponent<FadeScene> ();

			}
			return instance;
		}
	}
	
	public void OnApplicationQuit ()
	{
		instance = null;
	}
	
	void Start ()
	{
		DontDestroyOnLoad (CommonVariable.Instance); 
	}

	public void TransitionToScene (SceneName scenename)
	{
		m_NextLevel = scenename;
		if (transitionState == TransitionEvent.None) {
			transitionState = TransitionEvent.onCloseTransition;

		}
		CloseScene ();
		
		// LoadScene() in animation
		//OpenScene() in update
		
	}
	//-------Close scene-----
	private void CloseScene ()
	{
		if (transitionState == TransitionEvent.onCloseTransition) { 
			if (GameObject.Find ("Player") == null){ //menu thi moi chayj
				GameObject CloseScene = Instantiate (Resources.Load ("CloseFade_Menu")) as GameObject;
				CloseScene.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
				CloseScene.transform.localPosition = new Vector3(0,0,0);
				CloseScene.transform.localScale = Vector3.one;
			}else {
				//chay anim khac'
				GameObject CloseScene = Instantiate (Resources.Load ("CloseFade_Player")) as GameObject;
				CloseScene.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
				CloseScene.transform.localPosition = new Vector3(0,0,0);
				CloseScene.transform.localScale = Vector3.one;
			}
			transitionState = TransitionEvent.onLoadScene;					
		}
	}
	
	//------Load scene----
	public void LoadScene ()
	{
		if (transitionState == TransitionEvent.onLoadScene) {
			//Load scene ra. 
			Application.LoadLevel (m_NextLevel.ToString ());
		
			transitionState = TransitionEvent.onOpenTransition;
		}
	}
	
	//------Open scene----
	public void OpenScene ()
	{

		if (transitionState == TransitionEvent.onOpenTransition) {
			if (GameObject.Find ("Player") != null){ //chay ben scene Player
				CommonVariable.Instance.banner.Hide ();
				GameObject OpenScene = Instantiate (Resources.Load ("OpenFade_Player")) as GameObject;
				OpenScene.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
				OpenScene.transform.localPosition = new Vector3(0,0,0);
				OpenScene.transform.localScale = Vector3.one;
			} else {
				CommonVariable.Instance.banner.Show();
				GameObject OpenScene = Instantiate (Resources.Load ("OpenFade_Menu")) as GameObject;
				OpenScene.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
				OpenScene.transform.localPosition = new Vector3(0,0,0);
				OpenScene.transform.localScale = Vector3.one;
			}			
			transitionState = TransitionEvent.None;
			NotificationManager.Instance.PostNotification (this, "Load");
		}
	}
	
	void Update ()
	{	
		if (Application.loadedLevelName == m_NextLevel.ToString () && m_CurrentLevel != m_NextLevel) {
			//GameManager.resetScore();
			OpenScene ();
			m_CurrentLevel = m_NextLevel;
		}
	}
}
