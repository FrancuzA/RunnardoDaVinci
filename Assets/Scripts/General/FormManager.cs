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
    public bool consentProcessingData;
    public bool consentMarketingEmail;
    public bool consentMarketingSms;
    public bool consentMarketingPhone;
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
    public Toggle marketingToggleAll;
    public Toggle processingDataToggle;
    public Toggle marketingEmailToggle;
    public Toggle marketingSmsToggle;
    public Toggle marketingPhoneToggle;
    public Toggle TaCToggle;
    public Button playButton;

    private string currentNick;
    private string currentName;
    private string currentEmail;
    private string currentPhone;
    private string currentCountryCode;

    private string SavePath => Path.Combine(Application.persistentDataPath, "formData.json");
    private string FormattedPhone => $"+{currentCountryCode} {currentPhone}";

    private void OnEnable()
    {
        currentNick = null;
        currentName = null;
        currentEmail = null;
        currentPhone = null;
        currentCountryCode = null;
        TaCToggle.isOn = false;
        processingDataToggle.isOn = false;
        marketingEmailToggle.isOn = false;
        marketingSmsToggle.isOn = false;
        marketingPhoneToggle.isOn = false;
        marketingToggleAll.isOn = false;
    }

    void Start()
    {
        nickImput.onValueChanged.AddListener(ChangeNick);
        nameInput.onValueChanged.AddListener(ChangeName);
        emailInput.onValueChanged.AddListener(ChangeEmail);
        phoneInput.onValueChanged.AddListener(ChangePhoneNumber);
        countryCodeInput.onValueChanged.AddListener(ChangeCountryCode);
        marketingToggleAll.onValueChanged.AddListener(ToggleAllMarketing);
        playButton.onClick.AddListener(StartGame);
    }

    public void ToggleAllMarketing(bool value)
    {
        processingDataToggle.isOn = value;
        marketingEmailToggle.isOn = value;
        marketingSmsToggle.isOn = value;
        marketingPhoneToggle.isOn = value;
    }

    public void StartGame()
    {
        if (TaCToggle.isOn &&
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
            phone = FormattedPhone,
            consentProcessingData = processingDataToggle.isOn,
            consentMarketingEmail = marketingEmailToggle.isOn,
            consentMarketingSms = marketingSmsToggle.isOn,
            consentMarketingPhone = marketingPhoneToggle.isOn
        });

        File.WriteAllText(SavePath, JsonUtility.ToJson(formData, prettyPrint: true));
    }

    public void ChangeNick(string nick) => currentNick = nick;
    public void ChangeName(string name) => currentName = name;
    public void ChangeEmail(string email) => currentEmail = email;
    public void ChangePhoneNumber(string phoneNumber) => currentPhone = phoneNumber;
    public void ChangeCountryCode(string countryCode) => currentCountryCode = countryCode;
}