using UnityEngine;
using System.Collections;
using System.IO;

public class NoteGenerator : MonoBehaviour {

	public GameObject noteObject;
	public GameObject directionHintObject;
//	public float generateRadius;

	private BeatMap beatmap; // loaded from beatmap.json by LoadBeatMap()

	public string beatmapJson = "konosubaop.json";
	public string beatmapDir = "KonosubaOP/";
	private string dirPath; // Absolute Path, fetch by DirectoryInfo::Fullname

	private AudioClip audioClip = null;
	private AudioSource audioSource;

	private bool gameStarted = false;

	public float noteTimeAppearToHit = 0.6f;  // TODO: setting NoteController

	///// 
	private int i = 0; // method 2

	public GameObject TimelineCanvasPrefab;
	bool isPrepared = false;

	public float noteRadius = 0.2f;
	public float sphereRadius = 0.8f; // depends on the length of the player's arm
	public float planeDistance = 0.8f; // for plane mode
	public float space = 1.5f; // for plane mode, 1.3

	public float playerEyeHeight = 1.5f;
	public float playerArmHeight = 1.5f;
	public Vector3 playerPosition = new Vector3(0, 0, 0); // y = 0

	private Vector3 playerArmPosition;
	private Vector3 playerEyePosition;

	private int randomWidth = 5, randomHeight = 3;
	public bool[,] randomMap; // TODO: public for development

	RandomPositionGenerator randompos = new RandomPositionGenerator();

	public float timeToPrepare = 3f;
	float prepareTimer = 0;

	void Awake() {
//		Prepare ();
		playerArmPosition = new Vector3(playerPosition.x, playerArmHeight, playerPosition.z);
		playerEyePosition = new Vector3(playerPosition.x, playerEyeHeight, playerPosition.z);

		randomMap = new bool[5,3];
		for (int i = 0; i < randomWidth; i++) {
			for (int j = 0; j < randomHeight; j++) {
				randomMap [i, j] = true; // available
			}
		}
	}

	public void SelectBeatmap(string mapPath) {
		// Should be called before Prepare()
		// @mapPath = dir + map

		beatmapDir = mapPath.Substring (0, mapPath.IndexOf ('/')) + '/';
		beatmapJson = mapPath.Substring (mapPath.IndexOf ('/') + 1);
		Debug.Log (beatmapDir);
		Debug.Log (beatmapJson);
	}

	public void SelectBeatmap(string dir, string map) {
		// Should be called before Prepare()
		// @dir: beatmap's folder
		// @map: beatmap's json file
		//
		// example: SelectBeatmap('Test', 'test2.json');
		beatmapDir = dir + '/';
		beatmapJson = map;
		Debug.Log (beatmapDir);
		Debug.Log (beatmapJson);
	}

	public void Prepare() {
		dirPath = new DirectoryInfo (GameController.beatmapsDir + beatmapDir).FullName;
		beatmap = LoadBeatMap ();

		WWW www = new WWW("file://" + dirPath + beatmap.music);
		audioClip = www.audioClip;

		if (audioClip == null) {
			Debug.Log ("Load audioClip failed.");
		} else {
			Debug.Log ("Load audioClip success");
		}
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = audioClip;

		bool isRandom = false;
		// Random generator
		for (int cnt = 0; cnt < beatmap.map.Length; cnt++) {
			for (int j = 0; j < beatmap.map [cnt].notes.Length; j++) {
				switch (beatmap.map [cnt].notes [j].type) {
				case 1:
					{
						Vector3 pos = randompos.Generate (beatmap.map [cnt].t);
						pos += playerArmPosition;
						beatmap.map [cnt].notes [j].x = pos.x;
						beatmap.map [cnt].notes [j].y = pos.y;
						beatmap.map [cnt].notes [j].z = pos.z;
						beatmap.map [cnt].notes [j].type = 2;
						isRandom = true;
					}
					break;
				default:
					break;
				}
			}
		}

		if (isRandom == true) {
			File.Copy ((dirPath + beatmapJson),dirPath+beatmapJson+"backup.txt",false);
			using (FileStream fs = new FileStream(dirPath + beatmapJson, FileMode.Create)){
				using (StreamWriter writer = new StreamWriter(fs)){
					writer.Write(JsonUtility.ToJson (beatmap, true));
				}
			}
		}

		isPrepared = true;
		Debug.Log (isPrepared);

		Debug.Log ("Map Length: " + beatmap.map.Length); // Debug
	}

