using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class IcePlateBehaviour : MonoBehaviour
{
	public Image IceTexture;
	public List<Sprite> IceSprites;
	private int _hp;
	public bool IsBroken;
	public IceFadingBehaviour FadingBehave;

	// Use this for initialization
	void Start ()
	{
		_hp = 4;
		IsBroken = false;
		updateSprite ();
	}

	public void Stomped ()
	{
		_hp--;
		updateSprite ();
	}

	private void updateSprite ()
	{
		//Debug.Log ("Cracks are broken? " + IsBroken);
		switch (_hp) {
		case 4:
                // texture with no cracks at all
			IceTexture.sprite = IceSprites [0];
			break;
		case 3:
                // texture with small crack
			IceTexture.sprite = IceSprites [1];
			break;
		case 2:
                // change to texture with medium crack
			IceTexture.sprite = IceSprites [2];
			break;
		case 1:
                // change to nearly broken texture
			IceTexture.sprite = IceSprites [3];
			IsBroken = true;
			FadingBehave.StartIceFading ();
			break;

		}
	}


}
