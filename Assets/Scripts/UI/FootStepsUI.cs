using UnityEngine;
using System.Collections;

public class FootStepsUI : MonoBehaviour
{

	public SpriteSwap leftFootCtrl = null;
	public SpriteSwap rightFootCtrl = null;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (rightFootCtrl.HighlightedSprite ());
		StartCoroutine (leftFootCtrl.NormalSprite ());
	
	}

}
