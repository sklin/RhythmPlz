using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Maybe it should become 'UI_FolderList'
public class Editor_BeatmapList : MonoBehaviour {

	public GameObject buttonPrefab;

	public GameObject menuCanvas;
	public GameObject gameController;


	bool getBeatmaps = false;
	//PLUS
	//public GameObject mainCanvas;

	/*
	void Start(){
		mainCanvas.active = false;
	}*/

	void Update() {
		if (!getBeatmaps) {
			GetBeatmaps ();
		}
	}

	void GetBeatmaps() {
		// It should get beatmaps from GameController
		List<string> beatmaps = gameController.GetComponent<ListControl>().GetBeatmaps();

		foreach (var beatmap in beatmaps) {
			GameObject btn = Instantiate (buttonPrefab);
			btn.transform.SetParent (gameObject.transform);
			btn.GetComponentInChildren<Text> ().text = beatmap;
			btn.GetComponent<Button> ().onClick.AddListener (delegate {
				// menuCanvas.GetComponent<MenuManager> ().ToggleMenu(); move the GameController.StartGame()
				gameController.GetComponent<ListControl>().StartEdit(btn.GetComponentInChildren<Text> ().text);
			});
		}

		//gameObject.transform.localScale = new Vector3 (1, beatmaps.Length * buttonPrefab.GetComponent<LayoutElement> ().minHeight, 1);
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
			gameObject.GetComponent<RectTransform>().sizeDelta.x,
			//beatmaps.Length * buttonPrefab.GetComponent<LayoutElement> ().minHeight);
			beatmaps.Count * buttonPrefab.GetComponent<LayoutElement> ().minHeight);

		getBeatmaps = true;
	}

	void UpdateBeatmaps() {
		for (int i = transform.childCount-1; i >=0; i--) {
			Destroy(transform.GetChild(i));
		}

		GetBeatmaps ();
	}

}
