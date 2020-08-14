using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// Class that set dictionary
/// </summary>
[System.Serializable]
public class LocalizationManager : MonoBehaviour
{
    [SerializeField] private Language currentLanguage;

    /// <summary>
    /// All language
    /// </summary>
    private enum Language
    {
        eng,
        rus
    }

    private LocalizationManager instance;
    public bool IsInitialize { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    //public Language CurrentLanguage => currentLanguage;

    public static Dictionary<string, string> GetDictionary(LocalizationMainKeys.MainKey currentDictionaryNeed)
    {
        switch (currentDictionaryNeed)
        {
            case LocalizationMainKeys.MainKey.mainMenu:
                return LocalizationDictionary.localizedMainMenu;
            case LocalizationMainKeys.MainKey.game:
                return LocalizationDictionary.localizedGame;
        }

        Dictionary<string, string> errorDictionary = new Dictionary<string, string>();
        return errorDictionary;
    }

    public void SetLanguage(int newLanguageId)
    {
        Log.WriteLog("Set Language.", Log.LevelsOfLogs.INFO, "LocalizationManager");
        IsInitialize = false;
        switch (newLanguageId)
        {
            case 0:
                currentLanguage = Language.eng;
                break;
            case 1:
                currentLanguage = Language.rus;
                break;
            default:
                Log.WriteLog("Language ID isn't correct.", Log.LevelsOfLogs.WARNING, "LocalizationManager");
                break;
        }

        LoadLocalizedText();

        LocalizationTextSetter setter = FindObjectOfType<LocalizationTextSetter>();
        if (!setter) Log.WriteLog("TextSetter not set.", Log.LevelsOfLogs.ERROR, "LocalizationManager");
        setter.Setup();
    }

    public void LoadLocalizedText()
    {
        Log.WriteLog("Load Localized Text.", Log.LevelsOfLogs.INFO, "LocalizationManager");
        if (IsInitialize)
            return;
        /// Load json data
        var data = Resources.Load<TextAsset>("Localization/Localization");
        if (!data) Log.WriteLog("Can not read data from json.", Log.LevelsOfLogs.ERROR, "LocalizationManager");
        string json = data.text;
        if (json.Length == 0)
            Log.WriteLog("Can not open file to read json.", Log.LevelsOfLogs.ERROR, "LocalizationManager");
        json = GetObjectFromJson(json, currentLanguage.ToString()); // Get part of json with current language

        /// Set all dictionary from json
        LocalizationData loadedData;
        /// Main Menu Dictionary
        loadedData = JsonUtility.FromJson<LocalizationData>(GetObjectFromJson(json, LocalizationMainKeys.MainKey.mainMenu.ToString(), 1));
        LocalizationDictionary.localizedMainMenu.Clear();
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            LocalizationDictionary.localizedMainMenu.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
        /// Game Dictionary
        loadedData = JsonUtility.FromJson<LocalizationData>(GetObjectFromJson(json, LocalizationMainKeys.MainKey.game.ToString(), 1));
        LocalizationDictionary.localizedGame.Clear();
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            LocalizationDictionary.localizedGame.Add(loadedData.items[i].key, loadedData.items[i].value);
        }

        IsInitialize = true;
    }

    /// <summary>
    /// Return string that contains all after obj name from json
    /// Mode: 0 - get all main keys (for first uses),
    /// 1 - get just local kyes (for sets local keys in main Localization class)
    /// </summary>
    /// <param name="json"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    private string GetObjectFromJson(string json, string obj, int mode = 0)
    {
        Regex startReg, endReg; // Regex to find current laguege in json
        string wordToReplaceInJson = "items";
        string newJsonKyes = "";
        /// Set start and and patterns to find current Language
        if (mode == 0)
        {
            startReg = new Regex(@"""" + obj + @""":{");
            endReg = new Regex(@"}]}");
        }
        else
        {
            //startReg = new Regex(@"""" + obj + @""": [");
            startReg = new Regex("[{,]\"" + obj);
            endReg = new Regex(@"}]");
        }
        /// Get part of json with current Language
        /// start
        MatchCollection matches = startReg.Matches(json);
        foreach (Match match in matches)
        {
            newJsonKyes = json.Substring(match.Index + 1);
            if (mode == 1)
            {
                newJsonKyes = newJsonKyes.Replace(obj, wordToReplaceInJson);
            }
            break;
        }
        /// end
        matches = endReg.Matches(newJsonKyes);
        foreach (Match match in matches)
        {
            newJsonKyes = newJsonKyes.Remove(match.Index + match.Length);
            break;
        }
        newJsonKyes = "{" + newJsonKyes + "}";
        return newJsonKyes;
    }
}