using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


/// <summary>
/// Implements teams in a room/game with help of player properties. Access them by PhotonPlayer.GetTeam extension.
/// </summary>
/// <remarks>
/// Teams are defined by enum Team. Change this to get more / different teams.
/// There are no rules when / if you can join a team. You could add this in JoinTeam or something.
/// </remarks>
public class PunTeams : Photon.MonoBehaviour
{
    /// <summary>Enum defining the teams available. First team should be neutral (it's the default value any field of this enum gets).</summary>
    public enum Team : byte {none, red, blue};

    /// <summary>The main list of teams with their player-lists. Automatically kept up to date.</summary>
    /// <remarks>Note that this is static. Can be accessed by PunTeam.PlayersPerTeam. You should not modify this.</remarks>
    public static Dictionary<Team, List<PhotonPlayer>> PlayersPerTeam;
    
    /// <summary>Defines the player custom property name to use for team affinity of "this" player.</summary>
    public const string TeamPlayerProp = "team";

    public const string bScoreProp = "BS";
    public const string rScoreProp = "RS";

    //TEAMFEED SYSTEM
    public const string killFeedProp = "KF";
    private bool isKillFeedEmpty=true;


    #region Events by Unity and Photon

    public void Start()
    {
        PlayersPerTeam = new Dictionary<Team, List<PhotonPlayer>>();
        Array enumVals = Enum.GetValues(typeof (Team));
        foreach (var enumVal in enumVals)
        {
            PlayersPerTeam[(Team)enumVal] = new List<PhotonPlayer>();
        }
		
		this.UpdateTeams();
		
		//We get the playercount
		string players = this.countPlayers();
		string[] playerTab = players.Split(';');
		
		int blueCount = int.Parse(playerTab[0]);
		int redCount = int.Parse(playerTab[1]);
		
		//We get our player
		PhotonView myView = PhotonView.Get(this);
		PhotonPlayer myPlayer = myView.owner;
		
		//We set the team depending on the players
		if(myView.isMine)
		{
			if(blueCount <= redCount)
				TeamExtensions.SetTeam(myPlayer,Team.blue);
			else
				TeamExtensions.SetTeam(myPlayer,Team.red);
			
			Debug.Log("You are on team " + TeamExtensions.GetTeam(myPlayer));
		}
		
        //If we are the masterclient we verify the score and if there are none we set it
        if(PhotonNetwork.isMasterClient)
        {
            if (!TeamExtensions.isTeamScoreSet("Blue"))
            {
                TeamExtensions.SetTeamScore("Blue",0);
            }

            if (!TeamExtensions.isTeamScoreSet("Red"))
            {
                TeamExtensions.SetTeamScore("Red",0);
            }

            if (isKillFeedEmpty)
            {
                TeamExtensions.AddKillFeed(-1,-1);
            }
        }

    }


    /// <summary>Needed to update the team lists when joining a room.</summary>
    /// <remarks>Called by PUN. See enum PhotonNetworkingMessage for an explanation.</remarks>
    public void OnJoinedRoom()
    {
        
        this.UpdateTeams();
    }

    /// <summary>Refreshes the team lists. It could be a non-team related property change, too.</summary>
    /// <remarks>Called by PUN. See enum PhotonNetworkingMessage for an explanation.</remarks>
    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        this.UpdateTeams();
    }
    
    #endregion
    

    public void UpdateTeams()
    {
        Array enumVals = Enum.GetValues(typeof(Team));
        foreach (var enumVal in enumVals)
        {
            PlayersPerTeam[(Team)enumVal].Clear();
        }

        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            PhotonPlayer player = PhotonNetwork.playerList[i];
            Team playerTeam = player.GetTeam();
            PlayersPerTeam[playerTeam].Add(player);
        }
    }
	
	public string countPlayers()
	{
		int blues = 0;
		int reds = 0;
		
		foreach(PhotonPlayer player in PhotonNetwork.playerList)
		{
            Team playerTeam = player.GetTeam();
			if(playerTeam==Team.blue)
				blues++;
			else
				if(playerTeam==Team.red)
					reds++;
		}
		
		string total = blues.ToString() + ";" + reds.ToString();
		return total;
	}
}

