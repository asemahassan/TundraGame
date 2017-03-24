using UnityEngine;
using System.Collections;

public class TextureSwap : MonoBehaviour {

	public Texture[] textures;
	public int currentTexture;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {

			if (currentTexture == 9) {
				GetComponent<Renderer> ().material.mainTexture = textures [currentTexture];
			} else {
				currentTexture++;
				GetComponent<Renderer> ().material.mainTexture = textures [currentTexture];
			}
		}
	}
}
