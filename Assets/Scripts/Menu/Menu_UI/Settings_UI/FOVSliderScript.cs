using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FOVSliderScript : MonoBehaviour {

	private Slider slider;
	public int valFOV;
	
	
	// Use this for initialization
	void Start () {
		
		//We get the player FOV at the initialization
		slider = gameObject.GetComponent<Slider>();
		
		valFOV = PlayerPrefs.GetInt("playerFOV");
		if(valFOV!=null)
		{
			slider.value = valFOV;
		}
		//If the player hasnt set their FOV we put the default
		else {
			slider.value = 90;
			valFOV = 90;
		}
	}
	
	// Update is called once per frame
	void Update () {
		valFOV = Mathf.RoundToInt(slider.value);
		PlayerPrefs.SetInt("playerFOV",valFOV);
	}
}
