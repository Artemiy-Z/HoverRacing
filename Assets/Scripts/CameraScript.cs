﻿using System.Collections;	
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public Transform car;
	public Transform carClone;
	private PortalTraveller pTrav;
	public float distance = 6.4f;
	public float height = 1.4f;
	public float rotationDamping = 3.0f;
	public float heightDamping = 2.0f;
	public float zoomRatio = 0.5f;
	public float defaultFOV = 60f;

	private Quaternion rotationVector;

	public LayerMask ground;

	public int previousSide;
	private Vector3 nextPosition = Vector3.zero;
	private Quaternion nextRotation = Quaternion.identity;
	
	public Vector3 previousPortalPosition;
	public Vector3 firstPortalPosition;
	public Vector3 PortalForward;
	public int state = 0;
	public bool enteredPortal = false;

    private void Start()
    {
        pTrav = car.GetComponent<PortalTraveller>();
    }

    int SideOfPortal(Vector3 pos, Vector3 portalPos, Vector3 forward)
    {
        return System.Math.Sign(Vector3.Dot(pos - portalPos, forward));
    }

    void LateUpdate(){

		if(pTrav.graphicsClone)
		{
			carClone = pTrav.graphicsClone.transform;
		}

        transform.position = nextPosition;
        transform.rotation = nextRotation;

        Vector3 wantedPosition = car.position;
		Quaternion wantedRotation = rotationVector;
		if (state == 1)
		{
			wantedPosition = carClone.position;
			wantedRotation *= Quaternion.Euler(0, 180, 0);
		}

		wantedPosition -= wantedRotation * Vector3.forward * distance;
		wantedPosition += rotationVector * Vector3.up * height;

		transform.position = Vector3.MoveTowards(transform.position, wantedPosition, (wantedPosition-transform.position).magnitude);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, wantedRotation, Quaternion.Angle(transform.rotation, wantedRotation));
	}

	void FixedUpdate(){
		Quaternion temp = rotationVector;
		temp = car.GetComponent<Rigidbody>().rotation;
		rotationVector = temp;
		
		float acc = car.GetComponent<Rigidbody>().velocity.magnitude;
		GetComponent<Camera>().fieldOfView = defaultFOV + acc * zoomRatio * Time.fixedDeltaTime;
	}
}﻿
