using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scoreboardmanager : MonoBehaviour
{
    [Header("Leaderboard Slots")]
    public List<EntryManager> entrySlots = new List<EntryManager>(11);

    [Header("Add Popup")]
    public GameObject addEntryPopUp;
    public TMP_InputField addNickField;
    public TMP_InputField addScoreField;
    public Button confirmAddButton;

    [Header("Remove Popup")]
    public GameObject removeEntryPopUp;
    public TMP_InputField removeNickField;
    public Button confirmRemoveButton;

    [Header("Main Buttons")]
    public Button backButton;
    public Button addButton;
    public Button removeButton;

    private ScoreLoaderManager _scoreLoader;
    private string newNick;
    private int newScore;
    private string removeNick;

    private void Start()
    {
        _scoreLoader = Dependencies.Instance?.GetDependancy<ScoreLoaderManager>();

        addNickField?.onValueChanged.AddListener(SetNewNick);
        addScoreField?.onValueChanged.AddListener(SetNewScore);
        removeNickField?.onValueChanged.AddListener(SetRemoveNick);

        confirmAddButton?.onClick.AddListener(TryAddEntry);
        confirmRemoveButton?.onClick.AddListener(TryRemoveEntry);

        backButton?.onClick.AddListener(GoBack);
        addButton?.onClick.AddListener(OpenAddPopUp);
        removeButton?.onClick.AddListener(OpenRemovePopUp);

        ReloadEntrys();
    }

    public void GoBack()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void OpenAddPopUp()
    {
        addEntryPopUp.SetActive(true);
    }

    public void OpenRemovePopUp()
    {
        removeEntryPopUp.SetActive(true);
    }

    public void SetNewNick(string nick) => newNick = nick;

    public void SetNewScore(string score)
    {
        if (int.TryParse(score, out int parsed))
            newScore = parsed;
    }

    public void SetRemoveNick(string nick) => removeNick = nick;

    public void TryAddEntry()
    {
        if (string.IsNullOrEmpty(newNick) || newScore == 0) return;

        _scoreLoader?.AddNewScore(newNick, newScore);

        addNickField.text = "";
        addScoreField.text = "";
        newNick = null;
        newScore = 0;

        addEntryPopUp.SetActive(false);
        ReloadEntrys();
    }

    public void TryRemoveEntry()
    {
        if (string.IsNullOrEmpty(removeNick)) return;

        _scoreLoader?.RemoveScore(removeNick);

        removeNickField.text = "";
        removeNick = null;

        removeEntryPopUp.SetActive(false);
        ReloadEntrys();
    }

    public void ReloadEntrys()
    {
        var sortedList = _scoreLoader?.GetAllScores()
            .OrderByDescending(entry => entry.Value)
            .ToList();

        for (int i = 0; i < entrySlots.Count; i++)
        {
            if (i < sortedList?.Count)
                entrySlots[i].SetData(sortedList[i].Key, sortedList[i].Value);
            else
                entrySlots[i].SetData("---", 0);
        }
    }
}