using UnityEngine;
using System.Collections;

public class scoreScriptTest : Photon.MonoBehaviour {


	/*###########
	#   SCORE   #
	###########*/
	public string myTeam;
	
	public int blueCount=0;
	public int redCount=0;
	
	//Score tabs of each team. Contains 0:viewID of Player, 1: Kills, 2: Assists: 3:Deaths;
	public int[,] playerScoreBlue = new int[4,4];
	public int[,] playerScoreRed = new int[4,4];
	
	//We put -1 in each tab of the array to say its empty
	
	//Tabs containing all the players Gamertags
	public string[] playerTagsBlue = new string[4];
	public string[] playerTagsRed = new string[4];
	
	//The team's score
	public static int blueScore=0;
	public static int redScore=0;
	
	private bool drawTheScore;
	
	
	private bool addedOnBoard=false;
	
	public int myLastShot;
	
	public bool wasKilled=false;
	
	
	// Use this for initialization
	void Start () {
		myLastShot=0;
		
		//We put -1 in each tab of the array to say its empty
		for(int i=0; i<4; i++)
		{
			playerScoreBlue[i,0]=-1;
			playerScoreRed[i,0]=-1;
		}
		
		drawTheScore=false;
	}
	
	/*---------------------------- UPDATE ----------------------------*/
	
