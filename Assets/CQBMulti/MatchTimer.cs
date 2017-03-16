using System.IO;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;
 
public class MatchTimer : PunBehaviour
{
    private const string TimeToEndProp = "st";
    private double timeToEnd = 0f;
    public double SecondsBeforeEnd;   // set in inspector
 
	void Start ()
	{
		if(PhotonNetwork.isMasterClient)
		{
			SecondsBeforeEnd = 300.0f;
			setEndTime();
		}
	}
	
    public bool IsItEndYet
    {
        get { return IsTimeToEndKnown && PhotonNetwork.time > this.timeToEnd; }
    }
 
    public bool IsTimeToEndKnown
    {
        get { return this.timeToEnd > 0.001f; }
    }
 
    public double SecondsUntilItsEnd
    {
        get
        {
            if (this.IsTimeToEndKnown)
            {
                double delta = this.timeToEnd - PhotonNetwork.time;
                return (delta > 0.0f) ? delta : 0.0f;
            }
            else
            {
                return 0.0f;
            }
        }
    }
	
	public void setEndTime()
	{
		this.timeToEnd = PhotonNetwork.time + SecondsBeforeEnd;
 
		Hashtable timeProps = new Hashtable() {{TimeToEndProp, this.timeToEnd}};
		PhotonNetwork.room.SetCustomProperties(timeProps);
	}
 
    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            // the master client checks if a start time is set. we check a min value
            /*if (!this.IsTimeToEndKnown && PhotonNetwork.time > 0.0001f)
            {
                // no startTime set for room. calculate and set it as property of this room
                this.timeToEnd = PhotonNetwork.time + SecondsBeforeEnd;
 
                Hashtable timeProps = new Hashtable() {{TimeToEndProp, this.timeToEnd}};
                PhotonNetwork.room.SetCustomProperties(timeProps);
            }*/
        }
    }
 
    public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
    {
        //if(!PhotonNetwork.isMasterClient)
		//{
			if (propertiesThatChanged.ContainsKey(TimeToEndProp))
			{
				this.timeToEnd = (double) propertiesThatChanged[TimeToEndProp];
				Debug.Log("Got EndTime: " + this.timeToEnd + " is it time yet?! " + this.IsItEndYet);
			}
		//}
    }
 
    void OnGUI()
    {
        PhotonView myView = PhotonView.Get(this);
		
		if(myView.isMine)
		{
			GUI.Label(new Rect(500,150,400,50),"Is it time yet: " + this.IsItEndYet);
			GUI.Label(new Rect(500,200,400,50),"Match is finished in: " + (float)this.SecondsUntilItsEnd);
		}
	}
}