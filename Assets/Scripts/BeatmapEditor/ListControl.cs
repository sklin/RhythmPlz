using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ListControl : MonoBehaviour {

	public const string beatmapsDir = "./Beatmaps/";
	public const string configDir = "./Config/";

	DirectoryInfo beatmapsDirInfo;

//	public GameObject menuCanvas;

//	bool gameStarted = false;


	//PLUS
	public GameObject SelectCanvas;
	public GameObject mainCanvas;
	public GameObject contentCreate;

	/*
	void Start(){
		
	}*/


	// Use this for initialization
	void Start () {
		mainCanvas.SetActive (false);
		beatmapsDirInfo = new DirectoryInfo (beatmapsDir);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<string> GetBeatmaps() {
		List<string> beatmaps = new List<string>();
		foreach (var dirInfo in beatmapsDirInfo.GetDirectories()) {
			string dir = dirInfo.Name + "/";
			foreach (var fileInfo in dirInfo.GetFiles ("*.json")) {
				beatmaps.Add(dir + fileInfo.Name);
			}
		}
		return beatmaps;
	}

	public List<string> GetFolders() {
		List<string> folders = new List<string>();
		foreach (var dirInfo in beatmapsDirInfo.GetDirectories()) {
			folders.Add (dirInfo.Name);
		}
		return folders;
	}


	public void StartEdit(string mapPath){
		Debug.Log (mapPath);



		ScrollContentCreate sc = contentCreate.GetComponent<ScrollContentCreate> ();
		sc.Prepare (mapPath);
		SelectCanvas.SetActive (false);
		mainCanvas.SetActive (true);



		/*
		menuCanvas.GetComponent<MenuManager> ().HideMenu ();

		stageController = Instantiate (stageControllerPrefab) as GameObject;
		stageController.GetComponent<NoteGenerator> ().SelectBeatmap(mapPath);
		stageController.GetComponent<NoteGenerator> ().Prepare ();

		gameStarted = true;
		*/
	}


}
