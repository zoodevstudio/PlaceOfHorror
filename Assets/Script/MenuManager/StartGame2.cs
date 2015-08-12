using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StartGame2 : MonoBehaviour {



	// Use this for initialization
	void Start () { 
		//khởi tạo dữ liệu item
		if (!ES2.Exists (CommonVariable.Instance.name_save+"0")) {
			CreateData ();
			print ("tạo dữ liệu thành công");
		} else
			print ("Đã tạo dữ liệu lần trước rồi");
	}

	void CreateData(){
		List<string> ItemNormal = new List<string> (); 
		List<string> ItemKey = new List<string> (); 
		//List<string> ItemManufacture = new List<string> ();  //close manufac
		List<string> ItemNecessity = new List<string> (); 

		////List<string> HuyetKiem = new List<string> (); //close manufac
		//List<string> GiapGai = new List<string> (); //close manufac
		//List<string> GiapMau = new List<string> (); //close manufac

		 
		//ItemNormal.Add ("DemoNormal"); //ItemNormal.Add ("kiembf"); ItemNormal.Add ("giaplua"); ItemNormal.Add ("daikhonglo");
		//ItemKey.Add ("DemoKeyItem"); //ItemKey.Add ("KeyZungX"); //ItemKey.Add ("dao_off"); ItemKey.Add ("sung_on"); 
		//ItemManufacture.Add ("DemoManufacture_lock"); //ItemManufacture.Add ("giapgai_lock"); ItemManufacture.Add ("giapmau_lock"); 
		//ItemNecessity.Add ("DemoNecessItem_2"); //ItemNecessity.Add ("banggac_5"); ItemNecessity.Add ("nuoc_3");

	//	HuyetKiem.Add ("huyettruong"); HuyetKiem.Add ("kiembf");
	//	GiapGai.Add ("giapluoi"); GiapGai.Add ("giaplua");
		//GiapMau.Add ("daikhonglo"); GiapMau.Add ("itemkhac");

		ES2.Save(CommonVariable.Instance.name_save+"0", CommonVariable.Instance.name_save+"0?tag=0");
		ES2.Save(ItemNormal, "ItemNormal.txt?tag=0");
		ES2.Save(ItemKey, "ItemKey.txt?tag=0");
	//	ES2.Save(ItemManufacture, "ItemManufacture.txt?tag=0"); 
		ES2.Save(ItemNecessity, "ItemNecessity.txt?tag=0");

		ES2.Save (false, "settings?tag=audio");
		ES2.Save (false, "settings?tag=music");
		float vol = 1.0f;
		ES2.Save (vol, "settings?tag=vol"); 

		//ES2.Save(HuyetKiem, "DemoManufacture.txt"); //cái này để so sánh
		//ES2.Save(GiapGai, "giapgai.txt");
		//ES2.Save(GiapMau, "giapmau.txt");

	}

}
