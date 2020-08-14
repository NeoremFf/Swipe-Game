using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that set localized text in scene
/// </summary>
public class LocalizationTextSetter : MonoBehaviour
{
    public LocalizationMainKeys.MainKey currentLocationForLocalization;

    [Header("UI Text")]
    public List<TextUISetter> allUIText = new List<TextUISetter>();
    [Header("Text In Code")]
    public List<TextUISetter> allStringTextInCode = new List<TextUISetter>();

    private Dictionary<string, string> dictionary;

    public void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        Log.WriteLog("Setup Localized Text.", Log.LevelsOfLogs.INFO, "TextSetter");
        GetDictionary();
        SetText();
        Log.WriteLog("Setup Text was success.", Log.LevelsOfLogs.INFO, "TextSetter");
    }

    private void SetText()
    {
        Log.WriteLog("Set UI localized text.", Log.LevelsOfLogs.INFO, "TextSetter");
        string[] keys = null;
        switch (currentLocationForLocalization)
        {
            case LocalizationMainKeys.MainKey.mainMenu:
                keys = LocalizationMainKeys.GeneralKyes.mainMenu;
                break;
            case LocalizationMainKeys.MainKey.game:
                keys = LocalizationMainKeys.GeneralKyes.game;
                break;
        }

        int keysId = 0;
        for (int i = 0; i < allUIText.Count; i++, keysId++)
        {
            allUIText[i].SetText(dictionary[keys[keysId]]);
        }
        for (int i = 0; i < allStringTextInCode.Count; i++)
        {
            int count = allStringTextInCode[i].GetCountOfStringToSet();
            allStringTextInCode[i].ClearStringArray();
            for (int k = 0; k < count; k++, keysId++)
            {
                allStringTextInCode[i].SetVariableValue(dictionary[keys[keysId]]);
            }
        }
    }

    private void GetDictionary()
    {
        Log.WriteLog("Get dictionary.", Log.LevelsOfLogs.INFO, "TextSetter");
        dictionary = LocalizationManager.GetDictionary(currentLocationForLocalization);
        
        if (dictionary.Count == 0)
            Log.WriteLog("Dictionary is empty.", Log.LevelsOfLogs.ERROR, "TextSetter");
        else
            Log.WriteLog("Getting dictionary was success.", Log.LevelsOfLogs.INFO, "TextSetter");
    }
}
