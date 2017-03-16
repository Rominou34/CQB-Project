using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
 
public class networkManager : Photon.MonoBehaviour
{
	/*--- CONTROL VARIABLE ---*/
	public bool inMatch = false;

	//HASHTABLE
	public const string roomStatusProp = "status";
	//public enum RoomStatus : byte {none, lobby, game, end};
	
	
	/*#####################
	#   LOBBY VARIABLES   #
	#####################*/
	
	
	//public int[] mapVotesCount = new int[] {0,0,0,0};
	public string[] maps = new string[] {"Test","Test","Test","Test"};
	
	public string chosenMode="";
	public string chosenMap;
	
	public int chosenMapNum=-1;
	
	public string gameMode;
	public string mapName;
	
	//Les props de la room: 0:La map  1:Le mode
	public string roomName;
	
	

	/*########################
	#   IN-MATCH VARIABLES   #
	########################*/
	
	private bool roomConnected=false;
	private bool weaponChosen=false;
	
	public GameObject blueSpawn;
	public GameObject spawnBlue1;
	public GameObject spawnBlue2;
	
	public Material mat_soldat_bleu;
	public Material mat_soldat_rouge;
	
	private string hunt_ShotG="Hunting_Shotgun";
	
	public static int blueCount=0;
	public static int redCount=0;
	
	private bool isSpawned=false;
	public float timeCounter=0f;
	
	
	/*-------------------------------------------------------------------------------------*/
	
	
	/*###########
	#   START   #
	###########*/
	
	// Use this for initialization
    void Start()
    {
		PhotonNetwork.playerName = PlayerPrefs.GetString("Gamertag"); /*---------------------------------------------- BUG WITH THE PLAYER NAME HERE ------------------------------------------*/
		/*int playerNumb = Random.Range(0, 10000);
		PhotonNetwork.playerName = "player" + playerNumb.ToString();*/
		
		//PhotonNetwork.ConnectUsingSettings("0.1");
		//PhotonNetwork.networkingPeer.DebugOut = ExitGames.Client.Photon.DebugLevel.ALL;
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;

		if(PhotonNetwork.isMasterClient)
		{
			setStatus("Lobby");
		}
		else
		{
			if(getStatus()=="InGame")
			{
				inMatch=true;
			}
		}

    }
	
	void Awake () {
		DontDestroyOnLoad(this);
		PhotonNetwork.automaticallySyncScene = true;
	}
 
