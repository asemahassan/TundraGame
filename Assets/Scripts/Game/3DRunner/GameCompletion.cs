using UnityEngine;
using System.Collections;

namespace _3DRunner
{

public class GameCompletion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals ("Player")) 
		{
			//Debug.Log ("Game Completed");
			//GameController.GameStates = GameState.GameComplete;
		}
	}
}

}