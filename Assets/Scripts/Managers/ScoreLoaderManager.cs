using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ScoreLoaderManager : MonoBehaviour
{
    public Dependencies _dep;

    private string filePath;
    private Dictionary<string, int> allScores = new Dictionary<string, int>();

    [System.Serializable]
    private class ScoreData
    {
        public List<string> keys = new List<string>();
        public List<int> values = new List<int>();
    }

    private void Awake()
    {
        _dep = Dependencies.Instance;
        _dep.RegisterDependency<ScoreLoaderManager>(this);
    }

    void Start()
    {
       
        filePath = Application.persistentDataPath + "/scores.json";
        LoadScoresFromJson();
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
        for (int i = 0; i < data.keys.Count; i++)
            allScores[data.keys[i]] = data.values[i];
    }

    public void SaveScoresToJson()
    {
        ScoreData data = new ScoreData();
        foreach (var kvp in allScores)
        {
            data.keys.Add(kvp.Key);
            data.values.Add(kvp.Value);
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
        allScores[name] = score;
    }
}