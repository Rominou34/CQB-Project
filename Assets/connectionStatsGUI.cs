using UnityEngine;
using System.Collections;

public class connectionStatsGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		Debug.Log ("OnGui" + PhotonNetwork.connectionStateDetailed.ToString ());
        GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
	}
}
