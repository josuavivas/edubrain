﻿using UnityEngine;
using System.Collections;

public class AdmobVNTIS : MonoBehaviour {
	public string BannerAdUnitID = "YOUR_AD_UNIT_ID";
	public string TestDeviceID = "";
	public AdSize AdvertisementSize = AdSize.BANNER;
	public bool ShowOnLoad = true;
	public Rules[] AdvertisementRules;
	AndroidJavaObject jo;

	public enum AdSize{
		BANNER = 1,
		FULL_BANNER = 2,
		LEADERBOARD = 3,
		MEDIUM_RECTANGLE = 6,
		SMART_BANNER = 5,
		WIDE_SKYSCRAPER = 6
	};

	public enum Rules
	{
		ABOVE = 2,
		ALIGN_BASELINE = 4,
		ALIGN_BOTTOM = 8,
		ALIGN_END = 19,
		ALIGN_LEFT = 5,
		ALIGN_PARENT_BOTTOM = 12,
		ALIGN_PARENT_END = 21,
		ALIGN_PARENT_LEFT = 7,
		ALIGN_PARENT_RIGHT = 7,
		ALIGN_PARENT_START = 20,
		ALIGN_PARENT_TOP = 10,
		ALIGN_RIGHT = 7,
		ALIGN_START = 18, 	 
		ALIGN_TOP = 6,
		BELOW = 3,
		CENTER_HORIZONTAL = 14,
		CENTER_IN_PARENT = 13, 	
		CENTER_VERTICAL = 15,
		END_OF = 17,
		LEFT_OF = 0,
		RIGHT_OF = 1,
		START_OF = 16
	};

	// Dont destroy on load and prevent duplicate
	private static bool created = false;
	void Awake() {
		if (!created) {
			DontDestroyOnLoad(this.gameObject);
			created = true;

			int[] rule = new int[AdvertisementRules.Length + 1];
			
			rule[0] = (int)AdvertisementSize;
			
			if (rule.Length > 1) {
				for (int i = 0; i < AdvertisementRules.Length; i++) {
					rule [i+1] = (int)AdvertisementRules [i];
				}
			}
			
			jo = new AndroidJavaObject ("admob.admob",BannerAdUnitID,TestDeviceID,ShowOnLoad,rule);
		} else {
			Destroy(this.gameObject);
		}
	}

	public void showBanner(){
		if (jo != null)
			jo.Call ("showBanner");
		else
			Debug.LogError ("Null");
	}

	public void hideBanner(){
		if (jo != null)
			jo.Call ("removeBanner");
		else
			Debug.LogError ("Null");
	}

	public void setMargins(float x, float y){
		int left = (int)(x * Screen.width);
		int top = (int)(y * Screen.height);

		jo.Call ("setMargins", left, top);
	}

	public void setRules(Rules[] nrule, int size){
		int[] rule = new int[size];
		for (int i = 0; i < size; i++) {
			rule [i] = (int)nrule [i];
		}
		jo.Call ("setRules", rule);
	}
}
