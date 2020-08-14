using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("UI in game")]
    [SerializeField] private Text scoreUI = null;
    private static Text scoreUIHelper = null;

    [Header("UI in lose")]
    [SerializeField] private Text scoreLoseUI = null;
    [SerializeField] private Text bestScoreLoseUI = null;
    private static Text scoreLoseUIHelper = null;
    private static Text bestScoreLoseUHelperI = null;

    private static int score = 0;
    private static int bestScore = 0;
    private static int scoreAllLoop = 0;

    private static GameModes.Modes gameMode = GameModes.Modes.NoneGame;

    private void Start()
    {
        GameModeManager _mode = FindObjectOfType<GameModeManager>();
        if (!_mode) Log.WriteLog("Can not set game mode." + gameMode.ToString() + ".", Log.LevelsOfLogs.ERROR, "ScoreManager");
        gameMode = _mode.GetGameMode();
        Log.WriteLog("Set game mode: " + gameMode.ToString() + ".", Log.LevelsOfLogs.INFO, "ScoreManager");
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
        score = 0;
        scoreAllLoop = 0;

        scoreUIHelper = scoreUI;
        scoreUIHelper.text = score.ToString();

        scoreLoseUIHelper = scoreLoseUI;
        bestScoreLoseUHelperI = bestScoreLoseUI;
    }

    public static int GetCurrentScore() => score;

    public static void SetScoreToZero()
    {
        Log.WriteLog("Set score to zero.", Log.LevelsOfLogs.INFO, "ScoreManager");
        score = 0;
        scoreAllLoop = 0;
        scoreUIHelper.text = score.ToString();
    }

    public static void SetScoreToContinue()
    {
        score = 0;
    }

    public static void UpdateScore()
    {
        score++;
        scoreAllLoop ++;
        scoreUIHelper.text = scoreAllLoop.ToString();

        if (bestScore <= scoreAllLoop)
        {
            bestScore = scoreAllLoop;
        }
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