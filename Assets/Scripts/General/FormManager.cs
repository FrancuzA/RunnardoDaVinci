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
    public GameObject TutorialScreen;
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
    public Color invalidColor = Color.red;
    public Color toggleNormalColor = Color.white;

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
        currentCountryCode = "48";
        countryCodeInput.text = "48";
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
        playButton.onClick.AddListener(GoToTutorial);
    }

    public void ToggleAllMarketing(bool value)
    {
        processingDataToggle.isOn = value;
        marketingEmailToggle.isOn = value;
        marketingSmsToggle.isOn = value;
        marketingPhoneToggle.isOn = value;
    }

    public void GoToTutorial()
    {
        if (ValidateForm())
        {
            TutorialScreen.SetActive(true);
            PlayerPrefs.SetString("CurrentNick", currentNick);
            SaveFormData();
            Dependencies.Instance.GetDependancy<ScoreLoaderManager>().CurrentNick = currentNick;
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

    private bool ValidateForm()
    {
        bool isValid = true;

        isValid &= ValidateField(nickImput, !String.IsNullOrEmpty(currentNick));
        isValid &= ValidateField(nameInput, !String.IsNullOrEmpty(currentName));
        isValid &= ValidateField(emailInput, !String.IsNullOrEmpty(currentEmail));
        isValid &= ValidateField(countryCodeInput, !String.IsNullOrEmpty(currentCountryCode));
        isValid &= ValidateField(phoneInput, !String.IsNullOrEmpty(currentPhone) && currentPhone.Length == 9);
        isValid &= ValidateToggle(TaCToggle, TaCToggle.isOn);

        return isValid;
    }

    private bool ValidateField(TMP_InputField field, bool condition)
    {
        Image bg = field.GetComponent<Image>();
        bg.color = condition ? Color.white : invalidColor;
        return condition;
    }

    private bool ValidateToggle(Toggle toggle, bool condition)
    {
        var colors = toggle.colors;
        colors.normalColor = condition ? toggleNormalColor : invalidColor;
        toggle.colors = colors;
        return condition;
    }
}