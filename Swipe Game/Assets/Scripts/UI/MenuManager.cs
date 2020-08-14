using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Game")]
    [SerializeField] private GameObject playScene = null;
    [SerializeField] private GameObject loseScene = null;
    [SerializeField] private GameObject purchaseMoneyPanel = null;
    [SerializeField] private GameObject buyTicketsPanel = null;
    [SerializeField] private GameObject tutorialPanel = null;
    [SerializeField] private GameObject buttonExtraLive = null;
    [SerializeField] private GameObject closeAllPanel = null;
    private GameObject objToShowAfterInfoPane = null;

    [Header("Text UI")]
    [SerializeField] private Text moneyCountText = null;
    [SerializeField] private Text moneyCountAddAfterGameText = null;
    [SerializeField] private Text gemsCountText = null;
    [SerializeField] private Text ticketsCountText = null;

    [Header("Info Panel")]
    [SerializeField] private GameObject infoPanel = null;
    
    [Header("Answer Panel")]
    [SerializeField] private GameObject answerPanel = null;

    [Header("Game Manager")]
    [SerializeField] private GameObject gameController = null;
    [SerializeField] private AudioSource _sound = null;
    [SerializeField] private MoneyManager _resourcesManager = null;
    private ClassicGameManager _classic = null;
    private ChallengeGameManager _challenge = null;

    private AdsManager _ads = null;

    private void Start()
    {
        GameModeManager _mode = FindObjectOfType<GameModeManager>();
        switch (_mode.GetGameMode())
        {
            case GameModes.Modes.NoneGame:
                break;
            case GameModes.Modes.Classic:
                _classic = FindObjectOfType<ClassicGameManager>();
                if (!_classic)
                    Log.WriteLog("ClassicGameManager not set.", Log.LevelsOfLogs.ERROR, "SwipeManager");
                break;
            case GameModes.Modes.Challenge:
                _challenge = FindObjectOfType<ChallengeGameManager>();
                if (!_challenge)
                    Log.WriteLog("ClassicGameManager not set.", Log.LevelsOfLogs.ERROR, "SwipeManager");
                break;
            default:
                break;
        }
        _ads = FindObjectOfType<AdsManager>();
        if (!_ads) Log.WriteLog("Can not get AdsManager.", Log.LevelsOfLogs.ERROR, "MenuManager");
        buttonExtraLive.SetActive(true);

        MoneyManager _resources = FindObjectOfType<MoneyManager>();
        _resources?.SetEventsForUpdateUI(UpdateMoneyUI, UpdateGemsUI, UpdateTicketsUI);

        UpdateMoneyUI();
        UpdateGemsUI();
        UpdateTicketsUI();

        IsItFirstEnteringInGeme();

        if (!_sound)
        {
            Log.WriteLog("AudioSurce is null.", Log.LevelsOfLogs.ERROR, "MenuManager");
            return;
        }
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.SettingsSoundActive) == 1)
        {
            Log.WriteLog("Set sound: play.", Log.LevelsOfLogs.INFO, "MenuManager");
            _sound.Play();
        }
        else
        {
            Log.WriteLog("Set sound: stop.", Log.LevelsOfLogs.INFO, "MenuManager");
            _sound.Stop();
        }
    }

    private void IsItFirstEnteringInGeme()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.FirstEntering) == 1)
        {
            tutorialPanel.SetActive(true);
            PlayerPrefs.SetInt(PlayerPrefsKeys.FirstEntering, 0);
        }
        else
            tutorialPanel.SetActive(false);
    }

    #region Buttons On Lose Screen
    public void BackToMainMenu()
    {
        Log.WriteLog("Back to main menu.", Log.LevelsOfLogs.INFO, "MenuManager");
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        if (_classic)
        {
            playScene.SetActive(true);
            loseScene.SetActive(false);
            _classic.RestartLoop();
            buttonExtraLive.SetActive(true);
        }
        else
        {
            if (MoneyManager.GetTickets() > 0)
            {
                playScene.SetActive(true);
                loseScene.SetActive(false);
                _challenge.RestartLoop();
            }
            else
                AnswerPanel("NOT ENOUGH TICKETS TO PLAY\n<color=grye><size=70>BUY MORE?</size></color>", buyTicketsPanel);
        }
    }

    public void GetExtraLive()
    {
        if (_resourcesManager.BuyExtraLive())
        {
            InfoPanel("Extra Live Add!");
            loseScene.SetActive(false);
            playScene.SetActive(true);
            _classic?.ContinueLoop();
            _challenge?.ContinueLoop();
            buttonExtraLive.SetActive(false);
        }
        else
            AnswerPanel("NOT ENOUGH MONEY TO DO IT\n<color=grye><size=70>BUY MORE?</size></color>", purchaseMoneyPanel);
    }

    public void Share()
    {
        SocialManager.Share();
    }
    #endregion

    #region Update UI
    private void UpdateMoneyUI(int countMoneyAdd = 0)
    {
        string value = PlayerPrefs.GetInt(PlayerPrefsKeys.Money).ToString();
        moneyCountText.text = value;
        if (!moneyCountAddAfterGameText) moneyCountAddAfterGameText.text = countMoneyAdd.ToString();
    }

    private void UpdateGemsUI()
    {
        string value = PlayerPrefs.GetInt(PlayerPrefsKeys.Money).ToString();
        gemsCountText.text = value;
    }

    private void UpdateTicketsUI()
    {
        string value = PlayerPrefs.GetInt(PlayerPrefsKeys.Money).ToString();
        gemsCountText.text = value;
    }
    #endregion

    public void BuyTickets(int count)
    {
        ClosePanels();
        if (MoneyManager.BuyTicket(count))
            InfoPanel("TICKETS ADD");
        else
            AnswerPanel("NOT ENOUGH MONEY\n<color=grey>BUY MORE</color>", purchaseMoneyPanel);
    }

    public void AnswerPanel(string info, GameObject activeGO)
    {
        SetText(answerPanel, info);
        objToShowAfterInfoPane = activeGO;
        ClosePanels();
        ActivePanel(answerPanel);
    }

    public void GetAnswerFromUser(bool answer)
    {
        ClosePanels();
        if (answer)
            ActivePanel(objToShowAfterInfoPane);
    }

    public void InfoPanel(string info)
    {
        SetText(infoPanel, info);
        ClosePanels();
        ActivePanel(infoPanel);
    }

    public void ClosePanels()
    {
        infoPanel.SetActive(false);
        answerPanel.SetActive(false);
        closeAllPanel.SetActive(false);
        if (objToShowAfterInfoPane) objToShowAfterInfoPane.SetActive(false);
    }

    [System.Obsolete]
    public void FreeMoney()
    {
        Log.WriteLog("Free money.", Log.LevelsOfLogs.INFO, "MainMenuManager");
        _ads.ShowRewardedAd(AllState.AdsShowFor.FreeMoney);
    }

    public void PurchaseMoney(int id)
    {
        Log.WriteLog("Purchase money.", Log.LevelsOfLogs.INFO, "MainMenuManager");
        PurchaseManager _purchase = FindObjectOfType<PurchaseManager>();
        if (!_purchase) Log.WriteLog("PurchaseManager not set.", Log.LevelsOfLogs.ERROR, "MainMenuManager");
        _purchase.BuyConsumable(id);
        UpdateMoneyUI();
    }

    public void UpdateMoneyUI()
    {
        moneyCountText.text = PlayerPrefs.GetInt(PlayerPrefsKeys.Money).ToString();
    }

    public void CloseTutorialPanel()
    {
        tutorialPanel.SetActive(false);
    }

    private void ActivePanel(GameObject obj)
    {
        obj.SetActive(true);
        PlayAnim(obj);
        closeAllPanel.SetActive(true);
    }

    private void SetText(GameObject obj, string text)
    {
        TextUISetter _textInfo = obj.GetComponentInChildren<TextUISetter>();
        if (!_textInfo)
        {
            Log.WriteLog("Can not get Text from info panel", Log.LevelsOfLogs.ERROR, "MenuManager");
            return;
        }
        _textInfo.SetText(text);
    }

    private void PlayAnim(GameObject obj)
    {
        Animator anim = obj.GetComponent<Animator>();
        if (!anim)
        {
            Log.WriteLog("Can not get animator.", Log.LevelsOfLogs.WARNING, "MainMenuManager");
            return;
        }
        anim.Play("Scale");
    }
}
