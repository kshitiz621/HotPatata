﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GazeController : NetworkBehaviour {

	public Camera mainCamera;
	public GameObject headedSlug;
	public GameObject headlessSlug;
	public GameObject eye_L;
	public GameObject eye_R;
	public GameObject heart;
	public AudioListener listener;
	public OVRCameraRig OculusCameraRig;
	private GameObject gazeTarget;
	private bool activeExists;
	private RaycastHit hit;
	private Vector3 fwd;
	private GameObject human;
	public int solved;

	// Use this for initialization
	void Start () {
		human = GameObject.FindGameObjectWithTag("Human");
		if (isLocalPlayer)
		{
			OculusCameraRig.enabled = true;
			eye_L.SetActive(false);
			eye_R.SetActive(false);
			listener.enabled = true;
			headedSlug.SetActive(false);
			heart.SetActive(false);
			headlessSlug.SetActive(true);
			mainCamera.enabled = true;
			this.gameObject.tag = "Player";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.R))
			Application.LoadLevel(Application.loadedLevelName);

		//raycast hits object
		fwd = mainCamera.transform.TransformDirection (Vector3.forward);
		if (gazeTarget == null &&
		    Physics.Raycast (mainCamera.transform.position, fwd, out hit, 100) && 
			hit.collider.tag == "GazeAware")
		{
			gazeTarget = hit.collider.gameObject;
			gazeTarget.SendMessageUpwards ("LookedAt");
			gazeTarget.SendMessageUpwards ("AttachGazeController", this);
			
		}
		else if (gazeTarget != null &&
		         Physics.Raycast (mainCamera.transform.position, fwd, out hit, 100) &&
		         hit.collider.gameObject.name == gazeTarget.name)
		{
			gazeTarget.SendMessageUpwards ("LookedAt");
		}
//		print ("Gazing " + gazeTarget != null);
	}

	public void GazeRelease ()
	{
		gazeTarget = null;
	}

	public void Solve ()
	{
		print ("Solved" + solved);
		solved++;
	}
}
