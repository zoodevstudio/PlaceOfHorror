using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public AudioSource music_bg;
	public AudioSource music_rain;

	public AudioSource au_footstep;
	public AudioSource au_running;
	public AudioSource au_door_locking;
	public AudioSource au_door_open;
	public AudioSource au_cabinet_open;
	public AudioSource au_scream;
	public AudioSource au_roar;
	public AudioSource au_laugh;
	public AudioSource au_glass_broken;
	public AudioSource au_switch_on_off;
	public AudioSource au_hard_drive_shut_down;
	public bool _start;
	private GameObject player;

	// Use this for initialization
	void Start () {
		NotificationManager.Instance.AddListener (this, "Load");
		_start =false;
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(_start){
			StartCoroutine(RandomLaugh());
		}
	}

	IEnumerator RandomLaugh(){
		_start = false;
		int _number = Random.Range(20,60);
		au_laugh.Play();
		yield return new WaitForSeconds(_number);
		_start = true;
	}

	public void Load(){
		if (ES2.Exists ("settings")) {
			bool music = ES2.Load<bool> ("settings?tag=music");
			bool audio = ES2.Load<bool> ("settings?tag=audio");
			music_bg.mute = music;
			music_rain.mute = music;

			au_footstep.mute = audio;
			au_running.mute = audio;
			au_door_locking.mute = audio;
			au_door_open.mute = audio;
			au_cabinet_open.mute = audio;
			au_scream.mute = audio;
			au_roar.mute = audio;
			au_laugh.mute = audio;
			au_glass_broken.mute = audio;

			music_bg.volume = ES2.Load<float>("settings?tag=vol");

			print("music: "+music+"_"+audio);
		}
	}
}
