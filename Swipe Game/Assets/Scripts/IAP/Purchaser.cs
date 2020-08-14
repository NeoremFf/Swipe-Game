using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System;

public class Purchaser : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text _moneyUI = null;

    [Header("Count get money")]
    [SerializeField] private int moneyAdd_0;
    [SerializeField] private int moneyAdd_1;
    [SerializeField] private int moneyAdd_2;

    [Header("Count get gems")]
    [SerializeField] private int gemsAdd_0;
    [SerializeField] private int gemsAdd_1;
    [SerializeField] private int gemsAdd_2;

    /// <summary>
    /// Add current resources type func
    /// </summary>
    public event Action<int> _AddResuorces;
    /// <summary>
    /// Active info panel with message
    /// </summary>
    public event Action<string, int> _InfoPanel;

    private delegate void BuyItem(int add);
    private delegate void Info(int info);

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        PurchaseManager.OnPurchaseConsumable += PurchaseManager_OnPurchaseConsumable;
        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;
    }

    public void SetEvent_Menu(GameObject obj)
    {
        MainMenuManager _mainMenu = obj.GetComponent<MainMenuManager>();
        if (_mainMenu)
        {

        }
        else
        {
            MenuManager _menu = obj.GetComponent<MenuManager>();
            if (!_menu) return;

        }
    }

    private void PurchaseManager_OnPurchaseConsumable(PurchaseEventArgs args)
    {
        MainMenuManager _menu = FindObjectOfType<MainMenuManager>();
        int add = 0;
        BuyItem buyFunc = null;
        Info infoPanel = null;
        switch (args.purchasedProduct.definition.id)
        {
            case "money_0":
                Log.WriteLog($"Purchased {moneyAdd_0} money.", Log.LevelsOfLogs.INFO, "Purchaser");
                add = moneyAdd_0;
                buyFunc = MoneyManager.AddMoney;
                //infoPanel = _menu.CoinsAddInfoPanel;
                break;
            case "money_1":
                Log.WriteLog($"Purchased {moneyAdd_1} money.", Log.LevelsOfLogs.INFO, "Purchaser");
                add = moneyAdd_1;
                buyFunc = MoneyManager.AddMoney;
                //infoPanel = _menu.CoinsAddInfoPanel;
                break;
            case "money_2":
                Log.WriteLog($"Purchased {moneyAdd_2} money.", Log.LevelsOfLogs.INFO, "Purchaser");
                add = moneyAdd_2;
                buyFunc = MoneyManager.AddMoney;
                //infoPanel = _menu.CoinsAddInfoPanel;
                break;

            case "gems_0":
                Log.WriteLog($"Purchased {gemsAdd_0} gems.", Log.LevelsOfLogs.INFO, "Purchaser");
                add = gemsAdd_0;
                buyFunc = MoneyManager.AddGems;
                //infoPanel = _menu.GemsAddInfoPanel;
                break;
            case "gems_1":
                Log.WriteLog($"Purchased {gemsAdd_1} gems.", Log.LevelsOfLogs.INFO, "Purchaser");
                add = gemsAdd_1;
                buyFunc = MoneyManager.AddGems;
               // infoPanel = _menu.GemsAddInfoPanel;
                break;
            case "gems_2":
                Log.WriteLog($"Purchased {gemsAdd_2} gems.", Log.LevelsOfLogs.INFO, "Purchaser");
                add = gemsAdd_2;
                buyFunc = MoneyManager.AddGems;
               // infoPanel = _menu.GemsAddInfoPanel;
                break;

            case "support_0":
                Log.WriteLog("Supoort purching.", Log.LevelsOfLogs.INFO, "Purchaser");
                add = moneyAdd_2;
                break;

            default:
                Log.WriteLog("You try to purche item that is not in list!", Log.LevelsOfLogs.ERROR, "Purchaser");
                break;
        }
        buyFunc?.Invoke(add);
        infoPanel?.Invoke(add);
    }

    private void PurchaseManager_OnPurchaseNonConsumable(PurchaseEventArgs args)
    {
        
    }
}
