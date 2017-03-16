using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PlayerDeaths : MonoBehaviour
{
    public const string PlayerDeathProp = "death";
	
	void Update()
	{
		PhotonView myView = PhotonView.Get(this);
		PhotonPlayer myPlayer = myView.owner;
		//Debug.Log("Score: " + ScoreExtensions.GetScore(myPlayer).ToString());
	}
}


static class DeathExtensions
{
    public static void SetDeath(this PhotonPlayer player, int newDeath)
    {
        Hashtable death = new Hashtable();  // using PUN's implementation of Hashtable
        death[PlayerDeaths.PlayerDeathProp] = newDeath;

        player.SetCustomProperties(death);  // this locally sets the score and will sync it in-game asap.
    }

    public static void AddDeath(this PhotonPlayer player, int deathToAddToCurrent)
    {
        int current = player.GetDeath();
        current = current + deathToAddToCurrent;

        Hashtable death = new Hashtable();  // using PUN's implementation of Hashtable
        death[PlayerDeaths.PlayerDeathProp] = current;

        player.SetCustomProperties(death);  // this locally sets the score and will sync it in-game asap.
    }

    public static int GetDeath(this PhotonPlayer player)
    {
        object teamId;
        if (player.customProperties.TryGetValue(PlayerDeaths.PlayerDeathProp, out teamId))
        {
            return (int)teamId;
        }

        return 0;
    }
}