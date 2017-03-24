using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace _3DRunner
{
	public class PlayerRailController : MonoBehaviour
	{
		#region PRIVATE_VARIABLES

		private Terrain terrain;
		private Camera cameraObj;
		private SplineController playerSplineController;
		private SplineController wolfSplineController;
		private float currentOffset = 0;
		private float _leftFootDecay = 0;
		private float _rightFootDecay = 0;
		private float _timeWithoutContact = 0.5f;
		//this is the time needed for the feed until he touches the floor again
		private float _walkingTimeOut = 0;
		//this time "buffers" walking so the character has a slight sliding to smooth walking
		private float _walkingTimeOutMax = 1f;
		private bool _isWalking = false;
		private int numPoints;

		#endregion

		#region PUBLIC_VARIABLES

		public Floor.FloorControllerCSharp FloorCon;
		public GameObject wolf = null;
		public GameObject GameEnd = null;

		public float MaxOffset = 3;
		public float HeightOffset = 0;
		public float MovementSpeed = 12;

		public Button rightArrow = null;
		public Button leftArrow = null;

		public Button rightFoot = null;
		public Button leftFoot = null;

		public AudioSource _audio = null;

		public AudioClip[] _UISfx = null;
		public AudioClip[] _FootstepsSfx = null;

		#endregion

		void Awake ()
		{
			terrain = GameObject.Find ("NewTerrain").GetComponent<Terrain> ();
			numPoints = GameObject.Find ("PathObject").GetComponent<PathGenerator> ().numPoints;

			cameraObj = gameObject.GetComponent<Camera> ();
			playerSplineController = transform.parent.GetComponent<SplineController> ();
			wolfSplineController = wolf.GetComponent<SplineController> ();
		}

		void Start ()
		{
			InitSplineObjectsMovement ();

			//stop spline to move automatically
			wolfSplineController.Stop ();
			playerSplineController.Stop ();
		}

		void Update ()
		{
			if (Floor.FloorControllerCSharp.GetStatusOfReadingData ()) {
				this.GetPlayerInputFromFloor ();
			}

            //for testing in editor
			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
				this.PlayerMoveForward ();
			}

			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
				this.PlayerStrafeLeft ();
			}
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
				this.PlayerStrafeRight ();
			}
		}

		// Update is called once per frame
		void LateUpdate ()
		{
			if (GameController.GameStates == GameState.Runner) {
				Vector3 localPos = cameraObj.transform.localPosition;
				localPos.y += 10;
				localPos.y = terrain.SampleHeight (localPos) + HeightOffset;
				localPos.x = currentOffset;
				cameraObj.transform.localPosition = localPos;
			}
		}

		#region PLAYER_WALKING_GESTURE

		void InitSplineObjectsMovement ()
		{
			//only initiate running of wolf or player when game is in runner mode
			if (wolf != null)
				wolfSplineController.FollowSpline (OnPathEndsForWolf, null, null);
			
			playerSplineController.FollowSpline (OnPathEndsForPlayer, OnPathReachesNthNode, null);
            //Enable floor input
            Floor.FloorControllerCSharp.ResetData();
            Floor.FloorControllerCSharp.startReadingData = true;

			Debug.Log ("Init = FollowSpline");
		}

		// Callback for when Wolf object reaches end of path
		void OnPathEndsForWolf ()
		{
			//Wolf sitting animation
			if (!wolf.GetComponent<Animator> ().GetBool ("hasWon"))
				wolf.GetComponent<Animator> ().SetBool ("hasWon", true);

		}

		// Callback for when Player object reaches end of path
		void OnPathEndsForPlayer ()
		{
			Debug.Log ("Game Completed");
			GameController.GameStates = GameState.GameComplete;
		}

		void OnPathReachesNthNode (int i, SplineNode node)
		{
			if (i == numPoints - 5) {
				GameEnd.SetActive (true);
				Debug.Log ("Enabling tent");
			}
		}

		void PlayUIRandomSfx ()
		{
#if UNITY_EDITOR
            AudioClip _clip = _UISfx[UnityEngine.Random.Range(0, _UISfx.Length)] as AudioClip;
            if (!_audio.isPlaying)
                _audio.PlayOneShot(_clip, 0.25f);
#endif
        }

		void PlayFootStepsRandomSfx ()
		{
#if UNITY_EDITOR
            AudioClip _clip = _FootstepsSfx [UnityEngine.Random.Range (0, _FootstepsSfx.Length)] as AudioClip;

			if (!_audio.isPlaying)
				_audio.PlayOneShot (_clip, 0.25f);
#endif
        }


        public void GetPlayerInputFromFloor ()
		{
			if (FloorCon == null)
				FloorCon = GameObject.Find ("_GM and Singleton Scripts").GetComponent<Floor.FloorControllerCSharp> ();

			_leftFootDecay += Time.deltaTime;
			_rightFootDecay += Time.deltaTime;
			_walkingTimeOut -= Time.deltaTime;

			if (FloorCon != null) {
				/*We are starting first row item from bottom left (1x1), Item labeling as "FloorFlower_Rows#_Cols#"*/
				if (FloorCon.FloorInput.x == 1) {
					switch (FloorCon.FloorInput.y) {
					case 1:
						if (FloorCon.FloorInput.t2 >= FloorCon.StepValue || FloorCon.FloorInput.t3 >= FloorCon.StepValue) {
							this.PlayerStrafeLeft ();
						}
						break;
					case 2:
						if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue ||
						    FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue) {
							this.LeftFootActive ();
						}
						 if (FloorCon.FloorInput.t6 >= FloorCon.StepValue || FloorCon.FloorInput.t7 >= FloorCon.StepValue) {
							this.PlayerStrafeLeft ();
						}
						break;
					case 3:
						if (FloorCon.FloorInput.t2 >= FloorCon.StepValue || FloorCon.FloorInput.t3 >= FloorCon.StepValue) {
							this.PlayerStrafeRight ();
						}
						if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue ||
						    FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue) {
							this.RightFootActive ();
						}
						break;
					case 4:
						if (FloorCon.FloorInput.t6 >= FloorCon.StepValue || FloorCon.FloorInput.t7 >= FloorCon.StepValue) {
							this.PlayerStrafeRight ();
						}
						break;
					}
				}

			}
			//left feed on ground -> triggers data, right feet in air not -> walking. || other side
			if ((_leftFootDecay == 0 && _rightFootDecay >= _timeWithoutContact) || (_rightFootDecay == 0 && _leftFootDecay >= _timeWithoutContact)) {
				_isWalking = true;
				_walkingTimeOut = _walkingTimeOutMax;
			}
			//if player stands still (and no data triggers) or is triggering data too fast, he is not walking and the timeout happens 
			if (_walkingTimeOut <= 0f)
				_isWalking = false;

			//TODO: is walking feet highlight in UI
			if (_isWalking) {
				this.PlayerMoveForward ();
			}
            else if (!_isWalking)
            {
                this.PlayerStopMoving();
            }
        }

		#endregion

		#region PLAYER MOVEMENTS

		private void RightFootActive ()
		{
            rightFoot.Select();
            _rightFootDecay = 0;
            PlayFootStepsRandomSfx ();
			
		}

		private void LeftFootActive ()
		{
            leftFoot.Select();
            _leftFootDecay = 0;

            PlayFootStepsRandomSfx ();
			
		}

		public void PlayerStrafeLeft ()
		{

			leftArrow.Select ();
			PlayFootStepsRandomSfx ();

           // GameController.GameStates = GameState.LeftStrafe;
            currentOffset -= MovementSpeed * Time.deltaTime;
			// clamp offset
			currentOffset = Mathf.Clamp (currentOffset, -MaxOffset, MaxOffset);

		}

		public void PlayerStrafeRight ()
		{
			rightArrow.Select ();
			PlayFootStepsRandomSfx ();

           // GameController.GameStates = GameState.RightStrafe;

            currentOffset += MovementSpeed * Time.deltaTime;
			// clamp offset
			currentOffset = Mathf.Clamp (currentOffset, -MaxOffset, MaxOffset);
		}

		public void PlayerMoveForward ()
		{
			if (GameController.GameStates == GameState.Idle) {

				//set wolf animation state to running
				wolf.GetComponent<Animator> ().SetBool ("isRunning", true);
				if (!wolf.GetComponent<AudioSource> ().isPlaying)
					wolf.GetComponent<AudioSource> ().Play ();
				playerSplineController.Resume ();
				wolfSplineController.Resume ();
				GameController.GameStates = GameState.Runner;
			}
		}

		public void PlayerStopMoving ()
		{
			//Debug.Log ("Stop player spline movement");
			if (GameController.GameStates == GameState.Runner) {
				//set wolf animation state to idle
				wolf.GetComponent<Animator> ().SetBool ("isRunning", false);
				if (wolf.GetComponent<AudioSource> ().isPlaying)
					wolf.GetComponent<AudioSource> ().Stop ();
				playerSplineController.Stop ();
				wolfSplineController.Stop ();
				GameController.GameStates = GameState.Idle;
			}
		}
		#endregion
	}
}