using UnityEngine;
 
public class RandomMatchMaker : Photon.MonoBehaviour
{
    public Transform spawnPoint1;
	public Transform spawnPoint2;
	public Transform hunt_ShotG;
	
	private bool roomConnected=false;
	private bool weaponChosen=false;
	
	// Use this for initialization
    void Start()
    {
		PhotonNetwork.ConnectUsingSettings("0.1");
		PhotonNetwork.networkingPeer.DebugOut = ExitGames.Client.Photon.DebugLevel.ALL;
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		
		hunt_ShotG = Resources.Load("Hunting_Shotgun", typeof(Transform)) as Transform;
    }
 
    
	
	void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		
		if(weaponChosen == false)
		{
			if (GUI.Button(new Rect(250, 0, 200, 50), "Choose the Shotgun"))
			{
				if(roomConnected == true)
				{
					playerSpawn();
					weaponChosen = true;
				}
			}
		}
    }
	
	
	
	void OnJoinedLobby() //Joindre une room random des le lancement du jeu
	{
		PhotonNetwork.JoinRandomRoom();
	}
	
	
	
	void OnPhotonRandomJoinFailed() // Si on arrive pas a rentrer dans une room random
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom("testRoom", true, true, 8);
	}
	
	void OnPhotonCreateRoomFailed()
	{
		PhotonNetwork.offlineMode = true;
		PhotonNetwork.CreateRoom("localRoom", true, true, 8);
		Debug.Log("Could not join network.");
	}
	
	
	void OnJoinedRoom()
	{
		/*if (PhotonNetwork.playerList.Length == 1)
		{
			PhotonNetwork.Instantiate("soldierPrefab", spawnPoint1.transform.position, Quaternion.identity, 0);
		}
		else
		{
			PhotonNetwork.Instantiate("soldierPrefab", spawnPoint2.transform.position, Quaternion.identity, 0);
		}

		//playerSpawn();*/
		
		roomConnected = true;
		
		
	}
	
	
	
	public void playerSpawn() {
		
		GameObject player = PhotonNetwork.Instantiate("soldierPrefab", spawnPoint1.transform.position, Quaternion.identity, 0);
		Debug.Log(player);
			
		//var weaponSelec = Instantiate(weapon, player.transform.position, player.transform.rotation) as Transform;
		
		GameObject weapSlot = player.gameObject.transform.Find("Armature/Dos/Haut_Dos/Epaule_D/Bras_D/Avant_Bras_D/Main_D/Paume_D/Majeur_1_D/weaponSlot").gameObject;
		Debug.Log(weapSlot);
		
		GameObject weaponSelec = PhotonNetwork.Instantiate("Hunting_Shotgun", weapSlot.transform.position, Quaternion.identity, 0);
		weaponSelec.transform.parent = weapSlot.transform;
		
		/*weaponSelec.transform.parent = player.transform;
		weaponSelec.transform.localPosition = new Vector3(0.8f, 3.5f, 0.2f);*/
		//var weaponAim = weaponSelec.gameObject.AddComponent<weaponAim>();
		
		//var weaponScript = weapon.GetComponent<weaponName>();
		var playerScript = player.GetComponent<playerCreation>();
		
		//playerScript.actualWeapon = weaponScript.weapName;
		
		//Transform playerCam = player.transform.Find("Camera");
		//On active les scripts seulement pour notre joueur
		/*CharacterMotor motor = player.GetComponent<CharacterMotor>();
		motor.enabled = true;
			
		FPSInputController FpsInput = player.GetComponent<FPSInputController>();
		FpsInput.enabled = true;*/
			
		/*MouseLook mouse = player.GetComponent<MouseLook>();
		mouse.enabled = true;
			
		MouseLook mouseCam = playerCam.GetComponent<MouseLook>();
		mouseCam.enabled = true;
			
		playerCreation playCre = player.GetComponent<playerCreation>();
		playCre.enabled = true;
		
		WeaponAssign weapAss = player.GetComponent<WeaponAssign>();
		weapAss.enabled = true;
			
		weaponChoiceGUI weapGUI = player.GetComponent<weaponChoiceGUI>();
		weapGUI.enabled = true;
			
		player.gameObject.SetActive (true);
		playerCam.gameObject.SetActive (true);
		}*/
	}
}