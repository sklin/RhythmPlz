using UnityEngine;
using System.Collections;

public class DestroyInTime : MonoBehaviour {

	GameObject audioSource;
	GameObject scrollcontentcreate;
	float _time;
	public Material _material;
	int _index;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (audioSource.GetComponent<AudioSource> ().time - _time > 1.0f) {
			ScrollContentCreate sc = scrollcontentcreate.GetComponent<ScrollContentCreate> ();
			sc.noteList [_index]._alive = false;
			Destroy (gameObject);
		} else if (audioSource.GetComponent<AudioSource> ().time - _time > 0.5f) {
			//gameObject.GetComponent<Material>().
//			gameObject.GetComponent<MeshRenderer>().material=_material;
			//Debug.Log(gameObject.GetComponentsInChildren<MeshRenderer>().Length);
			if (gameObject.GetComponentsInChildren<MeshRenderer> ().Length == 2) 
			{
				gameObject.GetComponentsInChildren<MeshRenderer> () [0].material = _material;
				gameObject.GetComponentsInChildren<MeshRenderer> () [1].material = _material;
			}

		}
	
	}
	public void setVar(float _t,GameObject audiosource,int i,GameObject scrollcc){
		_time = _t;
		audioSource=audiosource;
		_index=i;
		scrollcontentcreate = scrollcc;
	}
}
