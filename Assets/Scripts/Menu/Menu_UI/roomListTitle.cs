using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class roomListTitle : MonoBehaviour {

	Text txt;
	
	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>();
		txt.text = "ROOMS (" + PhotonNetwork.GetRoomList().Length + ")";
	}
	
	// Update is called once per frame
	void Update () {
		txt = gameObject.GetComponent<Text>();
		txt.text = "ROOMS (" + PhotonNetwork.GetRoomList().Length + ")";
	}
}
