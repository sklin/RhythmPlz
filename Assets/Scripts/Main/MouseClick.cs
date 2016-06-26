using UnityEngine;
using System.Collections;

public class MouseClick : MonoBehaviour {

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 1000)) {
				if (hit.transform.tag == "Note") {
					hit.transform.parent.GetComponent<NoteController> ().Hit ();
				}
			}
		}
	}
}
