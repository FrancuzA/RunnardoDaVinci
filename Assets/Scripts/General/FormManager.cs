using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class FormEntry
{
    public string nick;
    public string name;
    public string email;
    public string phone;
}

[Serializable]
public class FormData
{
    public List<FormEntry> entries = new List<FormEntry>();
}

public class FormManager : MonoBehaviour
{
    public TMP_InputField nickImput;
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField phoneInput;
    public TMP_InputField countryCodeInput;
    public Toggle marketingToggle;
    public Toggle TaCToggle;
    public Button playButton;

    private string currentNick;
    private string currentName;
    private string currentEmail;
    private string currentPhone;
    private string currentCountryCode;
    private bool agreedToMarketing = false;
    private bool agreedToTaC = false;

    private string SavePath => Path.Combine(Application.persistentDataPath, "formData.json");

    private string FormattedPhone => $"+{currentCountryCode} {currentPhone}";

    private void OnEnable()
    {
        currentNick = null;
        currentName = null;
        currentEmail = null;
        currentPhone = null;
        currentCountryCode = null;
        agreedToMarketing = false;
        agreedToTaC = false;
    }

    void Start()
    {
        nickImput.onValueChanged.AddListener(ChangeNick);
        nameInput.onValueChanged.AddListener(ChangeName);
        emailInput.onValueChanged.AddListener(ChangeEmail);
        phoneInput.onValueChanged.AddListener(ChangePhoneNumber);
        countryCodeInput.onValueChanged.AddListener(ChangeCountryCode);
        marketingToggle.onValueChanged.AddListener(ChangeMarketingConsent);
        TaCToggle.onValueChanged.AddListener(ChangeTaCConsent);
        playButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        if (agreedToTaC && agreedToMarketing &&
            !String.IsNullOrEmpty(currentNick) &&
            !String.IsNullOrEmpty(currentName) &&
            !String.IsNullOrEmpty(currentEmail) &&
            !String.IsNullOrEmpty(currentPhone) &&
            !String.IsNullOrEmpty(currentCountryCode) &&
            currentPhone.Length == 9)
        {
            PlayerPrefs.SetString("CurrentNick", currentNick);
            SaveFormData();
            Dependencies.Instance.GetDependancy<ScoreLoaderManager>().CurrentNick = currentNick;
            SceneManager.LoadSceneAsync(1);
        }
    }

    private void SaveFormData()
    {
        FormData formData = new FormData();

        if (File.Exists(SavePath))
        {
            string existingJson = File.ReadAllText(SavePath);
            formData = JsonUtility.FromJson<FormData>(existingJson);
        }

        formData.entries.Add(new FormEntry
        {
            nick = currentNick,
            name = currentName,
            email = currentEmail,
            phone = FormattedPhone
        });

        File.WriteAllText(SavePath, JsonUtility.ToJson(formData, prettyPrint: true));
    }

    public void ChangeNick(string nick) => currentNick = nick;
    public void ChangeName(string name) => currentName = name;
    public void ChangeEmail(string email) => currentEmail = email;
    public void ChangePhoneNumber(string phoneNumber) => currentPhone = phoneNumber;
    public void ChangeCountryCode(string countryCode) => currentCountryCode = countryCode;
    public void ChangeMarketingConsent(bool hasAgreed) => agreedToMarketing = hasAgreed;
    public void ChangeTaCConsent(bool hasAgreed) => agreedToTaC = hasAgreed;
}