using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsManager : MonoBehaviour
{
    public float currentPoints;
    public int pointMultiplier;
    public float multipTime;
    public TextMeshProUGUI scoreTXT;
    public TextMeshProUGUI newHighScoreTXT;
    public TextMeshProUGUI currentHighScoreTXT;
    public TextMeshProUGUI currentNick;
    public GameObject deathScreen;
    public GameObject highScoreScreen;
    public GameObject addedPoints;

    private bool multipActive = false;
    private bool madeHighScore = false;
    private WaitForSecondsRealtime multipTimer;
    private Dependencies _dep;
    private ScoreLoaderManager _scoreloaderManager;
    private WaitForSecondsRealtime _newScoreTimer;


    private void Awake()
    {
        _dep = Dependencies.Instance;
        _dep.RegisterDependency<PointsManager>(this);
    }
    void Start()
    {
        _scoreloaderManager = _dep.GetDependancy<ScoreLoaderManager>();
        multipTimer = new WaitForSecondsRealtime(multipTime);
        currentPoints = 0;
        Time.timeScale = 1;
        var lastHighScore = _scoreloaderManager?.GetHighestScore();
        currentHighScoreTXT.text = $"Best: {lastHighScore}";
        _newScoreTimer = new WaitForSecondsRealtime(3);
        highScoreScreen.SetActive(false);
        currentNick.text = PlayerPrefs.GetString("CurrentNick", "Null");
    }

    void Update()
    {
        currentPoints += (Time.deltaTime * 100);
        scoreTXT.text = ((int)currentPoints).ToString();
    }

    public void StartMultip()
    {
        StartCoroutine(Multiplier());
    }

    public void Death()
    {
        madeHighScore = false;
        CheckHighScore();
        Time.timeScale = 0;
        var name = "Test" + Time.time.ToString();
        CheckHighScore();
        _scoreloaderManager?.AddNewScore(PlayerPrefs.GetString("CurrentNick", "Null"), (int)currentPoints);
        if(madeHighScore) StartCoroutine(ShowNewScore());
        deathScreen.SetActive(true);

    }

    private void CheckHighScore()
    {
       var lastBestScore = _scoreloaderManager?.GetScore(PlayerPrefs.GetString("CurrentNick", "Null"));
       if((int)currentPoints > lastBestScore) madeHighScore = true;
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void GoHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private IEnumerator Multiplier()
    {
        multipActive = true;
        addedPoints.SetActive(true);
        currentPoints += 100;
        yield return multipTimer;
        multipActive = false;
        addedPoints.SetActive(false);
    }

    private IEnumerator ShowNewScore()
    {
        highScoreScreen.SetActive(true);
        newHighScoreTXT.text =$"NEW HIGH SCORE: {(int)currentPoints}" ;
        yield return _newScoreTimer;
        highScoreScreen.SetActive(false);
    }


}
