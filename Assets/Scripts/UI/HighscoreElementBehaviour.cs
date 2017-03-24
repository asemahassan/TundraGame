using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class HighscoreElementBehaviour : MonoBehaviour {

    #region member
    public Image Background;
    public Text Rank;
    public Text Name;
    public Text Score;
    #endregion

    public void Init(int rank, string name, int score)
    {
        Rank.text = String.Format("#{0}", rank);
        Name.text = name;
        Score.text = score.ToString();
    }
}
