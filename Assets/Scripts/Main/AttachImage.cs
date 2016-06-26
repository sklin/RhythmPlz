using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class AttachImage : MonoBehaviour {
	
	string PathOfPic ="file://.\\Texture\\Title_Rhy.png";
//	string PathOfPic ="file://C:\\Users\\user\\Desktop\\1.png";
	public Texture tex;
	public GameObject rImage;
	public GameObject cube;    //CUBE START
	public Material Title_Rhy;
	public Material Title_Sel;

	float ImageChangeDelay=0;
	int ImageNow=0;
	// Use this for initialization
	void Start () {
		Debug.Log("Start");
		//		show_on_PC_picture ();
//		StartCoroutine(show_on_PC_picture());
		cube.GetComponent<Renderer>().material=Title_Rhy;
		ImageNow = 1;
	}

	// Update is called once per frame
	void Update () {
		ImageChangeDelay += Time.deltaTime;
		if (ImageChangeDelay > 3 && ImageNow == 2) {
			ImageChangeDelay = 0;
			cube.GetComponent<Renderer>().material=Title_Rhy;
//			PathOfPic = "file://.\\Texture\\Title_Rhy.png";
			ImageNow = 1;
//			StartCoroutine (show_on_PC_picture ());
		} else if (ImageChangeDelay > 7 && ImageNow == 1) {
			ImageChangeDelay = 0;
//			PathOfPic = "file://.\\Texture\\Title_Sel_to_Start.png";
			cube.GetComponent<Renderer>().material=Title_Sel;
			ImageNow = 2;
//			StartCoroutine (show_on_PC_picture ());
		}
		cube.transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);

	}

	private IEnumerator show_on_PC_picture()
	{
		Debug.Log(PathOfPic);

		WWW PC_picture = new WWW(PathOfPic);
		yield return PC_picture;//等待圖片路徑載入完成才繼續下面的步驟

		if( !File.Exists(PathOfPic)) {
			Debug.Log ("FAIL");
			//return null;
		}
		tex = PC_picture.texture;//載入圖片
		//show.mainTexture = picture;//顯示圖片
		cube.GetComponent<Renderer>().material.mainTexture=PC_picture.texture;
//		rImage.GetComponent<RawImage> ().texture = PC_picture.texture;
		Debug.Log("Load Complete");
		//		gb.GetComponent<Image> ().material.mainTexture = PC_picture.texture;
	}
}
