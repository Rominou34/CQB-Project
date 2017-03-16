using UnityEngine;
using System.Collections;

public class shotScript : Photon.MonoBehaviour {

	public int damageWeapon=20;
	public string weapTeam;
	public int taille_chargeur;
	public int mun_max;
	public bool rapidFire;
	public bool singleFire;
	public float recoilTime;
	public float reloadTime;

    //Shoot variables
    public int actualAmmo;
    private float reloadCounter=0f;
    public bool isReloading;
	
	//Recoil decal variables :p
	private float recoilDecal = 0f;
	private float shotCounter = 0f;

	// crossHair animation variables -=m0dem=- ;)
	private bool doCrossHairAnimation;
	private int crossHairAnimationStage;
	/*private GameObject upperHairImage;
	private GameObject lowerHairImage;
	private GameObject rightHairImage;
	private GameObject leftHairImage;*/
	
	private Texture upperHairImage;
	private Texture lowerHairImage;
	private Texture leftHairImage;
	private Texture rightHairImage;

	private GameObject bulletHit;
	
	private float Counter=Time.deltaTime;
	
	//this is only used if rapid fire is set to true
	private bool shooting = false;
	
	
	RaycastHit hit;
	// Use this for initialization
	void Start () {
		PhotonView photonView = PhotonView.Get(this);
		
		Transform bulletShot = gameObject.transform.Find("bulletSpawn");

		// crossHair parts
		/*upperHairImage = Resources.Load("upperHair") as GameObject;
		lowerHairImage = Resources.Load("lowerHair") as GameObject;
		rightHairImage = Resources.Load("rightHair") as GameObject;
		leftHairImage = Resources.Load("leftHair") as GameObject;
		
		PhotonNetwork.Instantiate("upperHair", new Vector3(0,4,0), Quaternion.identity, 0);*/
		
		upperHairImage = (Texture)Resources.Load("upperHair");
		lowerHairImage = (Texture)Resources.Load("lowerHair");
		leftHairImage = (Texture)Resources.Load("leftHair");
		rightHairImage = (Texture)Resources.Load("rightHair");

		// We set the stats of the weapons
        updateWeaponStats(0);
	}
	
	// Update is called once per frame
	void Update () {

        //updateWeaponStats();
        
        //----------------------- SHOOTING
        if (actualAmmo > 0)
        {
            improvedShot();
        }
        else
        {
            if(!isReloading)
            {
                reloadCounter = 0f;
                isReloading = true;
            }
            else
            {
                if (reloadCounter >= reloadTime)
                {
                    actualAmmo = taille_chargeur;
                    isReloading = false;
                }
                else
                    reloadCounter += Time.deltaTime;
            }
        }
		

        //-----------------------------RECOIL REDUCING

		//Reducing the recoil through time if the player doesn't shoot
		if(recoilDecal >= 0.002f) //We put a if so it doesn't go below zero
		{
			//The decal is 5% max ( 0.05f max )
			recoilDecal -= Time.deltaTime*0.06f; //With this speed we lower the biggest recoil in one second
		}
		else //If the recoil is very small we put it to zero instead of lowering it under zero
		{
			recoilDecal = 0f;
		}
		
        //--------------------- UPDATING TEAM

		//We get our team
		if(weapTeam!="Blue" || weapTeam!="Red")
		{
			//playerNetwork playNet = this.transform.parent.GetComponent<playerNetwork>();
			playerNetwork playNet = GetComponent<playerNetwork>();
			weapTeam = playNet.playerTeam;
		}
	}
	
	void OnGUI()
	{
		animateCrosshair(recoilDecal);
	}
	
	/*##############
	#   SHOOTING   #
	##############*/
	
