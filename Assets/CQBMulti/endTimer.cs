using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class endTimer : Photon.MonoBehaviour {

	private const string TimeToEndProp = "st";
    private double timeToEnd = 0f;
    public double SecondsBeforeEnd; 
	
	// Use this for initialization
	void Start () {
		if(PhotonNetwork.isMasterClient)
		{
			SecondsBeforeEnd = 600.0f;
			setEndTime();
		}
		else
		{
			timeToEnd = getEndTime();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!PhotonNetwork.isMasterClient)
			if(timeToEnd < 0.01f)
				timeToEnd = getEndTime();
	}
	
	void setEndTime() {
		this.timeToEnd = PhotonNetwork.time + SecondsBeforeEnd;
 
		Hashtable timeProps = new Hashtable() {{TimeToEndProp, this.timeToEnd}};
		PhotonNetwork.room.SetCustomProperties(timeProps);
	}
	
	public static double getEndTime()
	{
		object timer;
		if (PhotonNetwork.room.customProperties.TryGetValue(TimeToEndProp, out timer))
        {
            return (double)timer;
        }
		return 0.0f;
	}
	
	void OnGUI()
	{
		PhotonView myView = PhotonView.Get(this);
		
		if(myView.isMine)
		{
			GUI.Label(new Rect(500,250,400,50),"Match is finished in: " + (float)(this.timeToEnd - PhotonNetwork.time));
		}
	}
}
