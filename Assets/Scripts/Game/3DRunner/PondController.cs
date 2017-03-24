/*
PondController
This class handles pond, when player collides

*1. Trigger events
*2. Play Sfx
*3. Play some animation
*4. Destroy it after sometime
*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


namespace _3DRunner
{

	[RequireComponent (typeof(AudioSource))]
	public class PondController : MonoBehaviour
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
			_clip = Resources.Load (AssetsPath._fishPondSFXPath) as AudioClip;
			_audio = GetComponent<AudioSource> ();
		}


		private void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag.Equals ("Player")) {
				Debug.Log ("Fish Pond Collided with player");
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
            GameController.GameStates = GameState.FishPond2D;

            yield return new WaitForSeconds (1.0f);
			Debug.Log ("Loading fish pond scene....");

			LevelManager levelMng = GameObject.Find ("LevelManager").GetComponent<LevelManager> ();
			levelMng.DisableThe3DRun ();

            //These checks are safe to tackle issue of bumping into fishes and quiz at same time
            if (GameController.GameStates == GameState.QuizScene)
                SceneManager.UnloadScene("QuizScene");
            else if (GameController.GameStates == GameState.FishPond2D)
                SceneManager.LoadScene("FishPond2D", LoadSceneMode.Additive);
            else if (GameController.GameStates == GameState.Runner)
            {
                SceneManager.UnloadScene("QuizScene");
                SceneManager.UnloadScene("FishPond2D");

                levelMng.EnableThe3DRun("");
            }
        }

	}
}