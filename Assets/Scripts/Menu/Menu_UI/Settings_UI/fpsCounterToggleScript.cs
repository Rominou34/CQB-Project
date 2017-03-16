using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fpsCounterToggleScript : MonoBehaviour {
	
	Toggle showFPS; //The toggle
	private int showIt; //The choice of the player, 1=true, 0=false

	// Use this for initialization
	void Start () {
		showIt = PlayerPrefs.GetInt("showFPS");
		
		if (showIt == null) //If there is a setting for this
		{
			showIt = 0;
			PlayerPrefs.SetInt("showFPS",showIt);
		}
		else
		{
			if (showIt == 1)
			{
				showFPS = GetComponent<Toggle>();
				showFPS.isOn = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		showFPS = GetComponent<Toggle>();
		
		if(showFPS.isOn)
		{
			showIt=1;
		}
		else
		{
			showIt=0;
		}
		
		PlayerPrefs.SetInt("showFPS",showIt);
	}
}
