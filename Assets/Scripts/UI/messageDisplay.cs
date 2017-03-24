using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class messageDisplay : MonoBehaviour
{

	public GameObject catchMessage = null;
	public GameObject infoMessage = null;

	// Use this for initialization
	void Start ()
	{
		if (infoMessage != null)
			infoMessage.SetActive (true);
		if (catchMessage != null)
			catchMessage.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (infoMessage != null)
				infoMessage.SetActive (false);
			if (catchMessage != null)
				catchMessage.SetActive (true);
		}
	}
}
