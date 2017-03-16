using UnityEngine;
using System.Collections;

public class weaponChoiceGUI : MonoBehaviour {

	public bool start=true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			start=true;
		}
	}
	
	void OnGUI(){
		if(start==false) {
			GUI.Label(new Rect(400,400,400,50),"Press 1 to choose the shotgun.");
		}
	}
}
