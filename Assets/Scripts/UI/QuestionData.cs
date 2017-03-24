/*QuestionData:
 * This class contains the question data and the options
- This can be done using Plist and file handling properly but since its a prototype and will be dumped later. 
- So, a simple List is enough to serve the purpose for now. 
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent (typeof(AudioSource))]
public class QuestionData : MonoBehaviour
{
	public AudioSource _audio = null;
	//means question number 1 at index 0
	public static int currentQIndex = 0;
	// Key, Values(Containing question at index 0, correct option at 1 and rest are other options)
	List<string[]> _questionData = new List<string[]> ();
	public Text _questionText = null;
	public List<Text> _optionsButtonText = new List<Text> ();

	private string _correctOption = null;
	private Button _correctOptionBtnRef = null;
	private Button _selectedOptionBtnRef = null;

	public Floor.FloorControllerCSharp FloorCon;

	// Use this for initialization
	void OnEnable ()
	{

		if (Floor.FloorControllerCSharp.startReadingData == false)
			StartCoroutine (ActivateFloor ());
	}

	IEnumerator ActivateFloor ()
	{
		yield return new WaitForSeconds (1f);
		//Enable flooor input
		Floor.FloorControllerCSharp.ResetData ();
		Floor.FloorControllerCSharp.startReadingData = true;
	}

	void Start ()
	{
		//starting from question 1 and incrementally till last, (no repetition)
		if (currentQIndex == 0)
			//set index for first time
			PlayerPrefs.SetInt ("QuestionIndex", currentQIndex);
		else //get if its not 0
			currentQIndex = PlayerPrefs.GetInt ("QuestionIndex");
		
		AddTempData ();
		PresentAQuestion ();
	}


	void Update ()
	{
		#region ChecksOnSpecificPartOfFlower
		/*We are starting first row item from bottom left (1x1), Item labeling as "FloorFlower_Rows#_Cols#"*/
		if (FloorCon == null)
			FloorCon = GameObject.Find ("_GM and Singleton Scripts").GetComponent<Floor.FloorControllerCSharp> ();

		if (Floor.FloorControllerCSharp.GetStatusOfReadingData ()) {
			if (FloorCon.FloorInput.x == 1) {
				//bottom 1st row
				switch ((int)FloorCon.FloorInput.y) {
				case 1:
					if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue
					    || FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue) {
						SelectedOptionViaFloor ("OpC");
					}
					break;
				case 2:
					if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue
					    || FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue) {
						SelectedOptionViaFloor ("OpC");
					}
					break;
				case 3:
					if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue
					    || FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue) {
						SelectedOptionViaFloor ("OpD");
					}
					break;
				case 4:
					if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue
					    || FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue) {
						SelectedOptionViaFloor ("OpD");
					}
					break;
				}
			} else if (FloorCon.FloorInput.x == 2) {
				//top 2nd row
				switch ((int)FloorCon.FloorInput.y) {
				case 1:
					if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue
					    || FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue) {
						SelectedOptionViaFloor ("OpA");
					}
					break;
				case 2:
					if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue
					    || FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue) {
						SelectedOptionViaFloor ("OpA");
					}
					break;
				case 3:
					if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue
					    || FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue) {
						SelectedOptionViaFloor ("OpB");
					}
					break;
				case 4:
					if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue
					    || FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue) {
						SelectedOptionViaFloor ("OpB");
					}
					break;
				}

			}
		}
      
		#endregion
	}

	private void AddTempData ()
	{
		//temporary questions for the prototype, added to dictionary, keeping correct answer always at secondIndex in array
		_questionData.Add (new string[] {
			"Welche Pflanzenarten aus der Kältesteppe wurden im Spiel vorgestellt?",
			"Bärentrauben",
			"Riesen-Schönmoos",
			"Sumpfporst",
			"Kuhschellen"
		});

		_questionData.Add (new string[] {
			"Welche Tierarten waren im Spiel vertreten?",
			"Keine",
			"Polarfuchs",
			"Rentier",
			"Grizzlybär"
		});

		_questionData.Add (new string[] { 
			"Welchen Ort musstest du erreichen um das Spiel zu beenden?", 
			"Zeltlager", 
			"Meer", 
			"Wald", 
			"Stadt" 
		});


		_questionData.Add (new string[] {
			"Wie wird die Kaltsteppe in Englisch genannt?",
			"Tundra",
			"Mixed coniferous forest",
			"Houseboat",
			"Pampas"
		});
		_questionData.Add (new string[] {
			"Welche Umgebungsbedinungen kommen in der Kaltsteppe vor?",
			"Schnee",
			"Bäume",
			"Wüste",
			"Felsige Berge"
		});
	}

	private void PresentAQuestion ()
	{
		//temp string array to store options randomly chosen
		List<string> _finalOptionsList = new List<string> ();

		//set text of the question and options
		string[] _data = _questionData [currentQIndex];
		_questionText.text = (string)_data [0];

		//keeping always correct option at index 1
		_correctOption = _data [1];

		//set options, run loop 4 times on 
		for (int k = 0; k < 4; k++) {
			//only options A,B,C,D
			int randOpIndex = Random.Range (1, _data.Length);

			while (_finalOptionsList.Contains (_data [randOpIndex]))
				randOpIndex = Random.Range (1, _data.Length);
			
			_finalOptionsList.Add (_data [randOpIndex]);
		}

		for (int j = 0; j < _optionsButtonText.Count; j++) {

			string option = _finalOptionsList [j];
			Text opText = _optionsButtonText [j];

			if (option.Equals (_correctOption)) {
				//keep reference of this buttonText
				_correctOptionBtnRef = opText.gameObject.GetComponentInParent<Button> ();
			}
			opText.text = option;
			
		}
	}

	public void SelectedOptionViaFloor (string selButton)
	{
		//play select Btn Sfx
		if (!_audio.isPlaying)
			_audio.PlayOneShot (Resources.Load (AssetsPath._menuClickSFXPath) as AudioClip, 0.25f);

		_selectedOptionBtnRef = GameObject.Find (selButton).GetComponent<Button> ();
		if (_selectedOptionBtnRef != null) {
			Text optionText = _selectedOptionBtnRef.transform.FindChild ("OptionText").GetComponent<Text> () as Text;

			StartCoroutine (CheckIFCorrectAnswer (optionText.text));
		}
	}

	public void SelectedOption ()
	{
		//play select Btn Sfx
		if (!_audio.isPlaying)
			_audio.PlayOneShot (Resources.Load (AssetsPath._menuClickSFXPath) as AudioClip, 0.25f);

		string selButton = EventSystem.current.currentSelectedGameObject.name;
		_selectedOptionBtnRef = GameObject.Find (selButton).GetComponent<Button> ();
		if (_selectedOptionBtnRef != null) {
			Text optionText = _selectedOptionBtnRef.transform.FindChild ("OptionText").GetComponent<Text> () as Text;

			StartCoroutine (CheckIFCorrectAnswer (optionText.text));
		}
	}

	IEnumerator CheckIFCorrectAnswer (string optSelected)
	{
		if (optSelected.Equals (_correctOption)) {

#if UNITY_EDITOR
			//play a correct answer SFX
			_audio.PlayOneShot (Resources.Load (AssetsPath._correctAnswerSfxPath) as AudioClip, 0.25f);
#endif

			StatisticManager.Instance.Score += 5;
			UI.HUDController.UpdateScore ();

			//	Debug.Log ("_correctOptionBtnRef:" + _correctOptionBtnRef.name);
			_correctOptionBtnRef.transform.FindChild ("Tick").GetComponent<Image> ().enabled = true;

		} else { //show the correct answer and take back to the game

#if UNITY_EDITOR
			if (!_audio.isPlaying)
				_audio.PlayOneShot (Resources.Load (AssetsPath._wrongAnswerSfxPath) as AudioClip, 0.25f);
#endif
			yield return new WaitForSeconds (0.2f);

			_selectedOptionBtnRef.transform.FindChild ("Cross").GetComponent<Image> ().enabled = true;
			if (_correctOptionBtnRef != null) {
				_correctOptionBtnRef.transform.FindChild ("Tick").GetComponent<Image> ().enabled = true;
			}
		}

		//increment current question index by 1
		currentQIndex++;
		if (currentQIndex >= _questionData.Count) { //TEMP: restart numbering if reached end
			currentQIndex = 0;
		}
		PlayerPrefs.SetInt ("QuestionIndex", currentQIndex);	

		yield return new WaitForSeconds (5.0f);

		//go back to the place where 3d runner was halt and resume from there
		GameObject levelMng = GameObject.Find ("LevelManager");
		if (levelMng != null) {
			levelMng.GetComponent<LevelManager> ().EnableThe3DRun ("QuizScene");
		}
	}
}
