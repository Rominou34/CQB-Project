using UnityEngine;
using System.Collections;

public class GameLogic : Photon.MonoBehaviour {

	public static int playerWhoIsIt;
	private static PhotonView ScenePhotonView;
	
	// Use this for initialization
	void Start () {
		ScenePhotonView = this.GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
	void OnJoinedRoom()
	{
		//game logic: if this is the only player, we're "it"
		if (PhotonNetwork.playerList.Length == 1)
		{
			playerWhoIsIt = PhotonNetwork.player.ID;
		}
	Debug.Log("playerWhoIsIt: " + playerWhoIsIt);
	}
	
	
	[RPC]
	void TaggedPlayer(int playerID)
	{
		playerWhoIsIt = playerID;
		Debug.Log("TaggedPlayer: " + playerID);
	}
	
	
	void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerConnected: " + player);
		
		//when new players join, we send "who's it" to let them know
		//only one player will do this: the "master"
		
		if (PhotonNetwork.isMasterClient)
		{
			TagPlayer(playerWhoIsIt);
		}
	}
	
	
	public static void TagPlayer(int playerID)
	{
		Debug.Log("TagPlayer: " + playerID);
		ScenePhotonView.RPC("TaggedPlayer", PhotonTargets.All, playerID);
	}
	
	
	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerDisconnected: " + player);
		
		if (PhotonNetwork.isMasterClient)
		{
			if (player.ID == playerWhoIsIt)
			{
				//if the player who left was "it", the "master" is the new "it"
				TagPlayer(PhotonNetwork.player.ID);
			}
		}
	}
}
