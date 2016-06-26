using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class NoteMove : MonoBehaviour {
	float moveSpeed;
	public GameObject NotePrefab;
	GameObject NoteNow;
	public GameObject scrollcontentview;
	public bool isEdit=false;
	public int NoteNodeIndex;
//	Vector3 pos;

	// Use this for initialization
	void Start () {
//		pos = gameObject.transform.position;
		moveSpeed = 0.02f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.R)) {
			NoteNow.transform.position += transform.forward * moveSpeed;
		}
		if (Input.GetKey (KeyCode.T)) {
			NoteNow.transform.position -= transform.forward * moveSpeed;
		}
		if (Input.GetKey (KeyCode.F)) {
			NoteNow.transform.position -= transform.right * moveSpeed;
		}
		if (Input.GetKey (KeyCode.G)) {
			NoteNow.transform.position += transform.right * moveSpeed;
		}
		if (Input.GetKey (KeyCode.V)) {
			NoteNow.transform.position -= transform.up * moveSpeed;
		}
		if (Input.GetKey (KeyCode.B)) {
			NoteNow.transform.position += transform.up * moveSpeed;
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			NoteNow.transform.position += transform.up * moveSpeed;
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			NoteNow.transform.position -= transform.up * moveSpeed;
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			NoteNow.transform.position -= transform.right * moveSpeed;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			NoteNow.transform.position += transform.right * moveSpeed;
		}
	}
	public void saveNote(){
		ScrollContentCreate sc = scrollcontentview.GetComponent<ScrollContentCreate> ();
		NoteNode _node;
		_node = new NoteNode ();
		if (isEdit == false) {
			_node.pos = NoteNow.transform.position;
			_node.time = gameObject.GetComponent<AudioSource> ().time;
			_node.type = 2;
			_node._alive = false;
			sc.noteList.Add (_node);
			sc.noteList.Sort ((x, y) => {
				return x.time.CompareTo (y.time);
			});
			GameObject newButton = Instantiate (sc.noteButton) as GameObject;
			newButton.transform.SetParent (sc.gameObject.transform, false);
			newButton.transform.SetSiblingIndex (sc.noteList.IndexOf (_node));
			newButton.GetComponent<RectTransform> ().position = Vector3.zero;
			newButton.GetComponentInChildren<Text> ().text = _node.time + "," + _node.type;
			newButton.GetComponent<Button> ().onClick.AddListener (delegate {
				// menuCanvas.GetComponent<MenuManager> ().ToggleMenu(); move the GameController.StartGame()
				gameObject.GetComponent<MusicControl> ().JumpAndPause (
					float.Parse (newButton.GetComponentInChildren<Text> ().text.Substring (0,
						newButton.GetComponentInChildren<Text> ().text.IndexOf (','))),
					newButton.transform.GetSiblingIndex(),
					_node.pos
				);
			});
			sc.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (
				sc.gameObject.GetComponent<RectTransform> ().sizeDelta.x,
				sc.gameObject.GetComponent<RectTransform> ().sizeDelta.y +
				sc.noteButton.GetComponent<LayoutElement> ().minHeight
			);

			for (int i = 0; i < sc.noteList.Count; i++) {
				Debug.Log (sc.noteList [i].time);
			}
		}
		else 
		{
			_node.pos = NoteNow.transform.position;
			_node.type = 2;
			_node.time = gameObject.GetComponent<AudioSource> ().time;
			_node._alive = false;
			sc.noteList [NoteNodeIndex] = _node;
			sc.noteList.Sort ((x, y) => {
				return x.time.CompareTo (y.time);
			});
			sc.GetComponentsInChildren<Button> () [NoteNodeIndex].GetComponentInChildren<Text> ().text = _node.time + "," + _node.type;
			sc.GetComponentsInChildren<Button> () [NoteNodeIndex].onClick.RemoveAllListeners(); 
			sc.GetComponentsInChildren<Button> () [NoteNodeIndex].onClick.AddListener(delegate {
				// menuCanvas.GetComponent<MenuManager> ().ToggleMenu(); move the GameController.StartGame()
				gameObject.GetComponent<MusicControl> ().JumpAndPause (
					float.Parse (sc.GetComponentsInChildren<Button> () [NoteNodeIndex].GetComponentInChildren<Text> ().text.Substring (0,
						sc.GetComponentsInChildren<Button> () [NoteNodeIndex].GetComponentInChildren<Text> ().text.IndexOf (','))),
					sc.GetComponentsInChildren<Button> () [NoteNodeIndex].transform.GetSiblingIndex(),
					_node.pos
				);
			});

			sc.GetComponentsInChildren<Button> () [NoteNodeIndex].transform.SetSiblingIndex (sc.noteList.IndexOf (_node));

		}

		if(NoteNow)
			Destroy (NoteNow);   //DESTROY IT
		isEdit=false;

	}
	public void SelectNote(GameObject gb){
		NoteNow = gb;
	}
	public void CreateNote(){
		isEdit=false;
		if(NoteNow)
			Destroy (NoteNow);
		GameObject newNote = Instantiate (NotePrefab)as GameObject;
		newNote.transform.position = new Vector3 (0.0f, 1.0f, 0.0f);
		SelectNote (newNote);
	}
	public void CreateNote(Vector3 _vec){
		if(NoteNow)
			Destroy (NoteNow);
		GameObject newNote = Instantiate (NotePrefab)as GameObject;
		newNote.transform.position =_vec;
		SelectNote (newNote);
	}
	public void AbondonNote(){
		if (NoteNow)
			Destroy (NoteNow);
	}
	public void DeleteNote(){
		if (isEdit == true) {
			ScrollContentCreate sc = scrollcontentview.GetComponent<ScrollContentCreate> ();
			NoteNode _node;
			_node = new NoteNode ();
			sc.noteList.RemoveAt (NoteNodeIndex);
			sc.GetComponentsInChildren<Button> () [NoteNodeIndex].onClick.RemoveAllListeners(); 
//			sc.GetComponentsInChildren<Button> () [NoteNodeIndex]
			Destroy(sc.GetComponentsInChildren<Button> () [NoteNodeIndex].gameObject);

			sc.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (
				sc.gameObject.GetComponent<RectTransform> ().sizeDelta.x,
				sc.gameObject.GetComponent<RectTransform> ().sizeDelta.y -
				sc.noteButton.GetComponent<LayoutElement> ().minHeight
			);


			if(NoteNow)
				Destroy (NoteNow);   //DESTROY IT
			isEdit=false;
		}
	}


}
