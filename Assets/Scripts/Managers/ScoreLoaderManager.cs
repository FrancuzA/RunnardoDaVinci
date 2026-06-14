using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class ScoreLoaderManager : MonoBehaviour
{
    public Dependencies _dep;
    public string CurrentNick;

    private string filePath;
    private Dictionary<string, int> allScores = new Dictionary<string, int>();

    [System.Serializable]
    private class ScoreData
    {
        public List<string> nick = new List<string>();
        public List<int> score = new List<int>();
    }

    private void Awake()
    {
       
    }

    private void OnEnable()
    {
        _dep = Dependencies.Instance;
        _dep.RegisterDependency<ScoreLoaderManager>(this);
        DontDestroyOnLoad(this);
    }

    void Start()
    {
       
        filePath = Application.persistentDataPath + "/scores.json";
        CheckFileExists();
        LoadScoresFromJson();
    }


    private void CheckFileExists()
    {
        if (File.Exists(filePath)) return;

        ScoreData empty = new ScoreData();
        string json = JsonUtility.ToJson(empty, true);
        File.WriteAllText(filePath, json);
    }

    public void AddNewScore(string name, int points)
    {
        if (IsDuplicate(name, points)) return;
        AddOrUpdate(name, points);
        SaveScoresToJson();
    }

    public void LoadScoresFromJson()
    {
        if (!File.Exists(filePath)) return;

        string json = File.ReadAllText(filePath);
        ScoreData data = JsonUtility.FromJson<ScoreData>(json);

        allScores.Clear();
        for (int i = 0; i < data.nick.Count; i++)
            allScores[data.nick[i]] = data.score[i];
    }

    public void SaveScoresToJson()
    {
        ScoreData data = new ScoreData();
        foreach (var kvp in allScores)
        {
            data.nick.Add(kvp.Key);
            data.score.Add(kvp.Value);
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    private bool IsDuplicate(string name, int points)
    {
        allScores.TryGetValue(name, out var score);
        return score != 0 && score == points;
    }

    public void AddOrUpdate(string name, int score)
    {
        if (string.IsNullOrEmpty(name) || score == 0) return;
        if (allScores.ContainsKey(name))
        {
            if (allScores[name] > score) return;
        }
        allScores[name] = score;
    }

    public int GetHighestScore()
    {
        LoadScoresFromJson();
        var sortedEntries = GetAllScores()
            .OrderByDescending(entry => entry.Value)
            .ToList();
        if(sortedEntries.Count == 0) return 0;
        return sortedEntries[0].Value;

    }
    public int GetScore(string nick)
    {
        if(allScores.TryGetValue(nick, out int score))
        {
            return score;
        }

        return 0;
    }

    public void RemoveScore(string nick)
    {
        if (string.IsNullOrEmpty(nick)) return;
        if (allScores.Remove(nick))
            SaveScoresToJson();
    }

    public Dictionary<string, int> GetAllScores()
    {
        return allScores;
    }
}