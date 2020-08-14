using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject canvas = null;
    [SerializeField] private GameObject mainTextPlay = null;
    [SerializeField] private GameObject buttonsForPlay = null;
    [SerializeField] private GameObject settings = null;
    [SerializeField] private GameObject purchaseMenu = null;
    [SerializeField] private GameObject buyTicketsPanel = null;
    [SerializeField] private GameObject aboutGamePanel = null;
    [SerializeField] private GameObject infoPanel = null;

    [Header("Shop UI")]
    [SerializeField] private GameObject shopPanel = null;
    [SerializeField] private GameObject buyBackgroundsPanel = null;
    [SerializeField] private GameObject buySwipeEffectsPanel = null;
    [SerializeField] private Text gemsValueInMainPageText = null;
    [SerializeField] private Text gemsValueInBackgroundShopPageText = null;
    [SerializeField] private Text gemsValueInEffectShopPageText = null;

    [Header("Text")]
    [SerializeField] private Text currentMoney = null;
    [SerializeField] private Text currentTickets = null;
    [SerializeField] private Text infoText = null;

    [Header("Managers")]
    [SerializeField] private AdsManager _ads = null;
    [SerializeField] private AudioSource _sound = null;

    /// <summary>
    /// Strings with text for info panel
    /// </summary>
    private string needMoreMoney = string.Empty;
    private string needMoreGems = string.Empty;
    private string needMoreTickets = string.Empty;
    private string successPurchasing = string.Empty;
    private string itemSelected = string.Empty;
    private string ticketsAdd = string.Empty;
    private string coinsAdd = string.Empty;
    private string gemsAdd = string.Empty;
    private string aboutGame = string.Empty;
    private int countOfTextUI = 9; // count of variables with text for ui

    private void Start()
    {
        /******************************/
        //PlayerPrefs.DeleteAll();
        MoneyManager.AddMoney(5000);
        MoneyManager.AddGems(500);
        /********************************/

        MoneyManager _resources = FindObjectOfType<MoneyManager>();
        _resources?.SetEventsForUpdateUI(UpdateMoneyUI, UpdateGemsUI, UpdateTicketsUI);

        //Set ui text
        GetLocalizedText();

        UpdateMoneyUI();
        UpdateGemsUI();
        UpdateTicketsUI();

        if (!_sound)
        {
            Log.WriteLog("AudioSurce is null.", Log.LevelsOfLogs.ERROR, "MainMenuManager");
            return;
        }
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.SettingsSoundActive) == 1)
        {
            Log.WriteLog("Set sound: play.", Log.LevelsOfLogs.INFO, "MainMenuManager");
            _sound.Play();
        }
        else
        {
            Log.WriteLog("Set sound: stop.", Log.LevelsOfLogs.INFO, "MainMenuManager");
            _sound.Stop();
        }
    }

    #region Localizacion
    private void GetLocalizedText()
    {
        TextUISetter _textSetter = GetComponent<TextUISetter>();
        if (!_textSetter) Log.WriteLog("Cannot get TextUISetter.", Log.LevelsOfLogs.ERROR, "MainMenuManager");
        int id = 0;
        _textSetter.GetVariableValue(out needMoreMoney, id++);
        _textSetter.GetVariableValue(out needMoreGems, id++);
        _textSetter.GetVariableValue(out needMoreTickets, id++);
        _textSetter.GetVariableValue(out successPurchasing, id++);
        _textSetter.GetVariableValue(out itemSelected, id++);
        _textSetter.GetVariableValue(out ticketsAdd, id++);
        _textSetter.GetVariableValue(out coinsAdd, id++);
        _textSetter.GetVariableValue(out gemsAdd, id++);
        //_textSetter.GetVariableValue(out aboutGame, id++);
        if (id != countOfTextUI) Log.WriteLog("Count of Text set not equal variables.", Log.LevelsOfLogs.ERROR, "MainMenuManager");
    }
    #endregion

    #region Update UI value
    public void UpdateMoneyUI(int addCount = 0)
    {
        string value = PlayerPrefs.GetInt(PlayerPrefsKeys.Money).ToString();
        currentMoney.text = value;
    }

    public void UpdateGemsUI()
    {
        string value = PlayerPrefs.GetInt(PlayerPrefsKeys.Gems).ToString();
        gemsValueInMainPageText.text = value;
        gemsValueInBackgroundShopPageText.text = value;
        //gemsValueInEffectShopPageText.text = value;      
    }

    public void UpdateTicketsUI()
    {
        string value = PlayerPrefs.GetInt(PlayerPrefsKeys.TicketsForEnterToChallenge).ToString();
        currentTickets.text = value;    
    }
    #endregion

    #region Play
    public void LoadPlayButtons()
    {
        if (settings.activeSelf)
        {
            settings.SetActive(false);
            mainTextPlay.SetActive(true);
        }
        else if (purchaseMenu.activeSelf)
        {
            purchaseMenu.SetActive(false);
            mainTextPlay.SetActive(true);
        }
        else if (buttonsForPlay.activeSelf)
        {
            buttonsForPlay.SetActive(false);
            mainTextPlay.SetActive(true);
        }
        else if (infoPanel.activeSelf)
        {
            infoPanel.SetActive(false);
            mainTextPlay.SetActive(true);
        }
        else if (buyTicketsPanel.activeSelf)
        {
            buyTicketsPanel.SetActive(false);
            mainTextPlay.SetActive(true);
        }
        else
        {
            buttonsForPlay.SetActive(true);
            mainTextPlay.SetActive(false);
        }
    }

    public void PlayClassic()
    {
        Log.WriteLog("Enter to Classic mode.", Log.LevelsOfLogs.INFO, "MainMenuManager");
        GameModeManager.SetMode(GameModes.Modes.Classic);
        SceneManager.LoadScene(2);
    }

    public void PlayChallenge()
    {
        if (MoneyManager.GetTickets() <= 0)
        {
            ActiveInfoPanel($"<size=47>{needMoreTickets}\nBUY MORE</size>");
            return;
        }
        MoneyManager.AddTickets(-1);
        Log.WriteLog("Enter to Challenge mode.", Log.LevelsOfLogs.INFO, "MainMenuManager");
        GameModeManager.SetMode(GameModes.Modes.Challenge);
        SceneManager.LoadScene(2);
    }
    #endregion

    #region Tickets
    public void ActiveBuyTicketMenu()
    {
        buyTicketsPanel.SetActive(true);
        buttonsForPlay.SetActive(false);
    }

    public void BuyTickets(int count)
    {
        buttonsForPlay.SetActive(false);
        if (MoneyManager.BuyTicket(count))
        {
            buyTicketsPanel.SetActive(false);
            currentTickets.text = PlayerPrefs.GetInt(PlayerPrefsKeys.TicketsForEnterToChallenge).ToString();
            currentMoney.text = PlayerPrefs.GetInt(PlayerPrefsKeys.Money).ToString();
            ActiveInfoPanel($"{count} {ticketsAdd}");
        }
        else
        {
            ActiveInfoPanel($"<b>{needMoreMoney}</b>");
            buyTicketsPanel.SetActive(false);
        }
    }
    #endregion

    #region Purchase
    public void PurchaseMenu()
    {
        if (buttonsForPlay.activeSelf)
        {
            buttonsForPlay.SetActive(false);
            mainTextPlay.SetActive(true);
        }
        else if (settings.activeSelf)
        {
            settings.SetActive(false);
            mainTextPlay.SetActive(false);
        }
        else if (infoPanel.activeSelf)
        {
            infoPanel.SetActive(false);
            mainTextPlay.SetActive(false);
        }
        else if (buyTicketsPanel.activeSelf)
        {
            buyTicketsPanel.SetActive(false);
            mainTextPlay.SetActive(false);
        }

        if (purchaseMenu.activeSelf)
        {
            purchaseMenu.SetActive(false);
            mainTextPlay.SetActive(true);
        }
        else
        {
            purchaseMenu.SetActive(true);
            mainTextPlay.SetActive(false);
        }
    }

    [System.Obsolete]
    public void FreeMoney()
    {
        Log.WriteLog("Free money.", Log.LevelsOfLogs.INFO, "MainMenuManager");
        _ads.ShowRewardedAd(AllState.AdsShowFor.FreeMoney);
        currentMoney.text = PlayerPrefs.GetInt(PlayerPrefsKeys.Money).ToString();
    }

    public void PurchaseMoney(int id)
    {
        Log.WriteLog("Purchase money.", Log.LevelsOfLogs.INFO, "MainMenuManager");
        PurchaseManager _purchase = FindObjectOfType<PurchaseManager>();
        if (!_purchase) Log.WriteLog("PurchaseManager not set.", Log.LevelsOfLogs.ERROR, "MainMenuManager");
        _purchase.BuyConsumable(id);
    }
    #endregion

    #region Settings Panel
    public void Settings()
    {
        if (settings.activeSelf)
        {
            settings.SetActive(false);
            mainTextPlay.SetActive(true);
        }
        else
        {
            settings.SetActive(true);
            mainTextPlay.SetActive(false);
        }
    }

    public void SetSoundActive()
    {
        if (!_sound)
        {
            Log.WriteLog("AudioSurce is null.", Log.LevelsOfLogs.ERROR, "MainMenuManager");
            return;
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKeys.SettingsSoundActive) == 1)
        {
            Log.WriteLog("Set sound: stop.", Log.LevelsOfLogs.INFO, "MainMenuManager");
            _sound.Stop();
            PlayerPrefs.SetInt(PlayerPrefsKeys.SettingsSoundActive, 0);
        }
        else
        {
            Log.WriteLog("Set sound: play.", Log.LevelsOfLogs.INFO, "MainMenuManager");
            _sound.Play();
            PlayerPrefs.SetInt(PlayerPrefsKeys.SettingsSoundActive, 1);
        }
    }

    public void OpenLeaderboard()
    {
        ///////////////////
    }

    public void Share()
    {
#if UNITY_ANDROID
        string destination = Application.dataPath + @"/Resources/Socials/ShareImg.jpg";
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"),
            uriObject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"),
            "Can you beat my score?");
        intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser",
            intentObject, "Share your new score");
        currentActivity.Call("startActivity", chooser);
