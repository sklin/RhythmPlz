using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimelineBar : MonoBehaviour {
	
	public RectTransform timelineBar;
	public Text timeText;

	private AudioSource audioSource;
	private float timelineLength;

	private GameObject stageController = null;

	void Start() {
//		stageController = GameObject.FindWithTag ("StageController");
//		audioSource = stageController.GetComponent<AudioSource> ();
//
////		audioSource = transform.parent.GetComponent<AudioSource> ();
//		timelineLength = timelineBar.sizeDelta.x;
//
		timelineLength = timelineBar.sizeDelta.x;
		timeText.text = "";
	}

	void Update() {
		if (stageController == null) {
			stageController = GameObject.FindWithTag ("StageController");
			return;
		}

		if (audioSource == null) {
			audioSource = stageController.GetComponent<AudioSource> ();
		}

		if (audioSource != null) {
			float currentProgress = audioSource.time / audioSource.clip.length;
			timelineBar.sizeDelta = new Vector2 (currentProgress * timelineLength, timelineBar.sizeDelta.y);
			//Debug.Log ("Current Progress: " + currentProgress);

			timeText.text = audioSource.time.ToString ();
		}
	}
}
