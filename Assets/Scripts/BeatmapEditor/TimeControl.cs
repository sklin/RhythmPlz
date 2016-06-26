using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimeControl : MonoBehaviour,IScrollHandler,IDragHandler,IEndDragHandler{
	//UI
	public Text ShowCurrentTime;
	public Scrollbar controller;
	bool isDrag;
	//UI END

	//AUDIO SOURCE
	public AudioSource music;
	float lengthOfAudio;
	//AUDIO SOURCE END


	// Use this for initialization
	void Start () {
		ShowCurrentTime.text="0";
//		music.Play ();
//		lengthOfAudio = music.clip.length;
		isDrag = false;
//		EventSystem.current = controller;
//		controller.onValueChanged.AddListener();
//		controller.OnDrag()=OnDrag();
//		EnterTime.text="0.000";
//		InputTime=float.Parse (EnterTime.text);
		//EventTrigger=controller.GetComponent<EventTrigger>();

	}
	
	// Update is called once per frame
	void Update () {
		if (music.clip != null) 
		{
			lengthOfAudio = music.clip.length;
			float temp;
			temp = music.time;
			string x = "";
			x = x + "Time:" + temp.ToString () + "   Length:" + lengthOfAudio.ToString ();
			//Debug.Log(x);
			ShowCurrentTime.text = x;
		}
		if(isDrag==false)
			controller.value = music.time / lengthOfAudio;	//UPDATE THE POSITION OF BAR
	}
	public void OnScroll(PointerEventData e){
		Debug.Log("A");
	}
	public void OnBeginDrag(PointerEventData e){
		Debug.Log("B");
		isDrag=true;

	}
	public void OnDrag(PointerEventData e){
		Debug.Log("O");
		music.time = (controller.value / 1) * lengthOfAudio;

	}
	public void OnEndDrag(PointerEventData e){
		Debug.Log("E");
		isDrag = false;
	}

}
