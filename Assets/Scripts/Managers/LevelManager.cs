using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;
    public event EventHandler OnWin;
    public event EventHandler OnLose;
    public event EventHandler OnUpdateCombo;

    private ComboText comboText;
    private LevelGenerator levelScript;
    private bool pauseGame = false;
    public bool PauseGame
    {
        get => pauseGame;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        levelScript = GetComponent<LevelGenerator>();
        Debug.Log("Awake");
        // for debugging only InitGame();
    }

    void Start()
    {
        GameObject comboTextObj = GameObject.Find("ComboText");
        if(comboTextObj)
        {
            comboText = comboTextObj.GetComponent<ComboText>();
        }
    }

     void OnEnable()
     {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        //Debug.Log("OnEnable");
     }

     void OnDisable()
     {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        //Debug.Log("OnDisable");
     }

     void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
     {
        Debug.Log("Level Loaded");
        InitGame();
     }


    void InitGame()
    {
        gameObject.SetActive(true);
        levelScript.SetupScene(GameManager.instance.LevelSystem.level);
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        pauseGame = true;
        OnLose(this, EventArgs.Empty);
        Invoke("GoBackMenu", 5.0f);

    }

    public void WinLevel()
    {
        Debug.Log("WinLevel");
        OnWin(this, EventArgs.Empty);
        GameManager.instance.LevelSystem.level += 1;
        GameManager.instance.Save();
        Invoke("LoadIntroScene", 2.0f);
    }

    public void LoadIntroScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

    public void GoBackMenu()
    {
        GameManager.instance.EndRun();
        SceneManager.LoadScene(0);
    }

    public void AddScore(int point)
    {
        GameManager.instance.LevelSystem.score += point;
    }

    public void IncEnemyKill()
    {
        GameManager.instance.LevelSystem.nbKilled += 1;
    }

    public void TakeMoney(int _money)
    {
        GameManager.instance.LevelSystem.money += _money;
    }

    public void IncCombo()
    {
        GameManager.instance.LevelSystem.currentCombo += 1;
        if(GameManager.instance.LevelSystem.currentCombo > GameManager.instance.LevelSystem.maxCombo) {
            GameManager.instance.LevelSystem.maxCombo = GameManager.instance.LevelSystem.currentCombo;
        }
        OnUpdateCombo(this, EventArgs.Empty);
    }

    public void ResetCombo()
    {
        GameManager.instance.LevelSystem.currentCombo = 0;
        OnUpdateCombo(this, EventArgs.Empty);
    }

}