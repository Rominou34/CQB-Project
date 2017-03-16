using UnityEngine;
using System.Collections;

public class playerSettings : MonoBehaviour {

	//VARIABLES FOR THE GAMERTAG
	static string gamerTag;
	public string playerTag="";
	
	//VARIABLES FOR THE KEYBOARD TYPE
	static string keyboardMode;
	public string keyMode="";
	
	public Texture menuBg;
	
	private int totalKills;
	private int totalAssists;
	private int totalDeaths;
	private int shotFired;
	private int matchPlayed;


	// Use this for initialization
	void Start () {
		int randPlayNum = Random.Range(0, 10000);
		gamerTag="Guest" + randPlayNum.ToString();
		keyboardMode="";
		
		//When the player first launches the game we set all his stats to 0
		//KILLS
		totalKills = PlayerPrefs.GetInt("totalKills");
		if(totalKills==null)
			PlayerPrefs.SetInt("totalKills",0);
		
		
		//ASSISTS
		totalAssists = PlayerPrefs.GetInt("totalAssists");
		if(totalAssists==null)
			PlayerPrefs.SetInt("totalAssists",0);
		
		
		//DEATHS
		totalDeaths = PlayerPrefs.GetInt("totalDeaths");
		if(totalDeaths==null)
			PlayerPrefs.SetInt("totalDeaths",0);
		
		
		//SHOTS FIRED
		shotFired = PlayerPrefs.GetInt("shotFired");
		if(shotFired==null)
			PlayerPrefs.SetInt("shotFired", 0);
		
		
		//MATCHES PLAYED
		matchPlayed = PlayerPrefs.GetInt("matchPlayed");
		if(matchPlayed==null)
			PlayerPrefs.SetInt("matchPlayed",0);
	}
	
	// Update is called once per frame
	void Update () {
		keyboardMode=keyMode;
	
		if(PlayerPrefs.GetString("Gamertag") == null || PlayerPrefs.GetString("KeyboardType") == null)
		{
			Application.LoadLevel(2);
		}
		/*---------------------------------- ELSE --------------------------------*/
		else
		{
			Application.LoadLevel (1);
		}
	}
}