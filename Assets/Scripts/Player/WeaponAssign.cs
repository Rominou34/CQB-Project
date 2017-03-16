using UnityEngine;
using System.Collections;

public class WeaponAssign : MonoBehaviour {

	//On va créer un public transform par arme, le joueur choisira l'arme en appelant un transform
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var playerScript = transform.GetComponent<playerCreation>();
		
		//Choix du fusil de chasse avec la touche 1
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			playerScript.actualWeapon="Hunting Shotgun";
			playerScript.weapon = Resources.Load("Hunting_Shotgun", typeof(Transform)) as Transform;
		}
	}
}
