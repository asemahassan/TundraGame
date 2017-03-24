/*MenuController
*The class handles main menu UI
**feedback elements of UI
***Sound effects on button click
*
1. Start Button
2. Exit Button
3. Highscore
*/
using UnityEngine;
using Floor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

namespace UI
{
	public class MenuController : MonoBehaviour
	{

		public GameObject instructionPanel = null;

		public Button highScoreBtn = null;
		public Button quitBtn = null;
		public Button startBtn = null;

		public Button leftFootBtn = null;
		public Button rightFootBtn = null;

		private Canvas _dialogCanvas;
		public FloorControllerCSharp FloorCon;

		public float delayTime = 2.0f;

		public AudioSource _menuAudioSource = null;
		public AudioClip[] _menuClips = null;

		#region UNITY_METHODS

		// Update is called once per frame
		void Update ()
		{

			#if UNITY_EDITOR

			if (_dialogCanvas.transform.childCount == 0) { //to check if the entername dialog panel is not open
				if (Input.GetKeyUp (KeyCode.S)) {
					startBtn.Select ();
					PlayMenuSfx ("Button");
					ShowInstructionsPanel ();
				}

				if (Input.GetKeyUp (KeyCode.Q)) {
					quitBtn.Select ();
					PlayMenuSfx ("Button");
					Invoke ("QuitGame", delayTime);
				}

				if (Input.GetKeyUp (KeyCode.H)) {
					highScoreBtn.Select ();
					PlayMenuSfx ("Button");
					Invoke ("HighScore", delayTime);
				}

				if (Input.GetKeyUp (KeyCode.L)) {
					leftFootBtn.Select ();
					PlayMenuSfx ("Left");
				}

				if (Input.GetKeyUp (KeyCode.R)) {
					rightFootBtn.Select ();
					PlayMenuSfx ("Right");
				}
			}
#endif

            if (Input.GetKey(KeyCode.Escape))
            {
                QuitGame();
            }
				
			
			#region ChecksOnSpecificPartOfFlower
			/*We are starting first row item from bottom left (1x1), Item labeling as "FloorFlower_Rows#_Cols#"*/
			if (FloorCon == null)
				FloorCon = GameObject.Find ("_GM and Singleton Scripts").GetComponent<FloorControllerCSharp> ();

            if (Floor.FloorControllerCSharp.GetStatusOfReadingData())
            {
                if (FloorCon.FloorInput.x == 1)
                {
                    switch ((int)FloorCon.FloorInput.y)
                    {
                        case 1:
                            if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue
                                || FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue)
                            {
                                highScoreBtn.Select();
                                PlayMenuSfx("Button");
                                Invoke("HighScore", delayTime);
                            }
                            break;
                        case 2:
                            if (FloorCon.FloorInput.t6 >= FloorCon.StepValue || FloorCon.FloorInput.t7 >= FloorCon.StepValue)
                            {
                                highScoreBtn.Select();
                                Invoke("HighScore", delayTime);
                            }
                            else if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue
                                     || FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue)
                            {
                                leftFootBtn.Select(); //just highlighting foot
                                PlayMenuSfx("Left");
                            }

                            break;
                        case 3:
                            if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue
                                || FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue)
                            {
                                rightFootBtn.Select(); //just highlighting foot
                                PlayMenuSfx("Right");
                            }
                            break;
                        case 4:
                            if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue
                                || FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue)
                            {
                                quitBtn.Select();
                                PlayMenuSfx("Button");
                                Invoke("QuitGame", delayTime);
                            }
                            break;
                    }
                }
                else if (FloorCon.FloorInput.x == 2)
                {
                    switch ((int)FloorCon.FloorInput.y)
                    {
                        case 2:
                            if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue
                                || FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue)
                            {

                                startBtn.Select();
                                PlayMenuSfx("Button");
                                ShowInstructionsPanel();
                            }
                            break;
                        case 3:
                            if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue
                                || FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue)
                            {
                                startBtn.Select();
                                PlayMenuSfx("Button");
                                ShowInstructionsPanel();
                            }
                            break;
                    }

                }
            }
			#endregion 
		}

		void OnEnable ()
		{
			Cursor.visible = false;
			if (_dialogCanvas == null)
				_dialogCanvas = GameObject.Find ("DialogCanvas").GetComponent<Canvas> ();
			if (Floor.FloorControllerCSharp.startReadingData == false)
				StartCoroutine (ActivateFloor ());

		}

		#endregion

		private void PlayMenuSfx (string clip)
		{
#if UNITY_EDITOR
            if (clip.Equals("Button"))
            {
                if(!_menuAudioSource.isPlaying)
                _menuAudioSource.PlayOneShot(_menuClips[0] as AudioClip);
            }
				
			else if (clip.Equals("Left"))
            {
                if (!_menuAudioSource.isPlaying)
                    _menuAudioSource.PlayOneShot(_menuClips[1] as AudioClip);
            }
			else if (clip.Equals ("Right"))
            {
                if (!_menuAudioSource.isPlaying)
                    _menuAudioSource.PlayOneShot(_menuClips[2] as AudioClip);
            }
#endif
        }

		IEnumerator ActivateFloor ()
		{
			yield return new WaitForSeconds (1f);
            //Enable flooor input
            Floor.FloorControllerCSharp.ResetData();
            Floor.FloorControllerCSharp.startReadingData = true;
		}

		private void ShowInstructionsPanel ()
		{
			if (instructionPanel.GetComponent<Animator> ().enabled != true)
				instructionPanel.GetComponent<Animator> ().enabled = true;

			bool isHidden = instructionPanel.GetComponent<Animator> ().GetBool ("isHidden");
			if (isHidden) {
				instructionPanel.GetComponent<Animator> ().SetBool ("isHidden", false);
			}
			Debug.Log ("StartGame button pressed");

			Invoke ("HideInstructionsPanel", 3.0f);
			Invoke ("StartGame", 4.0f);

		}

		private void HideInstructionsPanel ()
		{
			bool isHidden = instructionPanel.GetComponent<Animator> ().GetBool ("isHidden");
			if (!isHidden)
				instructionPanel.GetComponent<Animator> ().SetBool ("isHidden", true);
		}

		#region BUTTONS_CONTROL

		public void StartGame ()
		{

            Floor.FloorControllerCSharp.startReadingData = false;
            Floor.FloorControllerCSharp.ResetData();

			Cursor.visible = true;

			if (_dialogCanvas.transform.childCount == 0) {
				GameObject go = Resources.Load (AssetsPath._EnterNamePrefabPath) as GameObject;
				GameObject enterName = Instantiate (go) as GameObject;
				RectTransform rect = enterName.GetComponent<RectTransform> ();
				rect.anchoredPosition3D = new Vector3 (rect.anchoredPosition3D.x, -100.0f, rect.anchoredPosition3D.z);

				var enterNameDialogbehaviour = enterName.GetComponent<EnterNameBehaviour> ();

				enterNameDialogbehaviour.Init (() => {
					SceneManager.LoadScene ("Runner3D");
				});
				enterName.transform.SetParent (_dialogCanvas.transform, false);
			}
		}

		public void QuitGame ()
		{ //Go to Demo Menu
			SceneManager.LoadScene ("SensFloor_DEMO");
		}

		public void HighScore ()
		{
            Floor.FloorControllerCSharp.startReadingData = false;
            Floor.FloorControllerCSharp.ResetData();

            SceneManager.LoadScene ("Highscore");
		}

		#endregion
	}
}
