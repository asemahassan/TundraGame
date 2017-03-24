using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FishBehaviour : MonoBehaviour
{
	public bool isCaught;
	public bool isDisplayedInfo;
	public bool isTriggered;
	public GameObject ParentObj;

	//specific properties for fish
	public string fishPath;
	public float pathtimer;
	public float pathDelay;

	private GameObject catchMessage = null;
    private GameObject infoMessage = null;


    // Use this for initialization

   void Awake()
    {
        if (infoMessage == null)
            infoMessage = GameObject.Find("infoMessageObj");
        if (catchMessage == null)
            catchMessage = GameObject.Find("catchMessageObj");

    }
    void Start ()
	{
		//put the fish on the predefined iTween path
		iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath (fishPath), "movetopath", false, 
			"time", pathtimer, "easeType", iTween.EaseType.easeInOutSine, "looptype", "loop", "delay", pathDelay));
		isCaught = false;
		isTriggered = false;
	}

	/// <summary>
	/// returns the object id of the closest point to the fish position
	/// </summary>
	/// <returns></returns>
	public int ReturnNearestNodeID ()
	{
		int nearestNodeId = 0;
		float minDistance = float.MaxValue;
		var nodeList = iTweenPath.GetPath (fishPath);
		if (nodeList.Length > 0) {
			for (int i = 0; i < nodeList.Length; i++) {
				float dist = Vector3.Distance (this.transform.position, nodeList [i]);

				if (dist < minDistance) {
					minDistance = dist;
					nearestNodeId = i;
				}
			}
		}      

		return nearestNodeId;
	}

    IEnumerator GoBackToRunnerScene()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject levelMng = GameObject.Find("LevelManager");
        if (levelMng != null)
        {
            levelMng.GetComponent<LevelManager>().EnableThe3DRun("FishPond2D");
        }

    }

    public void Stomped ()
	{
		// stomp function
		isCaught = true;

		//TODO: Change UI message here
		if (catchMessage != null)
			catchMessage.SetActive (true);
        if (infoMessage != null)
            infoMessage.SetActive(false);


        GameObject fish = GameObject.FindWithTag ("fish");

        GameObject levelMng = GameObject.Find("LevelManager");
        if (levelMng != null)
        {
            levelMng.GetComponent<LevelManager>().EnableThe3DRun("FishPond2D");
        }


        StatisticManager.Instance.Score += 20;
		UI.HUDController.UpdateScore ();

      // StartCoroutine(GoBackToRunnerScene());

        Destroy(fish);
	}

	IEnumerator FadeOut ()
	{
		for (float f = 1f; f >= 0; f -= 0.01f) {
			Color c = this.GetComponent<SpriteRenderer> ().color;
			c.a = f;
			this.GetComponent<SpriteRenderer> ().color = c;
			yield return new WaitForSeconds (0.1f);
		}
	}

	// This function is going to send trigger events
	void OnTriggerEnter (Collider col)
	{

		//if the ice is broken on either side and the player stomps on a fish...
		if (GameObject.Find ("flower1").GetComponent<IcePlateBehaviour> ().IsBroken ||
		    GameObject.Find ("flower2").GetComponent<IcePlateBehaviour> ().IsBroken) {            //catching fish is only possible if the ice is broken prior to the catching
			if (col.CompareTag ("fish")) {
				isTriggered = true;
			}
		}
	}
}
