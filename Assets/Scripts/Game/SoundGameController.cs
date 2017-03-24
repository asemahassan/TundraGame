using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Floor;

public class SoundGameController : MonoBehaviour
{
	public FloorControllerCSharp FloorCon;
	public TriangleController triangleCtrl;
	private string lastPetal = "";
	private GameObject canvasUI = null;

	// Use this for initialization
	void Start ()
	{
		canvasUI = GameObject.Find ("InteractableFloor");

		if (FloorCon == null)
			FloorCon = GameObject.Find ("_GM and Singleton Scripts").GetComponent<FloorControllerCSharp> ();

        if (Floor.FloorControllerCSharp.startReadingData == false)
            StartCoroutine(ActivateFloor());
    }

    IEnumerator ActivateFloor()
    {
        yield return new WaitForSeconds(1f);
        //Enable flooor input
        Floor.FloorControllerCSharp.ResetData();
        Floor.FloorControllerCSharp.startReadingData = true;

        Debug.Log("In SoundGame: " +Floor.FloorControllerCSharp.startReadingData);
    }

    //check which triangle is pressed by player
    void Update ()
	{
		int xRow = (int)FloorCon.FloorInput.x;
		int yCol = (int)FloorCon.FloorInput.y;

		for (int i = 1; i <= 8; ++i) {
			if (FloorCon.IsPetalStepped (i)) {
				ActivatePetal (xRow, yCol, i);
			}
		}
		#if UNITY_EDITOR
		//When testing in Editor, sound game 
		if (Input.GetKeyUp (KeyCode.Space)) {
			//just for testing purpose random selection of petals
			int randIndex = Random.Range (1, 9);
			ActivatePetal (Random.Range (1, FloorCon.floorSize.rows + 1), Random.Range (1, FloorCon.floorSize.cols + 1), randIndex);
		}
		#endif

		// close on ESC
		if (Input.GetKey (KeyCode.Escape)) {
            SceneManager.LoadScene("SensFloor_DEMO");
		}
	}

	private void ActivatePetal (int xRow, int yCol, int petalIndex)
	{
		//Update flower textures 
		string nameOfFlower = "FloorFlower_" + xRow.ToString () + "_" + yCol.ToString ();
		Debug.Log ("FlowerName:" + nameOfFlower);
		GameObject flowerObj = GameObject.Find (nameOfFlower);

		Transform child = flowerObj.transform.FindChild ("T" + petalIndex.ToString ());
        if (child != null)
        {
            triangleCtrl = child.GetComponent<TriangleController> ();
		    triangleCtrl.TriggerTriangle ();
        }
		    
	}
}
