using UnityEngine;
using System.Collections.Generic;
using System;

#region GameData
[Serializable]
public class GameData
{
    public List<GameDataEntry> DataList;

    public void CreateNew()
    {
        DataList = new List<GameDataEntry>();

        for(int i = 10; i>0;i--)
        {
            DataList.Add(new GameDataEntry("Spieler " + i,i * 10, "2016-08-21 14:" + (10+ i)));
        }
    }

    public GameData()
    {
        DataList = new List<GameDataEntry>();
    }

    //public void 
}
#endregion

#region GameDataEntry
[Serializable]
public class GameDataEntry
{
    public string UserName;
    public int Score;
    /// <summary>
    /// Play Date functions as unique user ID since only one user can play. Might be fixed later with a combination of Name and Date.
    /// </summary>
    public string PlayDate;

    public GameDataEntry(string name, int score, string date)
    {
        UserName = name;
        Score = score;
        PlayDate = date;
    }

    public GameDataEntry()
    {

    }
}
#endregion
