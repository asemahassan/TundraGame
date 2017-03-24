using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

public class StatisticManager : SingletonBehaviour<StatisticManager>
{
    #region member
    private int _score;
    private GameData _data;
    private string _persistentDataPath;

    public string CurrentPlayerName;
    #endregion

    public GameData Data
    {
        get { return _data; }
        private set { }
    }

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
        }
    }

    void Start()
    {
        _persistentDataPath = Application.persistentDataPath;
        LoadGame();
    }

    public void LoadGame()
    {
        if (File.Exists(Path.Combine(_persistentDataPath, "gameData.xml")))
        {
            _data = Storage.Load<GameData>(Path.Combine(_persistentDataPath, "gameData.xml"));
        }
        else
        {
            _data = new GameData();
            _data.CreateNew();
            SaveGame();
        }
    }

    public void SaveGame()
    {
        if (Storage.Save(_data, Path.Combine(_persistentDataPath, "gameData.xml")))
        {
            Debug.Log("GameData saved.");
        }
        else
        {
            Debug.Log("Could not save GameData");
        }
    }

    public void AddNewScore(string name, int score)
    {
        _data.DataList.Add(new GameDataEntry(name, score, DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
        var list = _data.DataList.OrderByDescending(player => player.Score).ThenBy(player => player.PlayDate);
        _data.DataList = new List<GameDataEntry>();
        foreach (var l in list)
        {
            _data.DataList.Add(l);
        }
    }
}
