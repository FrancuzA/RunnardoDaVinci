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
    public GameObject deathScreen;
    public GameObject highScoreScreen;

    private bool multipActive = false;
    private bool madeHighScore = false;
    private int currentMultip = 1;
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
        currentMultip = 1;
        Time.timeScale = 1;
        _newScoreTimer = new WaitForSecondsRealtime(5);
    }

    void Update()
    {
        currentPoints += (Time.deltaTime * currentMultip * 100);
        scoreTXT.text = ((int)currentPoints).ToString();
    }

    public void StartMultip()
    {
        if (!multipActive) StartCoroutine(Multiplier());
    }

    public void Death()
    {
        madeHighScore = false;
        CheckHighScore();
        currentMultip = 0;
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
        SceneManager.LoadScene(0);
    }

    private IEnumerator Multiplier()
    {
        multipActive = true;
        Debug.Log("Multip started");
        currentMultip = pointMultiplier;
        yield return multipTimer;
        currentMultip = 1;
        Debug.Log("Multip ended");
        multipActive = false;
    }

    private IEnumerator ShowNewScore()
    {
        highScoreScreen.SetActive(true);
        newHighScoreTXT.text =$"NEW HIGH SCORE: {(int)currentPoints}" ;
        yield return _newScoreTimer;
        highScoreScreen.SetActive(false);
    }


}
