using UnityEngine;
using System.Collections;

public class settingScript : MonoBehaviour {

	/*--- GAMERTAG ---*/
	private string playerTag;
	public string gamerTag;
	
	/*--- KEYBOARD ---*/
	private string keyMode;
	
	/*--- FOV SLIDER ---*/
	private int valueFOV;
	private float fValueFOV;
	
	

	// Use this for initialization
	void Start () {
	
		playerTag = PlayerPrefs.GetString("Gamertag");
		gamerTag = playerTag;
		
		keyMode = PlayerPrefs.GetString("KeyboardType");
		
		valueFOV = PlayerPrefs.GetInt("playerFOV");
		if(valueFOV == null)
		{
			valueFOV = 60;
		}
		
		fValueFOV = (float)valueFOV;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		
		backToMenu();
		
		
	
		gamertagSet();
		
		keyboardSet();
		
		fovSet();
		
		
		
		saveAll();
	}
	
	/*###############
	#    GAMERTAG   #
	###############*/
	void gamertagSet()
	{
		GUI.Label(new Rect(Screen.width/2-70, 50, 200, 20), "Change your Gamertag");
				
		playerTag = GUI.TextArea(new Rect(Screen.width/2-100, 75, 200, 20), playerTag, 200);
		gamerTag=playerTag;
	}

	
	/*###################
	#   KEYBOARD MODE   #
	###################*/
	void keyboardSet()
	{		
		if (GUI.Button(new Rect(Screen.width/2-210, Screen.height-100, 200, 50), "QWERTY"))
		{
			keyMode="QWERTY";
		}
				
		if (GUI.Button(new Rect(Screen.width/2+10, Screen.height-100, 200, 50), "AZERTY"))
		{
			keyMode="AZERTY";
		}
	}
	
	/*################
	#   FOV SLIDER   #
	################*/
	void fovSet() 
	{
		GUI.Label(new Rect(Screen.width/2-40, Screen.height/2+10, 200, 20), "Change the FOV");
		fValueFOV = GUI.HorizontalSlider(new Rect(Screen.width/2-100, Screen.height/2+50, 200, 20), fValueFOV, 40.0F, 100.0F);
		valueFOV = Mathf.RoundToInt(fValueFOV);
		GUI.Label(new Rect(Screen.width/2-2, Screen.height/2+25, 100, 20), valueFOV.ToString());
		
	}
	
	
	/*#################
	#   BACK BUTTON   #
	#################*/
	void backToMenu()
	{
		if (GUI.Button(new Rect(20, 20, 100, 50), "Go back and \n don't save"))
		{
			Application.LoadLevel(1);
		}
	}
	
	
	/*#################
	#   SAVE BUTTON   #
	#################*/
	void saveAll()
	{
		if (GUI.Button(new Rect(Screen.width-120, 20, 100, 50), "Save all"))
		{
			PlayerPrefs.SetString("Gamertag",gamerTag);
			PlayerPrefs.SetInt("playerFOV", valueFOV);
			PlayerPrefs.SetString("KeyboardType", keyMode);
			
			Application.LoadLevel(1);
		}
	}
}
