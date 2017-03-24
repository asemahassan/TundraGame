using UnityEngine;
using System.Collections;

namespace _3DRunner
{
	public class AutoDestroy : MonoBehaviour
	{
		private float destroyTime = 15.0f;
		// Use this for initialization
		void Start ()
		{
			//Random destroy time for each object
			//destroyTime = Random.Range(5.0f,10.0f);
			Invoke ("DestroySelf", destroyTime);
		}

		private void DestroySelf ()
		{
			if (this.gameObject.transform.parent != null)
				Destroy (this.gameObject.transform.parent.gameObject);
			else
				Destroy (this.gameObject);
		}
	}
}
