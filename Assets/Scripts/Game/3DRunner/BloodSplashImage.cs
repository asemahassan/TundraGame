using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace _3DRunner
{

public class BloodSplashImage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowPoisionedImage()
	{
			this.GetComponent<RawImage> ().enabled = true;

			Invoke ("HidePoisionedImage", 1.0f);
	}

	public void HidePoisionedImage()
	{
			this.GetComponent<RawImage> ().enabled = false;
	}

	}
}
