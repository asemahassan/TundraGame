using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public GameObject _3DRunnerObj = null;


	public void EnableThe3DRun (string unloadSceneName)
	{

        if (unloadSceneName.Length > 0)
        {
            SceneManager.UnloadScene(unloadSceneName);
        }

        if (unloadSceneName.Equals("FishPond2D"))
            EventManager.Instance.TriggerEvent(new FishCollectedEvent(10));

        _3DRunner.GameController.GameStates = GameState.Idle;

        if (unloadSceneName.Length > 0)
        {
            Destroy(GameObject.Find(unloadSceneName));
        }


        //Enable floor input
        Floor.FloorControllerCSharp.ResetData();
        Floor.FloorControllerCSharp.startReadingData = true;

        _3DRunnerObj.SetActive(true);

    }

    public void DisableThe3DRun ()
	{
		//Destroy all "Clones"
		GameObject[] allClones = GameObject.FindGameObjectsWithTag ("Points");
		foreach (GameObject point in allClones) {
			Destroy (point);
		}

        //Disable floor input
        Floor.FloorControllerCSharp.startReadingData = false;
        Floor.FloorControllerCSharp.ResetData();

        _3DRunnerObj.SetActive (false);

	}
}
