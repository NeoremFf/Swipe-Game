using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameObject _classic;
    [SerializeField] private GameObject _challenge;

    private static GameModes.Modes mode = GameModes.Modes.NoneGame;

    public static void SetMode(GameModes.Modes newMode)
    {
        mode = newMode;
    }

    public GameModes.Modes GetGameMode() => mode;

    private void Awake()
    {
        switch (mode)
        {
            case GameModes.Modes.NoneGame:
                break;
            case GameModes.Modes.Classic:
                _classic.SetActive(true);
                _challenge.SetActive(false);
                break;
            case GameModes.Modes.Challenge:
                _classic.SetActive(false);
                _challenge.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        var updateUI = FindObjectOfType<GameLoopUIUpdate>();
        MoneyManager.SetEvents(updateUI.UpdateMoneyUI, updateUI.UpdateGemsUI, updateUI.UpdateTicketsUI);
    }
}
