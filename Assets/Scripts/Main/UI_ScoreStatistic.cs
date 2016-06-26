using UnityEngine;
using UnityEngine.UI;
using System.Collections;

class ScoreStatistic {
	public const int PERFECT = 0;
	public const int GOOD = 1;
	public const int BAD = 2;
	public const int MISS = 3;

	public int score;

	public int[] cnts;
	public int noteCnt;

	public float accuracy;
	public int maxCombo;
}

public class UI_ScoreStatistic : MonoBehaviour {

	ScoreStatistic statistic;
	public GameObject scoreText;
	public GameObject judgeText;

	public const int PERFECT = 0;
	public const int GOOD = 1;
	public const int BAD = 2;
	public const int MISS = 3;

	void Start() {
		Reset ();
	}

	void Update() {
		if (statistic != null) {
			scoreText.GetComponent<Text>().text = "Score: " + statistic.score + "\n";
			scoreText.GetComponent<Text>().text += "Acc.: " + statistic.accuracy.ToString ("P") + "\n";
			scoreText.GetComponent<Text>().text += "Acc. Score: " + (statistic.accuracy*1000000).ToString() + "\n";
			scoreText.GetComponent<Text>().text += "Max Combo: " + statistic.maxCombo + "\n";

			// TODO: Should clean this
			judgeText.GetComponent<Text>().text = "Perfect: " + statistic.cnts [PERFECT].ToString () + "\n";
			judgeText.GetComponent<Text>().text += "Good: " + statistic.cnts [GOOD].ToString () + "\n";
			judgeText.GetComponent<Text>().text += "Bad: " + statistic.cnts [BAD].ToString () + "\n";
			judgeText.GetComponent<Text>().text += "Miss: " + statistic.cnts [MISS].ToString () + "\n";
		}
	}

	void Reset() {
		scoreText.GetComponent<Text>().text = "Score: 0\n";
		scoreText.GetComponent<Text>().text += " Acc.: 100%\n";
		scoreText.GetComponent<Text>().text += " Acc. Score: 1000000\n";
		scoreText.GetComponent<Text>().text += " Max Combo: 0\n";

		judgeText.GetComponent<Text>().text = " Perfect: 0\n";
		judgeText.GetComponent<Text>().text += " Good: 0\n";
		judgeText.GetComponent<Text>().text += " Bad: 0\n";
		judgeText.GetComponent<Text>().text += " Miss: 0\n";

		statistic = null;
	}

	public void Set() {
		statistic = new ScoreStatistic ();
	}

	public void SetScore(int _score) {
		statistic.score = _score;
	}

	public void SetCnts(int[] _cnts) {
		statistic.cnts = _cnts;
	}

	public void SetAccuracy(float _accuracy) {
		statistic.accuracy = _accuracy;
	}

	public void SetMaxCombo(int _maxCombo) {
		statistic.maxCombo = _maxCombo;
	}

	public void SetNoteCnt(int _noteCnt) {
		statistic.noteCnt = _noteCnt;
	}
}