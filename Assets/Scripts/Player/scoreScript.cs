using UnityEngine;
using System.Collections;

public class scoreScript : Photon.MonoBehaviour {


	/*###########
	#   SCORE   #
	###########*/
	public string myTeam; //Team of the player
	
	public int blueCount=0; //Number of blue players
	public int redCount=0; //Number of red players
	
	//Score tabs of each team. Contains 0:viewID of Player, 1: Kills, 2: Assists: 3:Deaths;
	//public static int[,] playerScoreBlue = new int[4,4]; //Original 2D tabs
	//public static int[,] playerScoreRed = new int[4,4];
	
	//The team's score
	public static int blueScore=0; //Blue score
	public static int redScore=0; //Red score
	
	private bool drawTheScore=false; //If the scoreboard is active
	
	
	private bool addedOnBoard=false; //If we're added on the scoreboard
	
	public int myLastShot; //The last player who shot us
	
	public bool wasKilled=false; //If I was killed
	
	//A counter for the update of the score when we connect
	private float updateCounter;
	
	//Update counter for the masterclient when he updates the other players
	private float updateTimer;
	
	private float pingTimer; //A timer for pinging players
	private float pingUpdate; //The counter to update ping
	
	private bool isUpdated; //If I have the correct scores
	
	//---------------------------- THE Score
	public int myKills;
	public int myAssists;
	public int myDeaths;
	
	//---------------------------- The Timer
	public float matchTimer=300f;
	
	private bool isMatchEnded;
	
	
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
		
		if(PhotonNetwork.isMasterClient)
		{
			updateTimer=0f;
			isUpdated = true;
		}
		else
		{
			isUpdated = false;
		}
		
		updateCounter = 0f;
		pingUpdate = 0f;
		
		myKills=0;
		myAssists=0;
		myDeaths=0;
		
		//Duration of the match
		if(PhotonNetwork.isMasterClient)
			matchTimer = 300f;
		