	void shoot()
	{
		Transform transCam = transform.Find("Camera");
		Camera cameraTest = transCam.GetComponent<Camera>();
		
		//Taking a random value in a circle depending on the recoilDecal
		Vector2 decalCircle = Random.insideUnitCircle; // returns a value in a circle of radius 1
		
		//We take the value and adapt it to the recoil size
        float decalx = (decalCircle.x)*recoilDecal;
        float decaly = (decalCircle.y)*recoilDecal;
		
		Debug.Log("DecalX: " + decalx);
		Debug.Log("DecalY: " + decaly);
		
		//Doing a shot with the recoil			
		Ray ray = cameraTest.ViewportPointToRay(new Vector3(0.5f+decalx, 0.5f+decaly, 0));
			
		GameObject[] players;
		players = GameObject.FindGameObjectsWithTag ("Player");
		
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
		{
			Debug.DrawLine(ray.origin, hit.point);
			for(int i=0; i < players.Length; i++)
			{
				Rigidbody rb = players[i].GetComponent<Rigidbody>();
				Collider col = players[i].GetComponent<Collider>();
				if(hit.rigidbody == rb)
				{
					PhotonView rootView = hit.rigidbody.transform.root.gameObject.GetComponent<PhotonView>();
					//PhotonView rootView = rb.gameObject.transform.root.GetComponent<PhotonView>();
						
					//We get the player id
					int targetID = rootView.owner.ID;
					PhotonPlayer target = PhotonPlayer.Find(targetID);
					
					//We get our own id
					int myID = photonView.owner.ID;
					
					//We create a string containing our id, our team and the damage he sould take
					string damageAndID = damageWeapon.ToString() + ";" + myID.ToString() + ";" + weapTeam;
					Debug.Log("Damage and ID: " + damageAndID);
					
					rootView.RPC("attack", PhotonTargets.All, damageAndID);
					//photonView.RPC("sendMyID", target, myID);
					rootView.RPC("sendMyID", PhotonTargets.All, damageAndID);
				}
				// just a test little bulletHit marker -=m0dem=- :)
				bulletHit = PhotonNetwork.Instantiate ("bulletHit", hit.point, Quaternion.identity, 0);
				bulletHit.transform.position = hit.point;
			}
		}

		//We add the recoil, while not going below 5%
		if(recoilDecal <= 0.08f ) {
			recoilDecal+=0.012f;
		}

        //Updating our global stats
        int totalShots = PlayerPrefs.GetInt("shotFired");
        totalShots++;
        PlayerPrefs.SetInt("shotFired", totalShots);

        //Updating our ammo
        actualAmmo--;
        mun_max--;
	}
	
	/*------------------------------------------------------------------------------*/
	
	/*#########################
	#   CROSSHAIR ANIMATION   #
	#########################*/
	
	void animateCrosshair(float valDecal)
	{		
		//We calculate the value of the recoil in pixels
		int pixelDecal = Mathf.RoundToInt(Screen.width*valDecal);
		
		//We print the textures at their positions, with the added recoil
		GUI.DrawTexture(new Rect( Screen.width/2-16, Screen.height/2-14-pixelDecal, 32, 32), upperHairImage );
		GUI.DrawTexture(new Rect( Screen.width/2-16, Screen.height/2-18+pixelDecal, 32, 32), lowerHairImage );
		GUI.DrawTexture(new Rect( Screen.width/2-16-pixelDecal, Screen.height/2-16, 32, 32), leftHairImage );
		GUI.DrawTexture(new Rect( Screen.width/2-16+pixelDecal, Screen.height/2-16, 32, 32), rightHairImage );
	}
	
	[RPC]
	void attack(string damAndID)
	{
		//We convert the string back to a tab using Split
		string[] damID = damAndID.Split(';');
		
		//We convert these values into int
		int dam = int.Parse(damID[0]);
		//int shooterID = int.Parse(damID[1]);
		
		//We get his team
		string shooterTeam = damID[2];
		
		playerNetwork playNet = GetComponent<playerNetwork>();
		
		//If we're not on the same team, I take damage
		if(shooterTeam != playNet.playerTeam) {
			playNet.health -= dam;
		}
		//Debug.Log(shooterID.ToString());
		//We set the "last shot" value to the last guy who shot us
		//playNet.lastShot = shooterID;
	}
	
	[RPC]
	void sendMyID(string damAndID)
	{
		//We convert the string back to a tab using Split
		string[] damID = damAndID.Split(';');
		
		//We get his ID
		int shooterID = int.Parse(damID[1]);
		
		//We get his team
		string shooterTeam = damID[2];
		
		playerNetwork playNet = GetComponent<playerNetwork>();
		
		//If we're not on the same team, I take damage
		if(shooterTeam != playNet.playerTeam) {
			playNet.lastShot = shooterID;
		}
	}

    
    //IMPROVED SHOOT FUNCTION
    void improvedShot()
    {
        //if single fire is set to true
        if (singleFire)
        {
            //we are using the left mouse button to shoot
            if (Input.GetButtonUp("Fire1"))
            {
                shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shoot();
                shooting = true;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                shooting = false;
            }
            if (shooting)
            {
                Counter += Time.deltaTime;
                if (recoilTime < Counter)
                {
                    shoot();
                    Counter = 0;
                }
            }
        }
    }

    //UPDATING THE WEAPON STATS
    public void updateWeaponStats(int weaponId)
    {
        damageWeapon=20;
	    taille_chargeur = globalWeaponStats.weaponMagSize[weaponId];
	    mun_max = globalWeaponStats.weaponMaxAmmo[weaponId];
        singleFire = globalWeaponStats.isSingleFire[weaponId];
	    recoilTime = globalWeaponStats.globalRecoilTime[weaponId];
	    reloadTime = globalWeaponStats.globalReloadTime[weaponId];
        actualAmmo = taille_chargeur;
    }

    public float getReloadStatus()
    {
    	return (reloadCounter/reloadTime)*100;
    }

    public float getReloadingTime()
    {
    	return reloadTime;
    }
}
