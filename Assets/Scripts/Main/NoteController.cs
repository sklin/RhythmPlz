using UnityEngine;
using System.Collections;

// Control a specified note
public class NoteController : MonoBehaviour {

	// Stage Info:
	//         1      2
	//   |-----------|-|
	//   a           b c
	// a: Appear, b: Hit, c: Disappear
	// By default:
	// - Appear-Hit: 4.0 sec   (timeToHit)
	// - Hit-Dispear: 0.1 sec  (timeToDisappear)

	//        1       2  3  4  5  6
	//   |----------|--|--|--|--|
	//   a                b     c
	//
	// a: Appear, b: Hit, c: Disappear
	//
	// 1: Bad
	// 2: Good
	// 3: Perfect
	// 4: perfect
	// 5: Bad
	// 6: Miss

	public float timeAppearToHit = 0.6f; // equals to timeBad+timeGood+timePerfect1
	public float timeHitToDisappear = 0.1f; // equals to timePerfect2+timeBad2

	public float timeBad = 0.3f;
	public float timeGood = 0.25f;
	public float timePerfect1 = 0.05f;
	public float timePerfect2 = 0.05f;
	public float timeBad2 = 0.05f;

	public GameObject childOutter;
	public GameObject childInner;

	private float startTime;
	public float timeToDisappear;
	private Vector3 originalScale;

	private ScoreManagement scoreManagement; // used for scoring

	public GameObject missParticleSystemPrefab;
	public GameObject hitParticleSystemPrefab;

	public int wIndex = -1, hIndex = -1;

	void Start () {
		startTime = Time.time;
		timeToDisappear = timeAppearToHit + timeHitToDisappear;
		originalScale = childOutter.transform.localScale;
		scoreManagement = transform.parent.GetComponent<ScoreManagement> ();
	}
	
	void Update () {
		if (Time.time - startTime > timeToDisappear) {
			Debug.Log ("Miss");
			scoreManagement.UpdateScore (ScoreManagement.MISS);
			GameObject missParticleSys = Instantiate(missParticleSystemPrefab, transform.position, transform.rotation) as GameObject;
			Destroy (missParticleSys, 0.4f);
			Destroy (gameObject); // comment for development
		}
		childOutter.transform.localScale = Vector3.Lerp(originalScale, new Vector3 (1, 0.02f, 1), (Time.time - startTime) / timeAppearToHit);
	}

	public bool Hit() {
		if (Time.time - startTime < 0.2f) {
			return false;
		}

		Debug.Log (Time.time - startTime);
		int result = Judge ();
		scoreManagement.UpdateScore (result);
		Destroy (gameObject);
		return true;
	}

	int Judge() {
		float t = Time.time - startTime;
		if (t < timeBad) { // Bad
			Debug.Log ("Bad");
			return ScoreManagement.BAD;
		} else if (t < timeBad + timeGood) { // Good
			Debug.Log ("Good");
			return ScoreManagement.GOOD;
		} else if (t < timeBad + timeGood + timePerfect1 + timePerfect2) { // Perfect
			GameObject hitParticleSys = Instantiate(hitParticleSystemPrefab, transform.position, transform.rotation) as GameObject;
			Destroy (hitParticleSys, 0.4f);
			Debug.Log ("Perfect");
			return ScoreManagement.PERFECT;
		} else if (t < timeBad + timeGood + timePerfect1 + timePerfect2 + timeBad2) { // Bad
			Debug.Log ("Bad");
			return ScoreManagement.BAD;
		} else { // TODO: Unknown stage
			Debug.Log ("WTF");
			return ScoreManagement.BAD;
		}
	}

	void OnDestroy() {
		if (wIndex != -1 && hIndex != -1) { // TODO: clean code
			if (FindObjectOfType<NoteGenerator> ()) {
				FindObjectOfType<NoteGenerator> ().randomMap [wIndex, hIndex] = true;
			}
		}
	}
}
