using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerListPanelScript : Photon.MonoBehaviour {
	
	Text playerName;
	private int playNum; //Incrementation

	// Use this for initialization
	void Start () {
		
		playNum=0;
		
		//For each button we get the player in the player list
		foreach(PhotonPlayer pl in PhotonNetwork.playerList)
		{
			//We get the button
			Transform playerButton = transform.GetChild(playNum);
			
			//We activate the button
			playerButton.gameObject.active = true;
			
			//We get the child text of the button
			Transform playerText = playerButton.transform.GetChild(0);
			
			//We change the text to the player gamertag
			playerName = playerText.gameObject.GetComponent<Text>();
			playerName.text = pl.name;
			
			//We increment
			playNum++;
		}
		
		//We deactivate the other buttons
		for(int i=playNum; i<8; i++)
		{
			Transform disabledButton = transform.GetChild(i);
		
			//We deactivate the button
			disabledButton.gameObject.active = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		playNum=0;
		
		//For each button we get the player in the player list
		foreach(PhotonPlayer pl in PhotonNetwork.playerList)
		{
			//We get the button
			Transform playerButton = transform.GetChild(playNum);
			
			//We activate the button
			playerButton.gameObject.active = true;
			
			//We get the child text of the button
			Transform playerText = playerButton.transform.GetChild(0);
			
			//We change the text to the player gamertag
			playerName = playerText.gameObject.GetComponent<Text>();
			playerName.text = pl.name;
			
			//We increment
			playNum++;
		}
		
		//We deactivate the other buttons
		for(int i=playNum; i<8; i++)
		{
			Transform disabledButton = transform.GetChild(i);
		
			//We deactivate the button
			disabledButton.gameObject.active = false;
		}
	}
}
