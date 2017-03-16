using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class basicHUDScript : MonoBehaviour {

	Text FPScount; //The FPS counter
	private float frameLength; //The length of a frame
	
	private int actFPS; //If we activate the FPS counter
	
	private float fpsUpdate; //To update the value every quarter of a second
	
	// Use this for initialization
	void Start () {
		//If the player don't want the FPS counter we deactivate it
		actFPS = PlayerPrefs.GetInt("showFPS");
		
		fpsUpdate = 0f;
		
		if(actFPS != 1)
		{
			Transform FPSc = transform.GetChild(0); //We get the FPS counter object
			FPSc.gameObject.active = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Transform FPSc = transform.GetChild(0); //We get the FPS counter object
		FPScount = FPSc.GetComponent<Text>();
		
		//We change the value based on the FPS
		if(fpsUpdate >= 0.125f) //We update the value every 1/8 second
		{
			FPScount.text = "FPS: " + fpsValue().ToString();
			fpsUpdate = 0f;
		}
		else
		{
			fpsUpdate += Time.deltaTime;
		}
	}
	
	//To calculate the value of the FPS
	private int fpsValue()
	{
		frameLength = Time.deltaTime;
		float fpsVal = 1.0f / frameLength;
		return Mathf.RoundToInt(fpsVal);
	}
}
