using Assets.Scripts.Resources.Game_Resources_DB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupManager : MonoBehaviour
{
    public static void Startup()
    {
        Log.WriteLog("Check PlayerPref keys.", Log.LevelsOfLogs.INFO, "StartupManager");
        CheckPlayerPrefKyes();
    }

    private static void CheckPlayerPrefKyes()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKeys.FirstEntering))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.FirstEntering, 1);
        }
        if (!PlayerPrefs.HasKey(PlayerPrefsKeys.Money))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.Money, 0);
        }
        if (!PlayerPrefs.HasKey(PlayerPrefsKeys.BestScoreClassic))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.BestScoreClassic, 0);
        }
        if (!PlayerPrefs.HasKey(PlayerPrefsKeys.BestScoreChallenge))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.BestScoreChallenge, 0);
        }
        if (!PlayerPrefs.HasKey(PlayerPrefsKeys.TicketsForEnterToChallenge))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.TicketsForEnterToChallenge, 0);
        }
        if (!PlayerPrefs.HasKey(PlayerPrefsKeys.Gems))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.Gems, 0);
        }
        if (!PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentBackgroundID))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentBackgroundID, 0);
        }
        if (!PlayerPrefs.HasKey(PlayerPrefsKeys.SettingsSoundActive))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.SettingsSoundActive, 1);
        }




        /************************************************************/
        GetGameDataFromDBOnStart.GetResourcesFromDB();
        /*************************************************************/
    }
}
