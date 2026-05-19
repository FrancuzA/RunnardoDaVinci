using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Scoreboardmanager : MonoBehaviour
{
    public GameObject addEntryPopUp;
    public GameObject entryPrefab;
    public GameObject Content;
    public TMP_InputField nickField;
    public TMP_InputField scoreField;
    public Button sendEntry;
    public Button addButton;
    public Button removeLastButton;
    public Button clearAllButton;



    private Dictionary<string, int> scores = new Dictionary<string, int>();
    private string lastEntry;
    private string currentName;
    private int currentScore;

    private void Start()
    {
        nickField.onValueChanged.AddListener(SetNewNick);
        nickField.onSubmit.AddListener(SetNewNick);
        scoreField.onSubmit.AddListener(SetNewScore);
        scoreField.onValueChanged.AddListener(SetNewScore);
        sendEntry.onClick.AddListener(TrySendEntry);
        addButton.onClick.AddListener(OpenEntryPopUp);
        removeLastButton.onClick.AddListener(ClearLastEntry);
        clearAllButton.onClick.AddListener(ClearWholeList);
    }


    public void OpenEntryPopUp()
    {
        addEntryPopUp.SetActive(true);
    }

    public void ClearLastEntry()
    {
        Remove(lastEntry);
        ReloadEntrys();

    }

    public void ClearWholeList()
    {
        Clear();
        ReloadEntrys();

    }

    public void SetNewNick(string name)
    {
        if (Contains(name))
        {
            nickField.textComponent.color = Color.red;
            currentName = "";
            return;
        }
        if(nickField.textComponent.color == Color.red) nickField.textComponent.color = Color.black;
        currentName = name;
    }

    public void SetNewScore(string score)
    {
        if (int.TryParse(score, out int parsed))
            currentScore = parsed;
    }

    public void ReloadEntrys()
    {
        Content.transform.DestroyAllChildren();
        if (scores.Count == 0) return;
        var sortedList = scores.OrderByDescending(entry => entry.Value).ToList();
        foreach (var entry in sortedList)
        {
            var entryGO = Instantiate(entryPrefab, Content.transform);

            EntryManager DataManger = entryGO.GetComponent<EntryManager>();
            DataManger?.SetData(entry.Key, entry.Value);
        }
        
    }

    public void TrySendEntry()
    {
        if (currentName == null || currentScore == 0) return;
        AddEntry(currentName, currentScore);
        lastEntry = currentName;
        nickField.text = "";
        scoreField.text = "";
        addEntryPopUp.SetActive(false);
        ReloadEntrys();
        currentName = null;
        currentScore = 0;
    }

    private void AddEntry(string name, int score)
    {
        scores.Add(name, score);
    }

    public bool Remove(string name)
    {
        if (name == null)
            return false;

        return scores.Remove(name);
    }

    public bool Contains(string name)
    {
        if (name == null)
            return false;

        return scores.ContainsKey(name);
    }

    public void Clear()
    {
        scores.Clear();
    }
}
