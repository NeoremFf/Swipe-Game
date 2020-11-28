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
    private static event Action<ResourcesUpdateUIEventArgs> updateMoneyUIEvent;
    /// <summary>
    /// Update count of gems on UI
    /// </summary>
    private static event Action<ResourcesUpdateUIEventArgs> updateGemsUIEvent;
    /// <summary>
    /// Update count of tickets on UI
    /// </summary>
    private static event Action<ResourcesUpdateUIEventArgs> updateTicketsUIEvent;

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
        money = PlayerPrefs.GetInt(PlayerPrefsKeys.Money);
        gems = PlayerPrefs.GetInt(PlayerPrefsKeys.Gems);
        tickets = PlayerPrefs.GetInt(PlayerPrefsKeys.TicketsForEnterToChallenge);

        GameModeManager _mode = FindObjectOfType<GameModeManager>();
        if (_mode) gameMode = _mode.GetGameMode();

        //var updateUI = FindObjectOfType<GameLoopUIUpdate>();
        //SetEvents(updateUI.UpdateMoneyUI, updateUI.UpdateGemsUI, updateUI.UpdateTicketsUI);
    }

    /// <summary>
    /// Set value to all events for update UI:
    /// 1 - money,
    /// 2 - gems,
    /// 3 - tickets
    /// </summary>
    /// <param name="handlers"></param>
    public static void SetEvents(params Action<ResourcesUpdateUIEventArgs>[] handlers)
    {
        updateMoneyUIEvent += handlers[0];
        updateGemsUIEvent += handlers[1];
        updateTicketsUIEvent += handlers[2];
    }

    /// <summary>
    /// Get count of tickets
    /// </summary>
    /// <returns>Count of tickets</returns>
    public static int GetTickets() => tickets;

    /// <summary>
    /// Add count to tickets
    /// </summary>
    public static void AddTickets(int value)
    {
        tickets += value;
        Log.WriteLog("Add value to tickets: " + value + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        Log.WriteLog("Current tickets: " + tickets + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        PlayerPrefs.SetInt(PlayerPrefsKeys.TicketsForEnterToChallenge, tickets);
        updateTicketsUIEvent?.Invoke(new ResourcesUpdateUIEventArgs(value));
    }

    /// <summary>
    /// try to buy some tickets
    /// </summary>
    /// <returns></returns>
    public static bool BuyTicket(int count)
    {
        Log.WriteLog("Try to buy ticket.", Log.LevelsOfLogs.INFO, "MoneyManager");
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
        gems += add;
        PlayerPrefs.SetInt(PlayerPrefsKeys.Gems, gems);
        Log.WriteLog("Current gems: " + gems + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        updateGemsUIEvent?.Invoke(new ResourcesUpdateUIEventArgs(add));
    }
    #endregion

    #region Get/Set Money
    /// <summary>
    /// Return current money
    /// </summary>
    /// <returns></returns>
    public static int GetMoney() => money;

    /// <summary>
    /// Add value to current money
    /// </summary>
    /// <param name="addMoney"></param>
    public static void AddMoney(int add)
    {
        Log.WriteLog("Add to money: " + add + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        money += add;
        PlayerPrefs.SetInt(PlayerPrefsKeys.Money, money);
        Log.WriteLog("Current money: " + money + ".", Log.LevelsOfLogs.INFO, "MoneyManager");
        updateMoneyUIEvent?.Invoke(new ResourcesUpdateUIEventArgs(money, add));
    }

    /// <summary>
    /// Add some resources after game loop:
    /// money - classic mode;
    /// gems - challenge mode;
    /// </summary>
    /// <param name="add"></param>
    public static void AddResourcesAfterGame(int add)
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
        }
    }
    #endregion

    #region Buy
    /// <summary>
    /// Buy extra live after lose to continue game
    /// </summary>
    /// <returns>true if purchased was successful, else - false</returns>
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

    /// <summary>
    /// Buy some item
    /// </summary>
    /// <returns>true if purchased was successful, else - false</returns>
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
