using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerListTitleScript : Photon.MonoBehaviour {
	
	Text txt;

	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
		txt.text = "PLAYERS (" + PhotonNetwork.playerList.Length + ")";
	}
	
	// Update is called once per frame
	void Update () {
		txt = GetComponent<Text>();
		txt.text = "PLAYERS (" + PhotonNetwork.playerList.Length + ")";
	}
}
