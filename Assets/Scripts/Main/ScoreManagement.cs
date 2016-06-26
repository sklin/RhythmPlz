using UnityEngine;
using System.Collections;

public class ScoreManagement : MonoBehaviour {

	public GUIText text;

	public const int PERFECT = 0; // TODO: SHOULD FIX
	public const int GOOD = 1;
	public const int BAD = 2;
	public const int MISS = 3;

	private int score = 0;

	public int perfectPoint = 1000;
	public int goodPoint = 7000;
	public int badPoint = 300;
	public int missPoint = 0;

	[Range(0f, 1f)]
	public float perfectAcc = 1.0f;
	[Range(0f, 1f)]
	public float goodAcc = 0.8f;
	[Range(0f, 1f)]
	public float badAcc = 0.5f;
	[Range(0f, 1f)]
	public float missAcc = 0.0f;

	private int[] _points;
	private float[] _accs;
	private int[] cnts;
	private int noteCnt;
	private float accuracy;
	private int combo;
	private int maxCombo;

	void Start() {
		score = 0;
		cnts = new int[4] { 0, 0, 0, 0 };
		_points = new int[4] { perfectPoint, goodPoint, badPoint, missPoint };
		_accs = new float[4] { perfectAcc, goodAcc, badAcc, missAcc };
		noteCnt = 0;
		accuracy = 1.0f;
		combo = 0;
		maxCombo = 0;

		text.text  = "Score: " + score.ToString() + "\n";
		text.text += " Perfect: " + cnts [PERFECT].ToString () + "\n";
		text.text += " Good: " + cnts [GOOD].ToString () + "\n";
		text.text += " Bad: " + cnts [BAD].ToString () + "\n";
		text.text += " Miss: " + cnts [MISS].ToString () + "\n";
		text.text += " Acc.: " + accuracy.ToString ("P") + "\n";
		text.text += " Acc. Score: " + (accuracy*1000000).ToString() + "\n";
		text.text += " Combo: " + combo + "\n";
		text.text += " Max Combo: " + maxCombo + "\n";
	}

	// Called when a note is hit or destroy by time
	// @type:
	//  0: perfect
	//  1: good
	//  2: bad
	//  3: miss
	public void UpdateScore(int type) {
		// Calculate accuracy
		if (noteCnt == 0) {
			accuracy = 1.0f;
		} else {
			accuracy = (cnts [PERFECT] * _accs[PERFECT] + cnts [GOOD] * _accs[GOOD] + cnts [BAD] * _accs[BAD] + cnts [MISS] * _accs[MISS])/noteCnt;
		}

		// Calculate combo
		if (type == MISS || type == BAD) {
			combo = 0;
		} else {
			if (combo == maxCombo) {
				maxCombo++;
			}
			combo++;
		}

		cnts [type]++;
		score += _points [type] * combo;
		noteCnt++;

		text.text  = "Score: " + score.ToString() + "\n";
		text.text += " Perfect: " + cnts [PERFECT].ToString () + "\n";
		text.text += " Good: " + cnts [GOOD].ToString () + "\n";
		text.text += " Bad: " + cnts [BAD].ToString () + "\n";
		text.text += " Miss: " + cnts [MISS].ToString () + "\n";
		text.text += " Acc.: " + accuracy.ToString ("P") + "\n";
		text.text += " Acc. Score: " + (accuracy*1000000).ToString() + "\n";
		text.text += " Combo: " + combo + "\n";
		text.text += " Max Combo: " + maxCombo + "\n";
	}

	public void Submit() {
		UI_ScoreStatistic scoreStatistic = FindObjectOfType<UI_ScoreStatistic> ();
		if (scoreStatistic) {
			scoreStatistic.Set ();
			scoreStatistic.SetAccuracy (accuracy);
			scoreStatistic.SetCnts (cnts);
			scoreStatistic.SetMaxCombo (maxCombo);
			scoreStatistic.SetNoteCnt (noteCnt);
			scoreStatistic.SetScore (score);
		}
	}
}
