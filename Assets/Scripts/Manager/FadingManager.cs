using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class FadingManager : SingletonBehaviour<FadingManager>
{
    [SerializeField]
    private Image _fadingImage;
    [SerializeField]
    private Image _groundFadingImage;
    [SerializeField]
    private float _fadeSpeed;
    private bool _isFadingSceneIn;
    private float _alphaFadingImage;
    private string _targetScene;
    private Action _callbackSceneChange;

    public string CurrentScene
    {
        get { return _targetScene;}
        private set { }
    }
    void Start()
    {
        _isFadingSceneIn = false;
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
    }

    void OnGUI()
    {
        if(_fadingImage.gameObject.activeSelf == true)
        {
            if (_isFadingSceneIn)
            {
                _alphaFadingImage = Mathf.Clamp01(_alphaFadingImage += _fadeSpeed * Time.deltaTime);
                _fadingImage.color = new Color(_fadingImage.color.r, _fadingImage.color.g, _fadingImage.color.b, _alphaFadingImage);
                _groundFadingImage.color = new Color(_groundFadingImage.color.r, _groundFadingImage.color.g, _groundFadingImage.color.b, _alphaFadingImage);
                if (_alphaFadingImage == 1)
                {
                    _callbackSceneChange();
                    _isFadingSceneIn = false;
                    return;
                }
            }
            else
            {
                _alphaFadingImage = Mathf.Clamp01(_alphaFadingImage -= _fadeSpeed * Time.deltaTime);
                _fadingImage.color = new Color(_fadingImage.color.r, _fadingImage.color.g, _fadingImage.color.b, _alphaFadingImage);
                _groundFadingImage.color = new Color(_groundFadingImage.color.r, _groundFadingImage.color.g, _groundFadingImage.color.b, _alphaFadingImage);
                if (_alphaFadingImage == 0)
                {
                    _fadingImage.gameObject.SetActive(false);
                    _groundFadingImage.gameObject.SetActive(false);
                    //todo fading finished
                }
            }
        }
    }

    public void OnSceneChange(string targetScene, Action callback)
    {
        _targetScene = targetScene;

        _callbackSceneChange = () =>
        {
            SceneManager.LoadScene(targetScene);
            var scene = SceneManager.GetActiveScene().name;
            //SceneManager.UnloadScene(0); 
        };
        if(callback != null)
            _callbackSceneChange += callback;
        _fadingImage.gameObject.SetActive(true);
        _groundFadingImage.gameObject.SetActive(true);
        _isFadingSceneIn = true;
    }
}
