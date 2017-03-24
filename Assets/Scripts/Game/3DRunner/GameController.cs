/*GameController:

This class handles
* HUD data health and score
* GameStates Runner to Idle
* Wolf and player animation controller
* VO and Sfx
* Health maximum limit, health decrease rate over time
* Right and Left arrow to strafe in game
*/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _3DRunner
{

	public class GameController : SingletonBehaviour<GameController>
	{
		#region PUBLIC_VARIABLES

		public static GameState GameStates = GameState.None;
		public static RawImage PoisonedImage = null;
		public GameObject DialogBox = null;
		public AudioSource _audio = null;
		public List<GameObject> animalsPopList = new List<GameObject> ();
		public PlayerRailController playerRailController;

		public float Health {
			get { return _health; }
			private set { }
		}

		#endregion

		#region PRIVATE_VARIABLES

		private float maxHealth = 200;
		private float _health;


		#endregion

		#region UNITY_METHODS

		void Awake ()
		{
			_health = maxHealth;

			GameStates = GameState.Idle;
			GameObject poisonImgObj = GameObject.Find ("PoisonImage");
			if (poisonImgObj != null)
				PoisonedImage = poisonImgObj.GetComponent<RawImage> ();

			//invoke wolf sound in background after random time
			InvokeRepeating ("PlayWolfVO", 7.0f, UnityEngine.Random.Range (15.0f, 50.0f));
		}

		// Use this for initialization
		void Start ()
		{
			//show cursor on screen all the time
			ShowCursorOnScreen ();
		}

		// Update is called once per frame
		void Update ()
		{
			updatePlayerHealth ();

			//GAME STATES
			if (GameStates == GameState.GameOver) {

				Debug.Log ("You have lost it");
				if (DialogBox != null) {
					DialogBox.SetActive (true);
					Text msg = DialogBox.transform.FindChild ("DialogText").GetComponent<Text> ();
					//msg.text = "GAME OVER" + "\n" + "You lost!\n\nTry eating less red berries!";
					msg.text = "GAME OVER" + "\n" + "Verloren!\n\nVersuche, roten Beeren auszuweichen!";
					PlayGameOverSFX ();
				}
				GameStates = GameState.None;

				//Go back to main menu after few seconds
				StartCoroutine (LoadSceneAfterDelay ("MainMenu"));

			} else if (GameStates == GameState.GameComplete) {
				Debug.Log ("Game Completed");
				if (DialogBox != null) {
					DialogBox.SetActive (true);
					Text msg = DialogBox.transform.FindChild ("DialogText").GetComponent<Text> ();
					//msg.text = "GAME COMPLETE" + "\n" + "You won!\n\nYou did great! Want to play again?";
					msg.text = "GEWONNEN" + "\n" + "Du hast gewonnen!\n\nDu warst super! Willst du nochmal spielen?";
					PlayWONSFX ();

					//take to the new scene for cinematic and then go to Highscore
				}
				GameStates = GameState.None;

				//Go back to main menu after few seconds
				StartCoroutine (LoadSceneAfterDelay ("Highscore", () => {
					EventManager.Instance.TriggerEvent (new GameOverEvent (UI.HUDController.PlayerName), 0.1f);
				}));

			}

            // close on ESC
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("SensFloor_DEMO");
            }

        }

		void PlayWONSFX ()
		{

			AudioClip _clip = Resources.Load (AssetsPath._wonSfxPath) as AudioClip;
			_audio.PlayOneShot (_clip, 0.25f);

		}

		void PlayWolfVO ()
		{
			AudioClip _clip = Resources.Load (AssetsPath._wolfSFXPath) as AudioClip;
			_audio.PlayOneShot (_clip, 0.25f);
		}

		void PlayGameOverSFX ()
		{

			AudioClip _clip = Resources.Load (AssetsPath._gameOverSfxPath) as AudioClip;
			_audio.PlayOneShot (_clip, 0.25f);

		}

		IEnumerator LoadSceneAfterDelay (string sceneName, Action callback = null)
		{
			yield return new WaitForSeconds (5.0f);
			SceneManager.LoadScene (sceneName);
			if (callback != null)
				callback ();
		}

		void OnEnable ()
		{
			if (EventManager.Instance != null) {

				EventManager.Instance.AddEventListener (OnItemCollected, ItemCollectedEvent.Identity);
				EventManager.Instance.AddEventListener (OnFishCollected, FishCollectedEvent.Identity);
			}
		}

		void OnDisable ()
		{
			if (EventManager.Instance != null) {
				EventManager.Instance.RemoveListener (OnItemCollected, ItemCollectedEvent.Identity);
				EventManager.Instance.RemoveListener (OnFishCollected, FishCollectedEvent.Identity);
			}
		}

		#endregion

		#region OBJECTS_GENERATOR

		private void PopInAnimals ()
		{

			if (GameStates == GameState.Runner) {

				if (animalsPopList.Count > 0) {
					//pick randomly one after random amount of time
					int randAnim = UnityEngine.Random.Range (0, animalsPopList.Count);
					GameObject animalSel = animalsPopList [randAnim];
					animalSel.GetComponent<AnimalSpriteController> ().Init ();
				}
				Invoke ("PopInAnimals", 10.0f);
			}
		}

		#endregion

		#region HUD_DATA

		/// <summary>
		/// Call this function to update the player's health
		/// </summary>
		private void updatePlayerHealth ()
		{
			_health -= 0.01f;
			UI.HUDController.UpdateHealth (_health);
			if (_health <= 0) {
				GameStates = GameState.GameOver;
			}
		}

		public void OnItemCollected (IEvent EventObject)
		{
			var evn = EventObject as ItemCollectedEvent;

			if (_health < maxHealth) {
				_health += evn.HealthValue;
				if (_health > maxHealth)
					_health = maxHealth;
			}
		}

		public void OnFishCollected (IEvent EventObject)
		{
			var evn = EventObject as FishCollectedEvent;
			_health += evn.HealthValue;
			Debug.Log ("FIshhealth added");
		}

		#endregion

		#region SCREEN CURSOR

		private void ShowCursorOnScreen ()
		{
			Cursor.visible = true;
			//Cursor.lockState = CursorLockMode.Locked;
		}

		#endregion
	}

}