	void Update () {
		if (!isPrepared) {
			return;
		}

//		if (!audioSource.isPlaying && audioSource.clip.loadState == AudioDataLoadState.Loaded) {
		if (!gameStarted && !audioSource.isPlaying && audioSource.clip.loadState == AudioDataLoadState.Loaded) {
			prepareTimer += Time.deltaTime;
			if(prepareTimer > timeToPrepare) {
				audioSource.Play ();
				audioSource.loop = false;
				StartGame();
	//			GameObject timelineCanvas = Instantiate (TimelineCanvasPrefab) as GameObject;
	//			timelineCanvas.transform.SetParent (transform);
			}
		}

		if (gameStarted) {
			if (i < beatmap.map.Length && (audioSource.time + noteTimeAppearToHit) > beatmap.map [i].t) { // TODO: note generate timing
				for (int j = 0; j < beatmap.map [i].notes.Length; j++) {
					switch (beatmap.map [i].notes [j].type) {
					case 1:
						// TODO: clean code
//						Sphere Mode
//						Vector3 pos = Random.onUnitSphere * sphereRadius;
//						pos.y = Mathf.Abs (pos.y);
//						pos.z = Mathf.Abs (pos.z);
//						pos += playerArmPosition;

//						Plane Mode
//						////////////////////////////
//						int randomWIndex = Random.Range (0, randomWidth);
//						int randomHIndex = Random.Range (0, randomHeight);
//						while (randomMap [randomWIndex, randomHIndex] != true) {
//							randomWIndex = Random.Range (0, randomWidth);
//							randomHIndex = Random.Range (0, randomHeight);
//						}
//						randomMap [randomWIndex, randomHIndex] = false;
//						Vector3 pos = new Vector3 (
//							(randomWIndex-(randomWidth/2)) * (noteRadius) * space,  // [-2, 3), i.e. {-2, -1, 0, 1, 2} // 5/2
//							(randomHIndex-(randomHeight/2)) * (noteRadius) * space,  // [-1, 2), i.e. {-1, 0, 1} // 3/2
//							planeDistance);
////						Vector3 pos = new Vector3 (
////							              Random.Range (-2, 3) * (noteRadius) * space,  // [-2, 3), i.e. {-2, -1, 0, 1, 2} // 5/2
////							              Random.Range (-1, 2) * (noteRadius) * space,  // [-1, 2), i.e. {-1, 0, 1} // 3/2
////							              planeDistance);
//						pos += playerArmPosition;
//						//GenerateNote (pos);
//						GenerateNote (pos, randomWIndex, randomHIndex);
//						////////////////
						{
							Vector3 pos = randompos.Generate (beatmap.map [i].t);
		//					Debug.Log (pos.ToString());
							pos += playerArmPosition;
							GenerateNote (pos);
						}
						break;
					case 2:
						{
							Vector3 pos = new Vector3(beatmap.map[i].notes[j].x, beatmap.map[i].notes[j].y, beatmap.map[i].notes[j].z);
							if (i + 1 < beatmap.map.Length) 
							{
								Vector3 nextPos = new Vector3 (beatmap.map [i + 1].notes [0].x, beatmap.map [i + 1].notes [0].y, beatmap.map [i + 1].notes [0].z);
								GenerateNote (pos, nextPos);
							}
							else
								GenerateNote (pos);
						}
						break;
					default:
						break;
					}
				}
				i++;
			}

			if (!audioSource.isPlaying) {
				gameObject.GetComponent<ScoreManagement> ().Submit ();
				Destroy (gameObject);
			}
		}
	}

//	void GenerateNote(Vector3 generatePosition = new Vector3(), int randomWIndex = -1, int randomHIndex = -1) {
//		GameObject newNote = Instantiate (noteObject, generatePosition, Quaternion.identity) as GameObject;
//		// TODO: clean code
//		//newNote.transform.LookAt(Camera.main.transform.position);
//		newNote.transform.LookAt(playerEyePosition);
//		//		newNote.transform.Rotate (90, 0, 0);
//		newNote.transform.localScale = newNote.transform.localScale * noteRadius;
//
//		newNote.transform.SetParent (gameObject.transform);
//		// TODO: note generate timing
//		newNote.GetComponent<NoteController>().timeAppearToHit = noteTimeAppearToHit;
//		newNote.GetComponent<NoteController> ().wIndex = randomWIndex;
//		newNote.GetComponent<NoteController> ().hIndex = randomHIndex;
//	}

