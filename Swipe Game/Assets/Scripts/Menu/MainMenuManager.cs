using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playButtons;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject purhaseMoney;

    [Header("Shop")]
    [SerializeField]
    private GameObject shop;
    [SerializeField]
    private GameObject shop_background;
    [SerializeField]
    private GameObject shop_effects;

    private static MainMenuManager instance = null;

    private void Start()
    {        
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    public void StartGame()
    {
        playButtons.SetActive(!playButtons.activeSelf);
    }

    public void StartClassicGame()
    {
        GameModeManager.SetMode(GameModes.Modes.Classic);
        SceneManager.LoadScene(2);
    }

    public void StartChallengeGame()
    {
        GameModeManager.SetMode(GameModes.Modes.Challenge);
        SceneManager.LoadScene(2);
    }

    // Settings
    public void OpenSettings()
    {
        settings.SetActive(!settings.activeSelf);
    }

    public void SwitchLanguage_Eng()
    {
        LocalizationManager local = FindObjectOfType<LocalizationManager>();
        local.SetLanguage(0);
    }

    public void SwitchLanguage_Rus()
    {
        LocalizationManager local = FindObjectOfType<LocalizationManager>();
        local.SetLanguage(1);
    }

    public void OpenLeaderboard()
    {
        // add
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void OpenShop()
    {
        shop.SetActive(!shop.activeSelf);
    }

    public void OpenShop_Background()
    {
        shop_background.SetActive(!shop_background.activeSelf);
    }

    public void OpenShop_Effects()
    {
        shop_effects.SetActive(!shop_effects.activeSelf);
    }

    #region Purchase gems
    public void OpenPurchaseMoney()
    {
        purhaseMoney.SetActive(!purhaseMoney.activeSelf);
    }

    public void BuyMoney(int id)
    {
        FindObjectOfType<PurchaseManager>().BuyConsumable(id);
    }
    #endregion
}
