using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [Header("Mode")]
    private static GameModes.Modes gameMode = GameModes.Modes.NoneGame;

    /// <summary>
    /// Update count of money on UI
    /// </summary>
    public static event Action<int> _UpdateMoneyUI;
    /// <summary>
    /// Update count of gems on UI
    /// </summary>
    public static event Action _UpdateGemsUI;
    /// <summary>
    /// Update count of tickets on UI
    /// </summary>
    public static event Action _UpdateTicketsUI;

    private static int money = -1;
    private static int gems = -1;
    private static int tickets = -1;

    /// <summary>
    /// Price List
    /// </summary>
    private enum allPrice
    {
        extraLive = 20,
        ticket = 50
    }

    private void Start()
    {
        GameModeManager _mode = FindObjectOfType<GameModeManager>();
        if (_mode) gameMode = _mode.GetGameMode();


        Log.WriteLog("Get value of current money.", Log.LevelsOfLogs.INFO, "MoneyManager");
        money = PlayerPrefs.GetInt(PlayerPrefsKeys.Money);
        if (money == -1) Log.WriteLog("Can not get value of money.", Log.LevelsOfLogs.ERROR, "MoneyManager");
        Log.WriteLog("Current money: " + money + ".", Log.LevelsOfLogs.INFO, "MoneyManager");

        gems = PlayerPrefs.GetInt(PlayerPrefsKeys.Gems);
        if (gems == -1) Log.WriteLog("Can not get value of money.", Log.LevelsOfLogs.ERROR, "MoneyManager");
        Log.WriteLog("Current gems: " + gems + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
    }

    #region Events for update UI
    public void SetEventsForUpdateUI(Action<int> moneyUI, Action gemsUI, Action ticketsUI)
    {
        if (_UpdateMoneyUI != null) _UpdateMoneyUI = null;
        if (_UpdateGemsUI != null) _UpdateGemsUI = null;
        if (_UpdateTicketsUI != null) _UpdateTicketsUI = null;

        _UpdateMoneyUI += moneyUI;
        _UpdateGemsUI += gemsUI;
        _UpdateTicketsUI += ticketsUI;
    }
    #endregion

    /// <summary>
    /// Get count of tickets
    /// </summary>
    public static int GetTickets()
    {
        if (tickets == -1) tickets = PlayerPrefs.GetInt(PlayerPrefsKeys.TicketsForEnterToChallenge);
        if (tickets == -1) Log.WriteLog("", Log.LevelsOfLogs.ERROR, "MoneyManager");
        return tickets;
    }

    /// <summary>
    /// Add count to tickets
    /// </summary>
    public static void AddTickets(int value)
    {
        if (tickets == -1) tickets = PlayerPrefs.GetInt(PlayerPrefsKeys.TicketsForEnterToChallenge);
        if (tickets == -1) Log.WriteLog("", Log.LevelsOfLogs.ERROR, "MoneyManager");
        tickets += value;
        Log.WriteLog("Add value to tickets: " + value + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        Log.WriteLog("Current tickets: " + tickets + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        PlayerPrefs.SetInt(PlayerPrefsKeys.TicketsForEnterToChallenge, tickets);
        _UpdateTicketsUI?.Invoke();
    }

    /// <summary>
    /// try to buy some tickets
    /// </summary>
    /// <returns></returns>
    public static bool BuyTicket(int count)
    {
        Log.WriteLog("Try to buy ticket.", Log.LevelsOfLogs.INFO, "MoneyManager");
        if (money == -1) money = PlayerPrefs.GetInt(PlayerPrefsKeys.Money);
        if (money == -1) Log.WriteLog("Can not get value of money.", Log.LevelsOfLogs.ERROR, "MoneyManager");
        int coast = (int)allPrice.ticket * count;
        if (money >= coast)
        {
            Log.WriteLog("The purchase is successful.", Log.LevelsOfLogs.INFO, "MoneyManager");
            money -= coast;
            PlayerPrefs.SetInt(PlayerPrefsKeys.Money, money);
            AddTickets(count);
            Log.WriteLog("Current money: " + money + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
            return true;
        }
        else
        {
            Log.WriteLog("The purchase is not successful.", Log.LevelsOfLogs.INFO, "MoneyManager");
            return false;
        }
    }

    #region Gems
    public static void AddGems(int add)
    {
        Log.WriteLog("Add to gems: " + add + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        if (gems == -1) gems = PlayerPrefs.GetInt(PlayerPrefsKeys.Gems);
        if (gems == -1) Log.WriteLog("Can not get value of gems.", Log.LevelsOfLogs.ERROR, "MoneyManager");
        gems += add;
        PlayerPrefs.SetInt(PlayerPrefsKeys.Gems, gems);
        Log.WriteLog("Current gems: " + gems + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        _UpdateGemsUI?.Invoke();
    }
    #endregion

    #region Get/Set Money
    /// <summary>
    /// Return current money
    /// </summary>
    /// <returns></returns>
    public static int GetMoney() => money;

    /// <summary>
    /// Set new value of money
    /// </summary>
    /// <param name="newMoney"></param>
    public static void SetMoney(int newMoney)
    {
        Log.WriteLog("Set new value to money: " + newMoney + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        money = newMoney;
        PlayerPrefs.SetInt(PlayerPrefsKeys.Money, money);
    }

    /// <summary>
    /// Add value to current money
    /// </summary>
    /// <param name="addMoney"></param>
    public static void AddMoney(int add)
    {
        Log.WriteLog("Add to money: " + add + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        if (money == -1) money = PlayerPrefs.GetInt(PlayerPrefsKeys.Money);
        if (money == -1) Log.WriteLog("Can not get value of money.", Log.LevelsOfLogs.ERROR, "MoneyManager");
        money += add;
        PlayerPrefs.SetInt(PlayerPrefsKeys.Money, money);
        Log.WriteLog("Current money: " + money + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        _UpdateMoneyUI?.Invoke(add);
    }

    public static void AddMoneyInGamesEnd(int add)
    {
        switch (gameMode)
        {
            // Add money
            case GameModes.Modes.Classic:
                AddMoney(add);
                break;
            // Add gems
            case GameModes.Modes.Challenge:
                AddGems(add);
                break;
            default:
                break;
        }
    }
    #endregion

    #region Buy
    /// <summary>
    /// if player has money to buy extra live - return true, else - false
    /// </summary>
    /// <returns></returns>
    public bool BuyExtraLive()
    {
        Log.WriteLog("Try to buy extra live.", Log.LevelsOfLogs.INFO, "MoneyManager");
        if (money >= (int)allPrice.extraLive)
        {
            Log.WriteLog("Successful.", Log.LevelsOfLogs.INFO, "MoneyManager");
            AddMoney(-(int)allPrice.extraLive);
            return true;
        }
        else
        {
            Log.WriteLog("Not successful (no money).", Log.LevelsOfLogs.INFO, "MoneyManager");
            return false;
        }
    }

    public bool BuyItem(int cost)
    {
        Log.WriteLog("Try to buy item.", Log.LevelsOfLogs.INFO, "MoneyManager");
        if (gems >= cost)
        {
            Log.WriteLog("Successful.", Log.LevelsOfLogs.INFO, "MoneyManager");
            AddGems(-cost);
            return true;
        }
        else
        {
            Log.WriteLog("Not successful (no gems).", Log.LevelsOfLogs.INFO, "MoneyManager");
            return false;
        }
    }
    #endregion
}