	void GenerateNote(Vector3 generatePosition = new Vector3(),Vector3 nextGeneratePosition = new Vector3(), Quaternion generateRotation = new Quaternion()) {
		GameObject newNote = Instantiate (noteObject, generatePosition, generateRotation) as GameObject;
		// TODO: clean code
		//newNote.transform.LookAt(Camera.main.transform.position);
		newNote.transform.LookAt(playerEyePosition);
//		newNote.transform.Rotate (90, 0, 0);
		newNote.transform.localScale = newNote.transform.localScale * noteRadius;

		newNote.transform.SetParent (gameObject.transform);
		// TODO: note generate timing
		newNote.GetComponent<NoteController>().timeAppearToHit = noteTimeAppearToHit;



		if(nextGeneratePosition!=null)
		{
			GameObject nextDirectionHint = Instantiate (directionHintObject, generatePosition, generateRotation) as GameObject;
			nextDirectionHint.transform.SetParent (newNote.transform);
			nextDirectionHint.transform.localScale = new Vector3 (1, 1, 1);
//			dir=dir.normalized;
//			dir.z = 0.03f;
			nextDirectionHint.transform.position = nextGeneratePosition;
			/*
			nextDirectionHint.transform.localPosition = 0.4f*nextDirectionHint.transform.localPosition.normalized;
			nextDirectionHint.transform.localPosition=new Vector3(nextDirectionHint.transform.localPosition.x,nextDirectionHint.transform.localPosition.y,0.03f);*/

			nextDirectionHint.transform.localPosition = 0.4f* new Vector3(
				nextDirectionHint.transform.localPosition.x,
				nextDirectionHint.transform.localPosition.y,
				0).normalized;
			nextDirectionHint.transform.localPosition=new Vector3(nextDirectionHint.transform.localPosition.x,nextDirectionHint.transform.localPosition.y,0.03f);
				//		nextDirectionHint.transform.position = 0.4f * randompos.GetNextDirection();
		}
	}

	//public static BeatMap LoadBeatMap() {
	BeatMap LoadBeatMap() {
//		Debug.Log (dirPath + "/" + beatmapJson);
//		if( !File.Exists(dirPath + "/" + beatmapJson)) {
		Debug.Log (dirPath);
		Debug.Log (dirPath + beatmapJson);
		if( !File.Exists(dirPath + beatmapJson)) {
			Debug.Log ("File dose not exist.");
			return null;
		}

//		StreamReader sr = new StreamReader (dirPath + "/" + beatmapJson);
		StreamReader sr = new StreamReader (dirPath + beatmapJson);
		if (sr == null) {
			Debug.Log ("File open failed.");
			return null;
		}
		string json = sr.ReadToEnd ();
		if (json.Length > 0) {
			return JsonUtility.FromJson<BeatMap> (json);
		}
		Debug.Log ("Json Length Error!");
		Debug.Log (json.Length);
		return null;
	}

	void StartGame() {
		gameStarted = true;
	}

}
