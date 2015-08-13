using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GoogleMobileAds.Api;
//using UnityEditor;

public class CommonVariable : MonoBehaviour
{
	// Thời gian chơi
	public int PlayTime = 0;
	public float TempPlayTime = 0;
	
	void Update ()
	{
		if (!isPause) {
			TempPlayTime += Time.deltaTime * 1;
			PlayTime = (int)Mathf.Round (TempPlayTime);
		}
	}

	public enum Language
	{ 
		Vietnamese,
		English
	}
	public Language GameLanguage = Language.Vietnamese;
	public string currentEventName;
	public bool isElectricOn = true;

	public bool isPause { get; set; }

	private static CommonVariable instance = null;

	public static CommonVariable Instance {
		get {
			if (instance == null) {
				instance = new GameObject ("CommonVariable").AddComponent<CommonVariable> ();
			}
			return instance;
		}
	}

	void Start ()
	{
		Ads ();
	}

	public BannerView banner;

	public void Ads ()
	{
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-8112894826901791/9216328060";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-8112894826901791/6194189263";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		banner = new BannerView (adUnitId, AdSize.Banner, AdPosition.Bottom);
		AdRequest request = new AdRequest.Builder ().Build ();
		banner.LoadAd (request); 
		print ("load ads");
	}

	public string loadi = "0";
	public string name_save = "GameSave";

	public string btn_Move { get; set; }

	public string btn_Jump { get; set; }

	public string btn_Action { get; set; }
	
	public string btn_Lamp { get; set; }

	public string btn_ChatBox { get; set; }

	public List<string> ChatBoxString { get; set; }
}
