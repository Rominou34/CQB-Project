using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
	private bool drawMenu; //Do we need to draw the menu
	
	//
	// Start()
	//
	void Start()
	{
		drawMenu = false;
	}

	//
	// Update()
	//
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if(drawMenu)
				drawMenu = false;
			else
				drawMenu = true;
		}
		
		
		
		/*if(drawMenu)
		{
			Transform menu = transform.Find("IGM"); //We get the transform of the panel
			menu.gameObject.active = true;
		}
		else
		{
			Transform menu = transform.Find("IGM"); //We get the transform of the panel
			menu.gameObject.active = false;
		}*/
		
		foreach (Transform child in transform)
		{
			if (child.name == "IGM")
			{
				Transform menuObj = transform.GetChild(0); //We get the transform of the panel
				menuObj.gameObject.active = drawMenu;
			}
		}
	}

	//To save the stats before leaving
	public void saveStats()
	{
		//We get our player
		PhotonView myView = PhotonView.Get(transform.parent.gameObject);
		PhotonPlayer myPlayer = myView.owner;
		
		//We get ou score
		int myKills = ScoreExtensions.GetScore(myPlayer);
		int myDeaths = DeathExtensions.GetDeath(myPlayer);
		
		Debug.Log("Got stats= K: " + myKills + " D: " + myDeaths);

		//We get the old values
		int totalKills = PlayerPrefs.GetInt("totalKills");
		int totalDeaths = PlayerPrefs.GetInt("totalDeaths");

		//We add the score
		totalKills+=myKills;
		totalDeaths+=myDeaths;

		//We set the new values
		PlayerPrefs.SetInt("totalKills", totalKills);
		PlayerPrefs.SetInt("totalDeaths", totalDeaths);
	}

	//To close the menu
	public void closeMenu()
	{
		drawMenu=false;
	}

	public void quitToMenu()
	{
		saveStats();

		//We destroy the network manager
		GameObject multiS = GameObject.Find("multiScripts");
		Destroy(multiS);

		PhotonNetwork.Disconnect();
		Application.LoadLevel(1);
	}

	public void quitToDesktop()
	{
		saveStats();

		//We destroy the network manager
		GameObject multiS = GameObject.Find("multiScripts");
		Destroy(multiS);
		
		PhotonNetwork.Disconnect();
		Application.Quit();
	}
}