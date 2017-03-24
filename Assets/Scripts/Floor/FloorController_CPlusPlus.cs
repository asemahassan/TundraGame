/*FloorController_CPlusPlus (NOT USING ANYMORE FOR THIS PROTOTYPE)
 * 
 * Takes data from C++ plugin into struct representing one flower
 * Checks step value on each petal, representing pressure should be more than 127
 * 
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Runtime.InteropServices;


namespace Floor
{
	public class FloorController_CPlusPlus : MonoBehaviour
	{
		private static FloorData _floorData;
		private static FloorSize _floorSize;

		public static bool startReadingData = false;

		//This method returns the pressed petal index for any flower.
		public int GetPressedPetalIndex ()
		{
			int petalIndex = 0;

			if (FloorInput.t1 >= StepValue)
				petalIndex = 1;
			else if (FloorInput.t2 >= StepValue)
				petalIndex = 2;
			else if (FloorInput.t3 >= StepValue)
				petalIndex = 3;
			else if (FloorInput.t4 >= StepValue)
				petalIndex = 4;
			else if (FloorInput.t5 >= StepValue)
				petalIndex = 5;
			else if (FloorInput.t6 >= StepValue)
				petalIndex = 6;
			else if (FloorInput.t7 >= StepValue)
				petalIndex = 7;
			else if (FloorInput.t8 >= StepValue)
				petalIndex = 8;

			return petalIndex;
		}

		/// <summary>
		/// Returns state of given petal index (1 to 8) if last value from device was >= StepValue
		/// careful! - min index is 1, not 0
		/// </summary>
		public bool IsPetalStepped (int petalIndex)
		{
			switch (petalIndex) {
			case 1:
				return FloorInput.t1 >= StepValue;
			case 2:
				return FloorInput.t2 >= StepValue;
			case 3:
				return FloorInput.t3 >= StepValue;
			case 4:
				return FloorInput.t4 >= StepValue;
			case 5:
				return FloorInput.t5 >= StepValue;
			case 6:
				return FloorInput.t6 >= StepValue;
			case 7:
				return FloorInput.t7 >= StepValue;
			case 8:
				return FloorInput.t8 >= StepValue;
			
			default: 
				Debug.LogWarning ("FloorController.IsPetalStepped(): invalid petal index " + petalIndex);
				return false;
			}
		}

		//This method returns the check on petal that has reached threshold of StompValue
		public bool IsPetalStomped (int petalIndex)
		{
			switch (petalIndex) {
			case 1:
				return FloorInput.t1 >= StompValue;
			case 2:
				return FloorInput.t2 >= StompValue;
			case 3:
				return FloorInput.t3 >= StompValue;
			case 4:
				return FloorInput.t4 >= StompValue;
			case 5:
				return FloorInput.t5 >= StompValue;
			case 6:
				return FloorInput.t6 >= StompValue;
			case 7:
				return FloorInput.t7 >= StompValue;
			case 8:
				return FloorInput.t8 >= StompValue;

			default: 
				Debug.LogWarning ("FloorController.IsPetalStomped(): invalid petal index " + petalIndex);
				return false;
			}
		}

		public struct FloorData
		{
			public float x, y;
			public float t1, t2, t3, t4, t5, t6, t7, t8;
		};

		public struct FloorSize
		{
			public int rows;
			public int cols;
		};

		public FloorData FloorInput {
			get { return _floorData; }
			set { }
		}

		public FloorSize floorSize {
			get { return _floorSize; }
			set { }
		}


		/// <summary>
		/// this value is needed to trigger button
		/// </summary>
		public int StepValue {
			get { return 3 + 127; }
			private set { }
		}

		/// <summary>
		/// this value is the threshhold to measure a stomping gesture
		/// </summary>
		public int StompValue {
			get { return 20 + 127; }
			private set { }
		}

		//to read a struct from C++ to C#
		[DllImport ("TactileFloorOpenCV", CallingConvention = CallingConvention.Cdecl)]
		private static extern FloorData GetDataFromFloorIntoUnity ();

		[DllImport ("TactileFloorOpenCV", CallingConvention = CallingConvention.Cdecl)]
		private static extern FloorSize GetFloorSizeIntoUnity ();

		[DllImport ("TactileFloorOpenCV")]
		private static extern IntPtr InitializeFileSetup ();

		#region UNITY_METHODS

		// Use this for initialization
		void Start ()
		{
			_floorData = new FloorData ();
			_floorSize = new FloorSize ();

			//Should be called in start of game  to setup all flowers in scene accordingly
			//	SetupFloor ();

			#if !UNITY_EDITOR_OSX
            Debug.Log("Connection with OpenCV: " + Marshal.PtrToStringAnsi(InitializeFileSetup()));
			#endif  
		}

		public static void ResetData ()
		{
			_floorData.x = 0;
			_floorData.y = 0;
			_floorData.t1 = _floorData.t2 = _floorData.t3 = _floorData.t4 = _floorData.t5 = _floorData.t6 = _floorData.t7 = _floorData.t8 = 0;
		}

		void Update ()
		{
			//start
			if (Input.GetKey (KeyCode.S) && startReadingData == false) {
				Image debugHint = GameObject.Find ("DebugRunningHint").GetComponent<Image> ();
				debugHint.color = new Color (1, 0, 0, debugHint.color.a);
				startReadingData = true;
 
			}
			//stop
			if (Input.GetKey (KeyCode.D) && startReadingData == true) {
				Image debugHint = GameObject.Find ("DebugRunningHint").GetComponent<Image> ();
				debugHint.color = new Color (0, 1, 0, debugHint.color.a);
				startReadingData = false;
			}

			//Read data only when toggled
			if (startReadingData) {
				GetFloorData ();
			}
		}

		public static bool GetStatusOfReadingData ()
		{
			return startReadingData;
		}

		#endregion

		#region FLOOR_DATA

		public void SetupFloor ()
		{
			//Get floor size defined in plugin
			FloorSize floorSize = new FloorSize ();

			floorSize = GetFloorSizeIntoUnity ();
			Debug.Log ("Floor size Rows: " + floorSize.rows + " Cols: " + floorSize.cols);

		}

		//Some methods to get floor data and do convertions for Unity Vector3
		public void GetFloorData ()
		{

			//Get floor data for each flower 
			FloorData newData = new FloorData ();

			// Debug.Log("Message from C++ Plugin: " + Marshal.PtrToStringAnsi(GetDataFromFloorIntoUnity()));

			newData = GetDataFromFloorIntoUnity ();
			Debug.Log ("Floor data from Plugin--> X: " + _floorData.x +
			" Y: " + newData.y +
			" T1: " + newData.t1 +
			" T2: " + newData.t2 +
			" T3: " + newData.t3 +
			" T4: " + newData.t4 +
			" T5: " + newData.t5 +
			" T6: " + newData.t6 +
			" T7: " + newData.t7 +
			" T8: " + newData.t8);

			/*
            if ( (_floorData.x == newData.x && _floorData.y == newData.y)
                || (_floorData.x == 0 && _floorData.y == 0) )
            {
                _floorData.t1 = Mathf.Max(_floorData.t1, newData.t1);
                _floorData.t2 = Mathf.Max(_floorData.t2, newData.t2);
                _floorData.t3 = Mathf.Max(_floorData.t3, newData.t3);
                _floorData.t4 = Mathf.Max(_floorData.t4, newData.t4);
                _floorData.t5 = Mathf.Max(_floorData.t5, newData.t5);
                _floorData.t6 = Mathf.Max(_floorData.t6, newData.t6);
                _floorData.t7 = Mathf.Max(_floorData.t7, newData.t7);
                _floorData.t8 = Mathf.Max(_floorData.t8, newData.t8);
            }
            */
			/*
            {
                _floorData.t1 = Mathf.Max(_floorData.t1, newData.t1);
                _floorData.t2 = Mathf.Max(_floorData.t2, newData.t2);
                _floorData.t3 = Mathf.Max(_floorData.t3, newData.t3);
                _floorData.t4 = Mathf.Max(_floorData.t4, newData.t4);
                _floorData.t5 = Mathf.Max(_floorData.t5, newData.t5);
                _floorData.t6 = Mathf.Max(_floorData.t6, newData.t6);
                _floorData.t7 = Mathf.Max(_floorData.t7, newData.t7);
                _floorData.t8 = Mathf.Max(_floorData.t8, newData.t8);
            }
            */

			_floorData = newData;
			switch (SceneManager.GetActiveScene ().name) {
			case "":
			default:
				break;
			}
		}

		#endregion
	}
}
