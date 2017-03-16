using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnlineMenuUI : Photon.MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		//PhotonNetwork.ConnectUsingSettings("0.1");
		x = 0;
	}
	
	// Update is called once per frame
	void Update () {
		playerTag = PlayerPrefs.GetString("Gamertag");
	}
	
	/*void OnGUI() {
		if(joinedLobby)
		{
			GUI.Label(new Rect(Screen.width/2-75, 10, 200, 20), "Joined the online Lobby");
			GUI.Label(new Rect(10,10,250,50),"Rooms number: " + PhotonNetwork.GetRoomList().Length);
		}
		else
		{
			// the code below is for the pending connection label to strobe -=m0dem=-
			if (lastStrobe > 30) {
				if (x == 4) {
					x = 0;
				}
				for (y = 0; y < x; y++) {
					tempPendingString += ".";
				}
				GUI.Label(new Rect(Screen.width/2-100, 10, 200, 20), "Connecting to the online Lobby" + tempPendingString);
				pendingString = tempPendingString;
				tempPendingString = "";
				x++;
				lastStrobe = 0;
			} else {
				GUI.Label(new Rect(Screen.width/2-100, 10, 200, 20), "Connecting to the online Lobby" + pendingString);
			}
			lastStrobe++;
		}
		playerTag = PlayerPrefs.GetString("Gamertag");
		
		/*###############
		#   ROOM LIST   #
		###############*/
		
		//We take the room list
		/*RoomInfo[] rooms = PhotonNetwork.GetRoomList();
		
		//We print the buttons for each room
		for (int i = 0; i < rooms.Length; i++)
		{
			if (GUI.Button(new Rect(Screen.width/2+150, 50+(70*i), 200, 50), rooms[i].name + " " + rooms[i].playerCount + "/" + rooms[i].maxPlayers))
			{
				PhotonNetwork.JoinRoom(rooms[i].name);
			}
		}
	}*/
	
	
	/*########################
	#   CREATE ROOM BUTTON   #
	########################*/
	public void createRoom()
	{
		playerTag = PlayerPrefs.GetString("Gamertag");
		myRoomName = playerTag+"s Room";
			
		PhotonNetwork.CreateRoom(myRoomName, true, true, 16);
	}
	
	
	/*########################
	#   RANDOM ROOM BUTTON   #
	########################*/
	public void joinRandomRoom()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	
	/*######################
	#   JOIN ROOM BUTTON   #
	######################*/
	public void joinRoom()
	{
		if(preciseSelected==false)
		{
			preciseSelected=true;
		}
	}
		
		/*if(preciseSelected)
		{
			joinPreciseRoom();
		}*/
	
	void OnJoinedLobby()
	{
		joinedLobby=true;
	}
	
	void OnJoinedRoom()
	{
		Application.LoadLevel("RoomLobby");
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
