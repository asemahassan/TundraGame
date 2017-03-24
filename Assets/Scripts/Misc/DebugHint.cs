using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugHint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// just makes an assigned image blink to see if Unity is still running or has crashed
	void Update ()
    {
        // animate alpha of hint rectangle to show if and how app is still running
        Image img = GetComponent<Image>();
        Color imgColor = img.color;
        imgColor.a = (Mathf.Sin(Time.time * 4) + 1) * 0.5f;
        img.color = imgColor;
    }
}
