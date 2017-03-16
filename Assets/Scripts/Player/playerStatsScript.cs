using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerStatsScript : MonoBehaviour {
	
	//The values
	private int totalKills;
	private int totalAssists;
	private int totalDeaths;
	private int shotFired;
	private int matchPlayed;
	
	//The GUI elements
	Text killNumber;
	Text assistNumber;
	Text deathNumber;
	Text shotCounter;
	Text matchCounter;
	Text KDRatio;
	
	//The transforms of the GUI elements
	Transform killC;
	Transform assistC;
	Transform deathC;
	Transform shotC;
	Transform matchC;
	Transform killDeath;

	// Use this for initialization
	void Start () {
		
		totalKills = PlayerPrefs.GetInt("totalKills");
		
		totalAssists = PlayerPrefs.GetInt("totalAssists");
		
		totalDeaths = PlayerPrefs.GetInt("totalDeaths");
		
		shotFired = PlayerPrefs.GetInt("shotFired");
		
		matchPlayed = PlayerPrefs.GetInt("matchPlayed");
		
		
		
		//We get the GUI elements and change their values
		
		//KILLS
		killC = transform.Find("killCounter");
		killNumber = killC.GetComponent<Text>();
		killNumber.text = "Kills: " + totalKills.ToString();
		
		//ASSISTS
		assistC = transform.Find("assistCounter");
		assistNumber = assistC.GetComponent<Text>();
		assistNumber.text = "Assists: " + totalAssists.ToString();
		
		//DEATHS
		deathC = transform.Find("deathCounter");
		deathNumber = deathC.GetComponent<Text>();
		deathNumber.text = "Deaths: " + totalDeaths.ToString();
		
		//KDR
		killDeath = transform.Find("KDR");
		KDRatio = killDeath.GetComponent<Text>();
		if(totalDeaths!=0)
		{
			float ratio = (float)totalKills / (float)totalDeaths;
			KDRatio.text = "KDR: " + ratio.ToString();
		}
		else
		{
			if(totalKills>0)
			{
				KDRatio.text = "KDR so high it lives \n in Jamaica";
			}
			else
				KDRatio.text = "KDR: 0";
		}
		
		//SHOTS FIRED
		shotC = transform.Find("shotCounter");
		shotCounter = shotC.GetComponent<Text>();
		shotCounter.text = "Shots fired: " + shotFired.ToString();
		
		//SHOTS FIRED
		matchC = transform.Find("matchCounter");
		matchCounter = matchC.GetComponent<Text>();
		matchCounter.text = "Matches played: " + matchPlayed.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void resetStats()
	{
		PlayerPrefs.SetInt("totalKills", 0);
		
		PlayerPrefs.SetInt("totalAssists", 0);
		
		PlayerPrefs.SetInt("totalDeaths", 0);
		
		PlayerPrefs.SetInt("shotFired", 0);
		
		PlayerPrefs.SetInt("matchPlayed", 0);
	}
}
