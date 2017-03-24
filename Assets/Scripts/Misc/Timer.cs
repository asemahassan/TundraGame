/*
 * Timer.cs
 *This class is just updating the timer on HUD,
 *Its timer counting up
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Timer : MonoBehaviour {

	#region PUBLIC_VARIABLES
	public Text timerText;
	#endregion

	#region PRIVATE_VARIABLES
	private float secondsCount;
	private int minuteCount;
	private int hourCount;
	#endregion

	#region UNITY_METHODS
	// Update is called once per frame
	void Update(){
		
		secondsCount += Time.deltaTime;

		string hours = string.Format("{0} {1}", hourCount, " h: ");
		string minutes = string.Format("{0} {1}", minuteCount, " m: ");
		string seconds = string.Format("{0} {1}", (int)secondsCount, " s");

		timerText.text = hours + minutes + seconds;

		if(secondsCount >= 60)
		{
			minuteCount++;
			secondsCount = 0;
		}
		else if(minuteCount >= 60)
		{
			hourCount++;
			minuteCount = 0;
		}    
	}
	#endregion
}



