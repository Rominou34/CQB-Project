using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scoreBoardScript : Photon.MonoBehaviour {
	
	Text displayScore;
	public string panelTeam;
	public string playerScore;

	// Use this for initialization
	void Start () {
		//Increment
		int i=0; //For each player
		int j=0; //For each button
		
		//For each button we display the score if there's one
		/*while(i<4)
		{
			if(panelTeam=="Blue")
			{
				if(scoreScript.playerScoreBlue[i,0]!=0) //If there's one player on this button
				{
					Transform child = transform.GetChild(j);
					child.gameObject.active = true; //We activate the button
					
					//We get the child text of the button
					Transform playerText = child.transform.GetChild(0);
					displayScore = playerText.gameObject.GetComponent<Text>();
					
					//We take the score to display
					playerScore = scoreScript.playerTagsBlue[i] + " | " + scoreScript.playerScoreBlue[i,1].ToString() + " | " + scoreScript.playerScoreBlue[i,2].ToString() + " | " + scoreScript.playerScoreBlue[i,3].ToString();
					displayScore.text = playerScore;
					
					j++;
				}
			}
			else
			{
				if(scoreScript.playerScoreRed[i,0]!=0) //If there's one player on this button
				{
					Transform child = transform.GetChild(j);
					child.gameObject.active = true; //We activate the button
					
					//We get the child text of the button
					Transform playerText = child.transform.GetChild(0);
					displayScore = playerText.gameObject.GetComponent<Text>();
					
					//We take the score to display
					playerScore = scoreScript.playerTagsRed[i] + " | " + scoreScript.playerScoreRed[i,1].ToString() + " | " + scoreScript.playerScoreRed[i,2].ToString() + " | " + scoreScript.playerScoreRed[i,3].ToString();
					displayScore.text = playerScore;
					
					j++;
				}
			}
			i++;
		}*/
		
		//We deactivate the useless buttons
		for (int n=j; n<4; n++)
		{
			Transform disabledButton = transform.GetChild(n);
		
			//We deactivate the button
			disabledButton.gameObject.active = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Increment
		int i=0; //For each player
		int j=0; //For each button

		foreach(PhotonPlayer pl in PhotonNetwork.playerList)
		{
			string playerTeam = TeamExtensions.getStringTeam(pl);
			
			//If the team of the player is the one of the panel
			if(playerTeam == panelTeam)
			{
				string scoreButton;
				if(panelTeam=="Blue")
					scoreButton = "blueNo" + (j+1).ToString();
				else
					scoreButton = "redNo" + (j+1).ToString();
					
				Transform child = transform.Find(scoreButton);
				child.gameObject.active = true; //We activate the button
				
				//We get the child text of the button
				Transform playerText = child.transform.GetChild(0);
				displayScore = playerText.gameObject.GetComponent<Text>();
					
				int playScore = ScoreExtensions.GetScore(pl);
				int playDeath = DeathExtensions.GetDeath(pl);
				displayScore.text = "(" + pl.ID + ") " + pl.name + " | " + playScore + " | " + playDeath;

				//BUTTONS COLORS

				//Host color
				ColorBlock hostColor = ColorBlock.defaultColorBlock;
 				hostColor.normalColor = new Color(0, 0.6f, 0.2f, 1);
 				hostColor.highlightedColor= new Color(0, 0.7f, 0.25f, 1);
 				hostColor.pressedColor = new Color(0,0.5f, 0.15f, 1);

 				//Player color
 				ColorBlock myColor = ColorBlock.defaultColorBlock;
 				myColor.normalColor = new Color(0, 0.2f, 0.6f, 1);
 				myColor.highlightedColor = new Color(0, 0.25f, 0.7f, 1);
 				myColor.pressedColor = new Color(0, 0.15f, 0.5f, 1);

 				ColorBlock defColor = ColorBlock.defaultColorBlock;
 				defColor.normalColor = new Color(0.3f, 0.3f, 0.3f, 1);
 				defColor.highlightedColor = new Color(0.4f, 0.4f, 0.4f, 1);
 				defColor.pressedColor = new Color(0.2f, 0.2f, 0.2f, 1);

 				if(pl.isLocal)
 				{
 					child.GetComponent<Button>().colors = myColor;
 				}
 				else {
 					if(pl.isMasterClient)
 					{
 						child.GetComponent<Button>().colors = hostColor;
 					}
 					else {
 						child.GetComponent<Button>().colors = defColor;
 					}
 				}
				
				j++;
			}
			i++;
		}
		
		//We deactivate the useless buttons
		for (int n=j; n<4; n++)
		{
			Transform disabledButton = transform.GetChild(n);
		
			//We deactivate the button
			disabledButton.gameObject.active = false;
		}
		
		
		
		
		
		
		
		//For each button we display the score if there's one
		/*while(i<4)
		{
			if(panelTeam=="Blue")
			{
				if(scoreScript.playerScoreBlue[i,0]!=0) //If there's one player on this button
				{
					Transform child = transform.GetChild(j);
					child.gameObject.active = true; //We activate the button
					
					//We get the child text of the button
					Transform playerText = child.transform.GetChild(0);
					displayScore = playerText.gameObject.GetComponent<Text>();
					
					//We take the score to display
					playerScore = scoreScript.playerTagsBlue[i] + " | " + scoreScript.playerScoreBlue[i,1].ToString() + " | " + scoreScript.playerScoreBlue[i,2].ToString() + " | " + scoreScript.playerScoreBlue[i,3].ToString();
					displayScore.text = playerScore;
					
					j++;
				}
			}
			else
			{
				if(scoreScript.playerScoreRed[i,0]!=0) //If there's one player on this button
				{
					Transform child = transform.GetChild(j);
					child.gameObject.active = true; //We activate the button
					
					//We get the child text of the button
					Transform playerText = child.transform.GetChild(0);
					displayScore = playerText.gameObject.GetComponent<Text>();
					
					//We take the score to display
					playerScore = scoreScript.playerTagsRed[i] + " | " + scoreScript.playerScoreRed[i,1].ToString() + " | " + scoreScript.playerScoreRed[i,2].ToString() + " | " + scoreScript.playerScoreRed[i,3].ToString();
					displayScore.text = playerScore;
					
					j++;
				}
			}
			i++;
		}*/
		
	}
}
