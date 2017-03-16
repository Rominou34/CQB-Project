using UnityEngine;
using System.Collections;

public class particleSystemDestroy : MonoBehaviour {

	public float lifeTime;
	private float counter=0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		PhotonView photonView = PhotonView.Get(this);
	
		if(counter >= lifeTime)
		{
			photonView.RPC("destroyParticle", PhotonTargets.All, null);
		}
		else
		{
			counter += Time.deltaTime;
		}
	}
	
	[RPC]
	void destroyParticle()
	{
		Destroy(gameObject);
	}
}
