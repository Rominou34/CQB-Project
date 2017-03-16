using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scoreUIScript : Photon.MonoBehaviour {
	
	Text txt;
	public int blueScore;
	public int redScore;
	public double endMoment;
	public double endChrono;
	public int endTime;

    private Transform timerPanel;
    private Transform bluePanel;
    private Transform redPanel;

    private Transform timerObj;
    private Transform blueObj;
    private Transform redObj;

    private Text blueText;
    private Text redText;
    private Text timerText;

	// Use this for initialization
	void Start () {
		/*txt = gameObject.GetComponent<Text>();
		
        blueScore = scoreScript.blueScore;
		redScore = scoreScript.redScore;
		
		txt.text="Blue - " + blueScore + " | " + redScore + " -  Red";*/
		endMoment = endTimer.getEndTime();
	}
	
	// Update is called once per frame
	void Update () {

        //Getting the transforms
        timerPanel = transform.Find("TimerPanel");
        bluePanel = transform.Find("BluePanel");
        redPanel = transform.Find("RedPanel");

        //Getting the text objects
        timerObj = timerPanel.GetChild(0);
        blueObj = bluePanel.GetChild(0);
        redObj = redPanel.GetChild(0);

        //Getting the score texts
        timerText = timerObj.GetComponent<Text>();
        blueText = blueObj.GetComponent<Text>();
        redText = redObj.GetComponent<Text>();

		blueScore = TeamExtensions.GetTeamScore("Blue");
		redScore = TeamExtensions.GetTeamScore("Red");
		
		//We get the timer
		endChrono = endMoment - PhotonNetwork.time;
		endTime = (int)(endChrono);
		
		//Blue score
        blueText.text = blueScore.ToString();
        redText.text = redScore.ToString();
		
		//-------- TIMER
		
		//Minutes
		timerText.text = (endTime/60).ToString() + ":";
		
		//Seconds
		if(endTime%60 < 10)
			timerText.text += "0" + (endTime%60).ToString();
		else
			timerText.text += (endTime%60).ToString();
	}
}