	// Update is called once per frame
	void Update () {
		PhotonView photonView = PhotonView.Get(this);
		
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
					photonView.RPC("updateMe", PhotonTargets.Others, myID);
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
			if (child.name != "scoreHUD" && child.name!="scoreBoardUI"){
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
				
				//----------------------- SYNCING BLUE SCORES ------------------------
				if(playerScoreBlue[0,0]!=-1)
				{
					stream.SendNext(playerScoreBlue[0,0]);
					stream.SendNext(playerScoreBlue[0,1]);
					stream.SendNext(playerScoreBlue[0,2]);
					stream.SendNext(playerScoreBlue[0,3]);
				}
				
				if(playerScoreBlue[1,0]!=-1)
				{
					stream.SendNext(playerScoreBlue[1,0]);
					stream.SendNext(playerScoreBlue[1,1]);
					stream.SendNext(playerScoreBlue[1,2]);
					stream.SendNext(playerScoreBlue[1,3]);
				}
				
				if(playerScoreBlue[2,0]!=-1)
				{
					stream.SendNext(playerScoreBlue[2,0]);
					stream.SendNext(playerScoreBlue[2,1]);
					stream.SendNext(playerScoreBlue[2,2]);
					stream.SendNext(playerScoreBlue[2,3]);
				}
				
				if(playerScoreBlue[3,0]!=-1)
				{
					stream.SendNext(playerScoreBlue[3,0]);
					stream.SendNext(playerScoreBlue[3,1]);
					stream.SendNext(playerScoreBlue[3,2]);
					stream.SendNext(playerScoreBlue[3,3]);
				}
				
				//----------------- SYNCING BLUE GAMERTAGS ------------------
				
				stream.SendNext(playerTagsBlue[0]);
				stream.SendNext(playerTagsBlue[1]);
				stream.SendNext(playerTagsBlue[2]);
				stream.SendNext(playerTagsBlue[3]);
				
				//--------------------------- SYNCING RED SCORES ---------------
				
				if(playerScoreRed[0,0]!=-1)
				{
					stream.SendNext(playerScoreRed[0,0]);
					stream.SendNext(playerScoreRed[0,1]);
					stream.SendNext(playerScoreRed[0,2]);
					stream.SendNext(playerScoreRed[0,3]);
				}
				
				if(playerScoreRed[1,0]!=-1)
				{
					stream.SendNext(playerScoreRed[1,0]);
					stream.SendNext(playerScoreRed[1,1]);
					stream.SendNext(playerScoreRed[1,2]);
					stream.SendNext(playerScoreRed[1,3]);
				}
				
				if(playerScoreRed[2,0]!=-1)
				{
					stream.SendNext(playerScoreRed[2,0]);
					stream.SendNext(playerScoreRed[2,1]);
					stream.SendNext(playerScoreRed[2,2]);
					stream.SendNext(playerScoreRed[2,3]);
				}
				
				if(playerScoreRed[3,0]!=-1)
				{
					stream.SendNext(playerScoreRed[3,0]);
					stream.SendNext(playerScoreRed[3,1]);
					stream.SendNext(playerScoreRed[3,2]);
					stream.SendNext(playerScoreRed[3,3]);
				}
				
				//--------------------------- SYNCING RED GAMERTAGS --------------
				
				stream.SendNext(playerTagsRed[0]);
				stream.SendNext(playerTagsRed[1]);
				stream.SendNext(playerTagsRed[2]);
				stream.SendNext(playerTagsRed[3]);
			}
		}
		else
		{
			//blueScore = (int)stream.ReceiveNext();
			//redScore = (int)stream.ReceiveNext();
			
			//------------------------- RECEIVING BLUE SCORES -------------------
			
			playerScoreBlue[0,0] = (int)stream.ReceiveNext();
			playerScoreBlue[0,1] = (int)stream.ReceiveNext();
			playerScoreBlue[0,2] = (int)stream.ReceiveNext();
			playerScoreBlue[0,3] = (int)stream.ReceiveNext();
			
			playerScoreBlue[1,0] = (int)stream.ReceiveNext();
			playerScoreBlue[1,1] = (int)stream.ReceiveNext();
			playerScoreBlue[1,2] = (int)stream.ReceiveNext();
			playerScoreBlue[1,3] = (int)stream.ReceiveNext();
			
			playerScoreBlue[2,0] = (int)stream.ReceiveNext();
			playerScoreBlue[2,1] = (int)stream.ReceiveNext();
			playerScoreBlue[2,2] = (int)stream.ReceiveNext();
			playerScoreBlue[2,3] = (int)stream.ReceiveNext();
			
			playerScoreBlue[3,0] = (int)stream.ReceiveNext();
			playerScoreBlue[3,1] = (int)stream.ReceiveNext();
			playerScoreBlue[3,2] = (int)stream.ReceiveNext();
			playerScoreBlue[3,3] = (int)stream.ReceiveNext();
			
			//---------------------- RECEIVING BLUE GAMERTAGS ---------------------------
			
			playerTagsBlue[0] = (string)stream.ReceiveNext();
			playerTagsBlue[1] = (string)stream.ReceiveNext();
			playerTagsBlue[2] = (string)stream.ReceiveNext();
			playerTagsBlue[3] = (string)stream.ReceiveNext();
			
			//---------------------------- RECEIVING RED SCORES --------------------
			
			playerScoreRed[0,0] = (int)stream.ReceiveNext();
			playerScoreRed[0,1] = (int)stream.ReceiveNext();
			playerScoreRed[0,2] = (int)stream.ReceiveNext();
			playerScoreRed[0,3] = (int)stream.ReceiveNext();
			
			playerScoreRed[1,0] = (int)stream.ReceiveNext();
			playerScoreRed[1,1] = (int)stream.ReceiveNext();
			playerScoreRed[1,2] = (int)stream.ReceiveNext();
			playerScoreRed[1,3] = (int)stream.ReceiveNext();
			
			playerScoreRed[2,0] = (int)stream.ReceiveNext();
			playerScoreRed[2,1] = (int)stream.ReceiveNext();
			playerScoreRed[2,2] = (int)stream.ReceiveNext();
			playerScoreRed[2,3] = (int)stream.ReceiveNext();
			
			playerScoreRed[3,0] = (int)stream.ReceiveNext();
			playerScoreRed[3,1] = (int)stream.ReceiveNext();
			playerScoreRed[3,2] = (int)stream.ReceiveNext();
			playerScoreRed[3,3] = (int)stream.ReceiveNext();
			
			//---------------------------- RECEIVING RED GAMERTAGS ----------------
			
			playerTagsRed[0] = (string)stream.ReceiveNext();
			playerTagsRed[1] = (string)stream.ReceiveNext();
			playerTagsRed[2] = (string)stream.ReceiveNext();
			playerTagsRed[3] = (string)stream.ReceiveNext();
		}
	}
	
	
	
	/*--------------------------------------------------------------------*/
	
	/*#########
	#   GUI   #
	#########*/
	
	/*--------------------------------------------------------------------*/
	
	void OnGUI()
	{
		
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
			if(playerScoreBlue[iB,0]!=-1)
			{
				string scoreToDraw= playerScoreBlue[iB,0] + " | " + playerTagsBlue[iB] + " | " + playerScoreBlue[iB,1] + " | " + playerScoreBlue[iB,2] + " | " + playerScoreBlue[iB,3];
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
			if(playerScoreRed[iR,0]!=-1)
			{
				string scoreToDraw= playerScoreRed[iR,0] + " | " + playerTagsRed[iR] + " | " + playerScoreRed[iR,1] + " | " + playerScoreRed[iR,2] + " | " + playerScoreRed[iR,3];
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
		string myTag = PhotonNetwork.playerName;
		
		/*int iA=0;
		bool doneA=false;
		
		//Retrieving our gamertag
		while(iA<4 && doneA==false)
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
		
		//We pack all of this
		string myTagTeam = myID.ToString() + ";" + myTag + ";" + myTeam;
		
		//We add ourself on his board
		myView.RPC("addOnBoard", sender, myTagTeam);
		
		
		//********* UPDATING OUR STATS ON THE BOARD
		//We get our stats
		int myKill=0;
		int myAssist=0;
		int myDeath=0;
		
		int iU=0;
		bool doneU=false;
		
		//Retrieving our score
		while(iU<4 && doneU==false)
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
		}
		
		//We pack this into a string
		string myTagScore = myID.ToString() + ";" + myKill.ToString() + ";" + myAssist.ToString() + ";" + myDeath.ToString();
		myView.RPC("updateScore", sender, myTagScore);
	}
	
	
	
	/*-------------------------- REQUESTING TO BE ADDED ----------------------*/
	
	[RPC]
	void addMe(string identity)
	{
		photonView.RPC("addOnBoard", PhotonTargets.All, identity);
		
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
		
		while(i<4 && done==false)
		{
			if(playerScoreBlue[i,0]==playerID || playerScoreRed[i,0]==playerID)
				done=true;
				
			if(playerScoreBlue[i,0]==-1 && playerTeam=="Blue" && done==false)
			{
				playerScoreBlue[i,0]=playerID;
				playerScoreBlue[i,1]=0;
				playerScoreBlue[i,2]=0;
				playerScoreBlue[i,3]=0;
				
				playerTagsBlue[i]=playerTag;
				Debug.Log("Added player " + playerID + " on team " + playerTeam);
				done=true;
			}
			if(playerScoreRed[i,0]==-1 && playerTeam=="Red" && done==false)
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
		
		while(i<4 && done==false)
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
		}
		
		//if (photonView.isMine)
			//photonView.RPC("updateScore", PhotonTargets.Others, tagTeam);
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
			if(playerScoreBlue[i,0]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreBlue[i,1]++;
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
			if(playerScoreRed[i,0]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreRed[i,1]++;
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
			if(playerScoreBlue[i,0]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreBlue[i,3]++;
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
			if(playerScoreRed[i,0]==killerID)
			{
				//We add +1 to his kill counter
				playerScoreRed[i,3]++;
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
}
