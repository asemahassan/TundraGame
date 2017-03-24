/*QuizController:

This class handles the questions asked to the player during game session about life in tundra.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace _3DRunner
{
	[RequireComponent (typeof(AudioSource))]
	public class QuizController : MonoBehaviour
	{
		#region PUBLIC_VARIABLES

		public float Volume = 0.25f;

		#endregion

		#region PRIVATE_VARIABLES

		private AudioClip _clip;
		private AudioSource _audio;

		#endregion

		#region UNITY_METHODS

		// Use this for initialization
		private void Start ()
		{
			_clip = Resources.Load (AssetsPath._pickupSFXPath) as AudioClip;
			_audio = GetComponent<AudioSource> ();
		}

		private void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag.Equals ("Player")) {
				_audio.PlayOneShot (_clip, Volume);

				//Load fish pond scene
				StartCoroutine (LoadSceneAfterDelay ());
				this.GetComponent<BoxCollider> ().enabled = false;
			}
		}

		#endregion

		IEnumerator LoadSceneAfterDelay ()
		{
			//pause the spline movements of wolf and player
			PlayerRailController playerCtrl = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerRailController> ();
			playerCtrl.PlayerStopMoving ();
            Debug.Log("Loading quiz scene....");
            GameController.GameStates = GameState.QuizScene;

            yield return new WaitForSeconds (1.0f);
			LevelManager levelMng = GameObject.Find ("LevelManager").GetComponent<LevelManager> ();
			levelMng.DisableThe3DRun ();

            //These checks are safe to tackle issue of bumping into fishes and quiz at same time
            if (GameController.GameStates == GameState.QuizScene)
			    SceneManager.LoadScene ("QuizScene", LoadSceneMode.Additive);
            else if(GameController.GameStates == GameState.FishPond2D)
                SceneManager.UnloadScene("FishPond2D");
            else if(GameController.GameStates == GameState.Runner)
            {
                SceneManager.UnloadScene("QuizScene");
                SceneManager.UnloadScene("FishPond2D");

                levelMng.EnableThe3DRun("");
            }

        }

	}
}