    void Update() {
		PhotonView photonView = PhotonView.Get(this);
		
		//When launching the match, if we haven't spawn yet we do
		if(inMatch)
		{
			timeCounter+=Time.deltaTime;
				
			mat_soldat_bleu = Resources.Load("texture_soldat_bleu", typeof(Material)) as Material;
			mat_soldat_rouge = Resources.Load("texture_soldat_rouge", typeof(Material)) as Material;
			
			if(isSpawned == false && timeCounter >= 5.0f)
			{
				GameObject playerCre = PhotonNetwork.Instantiate("playerCreator", new Vector3(0,0,0), Quaternion.identity, 0);
				playerSpawn playSpawn = playerCre.GetComponent<playerSpawn>();
				playSpawn.enabled = true;
				
				isSpawned=true;
			}
		}
		
		/*---------------- IF WE'RE IN THE LOBBY WE ACTIVATE THE LOBBY CHAT -------------*/
		roomLobbyChat lobbyChat = GetComponent<roomLobbyChat>();
		if(inMatch == false) {
			if(photonView.isMine)
			{
				lobbyChat.enabled = true;
			}
			
			/*-------------------- GUI ---------------------*/
			
			//When the mode and map are chosen we can start
			if (chosenMode!="" && chosenMapNum!=-1)
			{
				//We get the start button
				GameObject UIPanel = GameObject.Find("roomLobbyUI");
				Transform startButton = UIPanel.transform.Find("StartButton");
				Button startBut = startButton.gameObject.GetComponent<Button>();
				startBut.interactable = true;
			}
		}
		else {
			lobbyChat.enabled = false;
		}
		
		//When a new player joins we tell him we're in a match
		/*if(PhotonNetwork.playerList.Length != (redCount + blueCount) && PhotonNetwork.isMasterClient)
		{
			if(inMatch)
				photonView.RPC("isInMatch", PhotonTargets.Others, null);
		}*/
	}
	
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			//stream.SendNext(blueCount);
			//stream.SendNext(redCount);
		}
		else
		{
			//blueCount = (int)stream.ReceiveNext();
			//redCount = (int)stream.ReceiveNext();
		}
	}
	
	/*----------------------------------------------------------------------------*/
	
	/*#########
	#   GUI   #
	#########*/
	
	void OnGUI()
    {
		/*--- COMPTEUR ---*/
		if(inMatch) {
			if(timeCounter<5.0f)
			{
				GUI.Label(new Rect(Screen.width/2-60,Screen.height/2-10,250,50),"Starting in:" + (5.0f-timeCounter).ToString());
			}
		}
		/*--- VERIF IF INMATCH ---*/
		
		if(inMatch) /*--------------- IN MATCH -----------------*/
		{
			GUI.Label(new Rect(50,300,250,50),"In Match");
			
			/*############################
			#   IN MATCH GUI ( DEBUG )   #
			############################*/
		
			GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
			GUI.Label(new Rect(200,250,250,50),"Players number: " + PhotonNetwork.playerList.Length);
			if(PhotonNetwork.isMasterClient) {
				GUI.Label(new Rect(200,300,250,50),"Masterclient");
			}
			else {
				GUI.Label(new Rect(200,300,250,50),"Client");
			}
		}
		
    }
	
	
	/*-------------------------- LOBBY MANAGING -------------------------------------------*/
	
	/*######################
	#   VOTE POUR LA MAP   #
	######################*/
	void voteMap() {
		//Pour chaque map on affiche un bouton
		for (int i=0; i < maps.Length; i++)
		{
			//En cliquant sur le bouton on vote pour la map
			if (GUI.Button(new Rect(Screen.width/2-50, 75+(45*i), 100, 40), maps[i]))
			{
				if(photonView.isMine)
				{
				//photonView.RPC("addVote", PhotonTargets.MasterClient, i);
				//addVote(i);
				//mapVoted=true;
				chosenMap=maps[i];
				/*--------------------Change value if map number in the build change ------------*/
				chosenMapNum=7;
				}
			}
		}
	}
	
	
	
	/*#######################
	#   VOTE POUR LE MODE   #
	#######################*/
	void voteMode() {
		//Un bouton par gameMode
		if (GUI.Button(new Rect(Screen.width/2-200, 275, 120, 50), "Team Deathmatch"))
		{
			if(photonView.isMine)
			{
				//photonView.RPC("addModeVote", PhotonTargets.MasterClient, 0);
				//modeVoted=true;
				chosenMode="TDM";
			}
		}
		
		if (GUI.Button(new Rect(Screen.width/2-60, 275, 120, 50), "Deathmatch"))
		{
			if(photonView.isMine)
			{
				//photonView.RPC("addModeVote", PhotonTargets.MasterClient, 1);
				//modeVoted=true;
				chosenMode="DM";
			}
		}
		
		if (GUI.Button(new Rect(Screen.width/2+80, 275, 120, 50), "Domination"))
		{
			if(photonView.isMine)
			{
				//photonView.RPC("addModeVote", PhotonTargets.MasterClient, 2);
				//modeVoted=true;
				chosenMode="Domi";
			}
		}
	}
	
	
	/*###################
	#   RPC FUNCTIONS   #
	###################*/
	
	[RPC]
	void startTest() {
		inMatch=true;
		Application.LoadLevel("Test");
	}
	
	[RPC]
	void startMatch(int mapNum) {
		inMatch=true;
		Application.LoadLevel(mapNum);
	}
	
	[RPC]
	void isInMatch() {
		inMatch=true;
	}
	
	[RPC]
	void isPlaying(int senderID)
	{
		if(inMatch)
		{
			PhotonPlayer target = PhotonPlayer.Find(senderID);
			photonView.RPC("isInMatch", target, null);
		}
	}
	

	/*###################
	#   GUI FUNCTIONS   #
	###################*/
	
	public void voteMap(int mapNum) //The masterclient chooses the map
	{
		if(PhotonNetwork.isMasterClient)
			chosenMapNum = mapNum;
	}
	
	
	public void voteMode(string modeName) //The masterclient chooses the mode
	{
		if(PhotonNetwork.isMasterClient)
			chosenMode = modeName;  //"DM" , "TDM", "Domi"
	}
	
	
	public void launchMatch() //The masterclient launches the mode
	{
		if(PhotonNetwork.isMasterClient)
		{	
			gameMode=chosenMode;
			mapName=chosenMap;
			setStatus("InGame");	

			PhotonNetwork.LoadLevel(chosenMapNum);
		}

		PhotonNetwork.isMessageQueueRunning=false;
	}

	//To know if we're in a match or not
	/*public static RoomStatus getStatus(Room roomToGet)
	{
		object status;

        if (roomToGet.customProperties.TryGetValue(roomStatusProp, out status))
        {
        	return (RoomStatus)status;
        }
        else
        {
        	return RoomStatus.none;
        }
	}*/

	public static string getStatus()
	{
		object status;

        if (PhotonNetwork.room.customProperties.TryGetValue(roomStatusProp, out status))
        {
        	return (string)status;
        }
        else
        {
        	return "none";
        }
	}

	public static void setStatus(string statusToSet)
	{
		Hashtable roomStatus = new Hashtable() {{roomStatusProp, statusToSet}};
        PhotonNetwork.room.SetCustomProperties(roomStatus);
	}
	
	
	/*-------------------------- PHOTON FUNCTIONS ----------------*/

	void OnLevelWasLoaded(int level)
	{
		PhotonNetwork.isMessageQueueRunning=true;
		if(level>=7) //If we loaded a map
		{
			if(PhotonNetwork.isMasterClient)
			{
				setStatus("InGame");
			}
		}
	}

	public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(roomStatusProp))
		{
			string newStatus = (string)propertiesThatChanged[roomStatusProp];
			if (newStatus=="InGame")
			{
				inMatch=true;
			}
			else {
				if (newStatus=="Lobby")
				{
					inMatch=false;
				}
			}
		}
	}

	void OnPhotonPlayerConnected(PhotonPlayer connectedPlayer)
	{

	}
	
	
	
}	
	
