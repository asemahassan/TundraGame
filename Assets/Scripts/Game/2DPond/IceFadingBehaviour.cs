using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IceFadingBehaviour : MonoBehaviour {

    public SpriteRenderer IceSprite;
    public Image LeftCrack;
    public Image RightCrack;

    private AudioClip _IceBreaking = null;
    private AudioSource _audio = null;

    public void StartIceFading()
    {
        _audio = this.GetComponent<AudioSource>();
        _IceBreaking = Resources.Load(AssetsPath._iceBreakingSfxPath) as AudioClip;

        if (IceSprite.color.a >= 0.1f)
        {
            _audio.PlayOneShot(_IceBreaking, 0.25f);
            StartCoroutine("FadeOut");
        }            
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= 0; f -= 0.04f)
        {
            Color c = LeftCrack.color;
            c.a = f;
            LeftCrack.color = c;
            Color d = RightCrack.color;
            d.a = f;
            RightCrack.color = d;
            Color e = IceSprite.color;
            e.a = f;
            IceSprite.color = e;
            yield return new WaitForSeconds(0.1f);

            if (IceSprite.color.a <= 0.1f)
            {
                IceSprite.gameObject.SetActive(false);
                LeftCrack.gameObject.SetActive(false);
                RightCrack.gameObject.SetActive(false);
            }
        }
    }
}
