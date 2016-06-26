using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ScrollContentCreate : MonoBehaviour {
	/*
	struct Note{
	}*/

	public GameObject noteButton;
	//C
	DirectoryInfo beatmapsDirInfo;
	public const string beatmapsDir = "./Beatmaps/";
	public const string configDir = "./Config/";
	public string beatmapJson;
	private string dirPath; // Absolute Path, fetch by DirectoryInfo::Fullname
	private string beatmapDir;
	private BeatMap beatmap; // loaded from beatmap.json by LoadBeatMap()
	//V

	public GameObject NotePrefab;
	public GameObject Display;
	bool IsCreateBucket=false;
	public GameObject AS;
	public GameObject mainCanvas;

	public List<NoteNode> noteList;
	int notePointerFront=0;
	int notePointerEnd;
//	Note[][] noteBucket;

	MusicControl _musicControl;

	// Use this for initialization
	void Start () {

//		noteList.Sort(
		//C
		beatmapsDirInfo = new DirectoryInfo (beatmapsDir);
		//V
		/*
		beatmaps = new string[] {"aaa", "bbb", "ccc","ASD","SDJI","ASIJ","aaa", "bbb", "ccc","ASD","SDJI","ASIJ","aaa", "bbb", "ccc","ASD","SDJI","ASIJ"};

		for (int i=0; i<beatmaps.Length; i++) {
			GameObject newButton = Instantiate (noteButton)as GameObject;
			newButton.transform.SetParent (gameObject.transform,false);
			newButton.GetComponent<RectTransform> ().position = Vector3.zero;
			newButton.GetComponentInChildren<Text>().text=beatmaps[i];
		}
//		noteButton.GetComponent<LayoutElement> ().minHeight;
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
			gameObject.GetComponent<RectTransform>().sizeDelta.x,
			noteButton.GetComponent<LayoutElement>().minHeight * beatmaps.Length);
			*/
	}

	// Update is called once per frame
	void Update () {
		if (IsCreateBucket == true) 
		{
			float nowT = AS.GetComponent<AudioSource> ().time;
			for (int i = notePointerFront; i < noteList.Count; i++) {
				if (!noteList [i]._alive) 
				{
					float temp = noteList [i].time - nowT;
					if (temp < 0)
						temp = -temp;
					if (temp < 1.0f) {
						GameObject newNote = Instantiate (NotePrefab)as GameObject;
						newNote.transform.SetParent (Display.transform, false);
						newNote.GetComponent<RectTransform> ().position = noteList [i].pos;
						DestroyInTime Des = newNote.GetComponent<DestroyInTime> ();
						Des.setVar (noteList [i].time, AS,i,gameObject);
						noteList [i]._alive = true;

					}
				}
			}

			/*
			for (int i = (int)(_musicControl.AS.time) - 2; i < (int)(_musicControl.AS.time) + 2; i++) 
			{
				for (int j = 0; j < noteBucket [i].Count; j++) 
				{
					GameObject newNote = Instantiate (NotePrefab)as GameObject;
					newNote.transform.SetParent(Display,false);
					newNote.GetComponent<RectTransform>().position=noteBucket[i].
				}



				/*for(int k=0;k<noteBucket[i].Length;k++)
					noteBucket[k][0].pos=noteBucket[k][0].pos;
				*/
			/*}*/

		}
	
	}
	public void CreateButtonContent(BeatMap beatmaps){
		for (int i=0; i<beatmaps.map.Length; i++) 
			for (int j = 0; j < beatmap.map [i].notes.Length; j++)
			{
				Vector3 tempV=new Vector3(beatmaps.map[i].notes[j].x,beatmaps.map[i].notes[j].y,beatmaps.map[i].notes[j].z);
				GameObject newButton = Instantiate (noteButton)as GameObject;
				newButton.transform.SetParent (gameObject.transform,false);
				newButton.GetComponent<RectTransform> ().position = Vector3.zero;
				newButton.GetComponentInChildren<Text>().text=beatmaps.map[i].t.ToString()+","+beatmaps.map[i].notes[j].type.ToString();
				newButton.GetComponent<Button> ().onClick.AddListener (delegate {
					// menuCanvas.GetComponent<MenuManager> ().ToggleMenu(); move the GameController.StartGame()
					AS.GetComponent<MusicControl>().JumpAndPause(
						float.Parse(newButton.GetComponentInChildren<Text> ().text.Substring(0,
							newButton.GetComponentInChildren<Text> ().text.IndexOf(','))),
						newButton.transform.GetSiblingIndex(),
						tempV
					);
				});

			}
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
			gameObject.GetComponent<RectTransform>().sizeDelta.x,
			noteButton.GetComponent<LayoutElement>().minHeight * beatmap.map.Length);

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

	public void Prepare(string _path) {
		
		//dirPath = new DirectoryInfo (ListControl.beatmapsDir + beatmapDir).FullName;
//		dirPath=_path;

		beatmapDir = _path.Substring (0, _path.IndexOf ('/')) + '/';
		beatmapJson = _path.Substring (_path.IndexOf ('/') + 1);
		Debug.Log (beatmapDir);
		Debug.Log (beatmapJson);


		dirPath = new DirectoryInfo (ListControl.beatmapsDir+beatmapDir).FullName;
		Debug.Log (dirPath);
		beatmap = LoadBeatMap ();


		InputField[] inpF;
		inpF=mainCanvas.GetComponentsInChildren <InputField>();
		for (int i = 0; i < inpF.Length; i++) 
		{
			if (inpF [i].name == "InputName") {
				inpF [i].placeholder.GetComponent<Text>().text = beatmap.name;
			}

			else if (inpF [i].name == "InputAuthor") {
				inpF [i].placeholder.GetComponent<Text>().text = beatmap.author;
			}

			else if (inpF [i].name == "InputMusic") {
				inpF [i].placeholder.GetComponent<Text>().text = beatmap.music;
			}
		}






//		WWW www = new WWW("file://" + dirPath + beatmap.music);
		string ASname="file://" + dirPath + beatmap.music;
//		audioClip = www.audioClip;
		MusicControl mc=AS.GetComponent<MusicControl>();
		_musicControl = AS.GetComponent<MusicControl> ();
		mc.PrepareMusic (ASname);

		if (mc.AS.clip == null) {
			Debug.Log ("Load audioClip failed.");
		} else {
			Debug.Log ("Load audioClip success");
		}

		mc.AutoPlay ();
		CreateButtonContent (beatmap);

//		isPrepared = true;
//		Debug.Log (isPrepared);
	}


	BeatMap LoadBeatMap() {
		
		if( !File.Exists(dirPath + beatmapJson)) {
			Debug.Log ("FAIL");
			Debug.Log (dirPath + beatmapJson);
			return null;
		}

		StreamReader sr = new StreamReader (dirPath + "/" + beatmapJson);
		if (sr == null) {

			Debug.Log ("NULL FAIL");
			return null;
		}
		string json = sr.ReadToEnd ();
		if (json.Length > 0) {
			Debug.Log ("SUCCESS");
			return JsonUtility.FromJson<BeatMap> (json);
		}
		return null;
	}

	public void CreateList(){
		/*
		MusicControl mc=AS.GetComponent<MusicControl>();
		//		while (!mc.AS.isPlaying);
		Debug.Log((int)(mc.AS.clip.length));
		Debug.Log(mc.AS.clip.length);*/

		noteList=new List<NoteNode>();

		for (int i = 0; i < beatmap.map.Length; i++) 
			for (int j = 0; j < beatmap.map [i].notes.Length; j++)
		{
				NoteNode _note=new NoteNode();
				_note.time=beatmap.map[i].t;
				_note.type = beatmap.map [i].notes [j].type;
				_note.pos =new Vector3(beatmap.map [i].notes [j].x,beatmap.map [i].notes [j].y,beatmap.map [i].notes [j].z);
				_note._alive = false;
				noteList.Add (_note);
				//noteBucket [(int)(beatmap.map[i].t)].Add(_note);
		}
		notePointerEnd = noteList.Count;
		IsCreateBucket = true;

	}
	public void PrepareOutput(){
		
		InputField[] inpF;
		inpF=mainCanvas.GetComponentsInChildren <InputField>();
		for (int i = 0; i < inpF.Length; i++) 
		{
			if (inpF [i].name == "InputName") {
				inpF [i].placeholder.GetComponent<Text>().text = beatmap.name;
				inpF [i].interactable = true;
			}

			else if (inpF [i].name == "InputAuthor") {
				inpF [i].placeholder.GetComponent<Text>().text = beatmap.author;
				inpF [i].interactable = true;
			}

			else if (inpF [i].name == "InputMusic") {
				inpF [i].placeholder.GetComponent<Text>().text = beatmap.music;
				inpF [i].interactable = true;
			}
		}
	}
	public void OutputData(){
		BeatMap outputBM;
		//		float outputT=0;
		//		Note outputNode;
		TimeTick outputTimeTick;
		outputBM=new BeatMap();
		InputField[] inpF;
		inpF=mainCanvas.GetComponentsInChildren <InputField>();
		for (int i = 0; i < inpF.Length; i++) 
		{
			if (inpF [i].name == "InputName") {
				if (inpF [i].text != "") {
					outputBM.name = inpF [i].text;
				} else {
					outputBM.name = inpF [i].placeholder.GetComponent<Text> ().text;
				}
				inpF [i].interactable = false;
			}

			else if (inpF [i].name == "InputAuthor") {
				if (inpF [i].text != "") {
					outputBM.author = inpF [i].text;
				} else {
					outputBM.author = inpF [i].placeholder.GetComponent<Text> ().text;
				}
				inpF [i].interactable = false;
			}

			else if (inpF [i].name == "InputMusic") {
				if (inpF [i].text != "") {
					outputBM.music = inpF [i].text;
				} else {
					outputBM.music = inpF [i].placeholder.GetComponent<Text> ().text;
				}
				inpF [i].interactable = false;
			}
		}


		List<TimeTick> outputMap;
		outputTimeTick = new TimeTick ();
		outputMap = new List<TimeTick> ();
		for(int i=0;i<noteList.Count;i++)
		{
			
			if(i!=noteList.Count-1)
			{
				Debug.Log (i+" "+noteList.Count);
				if ((noteList [i].time - noteList [i + 1].time)<0.001f&&(noteList [i].time - noteList [i + 1].time)>-0.001f) {
					outputTimeTick.notes = new Note[2];

					for (int p = 0; p < 2; p++) {
						outputTimeTick.notes[p]=new Note();
						outputTimeTick.notes [p].type = noteList [i + p].type;
						outputTimeTick.notes [p].x = noteList [i + p].pos.x;
						outputTimeTick.notes [p].y = noteList [i + p].pos.y;
						outputTimeTick.notes [p].z = noteList [i + p].pos.z;
						outputTimeTick.t = noteList [i + p].time;
					}
					i++;
				}
				else
				{
					Debug.Log (i+" "+noteList.Count+" SecF");
					outputTimeTick.notes=new Note[1];
					outputTimeTick.notes[0]=new Note();
//					Debug.Log(if(outputTimeTick.notes[0]));
					Debug.Log (outputTimeTick.notes[0].type);
					outputTimeTick.notes[0].type=
						noteList[i].type;
					outputTimeTick.notes[0].x=noteList[i].pos.x;
					outputTimeTick.notes[0].y=noteList[i].pos.y;
					outputTimeTick.notes[0].z=noteList[i].pos.z;
					outputTimeTick.t = noteList [i].time;

				}
			}
			else
			{
				Debug.Log (i+" "+noteList.Count+" "+noteList[i].time+" Sec");
				outputTimeTick.notes=new Note[1];
				outputTimeTick.notes[0]=new Note();
				outputTimeTick.notes[0].type=noteList[i].type;
				outputTimeTick.notes[0].x=noteList[i].pos.x;
				outputTimeTick.notes[0].y=noteList[i].pos.y;
				outputTimeTick.notes[0].z=noteList[i].pos.z;
				outputTimeTick.t = noteList [i].time;

			}
			outputMap.Add(outputTimeTick);
			outputTimeTick=new TimeTick();
		}
		outputBM.map=new TimeTick[outputMap.Count];
		for(int i=0;i<outputMap.Count;i++)
		{
			outputBM.map[i]=outputMap[i];
		}

		Debug.Log(JsonUtility.ToJson (outputBM, true));
		//File.WriteAllText
		using (FileStream fs = new FileStream(dirPath + "/" + beatmapJson, FileMode.Create)){
			using (StreamWriter writer = new StreamWriter(fs)){
				writer.Write(JsonUtility.ToJson (outputBM, true));
			   }
		}


	}


}