#elif UNITY_IPHONE
#endif
    }

    public void SupportGame()
    {
        Log.WriteLog("Support game.", Log.LevelsOfLogs.INFO, "MainMenuManager");
        PurchaseManager _purchase = FindObjectOfType<PurchaseManager>();
        if (!_purchase) Log.WriteLog("PurchaseManager not set.", Log.LevelsOfLogs.ERROR, "MainMenuManager");
        _purchase.BuyNonConsumable(3);
    }

    public void RateGame()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.productName);
#endif
    }

    public void AboutGame()
    {
        ActiveInfoPanel(aboutGame);
    }

    public void OpenInstagram()
    {
        Application.OpenURL("https://instagram.com/bibimbap_games?igshid=r0yv6vl8njlw");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void SetLanguageToRu()
    {
        LocalizationManager locManager = FindObjectOfType<LocalizationManager>();
        if (!locManager) Log.WriteLog("LocalizationManager not set.", Log.LevelsOfLogs.ERROR, "MainMenuManager");
        Log.WriteLog("Set language to ru-RU.", Log.LevelsOfLogs.INFO, "MainMenuManager");
        locManager.SetLanguage(1);
        GetLocalizedText();
    }

    public void SetLanguageToEn()
    {
        LocalizationManager locManager = FindObjectOfType<LocalizationManager>();
        if (!locManager) Log.WriteLog("LocalizationManager not set.", Log.LevelsOfLogs.ERROR, "MainMenuManager");
        Log.WriteLog("Set language to en-EN.", Log.LevelsOfLogs.INFO, "MainMenuManager");
        locManager.SetLanguage(0);
        GetLocalizedText();
    }
    #endregion

    #region Shop
    public void SetShopPanelActive()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
    }

    public void SetBackgroundsPanelActive()
    {
        buyBackgroundsPanel.SetActive(!buyBackgroundsPanel.activeSelf);
    }

    public void SetEffectsPanelActive()
    {
        buySwipeEffectsPanel.SetActive(!buySwipeEffectsPanel.activeSelf);
        //if (buySwipeEffectsPanel.activeSelf)
            //UpdateGemsValue(gemsValueInEffectShopPageText);
    }
    #endregion

    #region InfoPanel
    public void ActiveInfoPanel(string info)
    {
        CloseAll();
        mainTextPlay.SetActive(false);

        infoPanel.SetActive(true);
        infoText.text = info;
    }

    public void ActiveInfoPanel(string info, int value)
    {
        CloseAll();
        mainTextPlay.SetActive(false);

        infoPanel.SetActive(true);
        infoText.text = $"{value} {info}";
    }
    #endregion

    public void CloseAll()
    {
        buttonsForPlay.SetActive(false);
        settings.SetActive(false);
        purchaseMenu.SetActive(false);
        buyTicketsPanel.SetActive(false);
        aboutGamePanel.SetActive(false);
        infoPanel.SetActive(false);

        mainTextPlay.SetActive(true);
    }
}
