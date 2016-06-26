using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class Note {
	public int type;
	//public Vector3 pos;
	public float x;
	public float y;
	public float z;
}

[Serializable]
public class TimeTick {
	public float t;
	public Note[] notes;
}

[Serializable]
public class BeatMap {
	public string name;
	public string author;
	public string music;
	public TimeTick[] map;
}


//[Serializable]
//public class Note {
//	public float t;
//}
//
//[Serializable]
//public class BeatMap {
//	public string name;
//	public string author;
//	public string music;
//	public Note[] map;
//}
