using UnityEngine;
using System.Collections;

public class JoystickController : MonoBehaviour {

	public float movementSpeed = 0.1f;
	public float smoothness = 0.85f;

	private Vector3 targetPosition;

	public SteamVR_TrackedObject trackedObj;
	public SteamVR_Controller.Device device;

	void Start() {
		device = SteamVR_Controller.Input((int)trackedObj.index);
	}

	void Update() {
//		if (device.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
//			Debug.Log ("GetPress = True");
//		}
//		if (device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
//			Debug.Log ("GetTouch = True");
//		}

//		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
//			Debug.Log ("GetPressDown = True");
//		}
//		if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
//			Debug.Log ("GetTouchDown = True");
//		}
	}

	//void OnTriggerEnter(Collider other) {
	void OnTriggerStay(Collider other) {
		Debug.Log (other.gameObject.name);
		//if( device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger) ) {
		if( device.GetTouch (SteamVR_Controller.ButtonMask.Trigger) ) {
			if (other.gameObject.tag == "Note") {
				if(other.transform.parent.GetComponent<NoteController> ().Hit ()) {
					device.TriggerHapticPulse(3000);
					if(gameObject.GetComponent<AudioSource>())
						gameObject.GetComponent<AudioSource>().Play();
	//				Debug.Log ("Hit");
				}
			}
		}
	}

//	void Start() { // for development
//		/*
//		targetPosition = transform.position;
//		*/
//	}

//	void Update() { // for development
//		/*
//		if (Input.GetKey (KeyCode.I)) {
//			targetPosition += transform.forward * movementSpeed;
//		}
//		if (Input.GetKey (KeyCode.J)) {
//			targetPosition -= transform.right * movementSpeed;
//		}
//		if (Input.GetKey (KeyCode.K)) {
//			targetPosition -= transform.forward * movementSpeed;
//		}
//		if (Input.GetKey (KeyCode.L)) {
//			targetPosition += transform.right * movementSpeed;
//		}
//		if (Input.GetKey (KeyCode.U)) {
//			targetPosition -= transform.up * movementSpeed;
//		}
//		if (Input.GetKey (KeyCode.O)) {
//			targetPosition += transform.up * movementSpeed;
//		}
//
//		transform.position = Vector3.Lerp (transform.position, targetPosition, (1.0f - smoothness));
//		*/
//			
//	}
}
