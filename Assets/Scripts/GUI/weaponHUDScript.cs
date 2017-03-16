using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class weaponHUDScript : MonoBehaviour {

    private Transform titleObj;
    private Transform infoObj;
    private Transform statusObj;

    private Text titleText;
    private Text infoText;
    private Slider statSlider;

    private int actualMun=0;
    private int maxMun=0;
    private bool reload;
    private float reloadStat;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    //Getting the player transform
        //shotScript shotS=null;
        foreach (Transform child in transform.root)
        {
            if (child.name != "globalHUD" && child.name != "scoreBoardUI" && child.name != "InGameMenu" && child.name != "littleScoreBoardUI" && child.name != "PlayerHUD")
            {
                shotScript shotS = child.gameObject.GetComponent<shotScript>();
                reload=shotS.isReloading;
                actualMun = shotS.actualAmmo;
                maxMun = shotS.mun_max;
                reloadStat = shotS.getReloadStatus();
            }
        }

        titleObj = transform.Find("WeaponTitle");
        infoObj = transform.Find("WeaponInfo");
        statusObj = transform.Find("ReloadStatus");

        titleText = titleObj.GetComponent<Text>();
        infoText = infoObj.GetComponent<Text>();
        statSlider = statusObj.GetComponent<Slider>();

        titleText.text = globalWeaponStats.weaponNames[0];

        //If we're not reloading
        if(!reload)
        {
            infoText.text = actualMun.ToString() + " | " + maxMun.ToString();
            statusObj.gameObject.active=false;
        }
        else
        {
            infoText.text = "RELOADING";

            statusObj.gameObject.active=true;
            statSlider.minValue = 0f;
            statSlider.maxValue = 100f;
            statSlider.value = reloadStat;
        }

	}
}
