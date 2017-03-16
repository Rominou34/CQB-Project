using UnityEngine;
using System.Collections;

public class matchVarScript : MonoBehaviour {

	public string gameMode;
	public string mapName;
	public bool inMatch;

	// Use this for initialization
	void Start () {
	
	}
	
	void Awake () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
