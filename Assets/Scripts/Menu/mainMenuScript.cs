using UnityEngine;
using System.Collections;

public class mainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*########################
	#   ONLINE MODE BUTTON   #
	########################*/
	public void openOnline()
	{
		Application.LoadLevel (5);
		Destroy(gameObject);
	}
	
	/*#########################
	#   OFFLINE MODE BUTTON   #
	#########################*/
	public void openOffline()
	{
		Application.LoadLevel ("OfflineMenu");
		Destroy(gameObject);
	}
		
	/*#####################
	#   SETTINGS BUTTON   #
	#####################*/
	public void openSettings()
	{
		Application.LoadLevel (4);
		Destroy(gameObject);
	}
	
	/*##################
	#   STATS BUTTON   #
	##################*/
	public void openStats()
	{
		Application.LoadLevel (3);
		Destroy(gameObject);
	}
	
	/*###################
	#   PLAYER BUTTON   #
	###################*/
	public void openPlayer()
	{
		Application.LoadLevel (2);
		Destroy(gameObject);
	}
}
