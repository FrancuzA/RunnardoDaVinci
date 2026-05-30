using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform entriesContainer;
    [SerializeField] private float refreshInterval = 10f;

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

        foreach (Transform child in entriesContainer)
            Destroy(child.gameObject);

        var sortedEntries = _scoreLoader.GetAllScores()
            .OrderByDescending(entry => entry.Value)
            .ToList();

        foreach (var entry in sortedEntries)
        {
            GameObject entryGO = Instantiate(entryPrefab, entriesContainer);
            EntryManager entryManager = entryGO.GetComponent<EntryManager>();
            entryManager.SetData(entry.Key, entry.Value);
        }
    }
}