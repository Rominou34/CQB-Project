using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class onlineMenuScript : Photon.MonoBehaviour {

	public string playerTag;
	
	//Counting
	private int i;
	private int j;
	private int x;
	private int y;
	
	//Useful variables
	private int roomNumber;
	private string myRoomName;
	private bool joinedLobby=false;
	private bool preciseSelected=false;
	private string tempPendingString;
	private string pendingString;
	private int lastStrobe;
	
	//GUI
	Text coStatus;
	private string displayConnection;

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("0.2");
		x = 0;
		
		//We get the connection status object
		Transform statusCo = transform.Find("ConnectionStatus");
		coStatus = statusCo.GetComponent<Text>();
		coStatus.text = "Joining the online lobby";
	}
	
	// Update is called once per frame
	void Update () {
		playerTag = PlayerPrefs.GetString("Gamertag");
		
		connectionStatus();
	}
	
	/*void OnGUI() {
		if(joinedLobby)
		{
			GUI.Label(new Rect(Screen.width/2-75, 10, 200, 20), "Joined the online Lobby");
			GUI.Label(new Rect(10,10,250,50),"Rooms number: " + PhotonNetwork.GetRoomList().Length);
		}
		else
		{
			
		}
		playerTag = PlayerPrefs.GetString("Gamertag");
		
		/*###############
		#   ROOM LIST   #
		###############*/
		/*
		//We take the room list
		RoomInfo[] rooms = PhotonNetwork.GetRoomList();
		
		//We print the buttons for each room
		for (int i = 0; i < rooms.Length; i++)
		{
			if (GUI.Button(new Rect(Screen.width/2+150, 50+(70*i), 200, 50), rooms[i].name + " " + rooms[i].playerCount + "/" + rooms[i].maxPlayers))
			{
				PhotonNetwork.JoinRoom(rooms[i].name);
			}
		}
		
		
		/*########################
		#   CREATE ROOM BUTTON   #
		########################*/
		/*if (GUI.Button(new Rect(Screen.width/2-100, 100, 200, 50), "Create a Room"))
		{
			myRoomName = playerTag+"'s Room";
			
			PhotonNetwork.CreateRoom(myRoomName, true, true, 16);
		}
		
		
		/*########################
		#   RANDOM ROOM BUTTON   #
		########################*/
		/*if (GUI.Button(new Rect(Screen.width/2-100, 170, 200, 50), "Join a Random Room"))
		{
			PhotonNetwork.JoinRandomRoom();
		}
		
		
		/*######################
		#   JOIN ROOM BUTTON   #
		######################*/
		/*if (GUI.Button(new Rect(Screen.width/2-100, 240, 200, 50), "Join a precise Room"))
		{
			if(preciseSelected==false)
			{
				preciseSelected=true;
			}
		}
		
		if(preciseSelected)
		{
			joinPreciseRoom();
		}*/
	//}
	
	//--------------------------- STROBE --------------------------
	void connectionStatus()
	{
		Transform statusCo = transform.Find("ConnectionStatus");
		coStatus = statusCo.GetComponent<Text>();
			
			
		// the code below is for the pending connection label to strobe -=m0dem=-
		if(!joinedLobby)
		{
			if (lastStrobe > 6) {
				if (x == 4) {
					x = 0;
				}
				for (y = 0; y < x; y++) {
					tempPendingString += ".";
				}
				displayConnection = "Connecting to the online Lobby" + tempPendingString;
				pendingString = tempPendingString;
				tempPendingString = "";
				x++;
				lastStrobe = 0;
			}
			else {
				displayConnection = "Connecting to the online Lobby" + pendingString;
			}
			lastStrobe++;
			
			coStatus.text = displayConnection;
		}
		else {
			
			coStatus.text = "Joined online lobby";
		}
	}
	
	/*########################
	#   CREATE ROOM BUTTON   #
	########################*/
	public void createRoom()
	{
		if(joinedLobby)
		{	
			myRoomName = playerTag+"'s Room";
			
			PhotonNetwork.CreateRoom(myRoomName, true, true, 8, new ExitGames.Client.Photon.Hashtable() { { "status", "lobby" } }, new string[] { "status" });
		}
	}
	
	
	/*########################
	#   RANDOM ROOM BUTTON   #
	########################*/
	public void joinRandomRoom()
	{
		if(joinedLobby)
		{	
			PhotonNetwork.JoinRandomRoom();
		}
	}

	
	/*######################
	#   JOIN ROOM BUTTON   #
	######################*/
	public void joinRoom()
	{
		if(joinedLobby)
		{	
			if(preciseSelected==false)
			{
				preciseSelected=true;
			}
		}
	}
	
	
	
	/*------------------------------------- BUTTONS IN ROOM LIST -------------------------*/
	
	/*#################
	#   JOIN ROOM NO  #
	#################*/
	public void joinRoomNumber(int i)
	{
		if(joinedLobby)
		{
			RoomInfo[] rooms = PhotonNetwork.GetRoomList();
			PhotonNetwork.JoinRoom(rooms[i].name);
		}
	}
	
	/*-----------------------------------------------------------------------------------*/
	
	void OnJoinedLobby()
	{
		joinedLobby=true;
	}
	
	void OnJoinedRoom()
	{
		Application.LoadLevel(6); //We load the room lobby
	}
	
	/*void joinPreciseRoom() {
		GUI.Label(new Rect(Screen.width/2-125, 300, 250, 25), "Enter the name of the Room");
		
		string preciseRoomName="";
		preciseRoomName = GUI.TextArea(new Rect(Screen.width/2-100, 330, 200, 20), preciseRoomName, 200);
		
		if (GUI.Button(new Rect(Screen.width/2-50, 360, 100, 25), "Join"))
		{
			PhotonNetwork.JoinRoom(preciseRoomName);
		}
	}*/
}
