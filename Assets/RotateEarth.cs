using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEarth : MonoBehaviour {

    float rotationx;
    float rotationy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rotationx = Input.GetAxis("Oculus_GearVR_LThumbstickX");
        rotationy = Input.GetAxis("Oculus_GearVR_LThumbstickY");
        transform.Rotate(new Vector3(rotationy, rotationx, 0));

		rotationx = Input.GetAxis("Oculus_GearVR_RThumbstickX");
		rotationy = Input.GetAxis("Oculus_GearVR_RThumbstickY");
		transform.Rotate(new Vector3(rotationy, rotationx, 0));
	}
}
