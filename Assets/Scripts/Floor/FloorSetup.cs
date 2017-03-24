/* FloorSetup
 * This class gets size of floor from floorcontroller and
 * generate flowers based on those values
 * For a sample, this is used with soundgame 
 * To test feedback of SensFloor and to draw visual flowers on it.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Floor
{

	public class FloorSetup : MonoBehaviour
	{

		public SoundGameMode soundMode = SoundGameMode.None;

		//fixed color values for this soundgame
		public Color[] colorValues = new Color[8];
		public AudioClip[] _colorAudioClips = new AudioClip[8];
		public AudioClip[] _pianoAudioClips = new AudioClip[8];

		public List<GameObject> generatedFlowers = new List<GameObject> ();

		public FloorControllerCSharp FloorCon;

		// offset for the x position of flowers in scene coordinates
		public float offset = 364.0f;
		public GameObject FlowerPrefab = null;

		//Starting position of first item, considering bottom left as item col=1 & row=1
		private Vector3 _initialPosForItem = new Vector3 (-19.0f, -368.0f, 0);

		private int rows, cols;

		// Use this for initialization
		void Start ()
		{
			if (FloorCon == null)
				FloorCon = GameObject.Find ("_GM and Singleton Scripts").GetComponent<FloorControllerCSharp> ();

			GenerateBasicFlowersOnFloor ();
			//initialization for soundgame setup
			SoundGameInitFlower ();
				
		}

		private void GenerateBasicFlowersOnFloor ()
		{
			if (FlowerPrefab == null)
				FlowerPrefab = Resources.Load (AssetsPath._flowerPrefabPath) as GameObject;

			rows = FloorCon.floorSize.rows;
			cols = FloorCon.floorSize.cols;

			Vector3 _newPosForItem = _initialPosForItem;
			string flowerName = "";

			generatedFlowers = new List<GameObject> ();

			//Starting first row item from bottom left (1x1), Item labeling as "FloorFlower_Rows#_Cols#"
			for (int r = 1; r <= rows; r++) {
				for (int c = 1; c <= cols; c++) {

					flowerName = "FloorFlower_" + r.ToString () + "_" + c.ToString ();

					//Create flower object and position on canvas with an offset
					GameObject flowerObj = Instantiate (FlowerPrefab) as GameObject;

					flowerObj.GetComponent<RectTransform> ().localPosition = _newPosForItem;
					flowerObj.GetComponent<RectTransform> ().localScale = new Vector3 (1.0f, 1.0f, 1.0f);

					flowerObj.name = flowerName;

					//SetParent using this method, otherwise issue with scaling of UI elements
					flowerObj.transform.SetParent (this.transform, false);

					generatedFlowers.Add (flowerObj);

					_newPosForItem = new Vector3 (_newPosForItem.x + offset, _newPosForItem.y, 0);
				}
				//for next row, add offset in Y position, considering + or - sign based on:  from which point you're starting the initial pos
				_newPosForItem = new Vector3 (_initialPosForItem.x, _newPosForItem.y + offset, _initialPosForItem.z);
			}
		}

		private void SoundGameInitFlower ()
		{

			if (generatedFlowers.Count > 0)
				for (int i = 0; i < generatedFlowers.Count; i++) {

					GameObject flowerObj = generatedFlowers [i] as GameObject;
					//Set audio clip for each flower
					TriangleController[] controllerList = flowerObj.GetComponentsInChildren<TriangleController> ();
					foreach (TriangleController triC in controllerList) {
						if (soundMode == SoundGameMode.Color)
							triC.sound = _colorAudioClips [i];
						else if (soundMode == SoundGameMode.Piano)
							triC.sound = _pianoAudioClips [i];
					}

					//Set color value for each petal in a specific flower
					foreach (Image img in flowerObj.GetComponentsInChildren<Image>()) {
						img.color = new Color (colorValues [i].r, colorValues [i].g, colorValues [i].b, img.color.a);
					}
					
				}
		}
	}
}