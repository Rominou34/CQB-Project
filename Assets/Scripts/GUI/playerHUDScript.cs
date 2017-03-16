using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerHUDScript : MonoBehaviour {

	private int actualHealth;

	Transform healthTitle;
	Transform healthSlider;
	
	Text healthInfo;

	Slider healthBar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		actualHealth=0;
		//We get the soldier transform
		foreach (Transform child in transform.root)
		{
			if (child.name != "globalHUD" && child.name!="scoreBoardUI" && child.name != "InGameMenu" && child.name!="littleScoreBoardUI" && child.name!="PlayerHUD")
			{
				playerNetwork playSet = child.gameObject.GetComponent<playerNetwork>();
				actualHealth = playSet.health;
			}
		}

		//We update its value on the title
		healthTitle = transform.Find("HealthTitle");
		healthInfo = healthTitle.gameObject.GetComponent<Text>();
		healthInfo.text = actualHealth.ToString();

		//We update its value on the slider
		healthSlider = transform.Find("Slider");
		healthBar = healthSlider.GetComponent<Slider>();
		healthBar.value = (float)actualHealth;
	}
}
