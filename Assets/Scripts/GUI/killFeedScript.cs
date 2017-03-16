using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class killFeedScript : MonoBehaviour {

	//Getting player IDs
	private string lastKills;
	private string[] lastKillFeed;
	private int lastKiller;
	private int lastKilled;

	//The cache
	private int actualKiller;
	private int actualDead;

	//Their names
	private string killerTag;
	private string deadTag;

	//Their photonplayers
	private PhotonPlayer killerPlayer;
	private PhotonPlayer deadPlayer;

	//Transforms
	private Transform textTransform;
	private Text killFeedText;

	//The counter for display
	private bool isDisplaying;
	private float counter;

	// Use this for initialization
	void Start () {
		textTransform = transform.GetChild(0);
		killFeedText = textTransform.GetComponent<Text>();

		actualDead=-1;
		actualKiller=-1;
	}
	
	// Update is called once per frame
	void Update () {

		lastKills = TeamExtensions.GetKillFeed();
		lastKillFeed = lastKills.Split(';');
		lastKiller = int.Parse(lastKillFeed[0]);
		lastKilled = int.Parse(lastKillFeed[1]);

		if(lastKiller!=-1 && lastKilled!=-1)
		{
			if(lastKiller!=actualKiller || lastKilled!=actualDead)
			{
				killerPlayer = PhotonPlayer.Find(lastKiller);
				deadPlayer = PhotonPlayer.Find(lastKilled);

				killerTag = killerPlayer.name;
				deadTag = deadPlayer.name;

				displayNewKill(killerTag, deadTag);

				actualDead=lastKilled;
				actualKiller=lastKiller;
			}
		}

		if(isDisplaying)
		{
			if(counter >= 0f)
			{
				counter-=Time.deltaTime;
			}
			else
			{
				isDisplaying=false;
				killFeedText.gameObject.active=false;
			}
		}
	}



	private void displayNewKill(string killerName, string deadName)
	{
		killFeedText.gameObject.active = true;
		isDisplaying=true;
		counter=4f;
		killFeedText.text = "<b><i>" + killerName + "</i></b> powned the shit out of <b><i>" + deadName + "</i></b> !!";
	}
}
