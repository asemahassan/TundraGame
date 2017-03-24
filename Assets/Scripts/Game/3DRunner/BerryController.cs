/*BerryController:

This class handles beryy, when player collides
1. TriggerEvents
2. Play Sfx
3. Destroy self after certain time.
*/

using UnityEngine;
using System.Collections;

namespace _3DRunner
{
	[RequireComponent (typeof(AudioSource))]
	public class BerryController : MonoBehaviour
	{
		#region PUBLIC_VARIABLE

		public BerriesType Type = BerriesType.None;
		public float DestroyTime = 0.5f;
		public float Volume = 0.25f;

		//for scaling animation
		public float RotationSpeed = 30;
		public float PumpSpeed = 2;
		public float PumpScale = 1.5f;

		#endregion

		#region PRIVATE_VARIABLES

		private AudioClip _clip;
		private AudioSource _audio;
		private int _value = 0;
		private int _healthValue = 0;

		#endregion

		#region UNITY_METHODS

		// Use this for initialization
		void Start ()
		{
			_audio = GetComponent<AudioSource> ();

			//Values subject to tweak, can also get from a Plist/Data class
			if (Type == BerriesType.Black) {
				_value = 10;
				_healthValue = 2;
				_clip = Resources.Load (AssetsPath._positiveBerrySfxPath) as AudioClip;
			} else if (Type == BerriesType.Red) {
				_value = -5;
				_healthValue = -10; //Adding negative effect to health
				_clip = Resources.Load (AssetsPath._negativeBerrySfxPath) as AudioClip;
			}
		}

		void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag.Equals ("Player")) {

				if (Type == BerriesType.Red) {
					//enabled poison effect image
					GameController.PoisonedImage.GetComponent<BloodSplashImage> ().ShowPoisionedImage ();
				}

				UI.HUDController.UpdateBerryPointsPopUP (_value.ToString ());
				_audio.PlayOneShot (_clip, Volume);

				StatisticManager.Instance.Score += _value;
				UI.HUDController.UpdateScore ();
				EventManager.Instance.TriggerEvent (new ItemCollectedEvent (_healthValue));

				if (this.gameObject.transform.parent != null)
					Destroy (this.gameObject.transform.parent.gameObject, DestroyTime);
				else
					Destroy (this.gameObject, DestroyTime);
			}
		}

		#endregion

	}
}
