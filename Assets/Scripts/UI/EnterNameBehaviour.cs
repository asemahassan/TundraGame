using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class EnterNameBehaviour : MonoBehaviour
{
    public InputField NameInput;
    private Action _callback;
    public void Init(Action callback)
    {
        _callback = callback;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            _callback();
            //this might break, check later and maybe use an event
            StatisticManager.Instance.CurrentPlayerName = NameInput.text;
            Destroy(gameObject);
        }
    }
}
