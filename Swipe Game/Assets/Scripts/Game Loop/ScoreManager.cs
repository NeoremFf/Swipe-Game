using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static int score = 0; // current score
    private static int bestScore = 0; // the best score of current mode
    private static int scoreAllLoop = 0; // all points scored in this game cycle (needed to continue after advertising or money after a loss)

    private static GameModes.Modes gameMode = GameModes.Modes.NoneGame;

    private delegate void GameUIUpdate(ScoreUpdateUIEventArgs e);
    private static event GameUIUpdate updateScoreUI = null;
    private static event GameUIUpdate updateLoseUI = null;

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

        var uiUpdater = FindObjectOfType<GameLoopUIUpdate>();
        updateScoreUI += uiUpdater.UpdateGameScoreUI;
        updateLoseUI += uiUpdater.UpdateLoseUI;

        updateScoreUI?.Invoke(new ScoreUpdateUIEventArgs(0));
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
        updateScoreUI?.Invoke(new ScoreUpdateUIEventArgs(score));
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
        updateScoreUI?.Invoke(new ScoreUpdateUIEventArgs(scoreAllLoop));
        if (bestScore <= scoreAllLoop)
            bestScore = scoreAllLoop;
    }

    public static void UpdateLose()
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
        updateLoseUI?.Invoke(new ScoreUpdateUIEventArgs(scoreAllLoop, bestScore));
    }
}