/*-------------------------------------------------------------------------------------*/

	
	/*[RPC]
	void playerSpawn1(string team) {
		
		GameObject player = PhotonNetwork.Instantiate("soldierPrefab", spawnBlue1.transform.position, Quaternion.identity, 0);
		Debug.Log(player);
			
		//var weaponSelec = Instantiate(weapon, player.transform.position, player.transform.rotation) as Transform;
		
		GameObject weapSlot = player.gameObject.transform.Find("Armature/Dos/Haut_Dos/Epaule_D/Bras_D/Avant_Bras_D/Main_D/Paume_D/Majeur_1_D/weaponSlot").gameObject;
		Debug.Log(weapSlot);
		
		GameObject weaponSelec = PhotonNetwork.Instantiate("Hunting_Shotgun", weapSlot.transform.position, Quaternion.identity, 0);
		weaponSelec.transform.parent = weapSlot.transform;
		
		/*weaponSelec.transform.parent = player.transform;
		weaponSelec.transform.localPosition = new Vector3(0.8f, 3.5f, 0.2f);*/
		//var weaponAim = weaponSelec.gameObject.AddComponent<weaponAim>();
		
		//var weaponScript = weapon.GetComponent<weaponName>();
		/*var playerScript = player.GetComponent<playerCreation>();
		playerScript.actualWeapon = "Hunting_Shotgun";
		playerScript.playerTeam = team;
		
		playerNetwork playerNet = player.GetComponent<playerNetwork>();
		playerNet.enabled = true;
		playerNet.playerTeam = team;
		Debug.Log("Spawned");
		
		
		//GameObject soldier = player.gameObject.transform.Find("Soldier").gameObject;
	}*/