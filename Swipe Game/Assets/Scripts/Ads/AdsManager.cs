using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool testMode = true;
    private static string gameId = "3561527";
    private static string regularPlacementId = "video";
    private static string rewardedVideoPlacementId = "rewardedVideo";
    private static string bannerPlacementId = "banner";

    [Header("Add money")]
    [SerializeField] private int countAddFreeMoney = 30;

    private static int countToShowAd = 0;
    private static bool canShowAd = true;

    private AllState.AdsShowFor adsShowFor;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitializeAds();
    }

    public void InitializeAds()
    {
        Log.WriteLog("Initialize Ads.", Log.LevelsOfLogs.INFO, "AdsManager");
        Advertisement.Initialize(gameId, testMode);

        //if (Advertisement.isInitialized)
        //    CheckRemoveAd();
    }

    private void Start()
    {
        //StartCoroutine(ShowBannerWhenReady());
    }

    private IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(bannerPlacementId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (!canShowAd)
        {
            Advertisement.Banner.Show(bannerPlacementId);
        }
    }


    public static void CheckRemoveAd()
    {
        //if (PurchaseManager.CheckBuyState("remove_ad"))
        //{
        //    canShowAd = false;
        //    Debug.Log("<color=pink>Bought Remove Ads - dont show ads.</color>");
        //    Advertisement.Banner.Hide();
        //}
    }

    #region ShowAds
    [System.Obsolete]
    public void ShowRegularAd()
    {
        if (canShowAd)
        {
            countToShowAd++;
            if (countToShowAd >= 3)
            {
                Time.timeScale = 0;
                countToShowAd = 0;
                if (Advertisement.IsReady(regularPlacementId))
                {
                    var showOptions = new ShowOptions
                    {
                        resultCallback = HandleShowResult
                    };
                    Advertisement.Show(regularPlacementId, showOptions);
                }
            }
        }
        else
        {
            Debug.Log("<color=pink>Bought Remove Ads - dont show ads.</color>");
        }
    }

    [System.Obsolete]
    public void ShowRewardedAd(AllState.AdsShowFor newAdsShowFor)
    {
        Log.WriteLog("Show ad to free money.", Log.LevelsOfLogs.INFO, "AdsManager");
        adsShowFor = newAdsShowFor;
        if (Advertisement.IsReady(rewardedVideoPlacementId))
        {
            var showOptions = new ShowOptions();
            showOptions.resultCallback = HandleShowResult;

            Advertisement.Show(rewardedVideoPlacementId, showOptions);
        }
        else
            Log.WriteLog("Ads not ready.", Log.LevelsOfLogs.WARNING, "AdsManager");
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Log.WriteLog("The ad was successfully shown.", Log.LevelsOfLogs.INFO, "AdsManager");
                switch (adsShowFor)
                {
                    case AllState.AdsShowFor.FreeMoney:
                        MainMenuManager _mainMenu = FindObjectOfType<MainMenuManager>();
                        if (_mainMenu)
                        {
                            //_mainMenu.CoinsAddInfoPanel(countAddFreeMoney);
                        }
                        else
                        {
                            MenuManager _menu = FindObjectOfType<MenuManager>();
                            if (_menu)
                            {
                                _menu.InfoPanel(countAddFreeMoney + " COINS ADD");
                                _menu.UpdateMoneyUI();
                            }
                        }
                        MoneyManager.AddMoney(countAddFreeMoney);
                        break;
                }
                break;

            case ShowResult.Skipped:
                Log.WriteLog("The ad was skipped before reaching the end.", Log.LevelsOfLogs.INFO, "AdsManager");
                break;

            case ShowResult.Failed:
                Log.WriteLog("The ad failed to be shown.", Log.LevelsOfLogs.ERROR, "AdsManager");
                break;
        }
    }

    public static void ShowBannerAd()
    {
        if (canShowAd)
        {
            if (Advertisement.IsReady(bannerPlacementId))
            {
                Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
                Advertisement.Banner.Show(bannerPlacementId);
            }
        }
        else
        {
            Debug.Log("<color=pink>Bought Remove Ads - dont show ads.</color>");
        }
    }
    #endregion

    public static void HideBannerAd()
    {
        if (canShowAd)
        {
            Advertisement.Banner.Hide();
        }
    }
}