using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class keyboardSettingScript : MonoBehaviour {
	
	public string keyboardType;
	public string buttonName;
	Button button;

	// Use this for initialization
	void Start () {
		keyboardType = PlayerPrefs.GetString("KeyboardType");
		
		//At the start we deactivate the already selected keyboard type
		if(keyboardType==buttonName)
		{
			button = gameObject.GetComponent<Button>();
			button.interactable = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		keyboardType = PlayerPrefs.GetString("KeyboardType");
	}
	
	public void setKeyboard()
	{
		PlayerPrefs.SetString("KeyboardType",buttonName);
	}
}
