using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Game_Loop;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameUI = null;
    [SerializeField]
    private GameObject losePanel = null;

    private static GameMenuManager instance = null;

    private void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        
        GameLoopManager gameManager = FindObjectOfType<GameLoopManager>();
        gameManager.SetUIEvent += HideLosePanel;
        gameManager.SetUIEvent += ShowGameUI;
    }

    public void RestartGame()
    {     
        FindObjectOfType<GameLoopManager>().RestartGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    private void HideLosePanel()
    {
        losePanel.SetActive(false);
    }

    private void ShowGameUI()
    {
        gameUI.SetActive(true);
    }
}