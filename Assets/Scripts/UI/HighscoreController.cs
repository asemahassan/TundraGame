using UnityEngine;
using Floor;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighscoreController : MonoBehaviour
{

	[SerializeField]
	private GameObject _contentArea;
	[SerializeField]
	private GameObject _scrollBar;
	[SerializeField]
	private ScrollRect _scrollView;

	[SerializeField]
	private GameObject _contentAreaRight;
	[SerializeField]
	private GameObject _scrollBarRight;
	[SerializeField]
	private ScrollRect _scrollViewRight;

	public FloorControllerCSharp FloorCon;
	public Text RankOneScore;
	public Text RankOneName;
	public Text RankTwoScore;
	public Text RankTwoName;
	public Text RankThreeScore;
	public Text RankThreeName;

	//UI Buttons
	public Button backBtn = null;
	public Button upBtn = null;
	public Button downBtn = null;

	// Use this for initialization
	void Start ()
	{
		updateScoreList ();
		//  Cursor.visible = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (FloorCon == null)
			FloorCon = GameObject.Find ("_GM and Singleton Scripts").GetComponent<FloorControllerCSharp> ();

        #region AssignUIToFloorTriangles
        /*We are starting first row item from bottom left (1x1), Item labeling as "FloorFlower_Rows#_Cols#"*/

        if (Floor.FloorControllerCSharp.GetStatusOfReadingData())
        {
            if (FloorCon.FloorInput.x == 2)
            {
                switch ((int)FloorCon.FloorInput.y)
                {
                    case 2:
                        if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue
                            || FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue)
                        {
                            backBtn.Select();
                            Invoke("OnBackButtonClicked", 0.5f);
                        }
                        break;
                    case 3:
                        if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue
                            || FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue)
                        {
                            backBtn.Select();
                            Invoke("OnBackButtonClicked", 0.5f);
                        }
                        break;
                }
            }
            //X = 1
            else
            {
                switch ((int)FloorCon.FloorInput.y)
                {
                    case 1:
                        if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue
                            || FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue)
                        {

                            upBtn.Select();
                            Invoke("OnScrollUpButtonClicked", 0.5f);
                        }
                        break;
                    case 4:
                        if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue
                            || FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue)
                        {
                            downBtn.Select();
                            Invoke("OnScrollDownButtonClicked", 0.5f);
                        }
                        break;
                }
            }
        }
		
		#endregion


		//Testin with keys the highscore menu

#if UNITY_EDITOR
		if (Input.GetKey (KeyCode.B)) {
			backBtn.Select ();
			Invoke ("OnBackButtonClicked", 0.5f);
		}

		if (Input.GetKey (KeyCode.U)) {
			upBtn.Select ();
			Invoke ("OnScrollUpButtonClicked", 0.5f);
		}

		if (Input.GetKey (KeyCode.D)) {
			downBtn.Select ();
			Invoke ("OnScrollDownButtonClicked", 0.5f);
		}
#endif
	}

	void OnEnable ()
	{
		EventManager.Instance.AddEventListener (OnGameOver, GameOverEvent.Identity);
        //  Cursor.visible = true;

        if (Floor.FloorControllerCSharp.startReadingData == false)
            StartCoroutine(ActivateFloor());
    }

	void OnDisable ()
	{
		EventManager.Instance.RemoveListener (OnGameOver, GameOverEvent.Identity);
	}

    IEnumerator ActivateFloor()
    {
        yield return new WaitForSeconds(1f);
        //Enable flooor input
        Floor.FloorControllerCSharp.ResetData();
        Floor.FloorControllerCSharp.startReadingData = true;
    }

    //User defined methods
    public void OnBackButtonClicked ()
	{
		StatisticManager.Instance.SaveGame ();
		Floor.FloorControllerCSharp.startReadingData = false;
		Floor.FloorControllerCSharp.ResetData ();
		SceneManager.LoadScene ("MainMenu");
	}

	public void OnScrollUpButtonClicked ()
	{
		_scrollBar.GetComponent<Scrollbar> ().value = Mathf.Clamp (_scrollBar.GetComponent<Scrollbar> ().value + _scrollView.scrollSensitivity, 0, 1);
		_scrollBarRight.GetComponent<Scrollbar> ().value = Mathf.Clamp (_scrollBarRight.GetComponent<Scrollbar> ().value + _scrollViewRight.scrollSensitivity, 0, 1);
	}

	public void OnScrollDownButtonClicked ()
	{
		_scrollBar.GetComponent<Scrollbar> ().value = Mathf.Clamp (_scrollBar.GetComponent<Scrollbar> ().value - _scrollView.scrollSensitivity, 0, 1);
		_scrollBarRight.GetComponent<Scrollbar> ().value = Mathf.Clamp (_scrollBarRight.GetComponent<Scrollbar> ().value - _scrollViewRight.scrollSensitivity, 0, 1);
	}

	public void OnGameOver (IEvent EventObject)
	{

		var evn = EventObject as GameOverEvent;
		Debug.Log (evn.PlayerName);
		StatisticManager.Instance.AddNewScore (evn.PlayerName, StatisticManager.Instance.Score);
		updateScoreList ();
	}

	private void updateScoreList ()
	{
		foreach (Transform child in _contentArea.transform) {
			Destroy (child.gameObject);
		}

		foreach (Transform child in _contentAreaRight.transform) {
			Destroy (child.gameObject);
		}

		GameObject prefab = Resources.Load (AssetsPath._HighscoreEntryPath) as GameObject;
		var dataList = StatisticManager.Instance.Data.DataList;

		var halfEntryCount = ((dataList.Count - 3) / 2f) < 0 ? 0 : ((dataList.Count - 3) / 2f);
        
		for (int i = 0; i < dataList.Count; i++) {
			switch (i) {
			case 0:
				RankOneName.text = dataList [i].UserName;
				RankOneScore.text = dataList [i].Score.ToString ();
				break;
			case 1:
				RankTwoName.text = dataList [i].UserName;
				RankTwoScore.text = dataList [i].Score.ToString ();
				break;
			case 2:
				RankThreeName.text = dataList [i].UserName;
				RankThreeScore.text = dataList [i].Score.ToString ();
				break;
			default:
				if (i - 3 < halfEntryCount) { //left scroll box
					GameObject highscoreEl = Instantiate (prefab) as GameObject;
					highscoreEl.transform.SetParent (_contentArea.transform, false);

					HighscoreElementBehaviour highscoreElBehaviour = highscoreEl.GetComponent<HighscoreElementBehaviour> ();
					highscoreElBehaviour.Init (i + 1, dataList [i].UserName, dataList [i].Score);
				} else {
					GameObject highscoreEl = Instantiate (prefab) as GameObject;
					highscoreEl.transform.SetParent (_contentAreaRight.transform, false);

					HighscoreElementBehaviour highscoreElBehaviour = highscoreEl.GetComponent<HighscoreElementBehaviour> ();
					highscoreElBehaviour.Init (i + 1, dataList [i].UserName, dataList [i].Score);
				}
				break;
			}

		}

	}
}
