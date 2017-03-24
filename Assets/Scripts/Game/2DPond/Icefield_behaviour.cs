using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


public class Icefield_behaviour : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	}

	public void Stomped ()
	{
		if (GameObject.Find ("flower1").GetComponent<IcePlateBehaviour> ().IsBroken) { // if the ice cracks then do fadeout
			StartCoroutine (FadeOut ());
		}
	}

	IEnumerator FadeOut ()
	{
		for (float f = 1f; f >= 0; f -= 0.01f) {
			Color c = this.GetComponent<SpriteRenderer> ().color;
			c.a = f;
			this.GetComponent<SpriteRenderer> ().color = c;
			yield return new WaitForSeconds (0.1f);
		}
	}
}
