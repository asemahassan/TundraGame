using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimalSpriteController : MonoBehaviour {

	private AudioSource _audio;


	public Sprite OrgSprite = null;
	public Sprite BlinkSprite = null;

	public Vector2 inPos = Vector2.zero;
	public Vector2 outPos = Vector2.zero;

	public AudioClip _clip;
	public float Volume = 0.25f; 

	// Use this for initialization
	public void Init () {

		_audio = GetComponent<AudioSource> ();

		AnimateSpriteIN();
		StartCoroutine (ChangeSprite());
		Invoke ("AnimateSpriteOUT",3.0f);
	}

	 void AnimateSpriteIN(){
		RectTransform uGuiElement = this.GetComponent<RectTransform> ();
		iTween.ValueTo(uGuiElement.gameObject, iTween.Hash(
			"from", uGuiElement.anchoredPosition,
			"to", inPos,
			"time", 0.1f,
			"onupdatetarget", this.gameObject, 
			"onupdate", "MoveGuiElement"));
		_audio.PlayOneShot (_clip,Volume);
		
	}
		
	 void AnimateSpriteOUT(){

		CancelInvoke ("AnimateSpriteOUT");

		RectTransform uGuiElement = this.GetComponent<RectTransform> ();
		iTween.ValueTo(uGuiElement.gameObject, iTween.Hash(
			"from", uGuiElement.anchoredPosition,
			"to", outPos,
			"time", 0.25f,
			"onupdatetarget", this.gameObject, 
			"onupdate", "MoveGuiElement"));
	}

	void MoveGuiElement(Vector2 position){
		RectTransform uGuiElement = this.GetComponent<RectTransform> ();
		uGuiElement.anchoredPosition = position;
	}

	IEnumerator ChangeSprite()
	{
		for (float f = 1f; f >= 0; f -= 0.01f)
		{
			Sprite currentSprite = this.GetComponent<Image>().sprite;
			if(currentSprite.name.Equals(OrgSprite.name))
				this.GetComponent<Image>().sprite = BlinkSprite;
			else
				this.GetComponent<Image>().sprite = OrgSprite;
				
			yield return new WaitForSeconds(1.0f);
		}
	}
}
