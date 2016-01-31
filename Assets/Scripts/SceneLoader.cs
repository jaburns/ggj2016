using System;
using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
    const float FADE_SPEED = 1.5f;

    public bool DisableAutoLoad = false;

    SpriteRenderer _fader;
    float _faderT;
    int _fading;
    Action _afterFade;

    void Awake()
    {
        if (FindObjectsOfType<SceneLoader>().Length > 1) {
            Destroy(gameObject);
            return;
        }

        if (!DisableAutoLoad) {
            if (Application.loadedLevelName != "Loader") {
                Application.LoadLevelAdditive("Loader");
            } else {
                Application.LoadLevelAdditive("Level_0");
            }
        }

        _fader = GameObject.Find("Faderz").GetComponent<SpriteRenderer>();
        _faderT = 0f;
        _fading = 1;

    }

    void Update()
    {
        if (_faderT < 1 && _fading == 1) {
            _faderT += FADE_SPEED * Time.deltaTime;
            if (_faderT >= 1) {
                _fading = 0;
            }
        }
        else if (_faderT > 0 && _fading == -1) {
            _faderT -= FADE_SPEED * Time.deltaTime;
            if (_faderT <= 0) {
                _fading = 0;
                _afterFade();
            }
        }
        _fader.color = new Color(0f,0f,0f,1f-_faderT);
    }

    void fadeThen(Action fn)
    {
        _fading = -1;
        _faderT = 1;
        _afterFade = fn;
    }

    static int currentLevel = 0;

    static public int CurLevel { get { return currentLevel % 5; } }

    static public void NextLevel()
    {
        if(currentLevel < 4)
        {
            currentLevel++;
        }
        SceneLoader.Instance.fadeThen(() => {
            Application.LoadLevel("Level_"+currentLevel);
        });
    }

    static public void StartGame()
    {
        SceneLoader.Instance.fadeThen(() => {
            Application.LoadLevel("Level_0");
        });
        currentLevel = 0;
    }

    static public void ReloadLevel()
    {
        SceneLoader.Instance.fadeThen(() => Application.LoadLevel(Application.loadedLevelName));
    }
}
