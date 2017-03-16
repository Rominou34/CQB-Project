using UnityEngine;
using System.Collections;

public class playerNetwork : Photon.MonoBehaviour {

	/* --- PLAYER --- */
	public float speed=4f;
	public float rotation=50f;
    // To see if the player is on the ground
	private bool isgrounded;
	
	/* --- NETWORK --- */
	// The last time we received an update from other players
	private float lastSynchronizationTime = 0f;
	// The delay of the sync
	private float syncDelay = 0f;
	// The duration of the sync ( i think )
	private float syncTime = 0f;
	// Starting pos for smoothing position
	private Vector3 syncStartPosition = Vector3.zero;
	// Ending pos for smoothing position
	private Vector3 syncEndPosition = Vector3.zero;
	// Starting rot for smoothing rotation
	private Quaternion syncStartRotation = Quaternion.identity;
	// Ending rot for smoothing rotation
	private Quaternion syncEndRotation = Quaternion.identity;
	
	/*------------- PLAYER SETTINGS ----------*/
	
	public string playerTeam;
	// Changing the value will keep it to 100
	// There must be some 'maxHealth' value somewhere
	public int health;

	// The keyboard mode from the player settings
	private string keyboardMode;
	// The FOV value from the player settings
	private int FOV;
	
	public bool isAlive=true;
	
	private float colorRPCTimer;
	
	
	//**********************************************
	//SHOOTING SCRIPT
	//**********************************************
	/*public int damageWeapon;
	public string weapTeam;
	public int taille_chargeur;
	public int mun_max;
	public bool rapidFire;
	public bool singleFire;
	public float recoilTime;
	public float reloadTime;
	
	private int Speed = 16000;
	
	private float Counter=Time.deltaTime;
	
	//this is only used if rapid fire is set to true
	private bool shooting = false;*/
	
	/*------------------------------ SCORE VALUES --------------------------------------*/
	public int lastShot; //The last guy who shot us
	
	
	
	/*-------------------------------------*/
	
	// Use this for initialization
	void Start () {
		keyboardMode = PlayerPrefs.GetString("KeyboardType");
		//PhotonPlayer player = new PhotonPlayer(true, 1,"player");
		
		colorRPCTimer = 0f;
	}
	
	// When the object is loaded and starts running
	void Awake()
    {
		
        Transform playerCam = transform.Find("Camera");
		
		/*-------------- WE SET THE CUSTOM FOV ON THE CAMERA -----------------*/
		Camera playerCamera = playerCam.GetComponent<Camera>();
		FOV = PlayerPrefs.GetInt("playerFOV");
		if(FOV != null)
		{
			playerCamera.fieldOfView = FOV;
		}
		/*-------------------------------------------------------------------*/
				
		Transform bras_D = gameObject.transform.Find("Armature/Dos/Haut_Dos/Epaule_D/Bras_D");
		armAim armAim_D = bras_D.GetComponent<armAim>();
				
		Transform bras_G = gameObject.transform.Find("Armature/Dos/Haut_Dos/Epaule_G/Bras_G");
		armAim armAim_G = bras_G.GetComponent<armAim>();
		
		MouseLook mouseCam = playerCam.GetComponent<MouseLook>();
				
		MouseLook mouselook = GetComponent<MouseLook>();
		
		shotScript shotEn = GetComponent<shotScript>();
				
		PhotonView photonView = PhotonView.Get(this);
       
		//We enable the scripts only for our player, and not for the other players in the room
        if (photonView.isMine)
        {
				//MINE: local player, simply enable the local scripts
				mouselook.enabled = true;
                mouseCam.enabled = true;
				armAim_D.enabled =true;
				armAim_G.enabled = true;
				shotEn.enabled = true;
						
				playerCam.gameObject.active = true;
        }
        else
        {
				mouselook.enabled = false;
				armAim_D.enabled = false;
				armAim_G.enabled = false;
				shotEn.enabled = false;
                        
				mouseCam.enabled = false;
				playerCam.gameObject.active = false;
				//playerControl.enabled = false;
        }
 
        gameObject.name = gameObject.name + photonView.viewID;
		
		//networkManager netMan= (networkManager)GameObject.Find("multiScripts").GetComponent("networkManager");
    }
	
