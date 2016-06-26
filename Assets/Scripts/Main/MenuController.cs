using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	// This should be attached to every menu.

	private Animator _animator;
	private CanvasGroup _canvasGroup;

	public bool isOpen {
		get { return _animator.GetBool ("IsOpen"); }
		set { _animator.SetBool ("IsOpen", value); }
	}

	void Awake() {
		_animator = GetComponent<Animator> ();
		_canvasGroup = GetComponent<CanvasGroup> ();

		var rect = GetComponent<RectTransform> ();
		rect.offsetMax = rect.offsetMin = new Vector2 (0, 0);
	}

	void Update() {
		// To avoid the disabled menu to be click.
		if (!_animator.GetCurrentAnimatorStateInfo (0).IsName ("Open")) {
			_canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
		} else {
			_canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
		}
	}
}
