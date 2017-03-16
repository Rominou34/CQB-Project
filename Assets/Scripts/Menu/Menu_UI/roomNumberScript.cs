using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class roomNumberScript : Photon.MonoBehaviour {

	Text txt;
	public int roomNum;
	public int pageNum;
	private bool joinedLobby=false;
	
	// Use this for initialization
	void Start () {
		//pageNum=0;
		
		if(joinedLobby)
		{
			//We get the page number
			pageManagingScript pageMan = transform.parent.gameObject.GetComponent<pageManagingScript>();
			pageNum = pageMan.pageNum;
			
			
			//We get the room list
			RoomInfo[] rooms = PhotonNetwork.GetRoomList();
			if(rooms[roomNum+pageNum]!=null)
			{
				foreach (Transform child in transform)
				{
					//We display the room if there's one
					txt = child.gameObject.GetComponent<Text>();
					string roomName = rooms[roomNum + 5*pageNum].name;
					string playCount = rooms[roomNum + 5*pageNum].playerCount + "/" + rooms[roomNum + 5*pageNum].maxPlayers;
					ExitGames.Client.Photon.Hashtable roomProps = rooms[roomNum + 5*pageNum].customProperties;
					//txt.text= rooms[roomNum + 5*pageNum].name + " ( " + rooms[roomNum + 5*pageNum].playerCount + "/" + rooms[roomNum + 5*pageNum].maxPlayers;
					txt.text = roomName + " ( " + playCount + " )\n" + roomProps["status"];
				}
			}
			else {
				//No room, no button
				this.gameObject.active = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(joinedLobby)
		{
			//We get the page number
			pageManagingScript pageMan = transform.parent.gameObject.GetComponent<pageManagingScript>();
			pageNum = pageMan.pageNum;
			
			
			//We get the room list
			RoomInfo[] rooms = PhotonNetwork.GetRoomList();
			if((roomNum + 5*pageNum) < rooms.Length)
			{
				//We change the text to the name of the room
				foreach (Transform child in transform)
				{
					txt = child.gameObject.GetComponent<Text>();
					string roomName = rooms[roomNum + 5*pageNum].name;
					string playCount = rooms[roomNum + 5*pageNum].playerCount + "/" + rooms[roomNum + 5*pageNum].maxPlayers;
					ExitGames.Client.Photon.Hashtable roomProps = rooms[roomNum + 5*pageNum].customProperties;
					//txt.text= rooms[roomNum + 5*pageNum].name + " ( " + rooms[roomNum + 5*pageNum].playerCount + "/" + rooms[roomNum + 5*pageNum].maxPlayers;
					txt.text = roomName + " ( " + playCount + " )\n" + roomProps["status"];
				}
			}
			else {
				//No room, no button
				this.gameObject.active = false;
			}
		}
	}
	
	void OnJoinedLobby()
	{
		joinedLobby=true;
	}
	
	public void joinRoomNumber(int i)
	{
		if(joinedLobby)
		{
			RoomInfo[] rooms = PhotonNetwork.GetRoomList();
			PhotonNetwork.JoinRoom(rooms[roomNum + 5*pageNum].name);
		}
	}
}
