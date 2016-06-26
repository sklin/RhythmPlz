using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ApplicationManager : MonoBehaviour {
	
	public void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
//		Application.LoadLevel (sceneName);
	}


	public void Quit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
