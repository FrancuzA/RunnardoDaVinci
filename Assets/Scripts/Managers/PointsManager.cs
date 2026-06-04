using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsManager : MonoBehaviour
{
    public float currentPoints;
    public int pointMultiplier;
    public float multipTime;
    public TextMeshProUGUI scoreTXT;
    public GameObject DeathScreen;

    private bool multipActive = false;
    private int currentMultip = 1;
    private WaitForSecondsRealtime multipTimer;
    private Dependencies _dep;
    private ScoreLoaderManager _scoreloaderManager;


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
        currentMultip = 0;
        Time.timeScale = 0;
        var name = "Test" + Time.time.ToString();
        _scoreloaderManager?.AddNewScore(PlayerPrefs.GetString("CurrentNick", "Null"), (int)currentPoints);
        DeathScreen.SetActive(true);

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


}
