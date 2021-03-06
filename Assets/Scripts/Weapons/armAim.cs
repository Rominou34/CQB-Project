﻿using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class armAim : MonoBehaviour {

	public bool coteDroit;
	
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	//public float minimumX = -360F;
	//public float maximumX = 360F;

	public float minimumY = -40F;
	public float maximumY = 40F;

	float rotationX = 0F;

	void Update ()
	{
		rotationX += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationX = Mathf.Clamp (rotationX, minimumY, maximumY);
			
		if(coteDroit)
		{
			transform.localEulerAngles = new Vector3(rotationX, 0, -90);
		}
		else {
			transform.localEulerAngles = new Vector3(-rotationX, 0, 90);
		}
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}