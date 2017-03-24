using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SpriteSwap : MonoBehaviour
{
	public Sprite normalImg;
	public Sprite highlightedImg;
	public float delayTime = 1;

	public IEnumerator NormalSprite ()
	{
		yield return new WaitForSeconds (delayTime);
		Image img = this.GetComponent<Image> ();
		img.sprite = normalImg;
		StartCoroutine (HighlightedSprite ());
	}

 
	public IEnumerator HighlightedSprite ()
	{
		yield return new WaitForSeconds (delayTime);
		Image img = this.GetComponent<Image> ();
		img.sprite = highlightedImg;
		StartCoroutine (NormalSprite ());
	}
}
