using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	public Toggle tgMusic;
	public Toggle tgAudio;
	public Slider sldVol;
	//trwowngf hop tam. bo thoi
	public AudioSource MusicMenu;
	public AudioSource AudioRain;

	// Use this for initialization
	void Start () {
		if (ES2.Exists ("settings")) {
			bool music = ES2.Load<bool> ("settings?tag=music");
			bool audio = ES2.Load<bool> ("settings?tag=audio");
			float vol = ES2.Load<float> ("settings?tag=vol");

			tgMusic.isOn = !music; //neu = false thif ep' thanh true de hien dau tich, vi mute = false thi moi play nhac
			tgAudio.isOn = !audio;
			sldVol.value = vol;

		}
	}

	public void SaveMusic(Toggle tg){ 
		ES2.Save (!tg.isOn, "settings?tag=music"); 
		MusicMenu.mute = !tg.isOn;
	}
	public void SaveAudio(Toggle tg){ 
		ES2.Save (!tg.isOn, "settings?tag=audio");
		if(AudioRain!=null){
			AudioRain.mute = !tg.isOn;
		}
	}
	public void ChangeVolume(Slider sldr){
		sldVol.value = sldr.value;
		ES2.Save (sldr.value, "settings?tag=vol"); 
		MusicMenu.volume = sldr.value;
	}
	
}
