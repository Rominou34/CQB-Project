using UnityEngine;
using System.Collections;

public class bulletScript : Photon.MonoBehaviour {
	
	public int damageBullet;
	public string originTeam;
	public float lifeTime;
	
	private float duration=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(duration < lifeTime)
		{
			duration += Time.deltaTime;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	void OnCollisionEnter (Collision collisionInfo)
    {
        if(collisionInfo.collider.tag == "Player")
        {
			//Destroy(col.gameObject);
        }
    }
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(GetComponent<Rigidbody>().position);
		}
		else
		{
			GetComponent<Rigidbody>().position = (Vector3)stream.ReceiveNext();
		}
	}
}
