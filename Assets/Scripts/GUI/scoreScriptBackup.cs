using UnityEngine;
using System.Collections;

public class scoreScriptBackup : Photon.MonoBehaviour {


	/*###########
	#   SCORE   #
	###########*/
	public string myTeam; //Team of the player
	
	public int blueCount=0; //Number of blue players
	public int redCount=0; //Number of red players
	
	//Score tabs of each team. Contains 0:viewID of Player, 1: Kills, 2: Assists: 3:Deaths;
	//public static int[,] playerScoreBlue = new int[4,4]; //Original 2D tabs
	//public static int[,] playerScoreRed = new int[4,4];
	
	public static int[] playerScoreBlue = new int[16]; //New 1D tabs
	public static int[] playerScoreRed = new int[16];
	
	//Tabs containing all the players Gamertags
	public static string[] playerTagsBlue = new string[4]; //All blue player gamertags
	public static string[] playerTagsRed = new string[4]; //All red player gamertags
	
	//The team's score
	public static int blueScore=0; //Blue score
	public static int redScore=0; //Red score
	
	private bool drawTheScore=false; //If the scoreboard is active
	
	
	private bool addedOnBoard=false; //If we're added on the scoreboard
	
	public int myLastShot; //The last player who shot us
	
	public bool wasKilled=false; //If I was killed
	
	//A counter for the update of the score when we connect
	private float updateCounter;
	
	private bool isUpdated; //If I have the correct scores
	
	
	// Use this for initialization
	void Start () {
		myLastShot=0;
		
		//We put -1 in each tab of the array to say its empty
		
		/*if(PhotonNetwork.isMasterClient)
		{
			for(int i=0; i<4; i++)
			{
				playerScoreBlue[i,0]=-1;
				playerScoreRed[i,0]=-1;
			}
		}*/
		
		drawTheScore=false;
		
		isUpdated = false;
		
		updateCounter = 0f;
	}
	
	/*---------------------------- UPDATE ----------------------------*/
	
	// Update is called once per frame
	void Update () {
		PhotonView photonView = PhotonView.Get(this);
		
		/*------------------------- VERIFYING SYNCHRONIZATION -------------*/
		//We check if we have the scores of everyone
		int numberOfScores = countPlayers();
		
		if(numberOfScores != PhotonNetwork.playerList.Length) //We do not have all scores
		{
			if(updateCounter >= 2.0f)
			{
				int myID = photonView.owner.ID;
			
				photonView.RPC("updateMe", PhotonTargets.All, myID);
				updateCounter = 0f;
				Debug.Log("Requesting update");
			}
			else
			{
				updateCounter += Time.deltaTime;
			}
		}
		
		/*--------------------------------------------------------------------*/
		
		//When our playerCreator is created we add ourself on the scoreboards
		playerSpawn playSpawn = GetComponent<playerSpawn>();
		
		
		//While we're not on the scoreboard we try to be added
		if(addedOnBoard==false)
		{
			//We make a string out of our gamertag and playerID
			string myTag = PlayerPrefs.GetString("Gamertag");
			int myID = photonView.owner.ID;
			
			//If we have all the components we send the RPC
			if(myTeam=="Blue" || myTeam=="Red")
			{
				string tagTeamID = myID.ToString() + ";" + myTag + ";" + myTeam;
				
				if(PhotonNetwork.isMasterClient)
				{
					photonView.RPC("addOnBoard", PhotonTargets.All, tagTeamID);
				}
				else
				{
					photonView.RPC("addOnBoard", PhotonTargets.All, tagTeamID);
					photonView.RPC("updateMe", PhotonTargets.All, myID);
				}
				
				/*if(photonView.isMine)
				{
					addOnBoard(tagTeamID);
				}*/
				addedOnBoard=true;
			}
		}
		
		
		//Managing the kills
		
		//We update the last shot each frame
		foreach (Transform child in transform)
		{
			if (child.name != "globalHUD" && child.name!="scoreBoardUI" && child.name != "InGameMenu"){
				playerNetwork playSet = child.GetComponent<playerNetwork>();
				myLastShot = playSet.lastShot;
			}
		}
		
		//If we're killed we add the kill and update the score
		if(wasKilled)
		{
			int myID = photonView.owner.ID;
			if(myTeam=="Blue")
			{
				photonView.RPC("addRedKill", PhotonTargets.All, myLastShot);
				photonView.RPC("addBlueDeath", PhotonTargets.All, myID);
			}
			else
			{
				photonView.RPC("addBlueKill", PhotonTargets.All, myLastShot);
				photonView.RPC("addRedDeath", PhotonTargets.All, myID);
			}
			wasKilled=false;
		}
		
		/*--------------------------- SCOREBOARD ----------------------*/

		//We get the scoreboard object
		Transform scoreB = transform.Find("scoreBoardUI");
		
		//Drawing the score
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			drawTheScore=true;
		}
		if(Input.GetKeyUp(KeyCode.Tab))
		{
			drawTheScore=false;
		}
		
