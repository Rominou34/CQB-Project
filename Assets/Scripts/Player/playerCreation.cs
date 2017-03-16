using UnityEngine;
using System.Collections;

public class playerCreation : Photon.MonoBehaviour {

	//Les variables relatives au joueur
	private int numPlayer=1;
	public string nomJoueur="AyXiit";
	public string playerTeam;
	public Transform weapon;
	public string actualWeapon;
	private bool weapChosen;

	
	
	// Use this for initialization
	void Start() {
		/*weapon = Resources.Load("Hunting_Shotgun");
		var weaponSelec = Instantiate(weapon, transform.position, transform.rotation) as Transform;
		
		weaponSelec.transform.parent = transform;
		weaponSelec.transform.localPosition = new Vector3(0.8f, 3.5f, 0.2f);
		var weaponAim = weaponSelec.gameObject.AddComponent<weaponAim>();
		weapChosen=true;
		
		var weaponScript = weapon.GetComponent<weaponName>();
		actualWeapon=weaponScript.weapName;*/
		
		//renderer.material = Resources.Load("texture_soldat_rouge", typeof(Material)) as Material;
		
		/*if (photonView.isMine) {
			if(playerTeam == "Red")
			{
				photonView.RPC("colorRed", PhotonTargets.AllBuffered, null);
			}
			if(playerTeam == "Blue")
			{
				photonView.RPC("colorBlue", PhotonTargets.AllBuffered, null);
			}
		}
		else {
			return;
		}*/
		

	}
	
	void OnJoinedRoom()
	{
		//renderer.material = Resources.Load("texture_soldat_rouge", typeof(Material)) as Material;
	}
	
	// Update is called once per frame
	void Update () {
		/*if ((Input.GetKeyDown(KeyCode.Mouse0)) && weapChosen!=true) {
			print ("Mouse key was pressed");
			var weaponSelec = Instantiate(weapon, transform.position, transform.rotation) as Transform;
		
			weaponSelec.transform.parent = transform;
			weaponSelec.transform.localPosition = new Vector3(0.8f, 3.5f, 0.2f);
			var weaponAim = weaponSelec.gameObject.AddComponent<weaponAim>();
			weapChosen=true;
		}
		
		var weaponScript = weapon.GetComponent<weaponName>();
		actualWeapon=weaponScript.weapName;*/
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(20,20,250,50),"Pseudo: " + nomJoueur);
		GUI.Label(new Rect(20,100,250,50),"Team: " + playerTeam);
		//GUI.Label(new Rect(200,20,250,20),"You are player number " + numPlayer.ToString());
		GUI.Label(new Rect(20,140,250,50),"Weapon: " + actualWeapon);
	}
	
	void WeaponAssign()
	{

	}
	
	[RPC]
	public void colorRed() {
		GameObject soldier = gameObject.transform.Find("Soldier").gameObject;
		soldier.GetComponent<Renderer>().material = Resources.Load("texture_soldat_rouge", typeof(Material)) as Material;
	}
	
	[RPC]
	public void colorBlue() {
		GameObject soldier = gameObject.transform.Find("Soldier").gameObject;
		soldier.GetComponent<Renderer>().material = Resources.Load("texture_soldat_bleu", typeof(Material)) as Material;
	}
}

