using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private List<EntryManager> places = new List<EntryManager>();
    [SerializeField] private float refreshInterval = 1f;

    private Dependencies _dep;
    private ScoreLoaderManager _scoreLoader;
    

    private void Awake()
    {
        _dep = Dependencies.Instance;
        _scoreLoader = _dep.GetDependancy<ScoreLoaderManager>();
    }

    private void Start()
    {
        PopulateLeaderboard();
        StartCoroutine(AutoRefresh());
    }

    private IEnumerator AutoRefresh()
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshInterval);
            PopulateLeaderboard();
        }
    }

    public void PopulateLeaderboard()
    {
        _scoreLoader.LoadScoresFromJson();

        

        var sortedEntries = _scoreLoader.GetAllScores()
            .OrderByDescending(entry => entry.Value)
            .ToList();


        for(int i = 0; i < 5; i++)
        {
            var placeInfo = places[i];
            placeInfo.SetData(sortedEntries[i].Key, sortedEntries[i].Value);
        }
    }

    public void GoBAckToMEnu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}