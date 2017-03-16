using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fovValueUpdate : MonoBehaviour {

	Text txt;
	private int valFOV; //The player value of the FOV
	
	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
		
		//We get the FOV Value
		valFOV = PlayerPrefs.GetInt("playerFOV");
		txt.text = valFOV.ToString();
		
	}
	
	// Update is called once per frame
	void Update () {
		txt = GetComponent<Text>();
		
		//We get the FOV Value
		valFOV = PlayerPrefs.GetInt("playerFOV");
		txt.text = valFOV.ToString();
	}
}
