//Switching sprites on key input

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Floor
{
	public class TriangleController : MonoBehaviour
	{
		public AudioClip sound;
		public Sprite NormalState = null;
		public Sprite PressedState = null;
		private Dictionary<string, AudioClip> m_sounds;
		public AudioSource m_source;
		private Coroutine m_lastCoroutine = null;

		// Use this for initialization
		void Start ()
		{
			this.GetComponent<Image> ().sprite = NormalState;
			m_source = this.transform.parent.GetComponent<AudioSource> ();
		}

		public void TriggerTriangle ()
		{
			PlaySound ();
			if (m_lastCoroutine != null)
				StopCoroutine (m_lastCoroutine);
			m_lastCoroutine = StartCoroutine (SpriteFadeCoroutine ());
		}

		//Kay: store GameObject of current flower to avoid triggering sound multiple times -
		//TODO: why is it still played at most 2 times instead of once?
	
		private GameObject lastFlowerObject = null;

		private void PlaySound ()
		{
			string parentName = this.transform.parent.gameObject.name;
			GameObject parentObject = this.transform.parent.gameObject;

			//This works perfect in editor and play sounds everytime the triangle is selected, doesnt work with floor data
		    //	m_source.PlayOneShot (sound);

            // if either flower has changed or last sound playback has already finished
            if (!m_source.isPlaying && parentObject != lastFlowerObject)
            {
                m_source.PlayOneShot(sound);
                lastFlowerObject = parentObject;
                Debug.Log("LastFlower:" + lastFlowerObject.name);
            }
        }

		private IEnumerator SpriteFadeCoroutine ()
		{
			Image image = this.transform.FindChild ("FilledTexture").GetComponent<Image> ();
			float fadeSpeed = 1.5f;
            
			Color c = image.color;
			float alpha = 1;
			c.a = alpha;
			image.color = c;

			yield return null;

			yield return new WaitForSeconds (0.3f);

			while (alpha > 0f) {
				alpha -= Time.deltaTime * fadeSpeed;
				c.a = alpha;
				image.color = c;
				yield return null;
			}
			c.a = 0.0f;
			image.color = c;
		}

		public void ChangeSprite ()
		{
			Sprite currentSpr = this.GetComponent<Image> ().sprite;
			if (currentSpr.name.Equals (NormalState.name)) {
				this.GetComponent<Image> ().sprite = PressedState;
			} else if (currentSpr.name.Equals (PressedState.name)) {
				this.GetComponent<Image> ().sprite = NormalState;
			}
		}
	}
}
