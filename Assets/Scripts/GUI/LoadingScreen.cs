using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	private Transform panelT;

	static bool loading_on = true;
 
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		panelT = transform.GetChild(0);

		//Once the level is loaded we destroy the loading screen
		if(!Application.isLoadingLevel)
			panelT.gameObject.active = false;
		else
			panelT.gameObject.active=true;
	}
}
