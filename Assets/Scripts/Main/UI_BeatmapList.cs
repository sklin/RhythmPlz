using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Maybe it should become 'UI_FolderList'
public class UI_BeatmapList : MonoBehaviour {

	public GameObject buttonPrefab;

	public GameObject menuCanvas;
	public GameObject gameController;
	//public GameObject difficultyButtonGroup;
	public GameObject[] difficultyButtons;

	public RawImage beatmapImage;
	public Texture defaultImage;

	bool getBeatmaps = false;

	public string currentFolder = "";

	void Awake() {
		StartCoroutine (UpdateImage ());
	}

	void Update() {
		if (!getBeatmaps) {
			GetBeatmaps ();
		}
	}

	void GetBeatmaps() {
		//		List<string> beatmaps = gameController.GetComponent<GameController>().GetBeatmaps();
		List<string> beatmaps = gameController.GetComponent<GameController>().GetFolders();

		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
			gameObject.GetComponent<RectTransform>().sizeDelta.x,
			beatmaps.Count * (50+10));

		foreach (var beatmap in beatmaps) {
			GameObject btn = Instantiate (buttonPrefab);
			btn.transform.SetParent (gameObject.transform);
			btn.GetComponent<RectTransform> ().localPosition = new Vector3 ( // TODO: Clean this code...
				btn.GetComponent<RectTransform> ().localPosition.x,
				btn.GetComponent<RectTransform> ().localPosition.y,
				0);
			btn.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
			btn.GetComponentInChildren<Text> ().text = beatmap;
			btn.GetComponent<Button> ().onClick.AddListener (delegate {
				// menuCanvas.GetComponent<MenuManager> ().ToggleMenu(); move the GameController.StartGame()
				//				gameController.GetComponent<GameController>().StartGame(btn.GetComponentInChildren<Text> ().text);
				currentFolder = btn.GetComponentInChildren<Text> ().text;
				//				gameController.GetComponent<GameController>().GetBeatmaps(currentFolder);
				UpdateDifficultyButtonGroup();
			});
		}

		//		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
		//			gameObject.GetComponent<RectTransform>().sizeDelta.x,
		//			beatmaps.Count * (50+10));
		//beatmaps.Count * (buttonPrefab.GetComponent<RectTransform>().sizeDelta.y+10));
		// beatmaps.Count * buttonPrefab.GetComponent<LayoutElement> ().minHeight); // for old version

		getBeatmaps = true;
	}

	IEnumerator UpdateImage() {
		Debug.Log ("UpdateImage Start");
		string dirPath = new DirectoryInfo (GameController.beatmapsDir + currentFolder + '/').FullName;
		string imageName = "mapImage.jpg";

		/*if(!dirPath.EndsWith (("\\"))) {
			dirPath = dirPath + "\\";
		}*/
		Debug.Log (dirPath);
		Debug.Log (dirPath + imageName);
		if (File.Exists (dirPath + imageName)) {
			Debug.Log ("File exist!");
			WWW www = new WWW ("file://" + dirPath + imageName);
			yield return www;
			beatmapImage.texture = www.texture;
		} else {
			Debug.Log ("File not found!");
			beatmapImage.texture = defaultImage;
		}
		Debug.Log ("UpdateImage Complete");
	}

	void UpdateBeatmaps() {
		for (int i = transform.childCount-1; i >=0; i--) {
			Destroy(transform.GetChild(i));
		}

		GetBeatmaps ();
	}

	void UpdateDifficultyButtonGroup() {
		StartCoroutine (UpdateImage ());

		foreach (var obj in difficultyButtons) {
			obj.SetActive (false);
		}

		List<string> beatmapList = gameController.GetComponent<GameController>().GetBeatmaps(currentFolder);
		int n = difficultyButtons.Length;
		if (beatmapList.Count < n)
			n = beatmapList.Count;
		for (int i = 0; i < n; i++) {
			int index = i;
			difficultyButtons [i].GetComponentInChildren<Text>().text = beatmapList[i].EndsWith (".json") ? beatmapList[i].Remove (beatmapList[i].Length - 5) : beatmapList[i];
			difficultyButtons [i].GetComponent<Button> ().onClick.RemoveAllListeners ();
			difficultyButtons [i].GetComponent<Button> ().onClick.AddListener (delegate {
				gameController.GetComponent<GameController>().StartGame(currentFolder, difficultyButtons[index].GetComponentInChildren<Text> ().text);
			});
			difficultyButtons [i].SetActive (true);
		}
	}

	/*void UpdateDifficultyButtonGroup() {
		foreach (Transform obj in difficultyButtonGroup.transform) {
			Destroy (obj.gameObject);
		}

		List<string> beatmapList = gameController.GetComponent<GameController>().GetBeatmaps(currentFolder);
		foreach (string beatmap in beatmapList) {
			GameObject btn = Instantiate (buttonPrefab);
			btn.transform.SetParent (difficultyButtonGroup.transform);
			btn.GetComponent<RectTransform> ().localPosition = new Vector3 ( // TODO: Clean this code...
				btn.GetComponent<RectTransform> ().localPosition.x,
				btn.GetComponent<RectTransform> ().localPosition.y,
				0);
			btn.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
			btn.GetComponentInChildren<Text> ().text = beatmap.EndsWith (".json") ? beatmap.Remove (beatmap.Length - 5) : beatmap;
			btn.GetComponent<Button> ().onClick.AddListener (delegate {
				gameController.GetComponent<GameController>().StartGame(currentFolder, btn.GetComponentInChildren<Text> ().text);
			});
		}

	}*/

}
