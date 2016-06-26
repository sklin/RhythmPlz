using UnityEngine;
using System.Collections;

public class CameraControllMenu : MonoBehaviour {

	public float movementSpeed = 0.1f;
	public float rotationSpeed = 4f;
	public float smoothness = 0.85f;
	
	public Quaternion targetRotation;
	
	private Vector3 targetPosition;
	
	private float targetRotationX;
	private float targetRotationY;
	
	// Use this for initialization
	void Start () {
		targetPosition = transform.position;
		targetRotation = transform.rotation;
		targetRotationX = transform.localRotation.eulerAngles.x;
		targetRotationY = transform.localRotation.eulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			targetPosition += transform.forward * movementSpeed;
		}
		if (Input.GetKey (KeyCode.A)) {
			targetPosition -= transform.right * movementSpeed;
		}
		if (Input.GetKey (KeyCode.S)) {
			targetPosition -= transform.forward * movementSpeed;
		}
		if (Input.GetKey (KeyCode.D)) {
			targetPosition += transform.right * movementSpeed;
		}
		if (Input.GetKey (KeyCode.Q)) {
			targetPosition -= transform.up * movementSpeed;
		}
		if (Input.GetKey (KeyCode.E)) {
			targetPosition += transform.up * movementSpeed;
		}
		
		if (Input.GetMouseButton (1)) {
			Cursor.visible = false;
			
			targetRotationY += Input.GetAxis ("Mouse X") * rotationSpeed;
			targetRotationX -= Input.GetAxis ("Mouse Y") * rotationSpeed;
			targetRotation = Quaternion.Euler (targetRotationX, targetRotationY, 0.0f);
		} else {
			Cursor.visible = true;
		}
		
		transform.position = Vector3.Lerp (transform.position, targetPosition, (1.0f - smoothness));
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, (1.0f - smoothness));
		
	}
	public void SetToFront(){
		//		gameObject.transform.position=new Vector3(0.0f,0.55f,1f);
		targetPosition=new Vector3(0.0f,0.55f,1f);
		//		gameObject.transform.rotation=new Quaternion(0.0f,0.0f,0.0f,0.0f);
		targetRotation = Quaternion.Euler (0.0f, 180f, 0.0f);
		targetRotationX = 0;
		targetRotationY = 180f;
		Debug.Log (targetRotationX);
		//transform.rotation = Quaternion.Euler (0.0f, 180f, 0.0f);
	}
	public void SetToBack(){

		//		gameObject.transform.position=new Vector3(0.0f,0.55f,-1f);
		targetPosition=new Vector3(0.0f,0.55f,-1f);
		//		gameObject.transform.rotation=new Quaternion(0.0f,180f,0.0f,0.0f);

		targetRotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
		targetRotationX = 0.0f;
		targetRotationY = 0.0f;
	//	transform.rotation = Quaternion.Euler (0.0f, 0f, 0.0f);
	}
	public void SetToTop(){

		//		gameObject.transform.position=new Vector3(0.0f,0.55f,-1f);
		targetPosition=new Vector3(0.0f,2f,0f);
		//		gameObject.transform.rotation=new Quaternion(0.0f,180f,0.0f,0.0f);

		targetRotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
		targetRotationX = 90.0f;
		targetRotationY = 0.0f;
		//	transform.rotation = Quaternion.Euler (0.0f, 0f, 0.0f);
	}
	public void SetToLeft(){

		//		gameObject.transform.position=new Vector3(0.0f,0.55f,-1f);
		targetPosition=new Vector3(-1f,0.55f,0f);
		//		gameObject.transform.rotation=new Quaternion(0.0f,180f,0.0f,0.0f);

		targetRotation = Quaternion.Euler (0.0f, 90.0f, 0.0f);
		targetRotationX = 0.0f;
		targetRotationY = 90.0f;
		//	transform.rotation = Quaternion.Euler (0.0f, 0f, 0.0f);
	}
	public void SetToRight(){

		//		gameObject.transform.position=new Vector3(0.0f,0.55f,-1f);
		targetPosition=new Vector3(1f,0.55f,0f);
		//		gameObject.transform.rotation=new Quaternion(0.0f,180f,0.0f,0.0f);

		targetRotation = Quaternion.Euler (0.0f, 270.0f, 0.0f);
		targetRotationX = 0.0f;
		targetRotationY = 270.0f;
		//	transform.rotation = Quaternion.Euler (0.0f, 0f, 0.0f);
	}

}