/// <summary>Extension used for PunTeams and PhotonPlayer class. Wraps access to the player's custom property.</summary>
static class TeamExtensions
{
    /// <summary>Extension for PhotonPlayer class to wrap up access to the player's custom property.</summary>
    /// <returns>PunTeam.Team.none if no team was found (yet).</returns>
    public static PunTeams.Team GetTeam(this PhotonPlayer player)
    {
        object teamId;
        if (player.customProperties.TryGetValue(PunTeams.TeamPlayerProp, out teamId))
        {
            return (PunTeams.Team)teamId;
        }

        return PunTeams.Team.none;
    }
	
	//Return "Blue" or "Red"
	public static string getStringTeam(this PhotonPlayer player)
	{
		PunTeams.Team playerTeam = player.GetTeam();
		if(playerTeam == PunTeams.Team.blue)
			return "Blue";
		else
			return "Red";
	}

    /// <summary>Switch that player's team to the one you assign.</summary>
    /// <remarks>Internally checks if this player is in that team already or not. Only team switches are actually sent.</remarks>
    /// <param name="player"></param>
    /// <param name="team"></param>
    public static void SetTeam(this PhotonPlayer player, PunTeams.Team team)
    {
        if (!PhotonNetwork.connectedAndReady)
        {
            Debug.LogWarning("SetTeam was called in state: " + PhotonNetwork.connectionStateDetailed + ". Not connectedAndReady.");
        }

        PunTeams.Team currentTeam = PhotonNetwork.player.GetTeam();
        if (currentTeam != team)
        {
            PhotonNetwork.player.SetCustomProperties(new Hashtable() {{PunTeams.TeamPlayerProp, (byte) team}});
        }
    }

    // Set a score to a team
    public static void SetTeamScore(string teamToSet, int scoreToSet)
    {
        if(teamToSet == "Blue")
        {
            Hashtable blueScores = new Hashtable() {{PunTeams.bScoreProp, scoreToSet}};
            PhotonNetwork.room.SetCustomProperties(blueScores);
        }
        else
        {
            Hashtable redScores = new Hashtable() {{PunTeams.rScoreProp, scoreToSet}};
            PhotonNetwork.room.SetCustomProperties(redScores);
        }
    }

    // Verify if the score is set for a team
    public static bool isTeamScoreSet(string teamToTest)
    {
        object teamId;

        if(teamToTest == "Blue")
        {
            
            if (PhotonNetwork.room.customProperties.TryGetValue(PunTeams.bScoreProp, out teamId))
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            if (PhotonNetwork.room.customProperties.TryGetValue(PunTeams.rScoreProp, out teamId))
            {
                return true;
            }
            else
                return false;
        }
    }

    // Get the score of a team
    public static int GetTeamScore(string teamToGet)
    {
        object teamId;

        if(teamToGet == "Blue")
        {
            
            if (PhotonNetwork.room.customProperties.TryGetValue(PunTeams.bScoreProp, out teamId))
            {
                //Debug.Log("Got blue score: " + (int)teamId);
                return (int)teamId;
            }
            else
                //Debug.Log("Error retrieving score");
                return 0;
        }
        else
        {
            if (PhotonNetwork.room.customProperties.TryGetValue(PunTeams.rScoreProp, out teamId))
            {
                //Debug.Log("Got red score: " + (int)teamId);
                return (int)teamId;
            }
            else
                //Debug.Log("Error retrieving score");
                return 0;
        }
    }

    public static void AddTeamScore(string teamToAdd)
    {
        int current = GetTeamScore(teamToAdd);
        int newScore = current+1;
        SetTeamScore(teamToAdd,newScore);
    }


    //KILLFEED
    public static void AddKillFeed(int killerID, int deadID)
    {
        if (!PhotonNetwork.connectedAndReady)
        {
            Debug.LogWarning("AddKillFeed was called in state: " + PhotonNetwork.connectionStateDetailed + ". Not connectedAndReady.");
        }

        string killFeedID = killerID.ToString() + ";" + deadID.ToString();

        PhotonNetwork.room.SetCustomProperties(new Hashtable() {{PunTeams.killFeedProp, killFeedID}});
    }

    public static string GetKillFeed()
    {
        object killFeedScore;

        if (PhotonNetwork.room.customProperties.TryGetValue(PunTeams.killFeedProp, out killFeedScore))
        {
            return (string)killFeedScore;
        }
        else
            return "-1;-1";
    }
}