		if(photonView.isMine) 
		{
			if(drawTheScore) //If the player press tab we activate the board
			{
				scoreB.gameObject.active = true;
			}
			else
			{
				scoreB.gameObject.active = false;
			}
		}
		else
		{
			scoreB.gameObject.active = false;
		}
	}
	
	void Awake() {

	}
	
	/*----------------------------- NETWORK ---------------------------*/
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
	{
		if (stream.isWriting)
		{
			if(PhotonNetwork.isMasterClient)
			{
				//stream.SendNext(blueScore);
				//stream.SendNext(redScore);
				
				
				/*stream.SendNext(playerScoreBlue[0]);
				stream.SendNext(playerScoreBlue[1]);
				stream.SendNext(playerScoreBlue[2]);
				stream.SendNext(playerScoreBlue[3]);*/
				
				//stream.SendNext(playerTagsBlue[]);
				
				
				/*stream.SendNext(playerScoreRed[0]);
				stream.SendNext(playerScoreRed[1]);
				stream.SendNext(playerScoreRed[2]);
				stream.SendNext(playerScoreRed[3]);*/
				
				//stream.SendNext(playerTagsRed[]);
			}
		}
		else
		{
			//blueScore = (int)stream.ReceiveNext();
			//redScore = (int)stream.ReceiveNext();
			
			
			/*playerScoreBlue[0] = (int)stream.ReceiveNext();
			playerScoreBlue[1] = (int)stream.ReceiveNext();
			playerScoreBlue[2] = (int)stream.ReceiveNext();
			playerScoreBlue[3] = (int)stream.ReceiveNext();*/
			
			//playerTagsBlue[] = (string[])stream.ReceiveNext();
			
			
			/*playerScoreRed[0] = (int)stream.ReceiveNext();
			playerScoreRed[1] = (int)stream.ReceiveNext();
			playerScoreRed[2] = (int)stream.ReceiveNext();
			playerScoreRed[3] = (int)stream.ReceiveNext();*/
			
			//playerTagsRed[] = (string[])stream.ReceiveNext();
		}
	}
	
	
	
	/*--------------------------------------------------------------------*/
	
	/*#########
	#   GUI   #
	#########*/
	
	/*--------------------------------------------------------------------*/
	
	void OnGUI()
	{
		/*PhotonView photonView = PhotonView.Get(this);
		//Drawing the score
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			drawTheScore=true;
		}
		if(Input.GetKeyUp(KeyCode.Tab))
		{
			drawTheScore=false;
		}
		
		if(drawTheScore)
		{
			if(photonView.isMine)
			{
				drawScore();
			}
		}*/
		
		
		//drawTeamScores();
	}
	
	
	/*------------------------------- SCOREBOARD ------------------------------*/
	
	void drawScore()
	{
		GUI.Box (new Rect(Screen.width/2-200, 150, 400, 240), "SCOREBOARD");
		
		/*--------------------------------- BLUE SCORE ------------------------------*/
		GUI.Box (new Rect(Screen.width/2-195, 175, 190, 220), "BLUE - " + blueScore.ToString());
		int iB=0; //Count to browse all the players
		int jB=0; //Count to display all the players
		
		while(iB<4)
		{
			//if(playerScoreBlue[iB,0]!=0)
			if(playerScoreBlue[4*iB] != 0) //For the 1D tab
			{
				string scoreToDraw= playerScoreBlue[4*iB] + " | " + playerTagsBlue[iB] + " | " + playerScoreBlue[4*iB+1] + " | " + playerScoreBlue[4*iB+2] + " | " + playerScoreBlue[4*iB+3];
				GUI.Box (new Rect(Screen.width/2-190, 200+(50*jB), 180, 40), scoreToDraw);
				jB++;
			}
			iB++;
		}
		
		/*--------------------------------- RED SCORE -------------------------------*/
		GUI.Box (new Rect(Screen.width/2+5, 175, 190, 220), redScore.ToString() + " - RED");
		int iR=0; //Count to browse all the players
		int jR=0; //Count to display all the players
		
		while(iR<4)
		{
			//if(playerScoreRed[iR,0]!=0)
			if(playerScoreRed[4*iR] != 0)
			{
				string scoreToDraw= playerScoreRed[4*iR] + " | " + playerTagsRed[iR] + " | " + playerScoreRed[4*iR+1] + " | " + playerScoreRed[4*iR+2] + " | " + playerScoreRed[4*iR+3];
				GUI.Box (new Rect(Screen.width/2+10, 200+(50*jR), 180, 40), scoreToDraw);
				jR++;
			}
			iR++;
		}
	}
	
	/*------------------------------------ TEAM SCORES --------------------------*/
	
	void drawTeamScores()
	{
		//Blue score
		GUI.Label(new Rect(Screen.width/2-20, 5, 100, 25), blueScore.ToString());
	
		//Red score
		GUI.Label(new Rect(Screen.width/2+20, 5, 100, 25), redScore.ToString());
	}
	
	
	
	
	
	/*-----------------------------#### SCORE MANAGEMENT ####--------------------*/
	
	
	/*#############################
	#   NETWORK SYNCHRONIZATION   #
	#############################*/
	
	
	
	
	/*------------------------- DELETING A PLAYER FROM THE BOARD ---------------*/
	
	void deletePlayer(int playerToDelete)
	{
		int i=0;
		bool done=false;
		
		/*while(i<4 && done==false)
		{
			if(playerScoreBlue[i,0] == playerToDelete)
			{
				playerScoreBlue[i,0] = 0;
				playerScoreBlue[i,1] = 0;
				playerScoreBlue[i,2] = 0;
				playerScoreBlue[i,3] = 0;
				
				playerTagsBlue[i] = "";
				
				done = true;
			}
			if(playerScoreRed[i,0] == playerToDelete)
			{
				playerScoreRed[i,0] = 0;
				playerScoreRed[i,1] = 0;
				playerScoreRed[i,2] = 0;
				playerScoreRed[i,3] = 0;
				
				playerTagsRed[i] = "";
				
				done = true;
			}
			i++;
		}*/	

		while(i<4 && done==false)
		{
			if(playerScoreBlue[4*i] == playerToDelete)
			{
				playerScoreBlue[4*i] = 0;
				playerScoreBlue[4*i+1] = 0;
				playerScoreBlue[4*i+2] = 0;
				playerScoreBlue[4*i+3] = 0;
				
				playerTagsBlue[i] = "";
				
				networkManager.blueCount--;
				
				done = true;
			}
			if(playerScoreRed[4*i] == playerToDelete)
			{
				playerScoreRed[4*i] = 0;
				playerScoreRed[4*i+1] = 0;
				playerScoreRed[4*i+2] = 0;
				playerScoreRed[4*i+3] = 0;
				
				playerTagsRed[i] = "";
				
				networkManager.redCount--;
				
				done = true;
			}
			i++;
		}	
	}
	
	
	/*----------------------- REQUESTING AN UPDATE ON THE TABS ---------------*/
	
	[RPC]
	void updateMe(int senderID)
	{
		//********** ADDING US ON THE BOARD
		
		//We get the ID of the new player who needs to be updated
		PhotonPlayer sender = PhotonPlayer.Find(senderID);
		
		//We get our own ID
		PhotonView myView = PhotonView.Get(this);
		int myID = myView.owner.ID;
		
		//We get our basic stats
		string myTag="";
		
		int iA=0;
		bool doneA=false;
		
		//Retrieving our gamertag
		/*while(iA<4 && doneA==false)
		{
			if(myTeam=="Blue")
			{
				myTag = playerTagsBlue[iA];
			}
			else
			{
				myTag = playerTagsRed[iA];
			}
			iA++;
		}*/
		
		myTag = PlayerPrefs.GetString("Gamertag");
		
		//We pack all of this
		string myTagTeam = myID.ToString() + ";" + myTag + ";" + myTeam;
		
		//We add ourself on his board
		myView.RPC("addOnBoard", PhotonTargets.All, myTagTeam);
		
		
		//********* UPDATING OUR STATS ON THE BOARD
		//We get our stats
		int myKill=0;
		int myAssist=0;
		int myDeath=0;
		
		int iU=0;
		bool doneU=false;
		
		//Retrieving our score
		/*while(iU<4 && doneU==false)
		{
			if(myTeam=="Blue")
			{
				if(playerScoreBlue[iU,0]==myID)
				{
					myKill = playerScoreBlue[iU,1];
					myAssist = playerScoreBlue[iU,2];
					myDeath = playerScoreBlue[iU,3];
					doneU=true;
				}
			}
			else {
				if(playerScoreRed[iU,0]==myID)
				{
					myKill = playerScoreRed[iU,1];
					myAssist = playerScoreRed[iU,2];
					myDeath = playerScoreRed[iU,3];
					doneU=true;
				}
			}
			iU++;
		}*/
		
		while(iU<4 && doneU==false)
		{
			if(myTeam=="Blue")
			{
				if(playerScoreBlue[4*iU]==myID)
				{
					myKill = playerScoreBlue[4*iU+1];
					myAssist = playerScoreBlue[4*iU+2];
					myDeath = playerScoreBlue[4*iU+3];
					doneU=true;
				}
			}
			else {
				if(playerScoreRed[4*iU]==myID)
				{
					myKill = playerScoreRed[4*iU+1];
					myAssist = playerScoreRed[4*iU+2];
					myDeath = playerScoreRed[4*iU+3];
					doneU=true;
				}
			}
			iU++;
		}
		
		//We pack this into a string
		string myTagScore = myID.ToString() + ";" + myKill.ToString() + ";" + myAssist.ToString() + ";" + myDeath.ToString();
		myView.RPC("updateScore", PhotonTargets.All, myTagScore);
	}
	
	
	
	/*-------------------------- REQUESTING TO BE ADDED ----------------------*/
	
	[RPC]
	void addMe(string identity)
	{
		photonView.RPC("addOnBoard", PhotonTargets.Others, identity);
		
		/*string[] tagID = identity.Split(';');
		int senderID = int.Parse(tagID[0]);
		PhotonPlayer sender = PhotonPlayer.Find(senderID);
		//photonView.RPC("sendScoreTabs", sender);*/
	}
	
	/*----------------------------- ADDING A PLAYER ---------------------------*/
	
	[RPC]
	void addOnBoard(string ident)
	{
		PhotonView photonView = PhotonView.Get(this);
		
		//We take back his infos
		string[] playerSet = ident.Split(';');
		
		int playerID = int.Parse(playerSet[0]);
		string playerTag = playerSet[1];
		string playerTeam = playerSet[2];
		
		//We look for an empty place on the board
		int i=0;
		bool done=false;
		
		/*while(i<4 && done==false)
		{
			if(playerScoreBlue[i,0]==playerID || playerScoreRed[i,0]==playerID)
				done=true;
				
			if(playerScoreBlue[i,0]==0 && playerTeam=="Blue" && done==false)
			{
				playerScoreBlue[i,0]=playerID;
				playerScoreBlue[i,1]=0;
				playerScoreBlue[i,2]=0;
				playerScoreBlue[i,3]=0;
				
				playerTagsBlue[i]=playerTag;
				Debug.Log("Added player " + playerID + " on team " + playerTeam);
				done=true;
			}
			if(playerScoreRed[i,0]==0 && playerTeam=="Red" && done==false)
			{
				playerScoreRed[i,0]=playerID;
				playerScoreRed[i,1]=0;
				playerScoreRed[i,2]=0;
				playerScoreRed[i,3]=0;
					
				playerTagsRed[i]=playerTag;
				Debug.Log("Added player " + playerID + " on team " + playerTeam);
				done=true;
			}
			i++;
		}*/
		
		while(i<4 && done==false)
		{
			if(playerScoreBlue[4*i]==playerID || playerScoreRed[4*i]==playerID)
				done=true;
				
			if(playerScoreBlue[4*i]==0 && playerTeam=="Blue" && done==false)
			{
				playerScoreBlue[4*i]=playerID;
				playerScoreBlue[4*i+1]=0;
				playerScoreBlue[4*i+2]=0;
				playerScoreBlue[4*i+3]=0;
				
				playerTagsBlue[i]=playerTag;
				Debug.Log("Added player " + playerID + " on team " + playerTeam);
				done=true;
			}
			if(playerScoreRed[4*i]==0 && playerTeam=="Red" && done==false)
			{
				playerScoreRed[4*i]=playerID;
				playerScoreRed[4*i+1]=0;
				playerScoreRed[4*i+2]=0;
				playerScoreRed[4*i+3]=0;
					
				playerTagsRed[i]=playerTag;
				Debug.Log("Added player " + playerID + " on team " + playerTeam);
				done=true;
			}
			i++;
		}

		//if (photonView.isMine)
			//photonView.RPC("addOnBoard", PhotonTargets.Others, ident);		
	}
	
	
	/*---------------------------------------------------------------------------*/
	// UPDATING THE SCORE
	/*---------------------------------------------------------------------------*/
	
	
	
	/*-------------------------------- SENDING A SCORE --------------------------*/
	
	[RPC]
	void updateScore(string tagTeam)
	{
		string[] tagID = tagTeam.Split(';');
		
		int playerID = int.Parse(tagID[0]);
		int playerKill = int.Parse(tagID[1]);
		int playerAssist = int.Parse(tagID[2]);
		int playerDeath = int.Parse(tagID[3]);
		
		int i=0;
		bool done=false;
		
		/*while(i<4 && done==false)
		{
			if(playerScoreBlue[i,0] == playerID)
			{
				playerScoreBlue[i,1] = playerKill;
				playerScoreBlue[i,2] = playerAssist;
				playerScoreBlue[i,3] = playerDeath;
				done=true;
			}
			
			if(playerScoreRed[i,0] == playerID)
			{
				playerScoreRed[i,1] = playerKill;
				playerScoreRed[i,2] = playerAssist;
				playerScoreRed[i,3] = playerDeath;
				done=true;
			}
			i++;
		}*/
		
		while(i<4 && done==false)
		{
			if(playerScoreBlue[4*i] == playerID)
			{
				playerScoreBlue[4*i+1] = playerKill;
				playerScoreBlue[4*i+2] = playerAssist;
				playerScoreBlue[4*i+3] = playerDeath;
				done=true;
			}
			
			if(playerScoreRed[4*i] == playerID)
			{
				playerScoreRed[4*i+1] = playerKill;
				playerScoreRed[4*i+2] = playerAssist;
				playerScoreRed[4*i+3] = playerDeath;
				done=true;
			}
			i++;
		}
	}
	
	
	/*------------------------------- ADDING A KILL ----------------------------*/
	
	//Add a kill to a blue player
	[RPC]
	void addBlueKill(int killerID)
	{		
		PhotonView photonView = PhotonView.Get(this);
		int i=0;
		bool done=false;
		while(i < 4 && done==false)
		{
			//When we found the case of the killer
			/*if(playerScoreBlue[i,0]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreBlue[i,1]++;
				done=true;
				Debug.Log("Kill added for player " + killerID);
			}
			i++;*/
			
			if(playerScoreBlue[4*i]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreBlue[4*i+1]++;
				done=true;
				Debug.Log("Kill added for player " + killerID);
			}
			i++;
		}
		
		blueScore++;
		
		//if (photonView.isMine)
			//photonView.RPC("addBlueKill", PhotonTargets.Others, killerID);
			
		//At the end we update the scoreboards for everyone
		//sendScoreTabs("Blue");
	}
	
	//Add a kill to a red player
	[RPC]
	void addRedKill(int killerID)
	{
		PhotonView photonView = PhotonView.Get(this);
		int i=0;
		bool done=false;
		while(i < 4 && done==false)
		{
			//When we found the case of the killer
			/*if(playerScoreRed[i,0]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreRed[i,1]++;
				done=true;
				Debug.Log("Kill added for player " + killerID);
			}
			i++;*/
			
			if(playerScoreRed[4*i]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreRed[4*i+1]++;
				done=true;
				Debug.Log("Kill added for player " + killerID);
			}
			i++;
		}
		
		redScore++;
		
		//if (photonView.isMine)
			//photonView.RPC("addRedKill", PhotonTargets.Others, killerID);
		
		//At the end we update the scoreboards for everyone
		//sendScoreTabs("Red");
	}
	
	
	/*----------------------------------- ADDING A DEATH -------------------------*/
	
	//Add a kill to a blue player
	[RPC]
	void addBlueDeath(int killerID)
	{		
		PhotonView photonView = PhotonView.Get(this);
		int i=0;
		bool done=false;
		while(i < 4 && done==false)
		{
			//When we found the case of the killer
			/*if(playerScoreBlue[i,0]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreBlue[i,3]++;
				done=true;
				Debug.Log("Death added for player " + killerID);
			}
			i++;*/
			
			if(playerScoreBlue[4*i]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreBlue[4*i+3]++;
				done=true;
				Debug.Log("Death added for player " + killerID);
			}
			i++;
		}
		
		//if (photonView.isMine)
			//photonView.RPC("addBlueKill", PhotonTargets.Others, killerID);
			
		//At the end we update the scoreboards for everyone
		//sendScoreTabs("Blue");
	}
	
	//Add a kill to a red player
	[RPC]
	void addRedDeath(int killerID)
	{
		PhotonView photonView = PhotonView.Get(this);
		int i=0;
		bool done=false;
		while(i < 4 && done==false)
		{
			//When we found the case of the killer
			/*if(playerScoreRed[i,0]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreRed[i,3]++;
				done=true;
				Debug.Log("Death added for player " + killerID);
			}
			i++;*/
			
			if(playerScoreRed[4*i]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreRed[4*i+3]++;
				done=true;
				Debug.Log("Death added for player " + killerID);
			}
			i++;
		}
		
		//if (photonView.isMine)
			//photonView.RPC("addRedKill", PhotonTargets.Others, killerID);
		
		//At the end we update the scoreboards for everyone
		//sendScoreTabs("Red");
	}
	
	private int countPlayers() //To count how many players we are aware of ( if we're updated )
	{
		int total = 0;
		
		for(int i=0; i<4; i++)
		{
			if(playerScoreBlue[4*i] != 0)
			{
				total++;
			}
			
			if(playerScoreRed[4*i] != 0)
			{
				total++;
			}
		}
		
		return total;
	}
	
	//Updating the team scores
	[RPC]
	void updateTeamScores(string scores)
	{
		string[] theScores = scores.Split(';');
		blueScore = int.Parse(theScores[0]);
		redScore = int.Parse(theScores[1]);
	}
	
	
	/*---------------------------------- ESSENTIAL PHOTON FUNCTIONS --------------*/
	
	/*--------------------- WHEN A PLAYER LEAVES WE DELETE HIS SCORE -------------*/
	
	void OnPhotonPlayerDisconnected(PhotonPlayer disconnectedPlayer)
	{
		int disconnectedID = disconnectedPlayer.ID;
		deletePlayer(disconnectedID);
	}
	
	void OnPhotonPlayerConnected(PhotonPlayer connectedPlayer)
	{
		if(PhotonNetwork.isMasterClient)
		{
			string teamScores = blueScore.ToString() + ";" + redScore.ToString();
			
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("updateTeamScores", connectedPlayer, teamScores);
		}
		
		//We add ourself on his board and update our score
		int hisID = connectedPlayer.ID;
		
		updateMe(hisID);
	}
}