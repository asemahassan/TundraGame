/*HUDController
* This class handles all HUD functions on UI elements
* PlayerName
* Score
* Health
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UI
{
	
	public class HUDController : MonoBehaviour
	{

		#region PUBLIC_VARIABLES

		public static string PlayerName {
			get {
				if (_playerNameText != null)
					return _playerNameText.text;
				else
					return "";
			}
			set {
				if (_playerNameText != null) {
					_playerNameText.text = value;
				}
					
			}
		}

		#endregion

		#region PRIVATE_VARIABLES

		private static Text _playerNameText = null;
		private static Text _scoreCounterText = null;
		public static GameObject HealthBarFill = null;
		private static GameObject _berryPointsPopUp = null;

		#endregion

		#region UNITY_METHODS

		//Find all UI Elements of HUD and assign to private variables
		void Awake ()
		{
			//Find all under HUD 
			_playerNameText = GameObject.Find ("PlayerName").GetComponent<Text> ();
			_scoreCounterText = GameObject.Find ("ScoreCount").GetComponent<Text> ();
			_berryPointsPopUp = GameObject.Find ("Points");

			HealthBarFill = GameObject.Find ("Fill");
		}

		// Use this for initialization
		void Start ()
		{
			_playerNameText.text = StatisticManager.Instance.CurrentPlayerName.ToUpper ();
		}

		#endregion

		#region HUD_CONTROLLER_STATIC

		public static void UpdateScore ()
		{
			if (_scoreCounterText != null) {
				_scoreCounterText.text = StatisticManager.Instance.Score.ToString ();
			}
		}

		public static void UpdateHealth (float count)
		{
			//health bar can decrease or increase according to the count value
			if (HealthBarFill != null)
				HealthBarFill.GetComponent<RectTransform> ().sizeDelta = new Vector2 (count * 2, HealthBarFill.GetComponent<RectTransform> ().sizeDelta.y);
		}


		public static void UpdateBerryPointsPopUP (string value)
		{

			if (_berryPointsPopUp != null) {
				GameObject popUp = (GameObject)Instantiate (_berryPointsPopUp, _berryPointsPopUp.transform.parent);
				popUp.tag = "Points";
				Text popUpText = popUp.GetComponent<Text> ();
				popUpText.enabled = true;
				popUpText.text = value;
				popUpText.GetComponent<Animation> ().Play ();
			}
		}

		#endregion

	}
}
