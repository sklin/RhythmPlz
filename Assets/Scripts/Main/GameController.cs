using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameController : MonoBehaviour {

	public const string beatmapsDir = "./Beatmaps/";
	public const string configDir = "./Config/";

	public GameObject stageControllerPrefab;

	//	DirectoryInfo applicationDirInfo;
	//	DirectoryInfo configDirInfo;
	DirectoryInfo beatmapsDirInfo;

	GameObject stageController = null;

	public GameObject menuCanvas;

	bool gameStarted = false;

	Config config = new Config();
	public Material[] skyboxes;
	public Material[] grounds;
	public AudioClip[] hitSounds;
	public GameObject groundObject;
	public GameObject hitSoundObject_1;
	public GameObject hitSoundObject_2;

	void Start () {
		//		applicationDirInfo = new DirectoryInfo (".");
		beatmapsDirInfo = new DirectoryInfo (beatmapsDir);
		//		configDirInfo = new DirectoryInfo (configDir);

		if (skyboxes.Length > 0) {
			config.skyboxIndex = 0;
			RenderSettings.skybox = skyboxes[config.skyboxIndex];
		}

		if (grounds.Length > 0) {
			config.groundIndex = 0;
			groundObject.GetComponent<MeshRenderer>().material = grounds[config.groundIndex];
		}

		if (hitSounds.Length > 0) {
			config.hitSoundIndex = 0;
			//foreach (var obj in FindObjectsOfType<JoystickController>()) {
			//	obj.gameObject.GetComponent<AudioSource> ().clip = hitSounds [config.hitSoundIndex];
			//}
			hitSoundObject_1.GetComponent<AudioSource>().clip = hitSounds[config.hitSoundIndex];
			hitSoundObject_2.GetComponent<AudioSource>().clip = hitSounds[config.hitSoundIndex];
		}

	}

	void Update() {
		if (gameStarted == true && stageController == null) {
			EndGame ();
		}
	}

	//	void OnGUI() {
	//		if (stageController != null) {
	//			return;
	//		}
	//		
	//		int posY = 10;
	//		foreach (var dirInfo in beatmapsDirInfo.GetDirectories()) {
	//			int posX = 10;
	//			string dir = dirInfo.Name + "/";
	//			foreach (var fileInfo in dirInfo.GetFiles ("*.json")) {
	//				if (GUI.Button (new Rect (posX, posY, 200, 50), dir + fileInfo.Name)) {
	//					stageController = Instantiate (stageControllerPrefab) as GameObject;
	//					stageController.GetComponent<NoteGenerator> ().SelectBeatmap(dirInfo.Name, fileInfo.Name);
	//					stageController.GetComponent<NoteGenerator> ().Prepare ();
	//				}
	//				posX += 210;
	//			}
	//			posY += 60;
	//		}
	//	}

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

	public List<string> GetBeatmaps(string folder) {
		List<string> beatmaps = new List<string>();
		//		string dir = folder + "/";
		DirectoryInfo beatmapDirInfo = new DirectoryInfo (beatmapsDir + '/' + folder);
		foreach (var fileInfo in beatmapDirInfo.GetFiles ("*.json")) {
			beatmaps.Add(fileInfo.Name);
		}
		return beatmaps;
	}

	public void StartGame(string mapPath) {
		if (stageController == null) {
			menuCanvas.GetComponent<MenuManager> ().HideMenu ();

			stageController = Instantiate (stageControllerPrefab) as GameObject;
			stageController.GetComponent<NoteGenerator> ().SelectBeatmap (mapPath);
			stageController.GetComponent<NoteGenerator> ().Prepare ();

			gameStarted = true;
		}
	}

	public void StartGame(string dir, string map) {
		if (!map.EndsWith (".json")) {
			map = map + ".json";
		}
		if (stageController == null) {
			menuCanvas.GetComponent<MenuManager> ().HideMenu ();

			stageController = Instantiate (stageControllerPrefab) as GameObject;
			stageController.GetComponent<NoteGenerator> ().SelectBeatmap (dir, map);
			stageController.GetComponent<NoteGenerator> ().Prepare ();

			gameStarted = true;
		}
	}

	public void EndGame() {
		gameStarted = false;
		menuCanvas.GetComponent<MenuManager> ().UnhideMenu ();
		menuCanvas.GetComponent<MenuManager> ().ShowMenu (menuCanvas.GetComponent<MenuManager> ().menus [3]); // TODO: Ugly...
	}

	///////////////////////////////////////////////////////////////////
	/// Options
	///////////////////////////////////////////////////////////////////
	public void ChangeSkybox() {
		if (skyboxes.Length > 0) {
			config.skyboxIndex++;
			if (skyboxes.Length == config.skyboxIndex)
				config.skyboxIndex = 0;
			RenderSettings.skybox = skyboxes [config.skyboxIndex];
		}
	}
	public void ChangeSkyboxReverse() {
		if (skyboxes.Length > 0) {
			config.skyboxIndex--;
			if (config.skyboxIndex < 0)
				config.skyboxIndex = skyboxes.Length-1;
			RenderSettings.skybox = skyboxes [config.skyboxIndex];
		}
	}

	public void ChangeGround() {
		if (grounds.Length > 0) {
			config.groundIndex++;
			if (grounds.Length == config.groundIndex)
				config.groundIndex = 0;
			groundObject.GetComponent<MeshRenderer>().material = grounds[config.groundIndex];
		}
	}
	public void ChangeGroundReverse() {
		if (grounds.Length > 0) {
			config.groundIndex--;
			if (config.groundIndex < 0)
				config.groundIndex = grounds.Length-1;
			groundObject.GetComponent<MeshRenderer>().material = grounds[config.groundIndex];
		}
	}

	public void ChangeHitSound() {
		if (hitSounds.Length > 0) {
			config.hitSoundIndex++;
			if (hitSounds.Length == config.hitSoundIndex)
				config.hitSoundIndex = 0;

			//foreach (var obj in FindObjectsOfType<JoystickController>()) {
			//	obj.gameObject.GetComponent<AudioSource> ().clip = hitSounds [config.hitSoundIndex];
			//}
			hitSoundObject_1.GetComponent<AudioSource>().clip = hitSounds[config.hitSoundIndex];
			hitSoundObject_2.GetComponent<AudioSource>().clip = hitSounds[config.hitSoundIndex];
		}
		PlayHitSound ();
	}
	public void ChangeHitSoundReverse() {
		if (hitSounds.Length > 0) {
			config.hitSoundIndex--;
			if (config.hitSoundIndex < 0)
				config.hitSoundIndex = hitSounds.Length - 1;

			//foreach (var obj in FindObjectsOfType<JoystickController>()) {
			//	obj.gameObject.GetComponent<AudioSource> ().clip = hitSounds [config.hitSoundIndex];
			//}
			Debug.Log(config.hitSoundIndex);
			hitSoundObject_1.GetComponent<AudioSource>().clip = hitSounds[config.hitSoundIndex];
			hitSoundObject_2.GetComponent<AudioSource>().clip = hitSounds[config.hitSoundIndex];
		}
		PlayHitSound ();
	}
	public void PlayHitSound() {
		FindObjectOfType<JoystickController> ().gameObject.GetComponent<AudioSource> ().Play ();
	}
}

/*
UI_BeatmapList 向 GameController 拿 Beatmaps ，產生 Beatmap List 中的各個 Button。
Button 的功能為 ToggleMenu() 和 呼叫 GameController 的 StartGame('folder', 'map')

TODO:
Beatmaps/
- Song1/
	- songInfo.json (fixed name, record map's filename, and music' filename)
	- music.ogg
	- map.json (will not contains music's filename)
	- map2.json (will not contains music's filename)
*/
