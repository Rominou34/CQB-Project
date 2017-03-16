using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class littleScoreboardScript : MonoBehaviour {
	
	Text blueScore;
	Text redScore;
	
	Text playerTag;
	Text playerScore;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		scoreScript scoreS = transform.root.GetComponent<scoreScript>();
		
		//We change the team scores
		Transform bluePanel = transform.Find("BluePanel");
		Transform redPanel = transform.Find("RedPanel");
		
		Transform blueText = bluePanel.GetChild(0);
		Transform redText = redPanel.GetChild(0);
		
		blueScore = blueText.GetComponent<Text>();
		redScore = redText.GetComponent<Text>();
		
		int bScore = scoreScript.blueScore;
		int rScore = scoreScript.redScore;
		
		blueScore.text = "BLUE\n\n" + bScore.ToString();
		redScore.text = "RED\n\n" + rScore.ToString();
		
		
		//Player score
		Transform playerP = transform.Find("PlayerPanel");
		
		//Changing the gamertag
		Transform playerT = playerP.transform.Find("PlayerTag");
		playerTag = playerT.GetComponent<Text>();
		playerTag.text = PlayerPrefs.GetString("Gamertag");
		
		//Changing the score
		Transform playerS = playerP.transform.Find("PlayerScore");
		playerScore = playerS.GetComponent<Text>();
		
		int pKills = scoreS.myKills;
		int pAssists = scoreS.myAssists;
		int pDeaths = scoreS.myDeaths;
		
		playerScore.text = pKills.ToString() + "            " + pAssists.ToString() + "            " + pDeaths.ToString();
	}
}