	/*
	* Called on each frame
	*/
	void Update()
    {
    	PhotonView photonView = PhotonView.Get(this);
        if (photonView.isMine) //For our player we just control it and set his color
		{
			InputMovement();
			InputColorChange(playerTeam);
		}
		else	//For the other players, we get their position and update it to see them smoothly
		{
			SyncedMovement();
		}
		
		if(isAlive==false)
		{
			//Destroy(gameObject);
			//photonView.RPC("destroyPlayer", PhotonTargets.All, null);
			destroyPlayer();
		}
		
		//Debug.Log("LAST SHOT" + lastShot);
		
		//scoreScript scoreSc = transform.parent.GetComponent<scoreScript>();
		//scoreSc.myLastShot = lastShot;
	}
	
	/*
	* Smoothes the movements of other players
	*/
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
		
		GetComponent<Rigidbody>().rotation = Quaternion.Lerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);

	}

	/*
	* Moves the player depending on input
	*/
    public void InputMovement()
    {
		CharacterController controller = GetComponent<CharacterController>();
        if(isgrounded)
		{
			//We stop the player incase he's landing
			GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			
			Vector3 movement = new Vector3(0,0,0);;
			float moveSpeed=400f;
		
			if ( (keyboardMode=="AZERTY" && Input.GetKey(KeyCode.Z)) || (keyboardMode=="QWERTY" && Input.GetKey(KeyCode.W)) )
			{
				movement+=transform.forward;
			}
				
			if (Input.GetKey(KeyCode.S))
				movement-=transform.forward;
	 
			if (Input.GetKey(KeyCode.D))
				movement+=transform.right;
	 
			if ( (keyboardMode=="AZERTY" && Input.GetKey(KeyCode.Q)) || (keyboardMode=="QWERTY" && Input.GetKey(KeyCode.A)))
				movement-=transform.right;
				
			GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + movement * speed * Time.deltaTime);

			//Jump
			if (Input.GetButtonDown("Jump"))
			{
				GetComponent<Rigidbody>().velocity = new Vector3(0, 5, 0);
				isgrounded=false;
				GetComponent<Rigidbody>().AddForce(movement * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
			}
		}
		//private void moveForward(float speed)
    }
	
	/*----------------- BETTER RIGIDBODY PHYSICS ( NOT WORKING ) --------------*/
	
	void FixedUpdate() {
		/*float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.GetAxis ("Horizontal");
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		float xSpeed = rigidbody.velocity.x;
		float zSpeed = rigidbody.velocity.y;
		float moveSpeed=20f-xSpeed - zSpeed;
		rigidbody.AddForce(movement * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);*/
		InputMovement();
	}
	
	
	
	/*#############
	#   NETCODE   #
	#############*/
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // The function to synchronize the player's position and rotation
	{
		// We send our updated position to the other players
		if (stream.isWriting)
		{
			stream.SendNext(GetComponent<Rigidbody>().position);
			
			stream.SendNext(GetComponent<Rigidbody>().rotation);
			
			stream.SendNext(playerTeam);
			
			stream.SendNext(health);
		}
		else
		// We receive the updated position of other players
		{
			syncEndPosition = (Vector3)stream.ReceiveNext();
			syncStartPosition = GetComponent<Rigidbody>().position;
			
			
			syncEndRotation = (Quaternion)stream.ReceiveNext();
			syncStartRotation = GetComponent<Rigidbody>().rotation;
	 
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			playerTeam = (string)stream.ReceiveNext();
			
			health = (int)stream.ReceiveNext();
		}
	}	
	
	
	
	/*#########
	#   GUI   #
	#########*/
	
	void OnGUI() {
		GUI.Label(new Rect(200,50,250,50),"Team Net: " + playerTeam);
		GUI.Label(new Rect(200,150,250,50),"playerCount: " + PhotonNetwork.playerList.Length);
		var multiObj = GameObject.Find("multiScripts");
		if(photonView.isMine)
		{
			GUI.Label(new Rect(20,60,250,50),"HP: " + health.ToString());
		}
		
	}
	
	
	
	/*###################
	#   RPC FUNCTIONS   #
	###################*/
	
	void InputColorChange(string team)
	{
		ChangeColorTo(team);
		if(colorRPCTimer > 1.0f)
		{
			if (photonView.isMine)
				photonView.RPC("ChangeColorTo", PhotonTargets.OthersBuffered, team);
			
			colorRPCTimer = 0f;
		}
		else
		{
			colorRPCTimer += Time.deltaTime;
		}
	}
	 
	[RPC]
	void ChangeColorTo(string colorTeam)
	{
		if(colorTeam=="Red") {
			GameObject soldier = gameObject.transform.Find("Soldier").gameObject;
			soldier.GetComponent<Renderer>().material = Resources.Load("texture_soldat_rouge", typeof(Material)) as Material;
		}
		else {
			GameObject soldier = gameObject.transform.Find("Soldier").gameObject;
			soldier.GetComponent<Renderer>().material = Resources.Load("texture_soldat_bleu", typeof(Material)) as Material;
		}
	 
		//if (photonView.isMine)
			//photonView.RPC("ChangeColorTo", PhotonTargets.OthersBuffered, colorTeam);
	}
	
	
	/*#####################
	#   OTHER FUNCTIONS   #
	#####################*/
	
	//Check if the player is grounded or not
	void OnCollisionEnter(Collision theCollision){
		if(theCollision.gameObject.tag == "Terrain")
		{
			isgrounded = true;
		}
	}
 
	//consider when character is jumping .. it will exit collision.
	void OnCollisionExit(Collision theCollision){
		if(theCollision.gameObject.tag == "Terrain")
		{
			isgrounded = false;
		}
	}
	
	[RPC]
	void destroyPlayer()
	{
		PhotonNetwork.Destroy(gameObject);
	}
	
	
	
	/*#############################################
	#   TRASH FUNCTIONS ( KEEP THEM IN BACKUP )   #
	#############################################*/
	
	/*----------------------- BACKUP MOVE FUNCTION -----------------------*/
	/*if ( (keyboardMode=="AZERTY" && Input.GetKey(KeyCode.Z)) || (keyboardMode=="QWERTY" && Input.GetKey(KeyCode.W)) )
				rigidbody.MovePosition(rigidbody.position + transform.forward * speed * Time.deltaTime);
	 
			if (Input.GetKey(KeyCode.S))
				rigidbody.MovePosition(rigidbody.position - transform.forward * speed * Time.deltaTime);
	 
			if (Input.GetKey(KeyCode.D))
				rigidbody.MovePosition(rigidbody.position + transform.right * speed * Time.deltaTime);
	 
			if ( (keyboardMode=="AZERTY" && Input.GetKey(KeyCode.Q)) || (keyboardMode=="QWERTY" && Input.GetKey(KeyCode.A)))
				rigidbody.MovePosition(rigidbody.position - transform.right * speed * Time.deltaTime);
				
	*/
	
	/*void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		
		Quaternion syncRotation = Quaternion.identity;
		Vector3 syncAngVelocity = Vector3.zero;
		
		if (stream.isWriting)
		{
			syncPosition = rigidbody.position;
			stream.Serialize(ref syncPosition);
	 
			
			syncVelocity = rigidbody.velocity;
			stream.Serialize(ref syncVelocity);
			
			
			syncRotation = rigidbody.rotation;
			stream.Serialize(ref syncRotation);
			
			syncAngVelocity = rigidbody.angularVelocity;
			stream.Serialize(ref syncAngVelocity);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			
			stream.Serialize(ref syncRotation);
			stream.Serialize(ref syncAngVelocity);
	 
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
	 
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = rigidbody.position;
			
			
			
			
			float axisLength = syncDelay * syncAngVelocity.magnitude * Mathf.Rad2Deg;
            Quaternion angularRotation = Quaternion.AngleAxis(axisLength, syncAngVelocity);
			
			syncEndRotation = syncRotation * angularRotation;
			syncStartRotation = rigidbody.rotation;
		}
	}*/
}