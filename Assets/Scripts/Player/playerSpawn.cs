using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerSpawn : Photon.MonoBehaviour {

	/*######################
	#   PLAYER VARIABLES   #
	######################*/
	
	public GameObject blueSpawn;
	
	public GameObject spawnBlue1;
	public GameObject spawnBlue2;
	
	public Material mat_soldat_bleu;
	public Material mat_soldat_rouge;
		
	private string hunt_ShotG="Hunting_Shotgun";
	
	public string actualTeam;
	
	public bool isAlive=false;
	
	private bool canSpawn=true;
	
	private float spawnCounter;
	private bool spawnCountStarted=false;
	
	/*The weapon pack of the player, containing his 3 weapons ( Primary,
	Secondary and the knife ),	can be changed in the "Player" menu */
	private string[] weaponPack = new string[] {"Hunting_Shotgun"};
	
	/*#######################
	#   CONTROL VARIABLES   #
	#######################*/
	public static int blueCount;
	public static int redCount;
	
	//TIMERS
	private float playerCountUpdateTimer;
	
	
	/*###########
	#   START   #
	###########*/

	// Use this for initialization
	void Start () {
		//On récupère l'ID du joueur
		PhotonView photonView = PhotonView.Get(this);
		int myID = photonView.owner.ID;
		
		//We activate the scoreScript
		scoreScript scoreEn = GetComponent<scoreScript>();
		
		if(photonView.isMine)
		{
			scoreEn.enabled = true;
		}
		else {
			scoreEn.enabled = false;
		}
		
		
		//On change le nom du joueur
		PhotonNetwork.playerName = PlayerPrefs.GetString("Gamertag"); /*--------------------------------------------------------BUG WITH THE PLAYER NAME, PUT BACK TO NORMAL, JUST FOR TEST -----------------------------*/
		/*int playerNumb = Random.Range(0, 10000);
		PhotonNetwork.playerName = "player" + playerNumb.ToString();*/
		
		
		networkManager netMan= (networkManager)GameObject.Find("multiScripts").GetComponent("networkManager");

		
		/*----- IN MATCH -----*/
		
		blueSpawn = GameObject.Find("spawn_Blue");
		
		spawnBlue1 = blueSpawn.transform.Find("spawnBlue1").gameObject;
		spawnBlue2 = blueSpawn.transform.Find("spawnBlue2").gameObject;
		
		playerCountUpdateTimer=0f;
		
		
		//We change the team of the player
		if(PhotonNetwork.isMasterClient)
		{
			blueCount = 0;
			redCount = 0;
			scoreScript scoreMan = GetComponent<scoreScript>();
			
			if(blueCount <= redCount) {
				actualTeam="Blue";
				blueCount++;
				scoreMan.myTeam = "Blue";
			}
			else
			{
				actualTeam="Red";
				redCount++;
				scoreMan.myTeam = "Red";
			}
		}
		else
		{
			this.photonView.RPC("ajoutTeam", PhotonTargets.MasterClient, myID);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		blueSpawn = GameObject.Find("spawn_Blue");
		
		spawnBlue1 = blueSpawn.transform.Find("spawnBlue1").gameObject;
		spawnBlue2 = blueSpawn.transform.Find("spawnBlue2").gameObject;
		
		checkDeath();
		updatePlayerCount();
		
		if(playerCountUpdateTimer >= 2.0f)
		{
			updatePlayers();
			playerCountUpdateTimer=0f;
		}
		else
		{
			playerCountUpdateTimer += Time.deltaTime;
		}

		PhotonView photonView = PhotonView.Get(this);

		Transform playerHUDPanel = transform.Find("PlayerHUD");
		if(isAlive && photonView.isMine)
			playerHUDPanel.gameObject.active = true;
		else
			playerHUDPanel.gameObject.active = false;
	}
	

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else
		{
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
		}
	}
	
	
	/*###########
	#   SPAWN   #
	###########*/
	
	public void spawnPlayer(string weapon, GameObject spawnPlace, string team) {
		
		GameObject player = PhotonNetwork.Instantiate("soldierPrefab", spawnPlace.transform.position, Quaternion.identity, 0);
		Debug.Log(player);
		
		//we set the soldier a child of the playerCreator and we set it alive
		player.transform.parent = transform;
		
		//We select the weapon slot in the right hand
		GameObject weapSlot = player.gameObject.transform.Find("Armature/Dos/Haut_Dos/Epaule_D/Bras_D/Avant_Bras_D/Main_D/Paume_D/Majeur_1_D/weaponSlot").gameObject;
		Debug.Log(weapSlot);
		
		//We spawn the weapon
		GameObject weaponSelec = PhotonNetwork.Instantiate(weapon, weapSlot.transform.position, Quaternion.identity, 0);
		
		
		shotScript shootingScript = weaponSelec.GetComponent<shotScript>();
		
		weaponSelec.transform.parent = weapSlot.transform;
		
		playerNetwork playerNet = player.GetComponent<playerNetwork>();
		//playerNet.enabled = true;
		playerNet.playerTeam = team;
		Debug.Log("Spawned player on team " + team);
		
		
		//GameObject soldier = player.gameObject.transform.Find("Soldier").gameObject;
	}
	
	/*#####################################################
	#   PRINTING THE SPAWN BUTTONS FOR ALL YOUR WEAPONS   #
	#####################################################*/
	
	void printSpawnButton(string[] weapPack)
	{
		for(int i=0; i<weapPack.Length; i++)
		{
			if(photonView.isMine)
			{
				if(canSpawn)
				{
					if (GUI.Button(new Rect(Screen.width/2-320+(220*i), 10, 200, 50), "Choose this weapon"))
					{
						spawnPlayer(weapPack[i], spawnBlue1, actualTeam);
						isAlive=true;
						canSpawn=false;
					}
				}
			}
		}
	}
	
	
	/*#########
	#   GUI   #
	#########*/
	
	void OnGUI() {
		if(PhotonNetwork.isMasterClient) {
			GUI.Label(new Rect(200,300,250,50),"True");
		}
		else {
			GUI.Label(new Rect(200,300,250,50),"False");
		}
			
		PhotonView photonView = PhotonView.Get(this);
		
		//Mettre les affichages ici si ils ne sont que pour le perso ( photonview.ismine )
		if(photonView.isMine)
		{
			/*if(isAlive==false)
			{
				if(canSpawn)
				{
					if (GUI.Button(new Rect(Screen.width/2-100, 10, 200, 50), "Choose the Shotgun"))
					{
						spawnPlayer(hunt_ShotG, spawnBlue1, actualTeam);
						isAlive=true;
					}
				}
			}*/
			
			GUI.Label(new Rect(300,300,250,50),"TeamTEST:" + actualTeam);
			GUI.Label(new Rect(300,400,250,50),"Gamertag:" + PhotonNetwork.playerName);
			if(isAlive==false) 
			{
				GUI.Label(new Rect(300,350,250,50),"Im dead");
			}
		}
		//FIN PHOTONVIEW.ISMINE
		
		//Respawn function
		respawn();
	}
	
	
	/*###########
	#   DEATH   #
	###########*/
	
	void checkDeath()
	{
		//We get the child transform ( the transform of the soldier )
		foreach (Transform child in transform)
		{
			if (child.name != "globalHUD" && child.name!="scoreBoardUI" && child.name != "InGameMenu" && child.name!="littleScoreBoardUI" && child.name!="PlayerHUD"){
				playerNetwork playSet = child.gameObject.GetComponent<playerNetwork>();
					
				if(playSet.health <= 0)
				{
					//We get the id of the last shot
					int killer = playSet.lastShot;
					
					/*if(actualTeam=="Blue")
					{
						addRedKill(killer);
						this.photonView.RPC("addRedKill", PhotonTargets.All, killer);
					}
					else {
						addBlueKill(killer);
						this.photonView.RPC("addBlueKill", PhotonTargets.All, killer);
					}*/
					
					//We trigger the respawn function by setting us non-alive
					playSet.isAlive = false;
					isAlive=false;
					
					scoreScript scoreMan = GetComponent<scoreScript>();
					scoreMan.wasKilled = true;
					
				}
			}
		}
	}
	
	
	/*#############
	#   RESPAWN   #
	#############*/
	
	void respawn()
	{
		/*------------------------------ RESPAWN ------------------------------*/
		//Right after we died we are not alive but we cant spawn so we start a counter of 5 seconds unitl we can respawn
		if(isAlive==false)
		{
			if(canSpawn==false)
			{
				if(spawnCountStarted==false) //We start the counter only once
				{
					spawnCounter=0f;
					spawnCountStarted=true;
				}
				else //If the counter is already started we keep it running
				{
					if(spawnCounter>=5f)
					{
						canSpawn=true;
						spawnCountStarted=false;  //Once we finished the counter we put it back to 0 for the next time we die
					}
					else
					{
						spawnCounter+=Time.deltaTime;
					}
				}
			}
			else //If isAlive==false and canSpawn==true => We're dead but we can spawn, so we draw the button to choose the weapon and spawn
			{
				printSpawnButton(weaponPack);  //A function which prints the button to choose your weapon, depending on the weapon you have
			}
		}
	}
	
	
	
	
	/*###################
	#   RPC FUNCTIONS   #
	###################*/
	
	//Kill the player
	void killPlayer()
	{
		foreach (Transform playChild in transform)
		{
			playerNetwork playerChild = playChild.GetComponent<playerNetwork>();
			playerChild.isAlive=false;
		}
	}
	
	//Add the player to a team
	[RPC]
	void ajoutTeam(int senderID)
	{
		networkManager netMan = (networkManager)GameObject.Find("multiScripts").GetComponent("networkManager");
		
		PhotonPlayer target = PhotonPlayer.Find(senderID);
		
		scoreScript scoreMan = GetComponent<scoreScript>();
		
		if(blueCount <= redCount) {
			//photonView.RPC("blueTeam", target, null );
			photonView.RPC("setTeam", target, "Blue");
			blueCount++;
			scoreMan.blueCount++;
		}
		else
		{
			//photonView.RPC("redTeam", target, null );
			photonView.RPC("setTeam", target, "Red");
			redCount++;
			scoreMan.redCount++;
		}
		
	}
	
	//To update player count of clients
	public void updatePlayers()
	{
		if(PhotonNetwork.isMasterClient)
		{
			string playerCounts = blueCount.ToString() + ";" + redCount.ToString();
			PhotonView masterView = PhotonView.Get(this);
			masterView.RPC("sendUpdatedPlayers", PhotonTargets.All, playerCounts);
		}
	}
	
	
	//Set the team of the player
	[RPC]
	void setTeam(string team)
	{
		actualTeam=team;
		
		scoreScript scoreMan = GetComponent<scoreScript>();
		scoreMan.myTeam = team;
	}
	
	//Send the updated player count to other players
	[RPC]
	void sendUpdatedPlayers(string playerNum)
	{
		string[] players = playerNum.Split(';');
		blueCount = int.Parse(players[0]);
		redCount = int.Parse(players[1]);
		Debug.Log("Updated players count");
	}
	
	
	/*-------------------------- GUI PANELS ---------------------*/
	public void updatePlayerCount()
	{
		PhotonView myView = PhotonView.Get(this);
		
		Transform HUDPanel = transform.Find("globalHUD");
		
		if(myView.isMine)
		{
			HUDPanel.gameObject.active = true;
			Transform playerCountP = HUDPanel.transform.Find("PlayerCountPanel");
			Transform playerCountT = playerCountP.transform.GetChild(0);
			Text playerC = playerCountT.GetComponent<Text>();
			
			playerC.text = "Players: " + blueCount.ToString() + " Blue | Red "+ redCount.ToString();
		}
		else
		{
			HUDPanel.gameObject.active = false;
		}
	}
	
	
	
	/*###########
	#   SCORE   #
	###########*/
	
	//Add a kill to a blue player
	/*[RPC]
	void addBlueKill(int killerID)
	{
		
		scoreScript scoreMan = GetComponent<scoreScript>();
		
		int i=0;
		while(i < 4)
		{
			//When we found the case of the killer
			if(scoreMan.playerScoreBlue[i,0] == killerID)
			{
				//We add +1 to his kill counter
				scoreMan.playerScoreBlue[i,1] = scoreMan.playerScoreBlue[i,1] + 1;
			}
			Debug.Log("TEST: " + killerID + " - " + i);
			i++;
		}
	}
	
	//Add a kill to a red player
	[RPC]
	void addRedKill(int killerID)
	{
		
		scoreScript scoreMan = GetComponent<scoreScript>();
		
		int i=0;
		while(i < 4)
		{
			//When we found the case of the killer
			if(scoreMan.playerScoreRed[i,0]==killerID)
			{
				//We add +1 to his kill counter
				scoreMan.playerScoreRed[i,1] = scoreMan.playerScoreRed[i,1] + 1;
			}
			Debug.Log("TEST: " + killerID + " - " + i);
			i++;
		}
	}*/
	
	
}
