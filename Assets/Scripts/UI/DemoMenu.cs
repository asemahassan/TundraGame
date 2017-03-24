using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoMenu : MonoBehaviour
{

	public GameObject _creditsPanel = null;
	public GameObject _blurrPanel = null;
	public AudioClip _menuClick = null;
	public AudioSource _audio = null;


    void Awake()
    {
       Cursor.visible = true;
    }

	IEnumerator LoadSceneWithDelay (string sceneName)
	{
		//wait for seconds after playing menu click
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("Just waited for 1 second...");
		SceneManager.LoadScene (sceneName);

	}

	void PlayMenuClick ()
	{
        if (!_audio.isPlaying)
			_audio.PlayOneShot (_menuClick, 0.25f);
    }

	public void SoundGame ()
	{ 
		PlayMenuClick ();
		StartCoroutine (LoadSceneWithDelay ("SoundGame2D"));
	}

	public void TundraGame ()
	{ 
		PlayMenuClick ();
		StartCoroutine (LoadSceneWithDelay ("MainMenu"));
	}

	public void QuitDemo ()
	{
		PlayMenuClick ();
		StartCoroutine (DelayQuit ());

	}

	private IEnumerator DelayQuit ()
	{
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("Just waited for 1 second...");
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit ();
		#endif
		
	}

	public void ShowCredits ()
	{
		PlayMenuClick ();

		//slide in panel of credits
		_blurrPanel.GetComponent<Image> ().enabled = true;

		if (_creditsPanel.GetComponent<Animator> ().enabled != true)
			_creditsPanel.GetComponent<Animator> ().enabled = true;

		bool isHidden = _creditsPanel.GetComponent<Animator> ().GetBool ("isHidden");
		if (isHidden) {
			_creditsPanel.GetComponent<Animator> ().SetBool ("isHidden", false);
		}
	}

	public void HideCredits ()
	{
		PlayMenuClick ();

		bool isHidden = _creditsPanel.GetComponent<Animator> ().GetBool ("isHidden");
		if (!isHidden)
			_creditsPanel.GetComponent<Animator> ().SetBool ("isHidden", true);

		//slide out panel of credits
		_blurrPanel.GetComponent<Image> ().enabled = false;
	}
}
