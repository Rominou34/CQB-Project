using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gamertagFieldScript : MonoBehaviour {

	Text txt;
	public string gamertag;
     
    void Start()
    {
		gamertag = PlayerPrefs.GetString("Gamertag");
		txt = GetComponent<Text>();
		
		//We put the player gamertag
		if(gamertag!=null)
			txt.text = gamertag;
    }
     
    void Update()
	{
		txt = GetComponent<Text>();
		PlayerPrefs.SetString("Gamertag", txt.text);
	}
}
