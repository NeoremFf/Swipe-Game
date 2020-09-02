using UnityEngine;
using UnityEngine.UI;

public class GameLoopUIUpdate : MonoBehaviour
{
    [Header("UI Timer")]
    [SerializeField] private GameObject timerBar = null;
    [SerializeField] private Text timerText = null;

    [Header("UI in game")]
    [SerializeField] private Text scoreGameUI = null;

    [Header("UI in lose")]
    [SerializeField] private Text scoreLoseUI = null;
    [SerializeField] private Text bestScoreLoseUI = null;

    [Header("Resources Value UI")]
    [SerializeField] private Text moneyValue = null;
    [SerializeField] private Text gemsValue = null;
    [SerializeField] private Text ticketsValue = null;
    [SerializeField] private Text resourcesValueGetsFromGameLoop = null;

    /// <summary>
    /// Update Timer UI in game loop
    /// </summary>
    /// <param name="sender">object that call event</param>
    /// <param name="e">info</param>
    public void UpdateTimerUI(object sender, TimerUpdateUIEventArgs e)
    {
        timerText.text = e.CurrentTime.ToString("0.0");
        timerBar.transform.localScale = new Vector3(e.CurrentTime / e.AllTime, timerBar.transform.localScale.y, 1);
    }

    /// <summary>
    /// Update lose ui
    /// </summary>
    /// <param name="e">info</param>
    public void UpdateLoseUI(ScoreUpdateUIEventArgs e)
    {
        scoreLoseUI.text = e.Score.ToString();
        bestScoreLoseUI.text = e.BestScore.ToString();
    }

    /// <summary>
    /// Update game score
    /// </summary>
    /// <param name="e">info</param>
    public void UpdateGameScoreUI(ScoreUpdateUIEventArgs e)
    {
        scoreGameUI.text = e.Score.ToString();
    }

    /// <summary>
    /// Update money value
    /// </summary>
    /// <param name="e">info</param>
    public void UpdateMoneyUI(ResourcesUpdateUIEventArgs e) //
    {
        moneyValue.text = e.Value.ToString();
        if (resourcesValueGetsFromGameLoop) resourcesValueGetsFromGameLoop.text = e.Value_2.ToString();
    }

    /// <summary>
    /// Update gems value
    /// </summary>
    /// <param name="e">info</param>
    public void UpdateGemsUI(ResourcesUpdateUIEventArgs e) //
    {
        gemsValue.text = e.Value.ToString();
        if (resourcesValueGetsFromGameLoop) resourcesValueGetsFromGameLoop.text = e.Value_2.ToString();
    }

    /// <summary>
    /// Update tickets value
    /// </summary>
    /// <param name="e">info</param>
    public void UpdateTicketsUI(ResourcesUpdateUIEventArgs e) //
    {
        if (ticketsValue) ticketsValue.text = e.Value.ToString();
    }
}