		isMatchEnded = false;
	}
	
	/*---------------------------- UPDATE ----------------------------*/
	
	// Update is called once per frame
	void Update () {
		PhotonView photonView = PhotonView.Get(this);
		
		/*------------------------- VERIFYING SYNCHRONIZATION -------------*/
		//We check if we have the scores of everyone
		
		/*if(!isUpdated) //We're not updated
		{
			if(updateCounter >= 2.0f)
			{
				int myID = photonView.owner.ID;
			
				photonView.RPC("updateMe", PhotonTargets.MasterClient, null);
				updateCounter = 0f;
				Debug.Log("Requesting update");
			}
			else
			{
				updateCounter += Time.deltaTime;
			}
		}
		
		/*----------------------------- WE UPDATE THE PLAYERS REGULARLY -----------*/
		if(PhotonNetwork.isMasterClient)
		{
			/*if(updateTimer >= 2.0f)
			{
				string theTeamScores = blueScore + ";" + redScore;
				photonView.RPC("updateTeamScores", PhotonTargets.Others, theTeamScores);
				updateTimer = 0f;
			}
			else
			{
				updateTimer += Time.deltaTime;
			}*/
			
			if(pingUpdate >= 5.0f)
			{
				pingFunction();
				pingUpdate = 0f;
			}
			else
			{
				pingUpdate += Time.deltaTime;
			}
			pingTimer += Time.deltaTime;
		}
		
		/*--------------------------------------------------------------------*/
		
		//When our playerCreator is created we add ourself on the scoreboards
		playerSpawn playSpawn = GetComponent<playerSpawn>();
		
		
		//Managing the kills
		
		//We update the last shot each frame
		foreach (Transform child in transform)
		{
			if (child.name != "globalHUD" && child.name!="scoreBoardUI" && child.name != "InGameMenu" && child.name!="littleScoreBoardUI" && child.name!="PlayerHUD"){
				playerNetwork playSet = child.GetComponent<playerNetwork>();
				myLastShot = playSet.lastShot;
			}
		}
		
		//If we're killed we add the kill and update the score
		if(wasKilled)
		{
			PhotonPlayer killer = PhotonPlayer.Find(myLastShot);
			PunPlayerScores playerScoreScript = GetComponent<PunPlayerScores>();
			ScoreExtensions.AddScore(killer,1);
			
			int myID = photonView.owner.ID;
			//photonView.RPC("addKill", killer, null);
			//photonView.RPC("addKill", PhotonTargets.All, myLastShot);
			
			if(myTeam=="Blue")
			{
				//photonView.RPC("addRedKill", PhotonTargets.All, myLastShot);
				//photonView.RPC("addRedKill", PhotonTargets.All, null);
				TeamExtensions.AddTeamScore("Red");
				//photonView.RPC("addBlueDeath", PhotonTargets.All, myID);
			}
			else
			{
				//photonView.RPC("addBlueKill", PhotonTargets.All, myLastShot);
				//photonView.RPC("addBlueKill", PhotonTargets.All, null);
				TeamExtensions.AddTeamScore("Blue");
				//photonView.RPC("addRedDeath", PhotonTargets.All, myID);
			}
			
			PhotonPlayer myPlayer = PhotonPlayer.Find(myID);
			//addDeath();
			DeathExtensions.AddDeath(myPlayer,1);

			//KILLFEED
			TeamExtensions.AddKillFeed(myLastShot,myID);
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
			if(drawTheScore || isMatchEnded) //If the player press tab we activate the board
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
		
		/*------------------------------ TIMER ------------------------*/
		
		//At the end of the match the masterclient stops it
		if(matchTimer<=0f)
		{
			if(PhotonNetwork.isMasterClient)
				photonView.RPC("stopMatch",PhotonTargets.All, null);
		}
		else
		{
			matchTimer-=Time.deltaTime;
			
			//if(PhotonNetwork.isMasterClient)
				//updateMatchTimer();
		}
	}
	
	void Awake() {

	}
	
	/*----------------------------- NETWORK ---------------------------*/
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
	{
		//if (stream.isWriting)
		//{
			//if(PhotonNetwork.isMasterClient)
			//{
				//stream.SendNext(blueScore);
				//stream.SendNext(redScore);
				
				
				/*stream.SendNext(playerScoreBlue[0]);
				stream.SendNext(playerScoreBlue[1]);
				stream.SendNext(playerScoreBlue[2]);
				stream.SendNext(playerScoreBlue[3]);
				
				//stream.SendNext(playerTagsBlue[]);
				
				
				/*stream.SendNext(playerScoreRed[0]);
				stream.SendNext(playerScoreRed[1]);
				stream.SendNext(playerScoreRed[2]);
				stream.SendNext(playerScoreRed[3]);
				
				//stream.SendNext(playerTagsRed[]);
			}
		}
		else
		{
			//blueScore = (int)stream.ReceiveNext();
			//redScore = (int)stream.ReceiveNext();
			
			
			playerScoreBlue[0] = (int)stream.ReceiveNext();
			playerScoreBlue[1] = (int)stream.ReceiveNext();
			playerScoreBlue[2] = (int)stream.ReceiveNext();
			playerScoreBlue[3] = (int)stream.ReceiveNext();
			
			//playerTagsBlue[] = (string[])stream.ReceiveNext();
			
			
			playerScoreRed[0] = (int)stream.ReceiveNext();
			playerScoreRed[1] = (int)stream.ReceiveNext();
			playerScoreRed[2] = (int)stream.ReceiveNext();
			playerScoreRed[3] = (int)stream.ReceiveNext();
			
			//playerTagsRed[] = (string[])stream.ReceiveNext();
		}*/
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
		PhotonView myView = PhotonView.Get(this);
		PhotonPlayer myPlayer = myView.owner;
		string myTeam = TeamExtensions.getStringTeam(myPlayer);
		GUI.Label(new Rect(400,250,250,50),"PUNTeam: " + myTeam);
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
	void updateMe()
	{
		//********** ADDING US ON THE BOARD
		
		//We get the ID of the new player who needs to be updated
		//PhotonPlayer sender = PhotonPlayer.Find(senderID);
		
		//We get our own ID
		PhotonView myView = PhotonView.Get(this);
		int myID = myView.owner.ID;
		
		string theTeamScores = blueScore + ";" + redScore;
		myView.RPC("updateTeamScores", PhotonTargets.All, theTeamScores);
	}
	
	
	/*---------------------------------------------------------------------------*/
	// UPDATING THE SCORE
	/*---------------------------------------------------------------------------*/
	
	
	/*------------------------------- ADDING A KILL ----------------------------*/
	
	//Add a kill to a blue player
	[RPC]
	void addKill(int ID)
	{	
		PhotonView photonView = PhotonView.Get(this);
		int myID = photonView.owner.ID;
		
		if(myID == ID)
		{
			ajoutKill();
			
			int globalKill = PlayerPrefs.GetInt("totalKills");
			globalKill++;
			PlayerPrefs.SetInt("totalKills",globalKill);
			this.myKills++;
		}
	}
	
	void ajoutKill()
	{
		int globalKill = PlayerPrefs.GetInt("totalKills");
		globalKill++;
		PlayerPrefs.SetInt("totalKills",globalKill);
		this.myKills++;
	}
	
	//Add a kill to a blue player TEST
	[RPC]
	void addKillToMe()
	{		
		PhotonView photonView = PhotonView.Get(this);
		int myID = photonView.owner.ID;
		
		int globalKill = PlayerPrefs.GetInt("totalKills");
		globalKill++;
		PlayerPrefs.SetInt("totalKills",globalKill);
		myKills++;
	}
	
	[RPC]
	void addMeKill(int ID)
	{
		PhotonView photonView = PhotonView.Get(this);
		int myID = photonView.owner.ID;
		
		if(myID == ID)
		{
			photonView.RPC("addKillToMe", PhotonTargets.All, null);
		}
	}
	
	//Add a kill to a red player
	[RPC]
	void addRedKill()
	{
		redScore++;
	}
	
	//Add a kill to a blue player
	[RPC]
	void addBlueKill()
	{
		blueScore++;
	}
	
	
	/*----------------------------------- ADDING A DEATH -------------------------*/
	
	//Add a kill to the player
	void addDeath()
	{		
		
		myDeaths++;
		int globalDeath = PlayerPrefs.GetInt("totalDeaths");
		globalDeath++;
		PlayerPrefs.SetInt("totalDeaths",globalDeath);
	}
	
	//Updating the team scores
	[RPC]
	void updateTeamScores(string scores)
	{
		if(!isUpdated)
		{
			string[] theScores = scores.Split(';');
			blueScore = int.Parse(theScores[0]);
			redScore = int.Parse(theScores[1]);
			
			isUpdated = true;
		}
	}
	
	
	/*---------------------------------- ESSENTIAL PHOTON FUNCTIONS --------------*/
	
	/*--------------------- WHEN A PLAYER LEAVES WE DELETE HIS SCORE -------------*/
	
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
		
		updateMe();
	}
	
	//
	//PING FUNCTION
	//
	public void pingFunction()
	{
		pingTimer = 0f;
		
		PhotonView masterView = PhotonView.Get(this);
		masterView.RPC("pingPlayer", PhotonTargets.All, null);
	}
	
	[RPC]
	void pingPlayer()
	{
		PhotonView clientView = PhotonView.Get(this);
		int myID = clientView.owner.ID;
		clientView.RPC("sendPingBack", PhotonTargets.MasterClient, myID);
	}
	
	[RPC]
	void sendPingBack(int clientID)
	{
		PhotonPlayer client = PhotonPlayer.Find(clientID);
		Debug.Log(client.name + "(" + clientID.ToString() + ") has a " + (pingTimer*1000.0).ToString() + " ping.");
	}
	
	
	/*------------------------------ TIMER ----------------------*/
	
	void updateMatchTimer()
	{
		PhotonView photonView = PhotonView.Get(this);
		
		//The masterclient updates the time once in a while
		float actualTimer = matchTimer;
		photonView.RPC("sendUpdatedTimer", PhotonTargets.Others, actualTimer);
	}
	
	[RPC]
	void sendUpdatedTimer(float newTimer)
	{
		matchTimer = newTimer;
	}
	
	//Stop the match
	[RPC]
	void stopMatch()
	{
		//We kill the player
		foreach (Transform child in transform)
		{
			if(child.gameObject.tag=="Player")
				PhotonNetwork.Destroy(child.gameObject);
		}
		
		//We display the score
		isMatchEnded = true;
	}		
}