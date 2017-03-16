using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class menuNavScript : Photon.MonoBehaviour {
	
	public int roomToLoad; //Then umber of the room to load for each button
	public bool onlineRoom; //If we need to disconnect from the network

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)) //If the player press escape
		{
			if(onlineRoom)
			{
				disconnectToRoom();
			}
			else
			{
				goBackToRoom();
			}
		}
	}
	
	public void goBackToRoom()
	{
		Application.LoadLevel(roomToLoad);
	}
	
	public void disconnectToRoom()
	{
		PhotonNetwork.Disconnect();
		Application.LoadLevel(roomToLoad);
	}
}
