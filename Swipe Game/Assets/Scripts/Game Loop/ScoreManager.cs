using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // UI here
    [Header("UI in game")]
    [SerializeField] private Text scoreUI = null;
    private static Text scoreUIHelper = null;

    [Header("UI in lose")]
    [SerializeField] private Text scoreLoseUI = null;
    [SerializeField] private Text bestScoreLoseUI = null;
    private static Text scoreLoseUIHelper = null;
    private static Text bestScoreLoseUHelperI = null;
    // 

    private static int score = 0; // current score
    private static int bestScore = 0; // the best score of current mode
    private static int scoreAllLoop = 0; // all points scored in this game cycle (needed to continue after advertising or money after a loss)

    private static GameModes.Modes gameMode = GameModes.Modes.NoneGame;

    /// <summary>
    /// Prepare Money Manager
    /// </summary>
    private void Start()
    {
        GameModeManager _mode = FindObjectOfType<GameModeManager>();
        if (!_mode) Log.WriteLog("Can not set game mode." + gameMode.ToString() + ".", Log.LevelsOfLogs.ERROR, "ScoreManager");
        gameMode = _mode.GetGameMode();
        Log.WriteLog("Set game mode: " + gameMode.ToString() + ".", Log.LevelsOfLogs.INFO, "ScoreManager");
        // Get best score of current mode
        switch (gameMode)
        {
            case GameModes.Modes.Classic:
                bestScore = PlayerPrefs.GetInt(PlayerPrefsKeys.BestScoreClassic);
                break;

            case GameModes.Modes.Challenge:
                bestScore = PlayerPrefs.GetInt(PlayerPrefsKeys.BestScoreChallenge);
                break;
        }
        Log.WriteLog("Best score: " + bestScore + ".", Log.LevelsOfLogs.INFO, "ScoreManager");

        // UI
        scoreUIHelper = scoreUI;
        scoreUIHelper.text = score.ToString();

        scoreLoseUIHelper = scoreLoseUI;
        bestScoreLoseUHelperI = bestScoreLoseUI;
    }

    /// <returns>Current score</returns>
    public static int GetCurrentScore() => score;

    /// <summary>
    /// Reset score to zero for new game loop
    /// </summary>
    public static void SetScoreToZero()
    {
        score = 0;
        scoreAllLoop = 0;
        // UI
        scoreUIHelper.text = score.ToString();
    }

    /// <summary>
    /// Reset score to zero for continue game loop
    /// </summary>
    public static void SetScoreToContinue()
    {
        score = 0;
    }

    public static void UpdateScore()
    {
        score++;
        scoreAllLoop ++;
        // UI
        scoreUIHelper.text = scoreAllLoop.ToString();

        if (bestScore <= scoreAllLoop)
            bestScore = scoreAllLoop;
    }

    public static void UpdateLoseUI()
    {
        Log.WriteLog("Update scores after death.", Log.LevelsOfLogs.INFO, "ScoreManager");
        switch (gameMode)
        {
            case GameModes.Modes.Classic:
                PlayerPrefs.SetInt(PlayerPrefsKeys.BestScoreClassic, bestScore);
                break;

            case GameModes.Modes.Challenge:
                PlayerPrefs.SetInt(PlayerPrefsKeys.BestScoreChallenge, bestScore);
                break;
        }

        MoneyManager.AddMoneyInGamesEnd(score);
        scoreLoseUIHelper.text = scoreAllLoop.ToString();
        bestScoreLoseUHelperI.text = bestScore.ToString();
    }
}