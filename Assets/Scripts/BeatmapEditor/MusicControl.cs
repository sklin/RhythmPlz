using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour {

	public AudioSource AS;
	bool autoplay=false;
	public GameObject scc;  //SCROLL CONTENT CREATE
//	public GameObject notemove;
	// Use this for initialization
	public GameObject mainCanvas;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (autoplay == true) {
			if (!AS.isPlaying && AS.clip.loadState == AudioDataLoadState.Loaded) {
				AS.Play ();
				AS.loop = false;
				autoplay = false;
				ScrollContentCreate temp = scc.GetComponent<ScrollContentCreate> ();
				temp.CreateList ();
				//			GameObject timelineCanvas = Instantiate (TimelineCanvasPrefab) as GameObject;
				//			timelineCanvas.transform.SetParent (transform);
			}
		}
	
	}
	public void PlayMusic(){
		AS.Play ();
	}
	public void PauseMusic(){
		AS.Pause ();
	}
	public void PrepareMusic(string name){
		Debug.Log (name);
		WWW www = new WWW(name);
		AS.clip = www.audioClip;


	}
	public void AutoPlay(){
		autoplay = true;
	}
/*	public bool isPrepare(){
		if(AS.clip==null){
			return false;
		} else {
			return true;
		}



	}*/
	public void JumpAndPause(float _time,int _index,Vector3 _vec){
		Debug.Log (_time + "  TIME");
		AS.time = _time;
		AS.Pause ();
		NoteMove nm = gameObject.GetComponent<NoteMove> ();
		nm.CreateNote (_vec);
		nm.isEdit = true;
		nm.NoteNodeIndex = _index;
		Button[] btn;
		btn=mainCanvas.GetComponentsInChildren <Button>();
		for (int i = 0; i < btn.Length; i++) 
		{
			if(btn[i].name=="Abandon"||btn[i].name=="Save"||btn[i].name=="Delete")
			btn [i].interactable = true;
		}
		
	}
}
